using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALProducto
    {
        public DataTable Listar_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter("SELECT Codigo, Nombre, Precio, Disponible, Stock FROM Producto WHERE Bit_Lo_Bo = 0 ORDER BY Nombre", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

   
        public List<Producto> ListarDisponibles_235JB()
        {
            List<Producto> lista_235JB = new List<Producto>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT Codigo, Nombre, Precio, Disponible, Stock FROM Producto WHERE Disponible = 1 AND Bit_Lo_Bo = 0 ORDER BY Nombre", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read()) lista_235JB.Add(Leer_235JB(r_235JB));
                }
            }
            return lista_235JB;
        }

        public Producto ObtenerPorCodigo_235JB(int codigo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT Codigo, Nombre, Precio, Disponible, Stock FROM Producto WHERE Codigo = @codigo", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@codigo", codigo_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    return r_235JB.Read() ? Leer_235JB(r_235JB) : null;
                }
            }
        }

        
        
        public int? ObtenerCodigoPorNombre_235JB(string nombre_235JB, int codigoExcluido_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT TOP 1 Codigo FROM Producto WHERE LTRIM(RTRIM(Nombre)) = @nombre AND Codigo <> @excluido AND Bit_Lo_Bo = 0 ORDER BY Codigo", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@nombre", nombre_235JB.Trim());
                cmd_235JB.Parameters.AddWithValue("@excluido", codigoExcluido_235JB);
                cn_235JB.Open();
                object resultado_235JB = cmd_235JB.ExecuteScalar();
                return resultado_235JB == null ? (int?)null : Convert.ToInt32(resultado_235JB);
            }
        }

        public int Insertar_235JB(Producto p_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "INSERT INTO Producto (Nombre, Precio, Disponible, Stock) VALUES (@nombre, @precio, @disponible, @stock); SELECT CAST(SCOPE_IDENTITY() AS int);", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@nombre", p_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@precio", p_235JB.Precio);
                cmd_235JB.Parameters.AddWithValue("@disponible", p_235JB.Disponible);
                cmd_235JB.Parameters.AddWithValue("@stock", p_235JB.Stock);
                cn_235JB.Open();
                return (int)cmd_235JB.ExecuteScalar();
            }
        }

        public void ActualizarProducto_235JB(Producto p_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "UPDATE Producto SET Nombre = @nombre, Precio = @precio, Disponible = @disponible, Stock = @stock WHERE Codigo = @codigo", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@codigo", p_235JB.Codigo);
                cmd_235JB.Parameters.AddWithValue("@nombre", p_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@precio", p_235JB.Precio);
                cmd_235JB.Parameters.AddWithValue("@disponible", p_235JB.Disponible);
                cmd_235JB.Parameters.AddWithValue("@stock", p_235JB.Stock);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }


        // Borrado logico: el producto queda en la base (y en sus pedidos y su bitacora) pero deja de aparecer en el sistema.
        // El trigger TR_Producto_RegistrarCambio registra la baja en Producto_C.
        public void Eliminar_235JB(int codigo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Producto SET Bit_Lo_Bo = 1 WHERE Codigo = @codigo AND Bit_Lo_Bo = 0", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@codigo", codigo_235JB);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        private static Producto Leer_235JB(SqlDataReader r_235JB)
        {
            return new Producto
            {
                Codigo = r_235JB.GetInt32(0),
                Nombre = r_235JB.GetString(1),
                Precio = r_235JB.GetInt32(2),
                Disponible = r_235JB.GetBoolean(3),
                Stock = r_235JB.GetInt32(4)
            };
        }
    }
}