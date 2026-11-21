using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DALAtencion
    {
        private const string TablaMesas_235JB = "ListaDeEsperaMesas";
        private const string TablaMozos_235JB = "ListaDeEsperaMozos";

        
        private static string Tabla_235JB(bool mesas_235JB)
        {
            return mesas_235JB ? TablaMesas_235JB : TablaMozos_235JB;
        }

        
        public List<Mesa> ObtenerMesasDisponibles_235JB()
        {
            List<Mesa> lista_235JB = new List<Mesa>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT NumeroMesa, Capacidad, Estado FROM Mesa WHERE Estado = 'Libre' ORDER BY NumeroMesa", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read())
                        lista_235JB.Add(new Mesa { NumeroMesa = r_235JB.GetInt32(0), Capacidad = r_235JB.GetInt32(1), Estado = r_235JB.GetString(2) });
                }
            }
            return lista_235JB;
        }

        // Mesas "Ocupada" que no figuran en la atencion de ningun pedido en curso
        public List<int> ObtenerMesasOcupadasSinAtencion_235JB()
        {
            List<int> lista_235JB = new List<int>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT m.NumeroMesa FROM Mesa m
                  WHERE m.Estado = 'Ocupada'
                    AND NOT EXISTS (SELECT 1 FROM Atencion a INNER JOIN Pedido p ON p.CodigoPedido = a.CodigoPedido
                                    WHERE a.NumeroMesa = m.NumeroMesa AND p.Estado = 'Confirmado')
                  ORDER BY m.NumeroMesa", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read())
                        lista_235JB.Add(r_235JB.GetInt32(0));
                }
            }
            return lista_235JB;
        }

        public void ActualizarEstadoMesa_235JB(int numeroMesa_235JB, string estado_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mesa SET Estado = @estado WHERE NumeroMesa = @n", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@estado", estado_235JB);
                cmd_235JB.Parameters.AddWithValue("@n", numeroMesa_235JB);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        // Mozos activos que todavia pueden tomar una mesa (atienden menos mesas que su capacidad), primero los que menos atienden
        public List<Mozo> ObtenerMozosDisponibles_235JB(int capacidad_235JB)
        {
            List<Mozo> lista_235JB = new List<Mozo>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT m.DNIMozo, m.Nombre, m.Apellido, m.Mail, m.NumTelefono, m.Estado, x.Mesas
                  FROM Mozo m
                  CROSS APPLY (SELECT COUNT(*) AS Mesas FROM Atencion a INNER JOIN Pedido p ON p.CodigoPedido = a.CodigoPedido
                               WHERE a.DNIMozo = m.DNIMozo AND p.Estado = 'Confirmado') x
                  WHERE m.Activo = 1 AND x.Mesas < @capacidad
                  ORDER BY x.Mesas, m.Apellido, m.Nombre", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@capacidad", capacidad_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read())
                    {
                        lista_235JB.Add(new Mozo
                        {
                            DNIMozo = r_235JB.GetInt32(0),
                            Nombre = r_235JB.GetString(1),
                            Apellido = r_235JB.GetString(2),
                            Mail = r_235JB.GetString(3),
                            NumTelefono = r_235JB.GetInt32(4),
                            Estado = r_235JB.GetString(5),
                            MesasAtendiendo = r_235JB.GetInt32(6)
                        });
                    }
                }
            }
            return lista_235JB;
        }

        // Pedidos en curso (confirmados con atencion) del mozo: las mesas que esta atendiendo
        public int ContarAtencionesEnCurso_235JB(int dniMozo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT COUNT(*) FROM Atencion a INNER JOIN Pedido p ON p.CodigoPedido = a.CodigoPedido
                  WHERE a.DNIMozo = @dni AND p.Estado = 'Confirmado'", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dniMozo_235JB);
                cn_235JB.Open();
                return (int)cmd_235JB.ExecuteScalar();
            }
        }

        public void ActualizarEstadoMozo_235JB(int dniMozo_235JB, string estado_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mozo SET Estado = @estado WHERE DNIMozo = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@estado", estado_235JB);
                cmd_235JB.Parameters.AddWithValue("@dni", dniMozo_235JB);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        // CU06 paso 4: registra la relacion cliente-pedido-mesa-mozo
        public void GuardarAtencion_235JB(Atencion a_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"INSERT INTO Atencion (CodigoPedido, DNI, NumeroMesa, DNIMozo)
                  VALUES (@pedido, @dni, @mesa, @mozo)", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@pedido", a_235JB.CodigoPedido);
                cmd_235JB.Parameters.AddWithValue("@dni", a_235JB.DNI);
                cmd_235JB.Parameters.AddWithValue("@mesa", a_235JB.NumeroMesa);
                cmd_235JB.Parameters.AddWithValue("@mozo", a_235JB.DNIMozo);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        // CU06 paso 5: datos del resumen de atencion
        public ResumenAtencion ObtenerResumen_235JB(int codigoPedido_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT a.CodigoPedido, a.NumeroMesa, m.Capacidad, z.Nombre, z.Apellido
                  FROM Atencion a
                  INNER JOIN Mesa m ON m.NumeroMesa = a.NumeroMesa
                  INNER JOIN Mozo z ON z.DNIMozo = a.DNIMozo
                  WHERE a.CodigoPedido = @c", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new ResumenAtencion
                    {
                        CodigoPedido = r_235JB.GetInt32(0),
                        NumeroMesa = r_235JB.GetInt32(1),
                        Capacidad = r_235JB.GetInt32(2),
                        NombreMozo = r_235JB.GetString(3),
                        ApellidoMozo = r_235JB.GetString(4)
                    };
                }
            }
        }

        
        public bool EstaEnListaDeEspera_235JB(bool mesas_235JB, int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT COUNT(*) FROM " + Tabla_235JB(mesas_235JB) + " WHERE DNI = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                return (int)cmd_235JB.ExecuteScalar() > 0;
            }
        }

        // CU05 y CU06 flujo 1.1: guarda al cliente en la lista de espera
        public void GuardarEnListaDeEspera_235JB(bool mesas_235JB, ListaDeEspera l_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "INSERT INTO " + Tabla_235JB(mesas_235JB) + " (DNI, Nombre, Apellido, Fecha, Hora) VALUES (@dni, @nombre, @apellido, @fecha, @hora)", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", l_235JB.DNI);
                cmd_235JB.Parameters.AddWithValue("@nombre", l_235JB.Nombre);
                cmd_235JB.Parameters.AddWithValue("@apellido", l_235JB.Apellido);
                cmd_235JB.Parameters.AddWithValue("@fecha", l_235JB.Fecha.Date);
                cmd_235JB.Parameters.AddWithValue("@hora", l_235JB.Hora);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        public ListaDeEspera ObtenerPrimeroEnListaDeEspera_235JB(bool mesas_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT TOP 1 DNI, Nombre, Apellido, Fecha, Hora FROM " + Tabla_235JB(mesas_235JB) + " ORDER BY Fecha, Hora", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new ListaDeEspera
                    {
                        DNI = r_235JB.GetInt32(0),
                        Nombre = r_235JB.GetString(1),
                        Apellido = r_235JB.GetString(2),
                        Fecha = r_235JB.GetDateTime(3),
                        Hora = r_235JB.GetString(4)
                    };
                }
            }
        }

        // La lista de espera de mesas la usan dos flujos: clientes con un pedido confirmado que esperan mesa (Gestion de pedido)
        // y clientes que esperan poder reservar una mesa (Gestion de reservas). Se distinguen por si tienen un pedido confirmado
        // sin atencion. Devuelve el primero (por Fecha y Hora) del tipo pedido.
        public ListaDeEspera ObtenerPrimeroEnListaDeEsperaDeMesas_235JB(bool conPedidoPendiente_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT TOP 1 l.DNI, l.Nombre, l.Apellido, l.Fecha, l.Hora FROM ListaDeEsperaMesas l
                  WHERE " + (conPedidoPendiente_235JB ? "" : "NOT ") + @"EXISTS (SELECT 1 FROM Pedido p
                                WHERE p.DNI = l.DNI AND p.Estado = 'Confirmado'
                                  AND NOT EXISTS (SELECT 1 FROM Atencion a WHERE a.CodigoPedido = p.CodigoPedido))
                  ORDER BY l.Fecha, l.Hora", cn_235JB))
            {
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new ListaDeEspera
                    {
                        DNI = r_235JB.GetInt32(0),
                        Nombre = r_235JB.GetString(1),
                        Apellido = r_235JB.GetString(2),
                        Fecha = r_235JB.GetDateTime(3),
                        Hora = r_235JB.GetString(4)
                    };
                }
            }
        }

        // CU05 y CU06 flujos 1.2 y 1.3: quita al cliente de la lista
        public void QuitarDeListaDeEspera_235JB(bool mesas_235JB, int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand("DELETE FROM " + Tabla_235JB(mesas_235JB) + " WHERE DNI = @dni", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                cmd_235JB.ExecuteNonQuery();
            }
        }

        
        public DataTable ListarTodasLasListasDeEspera_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(
                @"SELECT CASE WHEN EXISTS (SELECT 1 FROM Pedido p WHERE p.DNI = l.DNI AND p.Estado = 'Confirmado'
                                           AND NOT EXISTS (SELECT 1 FROM Atencion a WHERE a.CodigoPedido = p.CodigoPedido))
                              THEN 'Mesa' ELSE 'Reserva' END AS Lista, l.DNI, l.Nombre, l.Apellido, l.Fecha, l.Hora
                  FROM ListaDeEsperaMesas l
                  UNION ALL
                  SELECT 'Mozo' AS Lista, DNI, Nombre, Apellido, Fecha, Hora FROM ListaDeEsperaMozos
                  ORDER BY Fecha, Hora", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        
        public DataTable ListarAtencionesEnCurso_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(
                @"SELECT p.CodigoPedido AS Pedido, p.DNI, c.Nombre, c.Apellido, a.NumeroMesa AS Mesa,
                         z.Nombre + ' ' + z.Apellido AS Mozo, p.Total
                  FROM Pedido p
                  INNER JOIN Atencion a ON a.CodigoPedido = p.CodigoPedido
                  INNER JOIN Cliente c ON c.DNI = p.DNI
                  INNER JOIN Mozo z ON z.DNIMozo = a.DNIMozo
                  WHERE p.Estado = 'Confirmado'
                  ORDER BY p.CodigoPedido", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        // Clientes que esperan una mesa para reservar (los que no tienen un pedido confirmado sin atender), en orden de llegada
        public DataTable ListarEsperaDeReservas_235JB()
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(
                @"SELECT l.DNI, l.Nombre, l.Apellido, l.Fecha, l.Hora FROM ListaDeEsperaMesas l
                  WHERE NOT EXISTS (SELECT 1 FROM Pedido p WHERE p.DNI = l.DNI AND p.Estado = 'Confirmado'
                                    AND NOT EXISTS (SELECT 1 FROM Atencion a WHERE a.CodigoPedido = p.CodigoPedido))
                  ORDER BY l.Fecha, l.Hora", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }

        public DataTable ListarListaDeEspera_235JB(bool mesas_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlDataAdapter da_235JB = new SqlDataAdapter(
                "SELECT DNI, Nombre, Apellido, Fecha, Hora FROM " + Tabla_235JB(mesas_235JB) + " ORDER BY Fecha, Hora", cn_235JB))
            {
                DataTable dt_235JB = new DataTable();
                da_235JB.Fill(dt_235JB);
                return dt_235JB;
            }
        }
    }
}
