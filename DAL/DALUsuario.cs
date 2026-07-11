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
    public class DALUsuario
    {
        private readonly string connectionString =
        @"Data Source=localhost;Initial Catalog=CAFECCINO1;Integrated Security=True";

        private readonly SqlConnection cn;

        public DALUsuario()
        {
            cn = new SqlConnection(connectionString);
        }
        public Usuario ObtenerPorDNI(int dni)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT DNI, Username, Contraseña, Nombre, Apellido, Email, NumTelefono, Rol FROM Usuario WHERE DNI = @dni", cn))
            {
                cmd.Parameters.AddWithValue("@dni", dni);
                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return new Usuario(
                        Convert.ToInt32(rd["DNI"]),
                        rd["Nombre"].ToString(),
                        rd["Apellido"].ToString(),
                        rd["Email"].ToString(),
                        Convert.ToInt32(rd["NumTelefono"]),
                        rd["Username"].ToString(),
                        rd["Contraseña"].ToString(),
                        rd["Rol"].ToString()
                    );
                }
            }
        }
        public Usuario Login(Usuario user)
        {
            string query = @"SELECT u.DNI, u.Username, up.CodPermiso, p.Nombre as NombrePerfil
                     FROM Usuario u
                     INNER JOIN Usuario_Permiso up ON u.DNI = up.CodUsuario
                     INNER JOIN Permiso p ON up.CodPermiso = p.CodPermiso
                     WHERE u.Username = @User AND u.Contraseña = @Pass";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@User", user.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Pass", user.Contraseña);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario usuarioValido = new Usuario();

                            
                            usuarioValido.DNI = Convert.ToInt32(reader["DNI"]);
                            usuarioValido.NombreUsuario = reader["Username"].ToString();

                            
                            usuarioValido.Perfil = new Familia()
                            {
                                Id = Convert.ToInt32(reader["CodPermiso"]),
                                Nombre = reader["NombrePerfil"].ToString()
                            };

                            return usuarioValido;
                        }
                    }
                }
            }
            return null; 
        }



        public DataTable ListarUsuarios()
        {
            cn.Open();
            string query = @"SELECT DNI,
                        Username,
                        Contraseña,
                        Nombre,
                        Apellido,
                        Email,
                        NumTelefono,
                        Rol
                     FROM Usuario";
            SqlDataAdapter da = new SqlDataAdapter(query, cn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cn.Close();
            return dt;
        }

        
        public List<Usuario> ObtenerTodos()
        {
            DataTable dt = ListarUsuarios();
            List<Usuario> lista = new List<Usuario>();

            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new Usuario(
                    Convert.ToInt32(row["DNI"]),
                    row["Nombre"].ToString(),
                    row["Apellido"].ToString(),
                    row["Email"].ToString(),
                    Convert.ToInt32(row["NumTelefono"]),
                    row["Username"].ToString(),
                    row["Contraseña"].ToString(),
                    row["Rol"].ToString()
                ));
            }

            return lista;
        }
        public void RegistrarUsuario(Usuario usuario, int idPerfil)
        {
       
            EjecutarComando(
                "InsertarUsuario",
                $"{usuario.DNI}, '{usuario.NombreUsuario}', '{usuario.Contraseña}', '{usuario.Nombre}', '{usuario.Apellido}', '{usuario.Mail}', '{usuario.NumTelefono}', '{usuario.Rol}', {idPerfil}"
            );
        }


        public void ModificarUsuario(Usuario usuario)
        {
            EjecutarComando(
                "ModificarUsuario",
                $"{usuario.DNI}, '{usuario.NombreUsuario}', '{usuario.Contraseña}', '{usuario.Nombre}', '{usuario.Apellido}', '{usuario.Mail}', '{usuario.NumTelefono}', '{usuario.Rol}'");
        }
        public void EliminarUsuario(int dni)
        {
            EjecutarComando("EliminarUsuario", dni.ToString());
        }
        public void EjecutarComando(string comando, string parametros)
        {
            cn.Open();

            string query = "EXEC " + comando + " " + parametros;

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }

        
    }
}
