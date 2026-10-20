using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class DALPedido
    {
        
        // CU04 pasos 3-5: guarda pedido, descuenta stock y genera factura en una transaccion
        public void ConfirmarPedido_235JB(Pedido pedido_235JB, Factura factura_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                {
                    try
                    {
                        VerificarSinPedidoEnCurso_235JB(cn_235JB, tx_235JB, pedido_235JB.DNI);
                        GuardarPedido_235JB(cn_235JB, tx_235JB, pedido_235JB);
                        DescontarStock_235JB(cn_235JB, tx_235JB, pedido_235JB.Detalles);
                        GuardarFactura_235JB(cn_235JB, tx_235JB, pedido_235JB, factura_235JB);
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

        
      
        private void VerificarSinPedidoEnCurso_235JB(SqlConnection cn_235JB, SqlTransaction tx_235JB, int dni_235JB)
        {
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT COUNT(*) FROM Pedido WITH (UPDLOCK, HOLDLOCK) WHERE DNI = @dni AND Estado = 'Confirmado'", cn_235JB, tx_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                if ((int)cmd_235JB.ExecuteScalar() > 0)
                    throw new InvalidOperationException("El cliente ya tiene un pedido en curso.");
            }
        }

       
        public Pedido ObtenerPedidoEnCurso_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT TOP 1 CodigoPedido, DNI, Fecha, Hora, Total, Estado FROM Pedido
                  WHERE DNI = @dni AND Estado = 'Confirmado' ORDER BY CodigoPedido DESC", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new Pedido
                    {
                        CodigoPedido = r_235JB.GetInt32(0),
                        DNI = r_235JB.GetInt32(1),
                        Fecha = r_235JB.GetDateTime(2),
                        Hora = r_235JB.GetString(3),
                        Total = r_235JB.GetInt32(4),
                        Estado = r_235JB.GetString(5)
                    };
                }
            }
        }

       

        
        // Fin de la atencion: pedido Finalizado y libera mesa y mozo
        public void FinalizarAtencion_235JB(int codigoPedido_235JB, out int numeroMesa_235JB, out int dniMozo_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand cmd_235JB = new SqlCommand(
                            @"SELECT a.NumeroMesa, a.DNIMozo, p.Estado FROM Atencion a
                              INNER JOIN Pedido p ON p.CodigoPedido = a.CodigoPedido
                              WHERE a.CodigoPedido = @c", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                            using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                            {
                                if (!r_235JB.Read())
                                    throw new InvalidOperationException("El pedido no tiene una atencion registrada.");
                                numeroMesa_235JB = r_235JB.GetInt32(0);
                                dniMozo_235JB = r_235JB.GetInt32(1);
                                if (r_235JB.GetString(2) != "Confirmado")
                                    throw new InvalidOperationException("La atencion ya fue finalizada.");
                            }
                        }

                        Ejecutar_235JB(cn_235JB, tx_235JB, "UPDATE Pedido SET Estado = 'Finalizado' WHERE CodigoPedido = @c", codigoPedido_235JB, 0);
                        using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mesa SET Estado = 'Libre' WHERE NumeroMesa = @n", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@n", numeroMesa_235JB);
                            cmd_235JB.ExecuteNonQuery();
                        }
                        using (SqlCommand cmd_235JB = new SqlCommand("UPDATE Mozo SET Estado = 'Disponible' WHERE DNIMozo = @d", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@d", dniMozo_235JB);
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

        private void GuardarPedido_235JB(SqlConnection cn_235JB, SqlTransaction tx_235JB, Pedido pedido_235JB)
        {
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"INSERT INTO Pedido (DNI, Fecha, Hora, Total, Estado)
                  VALUES (@dni, @fecha, @hora, @total, @estado);
                  SELECT CAST(SCOPE_IDENTITY() AS int);", cn_235JB, tx_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", pedido_235JB.DNI);
                cmd_235JB.Parameters.AddWithValue("@fecha", pedido_235JB.Fecha.Date);
                cmd_235JB.Parameters.AddWithValue("@hora", pedido_235JB.Hora);
                cmd_235JB.Parameters.AddWithValue("@total", pedido_235JB.Total);
                cmd_235JB.Parameters.AddWithValue("@estado", pedido_235JB.Estado);
                pedido_235JB.CodigoPedido = (int)cmd_235JB.ExecuteScalar();
            }

            foreach (DetallePedido d_235JB in pedido_235JB.Detalles)
            {
                d_235JB.CodigoPedido = pedido_235JB.CodigoPedido;
                using (SqlCommand cmd_235JB = new SqlCommand(
                    "INSERT INTO DetallePedido (CodigoPedido, Codigo, Cantidad, Subtotal) VALUES (@pedido, @codigo, @cantidad, @subtotal)", cn_235JB, tx_235JB))
                {
                    cmd_235JB.Parameters.AddWithValue("@pedido", d_235JB.CodigoPedido);
                    cmd_235JB.Parameters.AddWithValue("@codigo", d_235JB.Codigo);
                    cmd_235JB.Parameters.AddWithValue("@cantidad", d_235JB.Cantidad);
                    cmd_235JB.Parameters.AddWithValue("@subtotal", d_235JB.Subtotal);
                    cmd_235JB.ExecuteNonQuery();
                }
            }
        }

        // CU04 paso 4: descuenta el stock
        private void DescontarStock_235JB(SqlConnection cn_235JB, SqlTransaction tx_235JB, List<DetallePedido> detalles_235JB)
        {
            foreach (DetallePedido d_235JB in detalles_235JB)
            {
                using (SqlCommand cmd_235JB = new SqlCommand(
                    "UPDATE Producto SET Stock = Stock - @cantidad WHERE Codigo = @codigo AND Stock >= @cantidad", cn_235JB, tx_235JB))
                {
                    cmd_235JB.Parameters.AddWithValue("@cantidad", d_235JB.Cantidad);
                    cmd_235JB.Parameters.AddWithValue("@codigo", d_235JB.Codigo);
                    if (cmd_235JB.ExecuteNonQuery() == 0)
                        throw new InvalidOperationException("No hay stock suficiente del producto " + d_235JB.Nombre + " para confirmar el pedido.");
                }
            }
        }

        // CU04 paso 5: genera y guarda la factura
        private void GuardarFactura_235JB(SqlConnection cn_235JB, SqlTransaction tx_235JB, Pedido pedido_235JB, Factura factura_235JB)
        {
            factura_235JB.CodigoPedido = pedido_235JB.CodigoPedido;
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"INSERT INTO Factura (CodigoPedido, DNI, Fecha, Hora, Total, Estado)
                  VALUES (@pedido, @dni, @fecha, @hora, @total, @estado);
                  SELECT CAST(SCOPE_IDENTITY() AS int);", cn_235JB, tx_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@pedido", factura_235JB.CodigoPedido);
                cmd_235JB.Parameters.AddWithValue("@dni", factura_235JB.DNI);
                cmd_235JB.Parameters.AddWithValue("@fecha", factura_235JB.Fecha.Date);
                cmd_235JB.Parameters.AddWithValue("@hora", factura_235JB.Hora);
                cmd_235JB.Parameters.AddWithValue("@total", factura_235JB.Total);
                cmd_235JB.Parameters.AddWithValue("@estado", factura_235JB.Estado);
                factura_235JB.NumeroFactura = (int)cmd_235JB.ExecuteScalar();
            }
        }

   
        // CU05 y CU06 flujo 1.3: pedido Cancelado, devuelve stock, factura Anulada, quita de las listas
        public void CancelarPedido_235JB(int codigoPedido_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                {
                    try
                    {
                        int dni_235JB;
                        string estado_235JB;
                        using (SqlCommand cmd_235JB = new SqlCommand("SELECT DNI, Estado FROM Pedido WHERE CodigoPedido = @c", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                            using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                            {
                                if (!r_235JB.Read()) throw new InvalidOperationException("El pedido no existe.");
                                dni_235JB = r_235JB.GetInt32(0);
                                estado_235JB = r_235JB.GetString(1);
                            }
                        }

                        if (estado_235JB != "Confirmado")
                            throw new InvalidOperationException("Solo se puede cancelar un pedido confirmado.");

                        using (SqlCommand cmd_235JB = new SqlCommand("SELECT COUNT(*) FROM Atencion WHERE CodigoPedido = @c", cn_235JB, tx_235JB))
                        {
                            cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                            if ((int)cmd_235JB.ExecuteScalar() > 0)
                                throw new InvalidOperationException("El pedido ya tiene una atencion registrada y no puede cancelarse.");
                        }

                        Ejecutar_235JB(cn_235JB, tx_235JB,
                            @"UPDATE p SET p.Stock = p.Stock + d.Cantidad
                              FROM Producto p INNER JOIN DetallePedido d ON d.Codigo = p.Codigo
                              WHERE d.CodigoPedido = @c", codigoPedido_235JB, dni_235JB);
                        Ejecutar_235JB(cn_235JB, tx_235JB, "UPDATE Factura SET Estado = 'Anulada' WHERE CodigoPedido = @c", codigoPedido_235JB, dni_235JB);
                        Ejecutar_235JB(cn_235JB, tx_235JB, "UPDATE Pedido SET Estado = 'Cancelado' WHERE CodigoPedido = @c", codigoPedido_235JB, dni_235JB);
                        Ejecutar_235JB(cn_235JB, tx_235JB, "DELETE FROM ListaDeEsperaMesas WHERE DNI = @dni", codigoPedido_235JB, dni_235JB);
                        Ejecutar_235JB(cn_235JB, tx_235JB, "DELETE FROM ListaDeEsperaMozos WHERE DNI = @dni", codigoPedido_235JB, dni_235JB);

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

        private static void Ejecutar_235JB(SqlConnection cn_235JB, SqlTransaction tx_235JB, string sql_235JB, int codigoPedido_235JB, int dni_235JB)
        {
            using (SqlCommand cmd_235JB = new SqlCommand(sql_235JB, cn_235JB, tx_235JB))
            {
                if (sql_235JB.Contains("@c")) cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                if (sql_235JB.Contains("@dni")) cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cmd_235JB.ExecuteNonQuery();
            }
        }

        public List<DetallePedido> ObtenerDetalle_235JB(int codigoPedido_235JB)
        {
            List<DetallePedido> lista_235JB = new List<DetallePedido>();
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT d.CodigoPedido, d.Codigo, p.Nombre, p.Precio, d.Cantidad, d.Subtotal
                  FROM DetallePedido d INNER JOIN Producto p ON p.Codigo = d.Codigo
                  WHERE d.CodigoPedido = @c", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    while (r_235JB.Read())
                    {
                        lista_235JB.Add(new DetallePedido
                        {
                            CodigoPedido = r_235JB.GetInt32(0),
                            Codigo = r_235JB.GetInt32(1),
                            Nombre = r_235JB.GetString(2),
                            Precio = r_235JB.GetInt32(3),
                            Cantidad = r_235JB.GetInt32(4),
                            Subtotal = r_235JB.GetInt32(5)
                        });
                    }
                }
            }
            return lista_235JB;
        }

        public Factura ObtenerFacturaPorPedido_235JB(int codigoPedido_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT NumeroFactura, CodigoPedido, DNI, Fecha, Hora, Total, Estado FROM Factura WHERE CodigoPedido = @c", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@c", codigoPedido_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new Factura
                    {
                        NumeroFactura = r_235JB.GetInt32(0),
                        CodigoPedido = r_235JB.GetInt32(1),
                        DNI = r_235JB.GetInt32(2),
                        Fecha = r_235JB.GetDateTime(3),
                        Hora = r_235JB.GetString(4),
                        Total = r_235JB.GetInt32(5),
                        Estado = r_235JB.GetString(6)
                    };
                }
            }
        }

        
        public Pedido ObtenerPedidoConfirmadoSinAtencion_235JB(int dni_235JB)
        {
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            using (SqlCommand cmd_235JB = new SqlCommand(
                @"SELECT TOP 1 CodigoPedido, DNI, Fecha, Hora, Total, Estado FROM Pedido
                  WHERE DNI = @dni AND Estado = 'Confirmado'
                    AND CodigoPedido NOT IN (SELECT CodigoPedido FROM Atencion)
                  ORDER BY CodigoPedido DESC", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@dni", dni_235JB);
                cn_235JB.Open();
                using (SqlDataReader r_235JB = cmd_235JB.ExecuteReader())
                {
                    if (!r_235JB.Read()) return null;
                    return new Pedido
                    {
                        CodigoPedido = r_235JB.GetInt32(0),
                        DNI = r_235JB.GetInt32(1),
                        Fecha = r_235JB.GetDateTime(2),
                        Hora = r_235JB.GetString(3),
                        Total = r_235JB.GetInt32(4),
                        Estado = r_235JB.GetString(5)
                    };
                }
            }
        }
    }
}
