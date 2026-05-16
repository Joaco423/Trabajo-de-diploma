using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public class DALUsuario
    {
        public bool Login(Usuario user)
        {
            SqlConnection cn = new SqlConnection(@"Data Source=localhost;Initial Catalog=CAFECCINO1;Integrated Security=True");
            cn.Open();

            string query = "SELECT COUNT(*) FROM Usuario WHERE UserName=@user AND Contraseña=@pass";

            SqlCommand cmd = new SqlCommand(query, cn);

            cmd.Parameters.AddWithValue("@user", user.Nombre);
            cmd.Parameters.AddWithValue("@pass", user.Contraseña);

            int cantidad = Convert.ToInt32(cmd.ExecuteScalar());

            cn.Close();

            return cantidad > 0;
        }
    }
}
