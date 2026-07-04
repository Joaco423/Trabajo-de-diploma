using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; // <-- Agregar este using
using BE;

namespace DAL
{
    public class DALDigitoVerificador
    {
        private string _conn = @"Data Source=localhost;Initial Catalog=CAFECCINO1;Integrated Security=True";

        public DigitoVerificador ObtenerPorTabla(string tabla)
        {
            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT Tabla, DVH, DVV FROM DigitoVerificador WHERE Tabla=@t", cn))
            {
                cmd.Parameters.AddWithValue("@t", tabla);
                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return new DigitoVerificador
                    {
                        Tabla = rd.GetString(0),
                        DVH = rd.GetString(1),
                        DVV = rd.GetString(2)
                    };
                }
            }
        }

        public void Guardar(DigitoVerificador dv)
        {
            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand(@"
            MERGE DigitoVerificador AS target
            USING (SELECT @tabla AS Tabla) AS source ON target.Tabla = source.Tabla
            WHEN MATCHED     THEN UPDATE SET DVH=@dvh, DVV=@dvv
            WHEN NOT MATCHED THEN INSERT (Tabla,DVH,DVV) VALUES (@tabla,@dvh,@dvv);",
                cn))
            {
                cmd.Parameters.AddWithValue("@tabla", dv.Tabla);
                cmd.Parameters.AddWithValue("@dvh", dv.DVH);
                cmd.Parameters.AddWithValue("@dvv", dv.DVV);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}