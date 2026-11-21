using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    
    
    public class DALReporte
    {
        private static DataTable Consultar_235JB(string sql_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(sql_235JB, cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        
        public DataTable ProductosMasOrdenados_235JB()
        {
            return Consultar_235JB(@"
                SELECT ROW_NUMBER() OVER (ORDER BY SUM(d.Cantidad) DESC, COUNT(DISTINCT d.CodigoPedido) DESC, pr.Nombre) AS [Puesto],
                       pr.Nombre AS [Producto], SUM(d.Cantidad) AS [Unidades], COUNT(DISTINCT d.CodigoPedido) AS [Pedidos],
                       SUM(d.Subtotal) AS [Facturado ($)]
                FROM DetallePedido d
                JOIN Pedido p ON p.CodigoPedido = d.CodigoPedido
                JOIN Producto pr ON pr.Codigo = d.Codigo
                WHERE p.Estado <> 'Cancelado'
                GROUP BY pr.Codigo, pr.Nombre
                ORDER BY [Puesto]");
        }

        
        public DataTable ClienteQueMasPidioCadaProducto_235JB()
        {
            return Consultar_235JB(@"
                WITH x AS (
                    SELECT pr.Codigo, pr.Nombre AS Producto, c.Nombre + ' ' + c.Apellido AS Cliente, c.DNI,
                           SUM(d.Cantidad) AS Unidades, COUNT(DISTINCT d.CodigoPedido) AS Pedidos,
                           ROW_NUMBER() OVER (PARTITION BY pr.Codigo ORDER BY SUM(d.Cantidad) DESC, COUNT(DISTINCT d.CodigoPedido) DESC, c.DNI) AS n,
                           SUM(SUM(d.Cantidad)) OVER (PARTITION BY pr.Codigo) AS TotalProducto
                    FROM DetallePedido d
                    JOIN Pedido p ON p.CodigoPedido = d.CodigoPedido
                    JOIN Producto pr ON pr.Codigo = d.Codigo
                    JOIN Cliente c ON c.DNI = p.DNI
                    WHERE p.Estado <> 'Cancelado'
                    GROUP BY pr.Codigo, pr.Nombre, c.DNI, c.Nombre, c.Apellido)
                SELECT Producto AS [Producto], Cliente AS [Cliente], DNI AS [DNI], Unidades AS [Unidades], Pedidos AS [Pedidos],
                       CAST(100.0 * Unidades / TotalProducto AS decimal(5,1)) AS [% del producto]
                FROM x
                WHERE n = 1
                ORDER BY TotalProducto DESC, Producto");
        }

        
        public DataTable ClientesQueMasGastaron_235JB()
        {
            return Consultar_235JB(@"
                SELECT TOP 10 ROW_NUMBER() OVER (ORDER BY SUM(p.Total) DESC, COUNT(*) DESC, c.DNI) AS [Puesto],
                       c.Nombre + ' ' + c.Apellido AS [Cliente], c.DNI AS [DNI], COUNT(*) AS [Pedidos],
                       SUM(p.Total) AS [Total gastado ($)], SUM(p.Total) / COUNT(*) AS [Ticket promedio ($)]
                FROM Pedido p
                JOIN Cliente c ON c.DNI = p.DNI
                WHERE p.Estado <> 'Cancelado'
                GROUP BY c.DNI, c.Nombre, c.Apellido
                ORDER BY [Puesto]");
        }

        
        public DataTable MozosConMasPedidos_235JB()
        {
            return Consultar_235JB(@"
                SELECT ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC, SUM(p.Total) DESC, m.DNIMozo) AS [Puesto],
                       m.Nombre + ' ' + m.Apellido AS [Mozo], COUNT(*) AS [Pedidos atendidos],
                       SUM(p.Total) AS [Total facturado ($)], SUM(p.Total) / COUNT(*) AS [Promedio por pedido ($)]
                FROM Atencion a
                JOIN Pedido p ON p.CodigoPedido = a.CodigoPedido
                JOIN Mozo m ON m.DNIMozo = a.DNIMozo
                WHERE p.Estado <> 'Cancelado'
                GROUP BY m.DNIMozo, m.Nombre, m.Apellido
                ORDER BY [Puesto]");
        }

        
        public DataTable HorariosConMasPedidos_235JB()
        {
            return Consultar_235JB(@"
                SELECT ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC, LEFT(p.Hora, 2)) AS [Puesto],
                       LEFT(p.Hora, 2) + ':00 a ' + RIGHT('0' + CAST((CAST(LEFT(p.Hora, 2) AS int) + 1) % 24 AS varchar(2)), 2) + ':00' AS [Franja horaria],
                       COUNT(*) AS [Pedidos], SUM(p.Total) AS [Total facturado ($)]
                FROM Pedido p
                WHERE p.Estado <> 'Cancelado'
                GROUP BY LEFT(p.Hora, 2)
                ORDER BY [Puesto]");
        }

        
        public DataTable ProductosMenosVendidos_235JB()
        {
            return Consultar_235JB(@"
                WITH v AS (
                    SELECT d.Codigo, SUM(d.Cantidad) AS Unidades, COUNT(DISTINCT d.CodigoPedido) AS Pedidos, SUM(d.Subtotal) AS Facturado
                    FROM DetallePedido d
                    JOIN Pedido p ON p.CodigoPedido = d.CodigoPedido
                    WHERE p.Estado <> 'Cancelado'
                    GROUP BY d.Codigo)
                SELECT ROW_NUMBER() OVER (ORDER BY ISNULL(v.Unidades, 0), ISNULL(v.Pedidos, 0), pr.Nombre) AS [Puesto],
                       pr.Nombre AS [Producto], ISNULL(v.Unidades, 0) AS [Unidades], ISNULL(v.Pedidos, 0) AS [Pedidos],
                       ISNULL(v.Facturado, 0) AS [Facturado ($)], CASE WHEN pr.Disponible = 1 THEN 'Sí' ELSE 'No' END AS [Disponible]
                FROM Producto pr
                LEFT JOIN v ON v.Codigo = pr.Codigo
                WHERE pr.Bit_Lo_Bo = 0
                ORDER BY [Puesto]");
        }

        
        public DataTable VentasPorDiaSemana_235JB()
        {
            return Consultar_235JB(@"
                WITH x AS (SELECT DATEDIFF(day, '19000101', p.Fecha) % 7 AS Dia, p.Total FROM Pedido p WHERE p.Estado <> 'Cancelado')
                SELECT ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC, SUM(x.Total) DESC, x.Dia) AS [Puesto],
                       CHOOSE(x.Dia + 1, 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado', 'Domingo') AS [Día],
                       COUNT(*) AS [Pedidos], SUM(x.Total) AS [Total facturado ($)], SUM(x.Total) / COUNT(*) AS [Promedio por pedido ($)]
                FROM x
                GROUP BY x.Dia
                ORDER BY [Puesto]");
        }

        
        public DataTable MesasQueMasFacturaron_235JB()
        {
            return Consultar_235JB(@"
                SELECT ROW_NUMBER() OVER (ORDER BY SUM(p.Total) DESC, COUNT(*) DESC, me.NumeroMesa) AS [Puesto],
                       me.NumeroMesa AS [Mesa], me.Capacidad AS [Capacidad], COUNT(*) AS [Pedidos],
                       SUM(p.Total) AS [Total facturado ($)], SUM(p.Total) / COUNT(*) AS [Promedio por pedido ($)]
                FROM Atencion a
                JOIN Pedido p ON p.CodigoPedido = a.CodigoPedido
                JOIN Mesa me ON me.NumeroMesa = a.NumeroMesa
                WHERE p.Estado <> 'Cancelado'
                GROUP BY me.NumeroMesa, me.Capacidad
                ORDER BY [Puesto]");
        }

        
        public DataTable MesasMasReservadas_235JB()
        {
            return Consultar_235JB(@"
                WITH x AS (
                    SELECT me.NumeroMesa, me.Capacidad,
                           SUM(CASE WHEN r.Estado <> 'Cancelada' THEN 1 ELSE 0 END) AS Reservas,
                           SUM(CASE WHEN r.Estado = 'Cumplida' THEN 1 ELSE 0 END) AS Cumplidas,
                           SUM(CASE WHEN r.Estado = 'Cancelada' THEN 1 ELSE 0 END) AS Canceladas
                    FROM Mesa me
                    LEFT JOIN Reserva r ON r.NumeroMesa = me.NumeroMesa
                    GROUP BY me.NumeroMesa, me.Capacidad)
                SELECT ROW_NUMBER() OVER (ORDER BY Reservas DESC, Cumplidas DESC, NumeroMesa) AS [Puesto],
                       NumeroMesa AS [Mesa], Capacidad AS [Capacidad], Reservas AS [Reservas], Cumplidas AS [Cumplidas],
                       Canceladas AS [Canceladas]
                FROM x
                ORDER BY [Puesto]");
        }

        
        public DataTable ClientesQueMasReservaron_235JB()
        {
            return Consultar_235JB(@"
                WITH x AS (
                    SELECT r.DNI,
                           SUM(CASE WHEN r.Estado <> 'Cancelada' THEN 1 ELSE 0 END) AS Reservas,
                           SUM(CASE WHEN r.Estado = 'Cumplida' THEN 1 ELSE 0 END) AS Cumplidas,
                           SUM(CASE WHEN r.Estado = 'Cancelada' THEN 1 ELSE 0 END) AS Canceladas
                    FROM Reserva r
                    GROUP BY r.DNI),
                pref AS (
                    SELECT r.DNI, r.NumeroMesa,
                           ROW_NUMBER() OVER (PARTITION BY r.DNI ORDER BY COUNT(*) DESC, r.NumeroMesa) AS n
                    FROM Reserva r
                    WHERE r.Estado <> 'Cancelada'
                    GROUP BY r.DNI, r.NumeroMesa)
                SELECT TOP 10 ROW_NUMBER() OVER (ORDER BY x.Reservas DESC, x.Cumplidas DESC, x.DNI) AS [Puesto],
                       c.Nombre + ' ' + c.Apellido AS [Cliente], c.DNI AS [DNI], x.Reservas AS [Reservas], x.Cumplidas AS [Cumplidas],
                       x.Canceladas AS [Canceladas], pref.NumeroMesa AS [Mesa preferida]
                FROM x
                JOIN Cliente c ON c.DNI = x.DNI
                LEFT JOIN pref ON pref.DNI = x.DNI AND pref.n = 1
                WHERE x.Reservas > 0
                ORDER BY [Puesto]");
        }

        
        public DataTable HorariosConMasReservas_235JB()
        {
            return Consultar_235JB(@"
                SELECT ROW_NUMBER() OVER (ORDER BY COUNT(*) DESC, LEFT(r.Hora, 2)) AS [Puesto],
                       LEFT(r.Hora, 2) + ':00 a ' + RIGHT('0' + CAST((CAST(LEFT(r.Hora, 2) AS int) + 1) % 24 AS varchar(2)), 2) + ':00' AS [Franja horaria],
                       COUNT(*) AS [Reservas], SUM(CASE WHEN r.Estado = 'Cumplida' THEN 1 ELSE 0 END) AS [Cumplidas]
                FROM Reserva r
                WHERE r.Estado <> 'Cancelada'
                GROUP BY LEFT(r.Hora, 2)
                ORDER BY [Puesto]");
        }
    }
}
