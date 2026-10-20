using BE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALMozo
    {
        public DataTable Listar_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(
                "SELECT DNIMozo, Nombre, Apellido, Mail, NumTelefono, Estado, Activo FROM Mozo ORDER BY Apellido, Nombre", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        public Mozo ObtenerPorDNI_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT DNIMozo, Nombre, Apellido, Mail, NumTelefono, Estado, Activo FROM Mozo WHERE DNIMozo = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new Mozo
                    {
                        DNIMozo = r_235JB.GetInt32(0),
                        Nombre = r_235JB.GetString(1),
                        Apellido = r_235JB.GetString(2),
                        Mail = r_235JB.GetString(3),
                        NumTelefono = r_235JB.GetInt32(4),
                        Estado = r_235JB.GetString(5),
                        Activo = r_235JB.GetBoolean(6)
                    };
                }
            }
        }

       
        public void Insertar_235JB(Mozo m_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"INSERT INTO Mozo (DNIMozo, Nombre, Apellido, Mail, NumTelefono, Estado, Activo)
                  VALUES (@dni, @nombre, @apellido, @mail, @telefono, 'Disponible', 1)", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", m_235JB.DNIMozo);
                cmd_235JB.Parameters.AddWithValue("@nombre", m_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@apellido", m_235JB.Apellido);
                cmd_235JB.Parameters.AddWithValue("@mail", m_235JB.Mail);
                cmd_235JB.Parameters.AddWithValue("@telefono", m_235JB.NumTelefono);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        public void Modificar_235JB(Mozo m_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "UPDATE Mozo SET Nombre = @nombre, Apellido = @apellido, Mail = @mail, NumTelefono = @telefono, Activo = @activo WHERE DNIMozo = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", m_235JB.DNIMozo);
                cmd_235JB.Parameters.AddWithValue("@nombre", m_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@apellido", m_235JB.Apellido);
                cmd_235JB.Parameters.AddWithValue("@mail", m_235JB.Mail);
                cmd_235JB.Parameters.AddWithValue("@telefono", m_235JB.NumTelefono);
                cmd_235JB.Parameters.AddWithValue("@activo", m_235JB.Activo);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        public void Desactivar_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mozo SET Activo = 0 WHERE DNIMozo = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }
    }
}