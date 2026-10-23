using BE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALMesa
    {
        public DataTable Listar_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter("SELECT NumeroMesa, Capacidad, Estado FROM Mesa ORDER BY NumeroMesa", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        public Mesa ObtenerPorNumero_235JB(int numeroMesa_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT NumeroMesa, Capacidad, Estado FROM Mesa WHERE NumeroMesa = @n", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@n", numeroMesa_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new Mesa { NumeroMesa = r_235JB.GetInt32(0), Capacidad = r_235JB.GetInt32(1), Estado = r_235JB.GetString(2) };
                }
            }
        }

        // Una mesa nueva queda "Libre" para poder asignarla.
        public void Insertar_235JB(Mesa m_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("INSERT INTO Mesa (NumeroMesa, Capacidad, Estado) VALUES (@n, @capacidad, 'Libre')", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@n", m_235JB.NumeroMesa);
                cmd_235JB.Parameters.AddWithValue("@capacidad", m_235JB.Capacidad);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        // Solo cambia la capacidad: el estado lo manejan la asignacion y la liberacion.
        public void Modificar_235JB(Mesa m_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mesa SET Capacidad = @capacidad WHERE NumeroMesa = @n", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@n", m_235JB.NumeroMesa);
                cmd_235JB.Parameters.AddWithValue("@capacidad", m_235JB.Capacidad);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        // Falla si la mesa tiene atenciones registradas.
        public void Eliminar_235JB(int numeroMesa_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("DELETE FROM Mesa WHERE NumeroMesa = @n", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@n", numeroMesa_235JB);
                cn_235JB.Open();
                try
                {
                    cmd_235JB.ExecuteNonQuery();
                }
                catch (SqlException ex_235JB) when (ex_235JB.Number == 547)
                {
                    throw new InvalidOperationException("No se puede eliminar la mesa porque tiene atenciones registradas.");
                }
            }
        }
    }
}
