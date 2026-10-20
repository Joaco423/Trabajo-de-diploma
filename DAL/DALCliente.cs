using BE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALCliente
    {
        public DataTable Listar_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter("SELECT DNI, Nombre, Apellido, Mail, NumTelefono, Domicilio, Activo FROM Cliente ORDER BY Apellido, Nombre", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        public Cliente ObtenerPorDNI_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT DNI, Nombre, Apellido, Mail, NumTelefono, Domicilio, Activo FROM Cliente WHERE DNI = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new Cliente { DNI = r_235JB.GetInt32(0), Nombre = r_235JB.GetString(1), Apellido = r_235JB.GetString(2), Mail = r_235JB.GetString(3), NumTelefono = r_235JB.GetInt32(4), Domicilio = r_235JB.GetString(5), Activo = r_235JB.GetBoolean(6) };
                }
            }
        }

        public void Insertar_235JB(Cliente c_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("INSERT INTO Cliente (DNI, Nombre, Apellido, Mail, NumTelefono, Domicilio, Activo) VALUES (@dni, @nombre, @apellido, @mail, @telefono, @domicilio, 1)", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", c_235JB.DNI);
                cmd_235JB.Parameters.AddWithValue("@nombre", c_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@apellido", c_235JB.Apellido);
                cmd_235JB.Parameters.AddWithValue("@mail", c_235JB.Mail);
                cmd_235JB.Parameters.AddWithValue("@telefono", c_235JB.NumTelefono);
                cmd_235JB.Parameters.AddWithValue("@domicilio", c_235JB.Domicilio);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        public void Modificar_235JB(Cliente c_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Cliente SET Nombre = @nombre, Apellido = @apellido, Mail = @mail, NumTelefono = @telefono, Domicilio = @domicilio, Activo = @activo WHERE DNI = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", c_235JB.DNI);
                cmd_235JB.Parameters.AddWithValue("@nombre", c_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@apellido", c_235JB.Apellido);
                cmd_235JB.Parameters.AddWithValue("@mail", c_235JB.Mail);
                cmd_235JB.Parameters.AddWithValue("@telefono", c_235JB.NumTelefono);
                cmd_235JB.Parameters.AddWithValue("@domicilio", c_235JB.Domicilio);
                cmd_235JB.Parameters.AddWithValue("@activo", c_235JB.Activo);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        public void Desactivar_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Cliente SET Activo = 0 WHERE DNI = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }
    }
}