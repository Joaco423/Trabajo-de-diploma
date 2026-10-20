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
                "SELECT DNI, Username, Contraseña, Nombre, Apellido, Email, NumTelefono, Rol, DVH, Activo, Bloqueado, IntentosFallidos FROM Usuario WHERE DNI = @dni", cn))
            {
                cmd.Parameters.AddWithValue("@dni", dni);
                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return MapearUsuario(rd);
                }
            }
        }
        public Usuario ObtenerPorUsername(string username)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "SELECT DNI, Username, Contraseña, Nombre, Apellido, Email, NumTelefono, Rol, DVH, Activo, Bloqueado, IntentosFallidos FROM Usuario WHERE Username = @username", cn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cn.Open();
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (!rd.Read()) return null;
                    return MapearUsuario(rd);
                }
            }
        }

        private Usuario MapearUsuario(SqlDataReader rd)
        {
            Usuario usuario = new Usuario(
                Convert.ToInt32(rd["DNI"]),
                rd["Nombre"].ToString(),
                rd["Apellido"].ToString(),
                rd["Email"].ToString(),
                Convert.ToInt32(rd["NumTelefono"]),
                rd["Username"].ToString(),
                rd["Contraseña"].ToString(),
                rd["Rol"].ToString(),
                rd["DVH"] == DBNull.Value ? 0 : Convert.ToInt32(rd["DVH"])
            );

            usuario.Activo = Convert.ToBoolean(rd["Activo"]);
            usuario.Bloqueado = Convert.ToBoolean(rd["Bloqueado"]);
            usuario.IntentosFallidos = Convert.ToInt32(rd["IntentosFallidos"]);

            return usuario;
        }

        public void ActualizarContraseña(string username, string nuevaContraseñaHash)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Usuario SET Contraseña = @Contraseña WHERE Username = @Username", cn))
            {
                cmd.Parameters.AddWithValue("@Contraseña", nuevaContraseñaHash);
                cmd.Parameters.AddWithValue("@Username", username);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

      
        public Usuario ObtenerParaLogin(string username)
        {
            string query = @"SELECT u.DNI, u.Username, u.Contraseña, u.Rol, u.Activo, u.Bloqueado, u.IntentosFallidos,
                     up.CodPermiso, p.Nombre as NombrePerfil
                     FROM Usuario u
                     INNER JOIN Usuario_Permiso up ON u.DNI = up.CodUsuario
                     INNER JOIN Permiso p ON up.CodPermiso = p.CodPermiso
                     WHERE u.Username = @User";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@User", username);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario usuarioEncontrado = new Usuario();

                            usuarioEncontrado.DNI = Convert.ToInt32(reader["DNI"]);
                            usuarioEncontrado.NombreUsuario = reader["Username"].ToString();
                            usuarioEncontrado.Contraseña = reader["Contraseña"].ToString();
                            usuarioEncontrado.Rol = reader["Rol"].ToString();
                            usuarioEncontrado.Activo = Convert.ToBoolean(reader["Activo"]);
                            usuarioEncontrado.Bloqueado = Convert.ToBoolean(reader["Bloqueado"]);
                            usuarioEncontrado.IntentosFallidos = Convert.ToInt32(reader["IntentosFallidos"]);

                            usuarioEncontrado.Perfil = new Familia()
                            {
                                Id = Convert.ToInt32(reader["CodPermiso"]),
                                Nombre = reader["NombrePerfil"].ToString()
                            };

                            return usuarioEncontrado;
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
                        Rol,
                        DVH,
                        Activo,
                        Bloqueado,
                        IntentosFallidos
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
                Usuario usuario = new Usuario(
                    Convert.ToInt32(row["DNI"]),
                    row["Nombre"].ToString(),
                    row["Apellido"].ToString(),
                    row["Email"].ToString(),
                    Convert.ToInt32(row["NumTelefono"]),
                    row["Username"].ToString(),
                    row["Contraseña"].ToString(),
                    row["Rol"].ToString(),
                   row["DVH"] == DBNull.Value ? 0 : Convert.ToInt32(row["DVH"])
                );

                usuario.Activo = Convert.ToBoolean(row["Activo"]);
                usuario.Bloqueado = Convert.ToBoolean(row["Bloqueado"]);
                usuario.IntentosFallidos = Convert.ToInt32(row["IntentosFallidos"]);

                lista.Add(usuario);
            }

            return lista;
        }
        public void RegistrarUsuario(Usuario usuario, int idPerfil)
        {
       
            EjecutarComando(
                "InsertarUsuario",
                $"{usuario.DNI}, '{usuario.NombreUsuario}', '{usuario.Contraseña}', '{usuario.Nombre}', '{usuario.Apellido}', '{usuario.Mail}', '{usuario.NumTelefono}', '{usuario.Rol}',{idPerfil}"
            );
        }


        public void ModificarUsuario(Usuario usuario)
        {
            EjecutarComando(
                "ModificarUsuario",
                $"{usuario.DNI}, '{usuario.NombreUsuario}', '{usuario.Contraseña}', '{usuario.Nombre}', '{usuario.Apellido}', '{usuario.Mail}', '{usuario.NumTelefono}', {usuario.Rol}");
        }
        public void EliminarUsuario(int dni)
        {
            EjecutarComando("EliminarUsuario", dni.ToString());
        }

        public void ActualizarActivo(int dni, bool activo)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Usuario SET Activo = @Activo WHERE DNI = @DNI", cn))
            {
                cmd.Parameters.AddWithValue("@Activo", activo);
                cmd.Parameters.AddWithValue("@DNI", dni);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Desbloquear(int dni)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Usuario SET Bloqueado = 0, IntentosFallidos = 0 WHERE DNI = @DNI", cn))
            {
                cmd.Parameters.AddWithValue("@DNI", dni);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        
        public bool RegistrarIntentoFallido(int dni)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(@"
                UPDATE Usuario
                SET IntentosFallidos = IntentosFallidos + 1,
                    Bloqueado = CASE WHEN IntentosFallidos + 1 >= 3 THEN 1 ELSE Bloqueado END
                OUTPUT INSERTED.Bloqueado
                WHERE DNI = @DNI", cn))
            {
                cmd.Parameters.AddWithValue("@DNI", dni);
                cn.Open();
                object resultado = cmd.ExecuteScalar();
                return resultado != null && Convert.ToBoolean(resultado);
            }
        }

        public void ResetearIntentosFallidos(int dni)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Usuario SET IntentosFallidos = 0 WHERE DNI = @DNI", cn))
            {
                cmd.Parameters.AddWithValue("@DNI", dni);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void EjecutarComando(string comando, string parametros)
        {
            cn.Open();

            string query = "EXEC " + comando + " " + parametros;

            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.ExecuteNonQuery();

            cn.Close();
        }
        public void ActualizarDVH(int dni, int dvh)
        {
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Usuario 
              SET DVH = @DVH
              WHERE DNI = @DNI", cn);

                cmd.Parameters.AddWithValue("@DVH", dvh);
                cmd.Parameters.AddWithValue("@DNI", dni);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
