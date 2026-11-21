using BE;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    
    public class DALReserva
    {
        private static Reserva Leer_235JB(SqlDataReader r_235JB)
        {
            return new Reserva
            {
                CodigoReserva = r_235JB.GetInt32(0),
                DNI = r_235JB.GetInt32(1),
                NumeroMesa = r_235JB.GetInt32(2),
                Fecha = r_235JB.GetDateTime(3),
                Hora = r_235JB.GetString(4),
                Estado = r_235JB.GetString(5)
            };
        }

        public Reserva ObtenerActivaPorDNI_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT TOP 1 CodigoReserva, DNI, NumeroMesa, Fecha, Hora, Estado FROM Reserva WHERE DNI = @dni AND Estado = 'Activa'", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                    return r_235JB.Read() ? Leer_235JB(r_235JB) : null;
            }
        }

        public Reserva ObtenerPorCodigo_235JB(int codigo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT CodigoReserva, DNI, NumeroMesa, Fecha, Hora, Estado FROM Reserva WHERE CodigoReserva = @c", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@c", codigo_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                    return r_235JB.Read() ? Leer_235JB(r_235JB) : null;
            }
        }


        public DataTable ListarActivas_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(
                @"SELECT r.CodigoReserva AS Reserva, r.DNI, c.Nombre, c.Apellido, r.NumeroMesa AS Mesa, r.Fecha, r.Hora
                  FROM Reserva r
                  INNER JOIN Cliente c ON c.DNI = r.DNI
                  WHERE r.Estado = 'Activa'
                  ORDER BY r.CodigoReserva", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        
        public void Reservar_235JB(Reserva r_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mesa SET Estado = 'Reservada' WHERE NumeroMesa = @n AND Estado = 'Libre'", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@n", r_235JB.NumeroMesa);
                            if (cmd_235JB.ExecuteNonQuery() == 0)
                                throw new InvalidOperationException("La mesa ya no esta libre.");
                        }

                        using (SqlCommand cmd_235JB = new SqlCommand(
                            @"INSERT INTO Reserva (DNI, NumeroMesa, Fecha, Hora, Estado) VALUES (@dni, @n, @fecha, @hora, 'Activa');
                              SELECT CAST(SCOPE_IDENTITY() AS int);", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@dni", r_235JB.DNI);
                            cmd_235JB.Parameters.AddWithValue("@n", r_235JB.NumeroMesa);
                            cmd_235JB.Parameters.AddWithValue("@fecha", r_235JB.Fecha.Date);
                            cmd_235JB.Parameters.AddWithValue("@hora", r_235JB.Hora);
                            r_235JB.CodigoReserva = (int)cmd_235JB.ExecuteScalar();
                        }

                        tx_235JB.Commit();
                    }
                    catch (SqlException ex_235JB) when (ex_235JB.Number == 2601 || ex_235JB.Number == 2627)
                    {
                        tx_235JB.Rollback();
                        throw new InvalidOperationException("El cliente ya tiene una reserva activa.");
                    }
                    catch
                    {
                        tx_235JB.Rollback();
                        throw;
                    }
                }
            }
        }

        
        public void Cumplir_235JB(int codigo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Reserva SET Estado = 'Cumplida' WHERE CodigoReserva = @c AND Estado = 'Activa'", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigo_235JB);
                            if (cmd_235JB.ExecuteNonQuery() == 0)
                                throw new InvalidOperationException("La reserva no esta activa.");
                        }
                        using (SqlCommand cmd_235JB = new SqlCommand(
                            "UPDATE Mesa SET Estado = 'Ocupada' WHERE Estado = 'Reservada' AND NumeroMesa = (SELECT NumeroMesa FROM Reserva WHERE CodigoReserva = @c)", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigo_235JB);
                            cmd_235JB.ExecuteNonQuery();
                        }
                        tx_235JB.Commit();
                    }
                    catch
                    {
                        tx_235JB.Rollback();
                        throw;
                    }
                }
            }
        }
        
        public int Cancelar_235JB(int codigo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                {
                    try
                    {
                        int mesa_235JB;
                        using (SqlCommand cmd_235JB = new SqlCommand("SELECT NumeroMesa, Estado FROM Reserva WITH (UPDLOCK) WHERE CodigoReserva = @c", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigo_235JB);
                            using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                            {
                                if (!r_235JB.Read() || r_235JB.GetString(1) != "Activa")
                                    throw new InvalidOperationException("La reserva no esta activa.");
                                mesa_235JB = r_235JB.GetInt32(0);
                            }
                        }

                        using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Reserva SET Estado = 'Cancelada' WHERE CodigoReserva = @c", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigo_235JB);
                            cmd_235JB.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mesa SET Estado = 'Libre' WHERE NumeroMesa = @n AND Estado = 'Reservada'", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@n", mesa_235JB);
                            cmd_235JB.ExecuteNonQuery();
                        }

                        tx_235JB.Commit();
                        return mesa_235JB;
                    }
                    catch
                    {
                        tx_235JB.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
