using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
   
    public class BLLPedido
    {
        private readonly DALProducto _productoDAL_235JB = new DALProducto();
        private readonly DALPedido _pedidoDAL_235JB = new DALPedido();
        private readonly DALCliente _clienteDAL_235JB = new DALCliente();
        private readonly DALAtencion _atencionDAL_235JB = new DALAtencion();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        // CU03 pasos 1 y 3
        public List<Producto> ObtenerProductosDisponibles_235JB()
        {
            return _productoDAL_235JB.ListarDisponibles_235JB();
        }

        
        // CU03 pasos 3-6: valida disponibilidad y stock (flujos 4.1 y 4.2 salen como excepciones)
        public int SeleccionarProducto_235JB(List<DetallePedido> detalle_235JB, int codigoProducto_235JB, int cantidad_235JB)
        {
            if (detalle_235JB == null) throw new ArgumentNullException("detalle");
            if (cantidad_235JB <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");

         
            if (!Singleton.Instancia.EsAdministrador() && !_integridad_235JB.ValidarIntegridadGeneral())
                throw new InvalidOperationException("msg_SistemaCaido");

            Producto producto_235JB = _productoDAL_235JB.ObtenerPorCodigo_235JB(codigoProducto_235JB);
            if (producto_235JB == null)
                throw new InvalidOperationException("El producto no existe.");

            // CU03 flujo 4.1
            if (!producto_235JB.Disponible)
            {
                InvalidOperationException error_235JB = new InvalidOperationException("msg_ProductoNoDisponible");
                error_235JB.Data["Nombre"] = producto_235JB.Nombre;
                throw error_235JB;
            }

            DetallePedido existente_235JB = detalle_235JB.FirstOrDefault(d_235JB => d_235JB.Codigo == producto_235JB.Codigo);
            int cantidadTotal_235JB = cantidad_235JB + (existente_235JB != null ? existente_235JB.Cantidad : 0);

            // CU03 flujo 4.2
            if (producto_235JB.Stock < cantidadTotal_235JB)
            {
                InvalidOperationException error_235JB = new InvalidOperationException("msg_StockInsuficiente");
                error_235JB.Data["Nombre"] = producto_235JB.Nombre;
                error_235JB.Data["Stock"] = producto_235JB.Stock;
                throw error_235JB;
            }

            if (existente_235JB != null)
            {
                existente_235JB.Cantidad = cantidadTotal_235JB;
                existente_235JB.Precio = producto_235JB.Precio;
            }
            else
            {
                detalle_235JB.Add(new DetallePedido
                {
                    Codigo = producto_235JB.Codigo,
                    Nombre = producto_235JB.Nombre,
                    Precio = producto_235JB.Precio,
                    Cantidad = cantidad_235JB
                });
            }

            return CalcularImporte_235JB(detalle_235JB);
        }

     
        // CU03 paso 5: subtotales y total
        public int CalcularImporte_235JB(List<DetallePedido> detalle_235JB)
        {
            foreach (DetallePedido d_235JB in detalle_235JB)
                d_235JB.Subtotal = d_235JB.Precio * d_235JB.Cantidad;

            return detalle_235JB.Sum(d_235JB => d_235JB.Subtotal);
        }

       
       
        public bool EstaEnEsperaDeReserva_235JB(int dni_235JB)
        {
            return _atencionDAL_235JB.EstaEnListaDeEspera_235JB(true, dni_235JB)
                && _pedidoDAL_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(dni_235JB) == null;
        }

        // CU04 pasos 3-5: pedido Confirmado + detalle + stock (una transaccion en DAL), luego digito verificador y evento
        public Pedido ConfirmarPedido_235JB(int dni_235JB, List<DetallePedido> detalle_235JB)
        {
            if (detalle_235JB == null || detalle_235JB.Count == 0)
                throw new InvalidOperationException("El pedido no tiene productos.");
            if (_clienteDAL_235JB.ObtenerPorDNI_235JB(dni_235JB) == null)
                throw new InvalidOperationException("El cliente no esta registrado.");

           
            Pedido enCurso_235JB = _pedidoDAL_235JB.ObtenerPedidoEnCurso_235JB(dni_235JB);
            
            if (enCurso_235JB != null)
            {
                InvalidOperationException error_235JB = new InvalidOperationException("msg_PedidoEnCurso");
                error_235JB.Data["CodigoPedido"] = enCurso_235JB.CodigoPedido;
                throw error_235JB;
            }

            if (EstaEnEsperaDeReserva_235JB(dni_235JB))
                throw new InvalidOperationException("msg_EnEsperaDeReserva");

            DateTime ahora_235JB = DateTime.Now;
            string hora_235JB = ahora_235JB.ToString("HH:mm:ss");
            int total_235JB = CalcularImporte_235JB(detalle_235JB);

            Pedido pedido_235JB = new Pedido
            {
                DNI = dni_235JB,
                Fecha = ahora_235JB.Date,
                Hora = hora_235JB,
                Total = total_235JB,
                Estado = "Confirmado",
                Detalles = detalle_235JB
            };

            _pedidoDAL_235JB.ConfirmarPedido_235JB(pedido_235JB);

            _integridad_235JB.SincronizarFilasNuevas("Pedido");
            _integridad_235JB.SincronizarFilasNuevas("DetallePedido");
            foreach (DetallePedido d_235JB in detalle_235JB)
                _integridad_235JB.SincronizarFilaPorClave("Producto", d_235JB.Codigo);
            // El descuento de stock agrega registros en la bitacora de cambios (trigger de Producto)
            _integridad_235JB.SincronizarTabla("Producto_C");
            BLLProducto.AvisarProductosCambiados_235JB();

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Pedido " + pedido_235JB.CodigoPedido + " confirmado. Total " + pedido_235JB.Total, "Pedidos", "Media");

            return pedido_235JB;
        }

       
        // CU05 y CU06 flujo 1.3: devuelve stock, cancela el pedido y quita de la lista (digito verificador + evento)
        public void CancelarPedido_235JB(int codigoPedido_235JB)
        {
            List<DetallePedido> detalle_235JB = _pedidoDAL_235JB.ObtenerDetalle_235JB(codigoPedido_235JB);

            _pedidoDAL_235JB.CancelarPedido_235JB(codigoPedido_235JB);

            foreach (DetallePedido d_235JB in detalle_235JB)
                _integridad_235JB.SincronizarFilaPorClave("Producto", d_235JB.Codigo);
            // La devolucion de stock agrega registros en la bitacora de cambios (trigger de Producto)
            _integridad_235JB.SincronizarTabla("Producto_C");
            BLLProducto.AvisarProductosCambiados_235JB();
            _integridad_235JB.SincronizarFilaPorClave("Pedido", codigoPedido_235JB);
            _integridad_235JB.SincronizarFilasNuevas("ListaDeEsperaMesas");
            _integridad_235JB.SincronizarFilasNuevas("ListaDeEsperaMozos");

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Pedido " + codigoPedido_235JB + " cancelado", "Pedidos", "Alta");
        }

        public Pedido ObtenerPedidoConfirmadoSinAtencion_235JB(int dni_235JB)
        {
            Pedido pedido_235JB = _pedidoDAL_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(dni_235JB);
            if (pedido_235JB != null)
                pedido_235JB.Detalles = _pedidoDAL_235JB.ObtenerDetalle_235JB(pedido_235JB.CodigoPedido);
            return pedido_235JB;
        }

        
        public Pedido ObtenerPedidoEnCurso_235JB(int dni_235JB)
        {
            return _pedidoDAL_235JB.ObtenerPedidoEnCurso_235JB(dni_235JB);
        }

    }
}
