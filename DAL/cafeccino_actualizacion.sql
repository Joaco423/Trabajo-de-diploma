-- Actualiza la estructura de una base creada por el instalador 1.0 a la de la version 2.0, sin perder datos.
-- Lo ejecuta el programa cuando, al instalar la 2.0, el usuario elige conservar la base que ya existia.
-- Cada paso se hace solo si falta: sobre una base que ya tiene la estructura 2.0 no cambia nada.
-- (La configuracion que falte, como permisos y traducciones, la agrega el programa desde el script de creacion.)

-- 1. Baja logica de productos
IF COL_LENGTH(N'dbo.Producto', N'Bit_Lo_Bo') IS NULL
    ALTER TABLE [dbo].[Producto] ADD [Bit_Lo_Bo] bit NOT NULL CONSTRAINT [DF_Producto_Bit_Lo_Bo] DEFAULT ((0));
GO

-- 2. Bitacora de cambios de producto. Cada producto arranca con su estado actual como registro activo,
--    para poder volver a el desde la bitacora despues de modificarlo.
IF OBJECT_ID(N'dbo.Producto_C', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Producto_C] (
        [CodigoCambio] int IDENTITY(1,1) NOT NULL,
        [CodigoProducto] int NOT NULL,
        [Fecha] date NOT NULL,
        [Hora] nvarchar(8) COLLATE Modern_Spanish_CI_AS NOT NULL,
        [Nombre] nvarchar(50) COLLATE Modern_Spanish_CI_AS NOT NULL,
        [Precio] int NOT NULL,
        [Disponible] bit NOT NULL,
        [Stock] int NOT NULL,
        [Act] bit NOT NULL CONSTRAINT [DF_Producto_C_Act] DEFAULT ((0)),
        [DVH] int NULL,
        CONSTRAINT [PK_Producto_C] PRIMARY KEY CLUSTERED ([CodigoCambio] ASC)
    );
    ALTER TABLE [dbo].[Producto_C] WITH CHECK ADD CONSTRAINT [FK_Producto_C_Producto] FOREIGN KEY ([CodigoProducto]) REFERENCES [dbo].[Producto] ([Codigo]);

    INSERT INTO [dbo].[Producto_C] (CodigoProducto, Fecha, Hora, Nombre, Precio, Disponible, Stock, Act)
    SELECT Codigo, CAST(GETDATE() AS date), CONVERT(nvarchar(8), GETDATE(), 108), Nombre, Precio, Disponible, Stock, 1
    FROM [dbo].[Producto]
    WHERE Bit_Lo_Bo = 0
    ORDER BY Codigo;
END
GO

-- 3. Trigger que registra en la bitacora cada cambio de un producto
IF OBJECT_ID(N'dbo.TR_Producto_RegistrarCambio', N'TR') IS NULL
    EXEC (N'CREATE TRIGGER dbo.TR_Producto_RegistrarCambio ON dbo.Producto
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    -- El cambio viene de activar un registro de Producto_C (TR_Producto_C_Activar): no es un cambio nuevo
    IF TRIGGER_NESTLEVEL() > 1 RETURN;

    -- Solo las filas cuyos datos cambiaron (actualizar el digito verificador DVH no es un cambio)
    DECLARE @cambios TABLE (Codigo int PRIMARY KEY, Nombre nvarchar(50), Precio int, Disponible bit, Stock int);
    INSERT INTO @cambios (Codigo, Nombre, Precio, Disponible, Stock)
    SELECT i.Codigo, i.Nombre, i.Precio, i.Disponible, i.Stock
    FROM (SELECT Codigo, Nombre, Precio, Disponible, Stock, Bit_Lo_Bo FROM inserted
          EXCEPT
          SELECT Codigo, Nombre, Precio, Disponible, Stock, Bit_Lo_Bo FROM deleted) i;

    IF NOT EXISTS (SELECT 1 FROM @cambios) RETURN;

    UPDATE c SET Act = 0
    FROM dbo.Producto_C c
    JOIN @cambios x ON x.Codigo = c.CodigoProducto
    WHERE c.Act = 1;

    INSERT INTO dbo.Producto_C (CodigoProducto, Fecha, Hora, Nombre, Precio, Disponible, Stock, Act)
    SELECT Codigo, CAST(GETDATE() AS date), CONVERT(nvarchar(8), GETDATE(), 108), Nombre, Precio, Disponible, Stock, 1
    FROM @cambios
    ORDER BY Codigo;
END');
GO

-- 4. Trigger que, al activar un registro de la bitacora, vuelve el producto a esos datos
IF OBJECT_ID(N'dbo.TR_Producto_C_Activar', N'TR') IS NULL
    EXEC (N'CREATE TRIGGER dbo.TR_Producto_C_Activar ON dbo.Producto_C
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT UPDATE(Act) RETURN;

    -- Registros que pasaron de 0 a 1 (si se activan varios del mismo producto gana el mas nuevo)
    DECLARE @activados TABLE (CodigoCambio int PRIMARY KEY, CodigoProducto int);
    INSERT INTO @activados (CodigoCambio, CodigoProducto)
    SELECT MAX(i.CodigoCambio), i.CodigoProducto
    FROM inserted i
    JOIN deleted d ON d.CodigoCambio = i.CodigoCambio
    WHERE i.Act = 1 AND d.Act = 0
    GROUP BY i.CodigoProducto;

    IF NOT EXISTS (SELECT 1 FROM @activados) RETURN;

    -- Un solo registro activo por producto
    UPDATE c SET Act = CASE WHEN a.CodigoCambio = c.CodigoCambio THEN 1 ELSE 0 END
    FROM dbo.Producto_C c
    JOIN @activados a ON a.CodigoProducto = c.CodigoProducto
    WHERE c.Act = 1 OR c.CodigoCambio = a.CodigoCambio;

    -- El producto vuelve a los datos de ese registro (y sale del borrado logico)
    UPDATE p SET Nombre = c.Nombre, Precio = c.Precio, Disponible = c.Disponible, Stock = c.Stock, Bit_Lo_Bo = 0
    FROM dbo.Producto p
    JOIN @activados a ON a.CodigoProducto = p.Codigo
    JOIN dbo.Producto_C c ON c.CodigoCambio = a.CodigoCambio;
END');
GO

-- 5. Un cliente no puede tener dos reservas activas (si los datos que ya hay no lo cumplen, el indice no se crea)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_Reserva_ClienteActiva' AND object_id = OBJECT_ID(N'dbo.Reserva'))
   AND NOT EXISTS (SELECT DNI FROM [dbo].[Reserva] WHERE Estado = 'Activa' GROUP BY DNI HAVING COUNT(*) > 1)
    CREATE UNIQUE NONCLUSTERED INDEX [UX_Reserva_ClienteActiva] ON [dbo].[Reserva] ([DNI] ASC) WHERE ([Estado]='Activa');
GO
