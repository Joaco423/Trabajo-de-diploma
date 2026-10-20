using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
  
    public class DALIntegridad
    {
        private readonly string _conn = @"Data Source=localhost;Initial Catalog=CAFECCINO1;Integrated Security=True";

        private static readonly Dictionary<string, string[]> ClavesPrimarias = new Dictionary<string, string[]>
        {
            { "Usuario", new[] { "DNI" } },
            { "Permiso", new[] { "CodPermiso" } },
            { "Permiso_Permiso", new[] { "CodPadre", "CodHijo" } },
            { "Usuario_Permiso", new[] { "CodUsuario", "CodPermiso" } },
            { "Idioma", new[] { "IdIdioma" } },
            { "Traduccion", new[] { "IdTraduccion" } },
            { "ControlIdioma", new[] { "IdControl" } },
            { "Evento", new[] { "CodEvento" } },
            { "BitacoraUsuario", new[] { "IdBitacora" } },
            { "Cliente", new[] { "DNI" } },
            { "Producto", new[] { "Codigo" } },
            { "Mesa", new[] { "NumeroMesa" } },
            { "Mozo", new[] { "DNIMozo" } },
            { "Pedido", new[] { "CodigoPedido" } },
            { "DetallePedido", new[] { "CodigoPedido", "Codigo" } },
            { "Factura", new[] { "NumeroFactura" } },
            { "Atencion", new[] { "CodigoPedido" } },
            { "ListaDeEsperaMesas", new[] { "DNI" } },
            { "ListaDeEsperaMozos", new[] { "DNI" } },
        };

        public IReadOnlyList<string> ObtenerTablasControladas()
        {
            return ClavesPrimarias.Keys.ToList();
        }

        public string[] ObtenerClavePrimaria(string tabla)
        {
            return ClavesPrimarias[tabla];
        }

        public DataTable ObtenerFilas(string tabla)
        {
            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand($"SELECT * FROM [{tabla}]", cn))
            {
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                cn.Open();
                da.Fill(dt);
                return dt;
            }
        }

        public void ActualizarDVH(string tabla, string[] clavePrimaria, DataRow fila, int dvh)
        {
            string where = string.Join(" AND ", clavePrimaria.Select((c, i) => $"[{c}] = @pk{i}"));

            using (SqlConnection cn = new SqlConnection(_conn))
            using (SqlCommand cmd = new SqlCommand($"UPDATE [{tabla}] SET DVH = @dvh WHERE {where}", cn))
            {
                cmd.Parameters.AddWithValue("@dvh", dvh);
                for (int i = 0; i < clavePrimaria.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@pk{i}", fila[clavePrimaria[i]]);
                }
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
