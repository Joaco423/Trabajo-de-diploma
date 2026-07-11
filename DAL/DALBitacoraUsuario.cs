using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class DALBitacoraUsuario
    {
        private string _conn = @"Data Source=localhost;Initial Catalog=CAFECCINO1;Integrated Security=True";

        public void RegistrarCambio(BitacoraUsuario bitacora)
        {
            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand(@"
                INSERT INTO BitacoraUsuario 
                (FechaHora, UsuarioEditor, Operacion, DNIAfectado, DatosAnteriores, DatosNuevos)
                VALUES 
                (GETDATE(), @editor, @operacion, @dni, @anteriores, @nuevos)", cn))
            {
                cmd.Parameters.AddWithValue("@editor", bitacora.UsuarioEditor);
                cmd.Parameters.AddWithValue("@operacion", bitacora.Operacion);
                cmd.Parameters.AddWithValue("@dni", bitacora.DNIAfectado);
                cmd.Parameters.AddWithValue("@anteriores", (object)bitacora.DatosAnteriores ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nuevos", (object)bitacora.DatosNuevos ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable ObtenerCambios()
        {
            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT * FROM BitacoraUsuario ORDER BY FechaHora DESC", cn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public BitacoraUsuario ObtenerPorId(int idBitacora)
        {
            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT * FROM BitacoraUsuario WHERE IdBitacora = @id", cn))
            {
                cmd.Parameters.AddWithValue("@id", idBitacora);
                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return new BitacoraUsuario
                    {
                        IdBitacora = rd.GetInt32(0),
                        FechaHora = rd.GetDateTime(1),
                        UsuarioEditor = rd.GetString(2),
                        Operacion = rd.GetString(3),
                        DNIAfectado = rd.GetInt32(4),
                        DatosAnteriores = rd["DatosAnteriores"] == DBNull.Value ? null : rd.GetString(5),
                        DatosNuevos = rd["DatosNuevos"] == DBNull.Value ? null : rd.GetString(6)
                    };
                }
            }
        }
    }
}
