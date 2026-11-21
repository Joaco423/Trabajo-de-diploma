using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL
{
    public class DALEvento
    {
        private string conexion { get { return Conexion.cadena; } }
        public void RegistrarEvento(BEEvento evento)
        {
            SqlConnection cn = new SqlConnection(conexion);

            string query = @"INSERT INTO Evento
                        (NombreUsuario, Fecha, Evento, Modulo, Criticidad)
                        VALUES
                        (@NombreUsuario, @Fecha, @Evento, @Modulo, @Criticidad)";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@NombreUsuario", evento.NombreUsuario);
            cmd.Parameters.AddWithValue("@Fecha", evento.Fecha);
            cmd.Parameters.AddWithValue("@Evento", evento.Evento);
            cmd.Parameters.AddWithValue("@Modulo", evento.Modulo);
            cmd.Parameters.AddWithValue("@Criticidad", evento.Criticidad);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        
        
        public List<string> ListarValoresDistintos_235JB(string columna_235JB, string modulo_235JB)
        {
            if (columna_235JB != "Modulo" && columna_235JB != "Criticidad" && columna_235JB != "Evento")
                throw new ArgumentException("Columna no permitida: " + columna_235JB);

            List<string> valores_235JB = new List<string>();
            using (SqlConnection cn_235JB = new SqlConnection(conexion))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT DISTINCT " + columna_235JB + " FROM Evento WHERE " + columna_235JB + " IS NOT NULL AND (@Modulo = '' OR Modulo = @Modulo) ORDER BY " + columna_235JB, cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@Modulo", modulo_235JB ?? "");
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read())
                        valores_235JB.Add(r_235JB.GetString(0));
                }
            }
            return valores_235JB;
        }

        
        
        public DataTable FiltrarEventos_235JB(string modulo_235JB, string criticidad_235JB, string patronEvento_235JB, string fecha_235JB)
        {
            DataTable tabla_235JB = new DataTable();
            using (SqlConnection cn_235JB = new SqlConnection(conexion))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT * FROM Evento
                  WHERE (@Modulo = '' OR Modulo = @Modulo)
                    AND (@Criticidad = '' OR Criticidad = @Criticidad)
                    AND (@Evento = '' OR Evento LIKE @Evento ESCAPE '\')
                    AND (@Fecha = '' OR CONVERT(date, Fecha) = @Fecha)
                  ORDER BY Fecha DESC, CodEvento DESC", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@Modulo", modulo_235JB ?? "");
                cmd_235JB.Parameters.AddWithValue("@Criticidad", criticidad_235JB ?? "");
                cmd_235JB.Parameters.AddWithValue("@Evento", patronEvento_235JB ?? "");
                cmd_235JB.Parameters.AddWithValue("@Fecha", fecha_235JB ?? "");
                using (SqlDataAdapter da_235JB = new SqlDataAdapter(cmd_235JB))
                    da_235JB.Fill(tabla_235JB);
            }
            return tabla_235JB;
        }

        public DataTable ListarEventos()
        {
            SqlConnection cn = new SqlConnection(conexion);

            string query = "SELECT * FROM Evento ORDER BY Fecha DESC, CodEvento DESC";

            SqlDataAdapter da = new SqlDataAdapter(query, cn);

            DataTable dt = new DataTable();

            da.Fill(dt);

            return dt;
        }
        public DataTable FiltrarEventos(string modulo, string criticidad,string evento, string fecha)
        {
            SqlConnection cn = new SqlConnection(conexion);

            string query = @"SELECT *
                     FROM Evento
                     WHERE (@Modulo = '' OR Modulo = @Modulo)
                     AND (@Criticidad = '' OR Criticidad = @Criticidad)  AND (@Fecha = '' OR CONVERT(date, Fecha) = @Fecha)     
                      AND (@Evento = '' OR Evento = @Evento)    
                     ORDER BY Fecha DESC";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@Evento", evento);
            cmd.Parameters.AddWithValue("@Fecha", fecha);
            cmd.Parameters.AddWithValue("@Modulo", modulo);
            cmd.Parameters.AddWithValue("@Criticidad", criticidad);
       

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            da.Fill(dt);

            return dt;
        }
    }
}
