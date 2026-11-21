using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DAL
{
    // Bitacora de cambios de producto (tabla Producto_C). Los registros los crea el trigger TR_Producto_RegistrarCambio;
    // al poner Act = 1 en un registro, el trigger TR_Producto_C_Activar actualiza Producto con esos datos.
    public class DALProducto_C
    {
        private const string Columnas_235JB =
            "SELECT CodigoCambio, CodigoProducto, Fecha, Hora, Nombre, Precio, Disponible, Stock, Act FROM Producto_C";

        public List<Producto_C> ListarCambios_235JB()
        {
            return ObtenerCambiosSegunFiltros_235JB(null, null, null, null);
        }

        // Filtros opcionales: codigo de producto, nombre y rango de fechas (inclusive)
        public List<Producto_C> ObtenerCambiosSegunFiltros_235JB(int? codigoProducto_235JB, string nombre_235JB, DateTime? fechaIni_235JB, DateTime? fechaFin_235JB)
        {
            List<Producto_C> lista_235JB = new List<Producto_C>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand())
            {
                StringBuilder sql_235JB = new StringBuilder(Columnas_235JB + " WHERE 1 = 1");
                if (codigoProducto_235JB.HasValue)
                {
                    sql_235JB.Append(" AND CodigoProducto = @codigo");
                    cmd_235JB.Parameters.AddWithValue("@codigo", codigoProducto_235JB.Value);
                }
                if (!string.IsNullOrWhiteSpace(nombre_235JB))
                {
                    sql_235JB.Append(" AND Nombre = @nombre");
                    cmd_235JB.Parameters.AddWithValue("@nombre", nombre_235JB.Trim());
                }
                if (fechaIni_235JB.HasValue)
                {
                    sql_235JB.Append(" AND Fecha >= @fechaIni");
                    cmd_235JB.Parameters.AddWithValue("@fechaIni", fechaIni_235JB.Value.Date);
                }
                if (fechaFin_235JB.HasValue)
                {
                    sql_235JB.Append(" AND Fecha <= @fechaFin");
                    cmd_235JB.Parameters.AddWithValue("@fechaFin", fechaFin_235JB.Value.Date);
                }
                sql_235JB.Append(" ORDER BY CodigoCambio DESC");

                cmd_235JB.Connection = cn_235JB;
                cmd_235JB.CommandText = sql_235JB.ToString();
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read()) lista_235JB.Add(Leer_235JB(r_235JB));
                }
            }
            return lista_235JB;
        }

        // Valores para los filtros (todos los que aparecen en la bitacora)
        public List<int> ObtenerCodigos_235JB()
        {
            List<int> codigos_235JB = new List<int>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT DISTINCT CodigoProducto FROM Producto_C ORDER BY CodigoProducto", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read()) codigos_235JB.Add(r_235JB.GetInt32(0));
                }
            }
            return codigos_235JB;
        }

        public List<string> ObtenerNombres_235JB()
        {
            List<string> nombres_235JB = new List<string>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT DISTINCT Nombre FROM Producto_C ORDER BY Nombre", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read()) nombres_235JB.Add(r_235JB.GetString(0));
                }
            }
            return nombres_235JB;
        }

        public Producto_C ObtenerCambio_235JB(int codigoCambio_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(Columnas_235JB + " WHERE CodigoCambio = @codigo", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@codigo", codigoCambio_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    return r_235JB.Read() ? Leer_235JB(r_235JB) : null;
                }
            }
        }

        // Pone Act = 1 en el registro. El trigger TR_Producto_C_Activar desactiva los demas del producto y actualiza Producto.
        // Devuelve el producto con los datos de ese registro (null si el registro no existe).
        public Producto ActivarCambio_235JB(int codigoCambio_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "UPDATE Producto_C SET Act = 1 WHERE CodigoCambio = @codigo AND Act = 0; " + Columnas_235JB + " WHERE CodigoCambio = @codigo", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@codigo", codigoCambio_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    Producto_C cambio_235JB = Leer_235JB(r_235JB);
                    return new Producto
                    {
                        Codigo = cambio_235JB.CodigoProducto,
                        Nombre = cambio_235JB.Nombre,
                        Precio = cambio_235JB.Precio,
                        Disponible = cambio_235JB.Disponible,
                        Stock = cambio_235JB.Stock
                    };
                }
            }
        }

        private static Producto_C Leer_235JB(SqlDataReader r_235JB)
        {
            return new Producto_C
            {
                CodigoCambio = r_235JB.GetInt32(0),
                CodigoProducto = r_235JB.GetInt32(1),
                Fecha = r_235JB.GetDateTime(2),
                Hora = r_235JB.GetString(3),
                Nombre = r_235JB.GetString(4),
                Precio = r_235JB.GetInt32(5),
                Disponible = r_235JB.GetBoolean(6),
                Stock = r_235JB.GetInt32(7),
                Act = r_235JB.GetBoolean(8)
            };
        }
    }
}
