-- =====================================================================================
-- Limpia productos duplicados por nombre (quedan por haber agregado en vez de modificar).
-- Se queda con el que tiene mas stock (si empatan, el de menor codigo).
--   * Los sobrantes que NO figuran en pedidos se eliminan.
--   * Los sobrantes que figuran en pedidos se marcan como no disponibles (no se pueden borrar).
-- Se puede correr varias veces. Despues: Administrador > Verificacion de Integridad > Recalcular.
-- =====================================================================================
USE CAFECCINO1;
GO

-- Vista previa: los productos que hoy estan duplicados
SELECT p.Codigo, p.Nombre, p.Precio, p.Stock, p.Disponible,
       (SELECT COUNT(*) FROM dbo.DetallePedido d WHERE d.Codigo = p.Codigo) AS EnPedidos
FROM dbo.Producto p
WHERE EXISTS (SELECT 1 FROM dbo.Producto q
              WHERE LTRIM(RTRIM(q.Nombre)) = LTRIM(RTRIM(p.Nombre)) AND q.Codigo <> p.Codigo)
ORDER BY p.Nombre, p.Stock DESC, p.Codigo;
GO

-- Limpieza
;WITH R AS (
    SELECT Codigo,
           ROW_NUMBER() OVER (PARTITION BY LTRIM(RTRIM(Nombre)) ORDER BY Stock DESC, Codigo) AS rn
    FROM dbo.Producto
)
SELECT Codigo INTO #Sobrantes FROM R WHERE rn > 1;

DELETE p
FROM dbo.Producto p
INNER JOIN #Sobrantes s ON s.Codigo = p.Codigo
WHERE NOT EXISTS (SELECT 1 FROM dbo.DetallePedido d WHERE d.Codigo = p.Codigo);

UPDATE p SET Disponible = 0
FROM dbo.Producto p
INNER JOIN #Sobrantes s ON s.Codigo = p.Codigo;

DROP TABLE #Sobrantes;
GO

SELECT Codigo, Nombre, Precio, Stock, Disponible FROM dbo.Producto ORDER BY Nombre, Codigo;
GO
