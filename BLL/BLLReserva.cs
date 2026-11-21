using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    
    public class BLLReserva
    {
        private readonly DALReserva _dal_235JB = new DALReserva();
        private readonly DALMesa _mesaDAL_235JB = new DALMesa();
        private readonly DALCliente _clienteDAL_235JB = new DALCliente();
        private readonly DALPedido _pedidoDAL_235JB = new DALPedido();
        private readonly DALAtencion _atencionDAL_235JB = new DALAtencion();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();
        private readonly GeneradorPDF _pdf_235JB = new GeneradorPDF();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        public DataTable ListarActivas_235JB()
        {
            return _dal_235JB.ListarActivas_235JB();
        }

        
        public DataTable ListarEsperaDeReservas_235JB()
        {
            return _atencionDAL_235JB.ListarEsperaDeReservas_235JB();
        }


        public int GenerarReporteReservasActivas_235JB(string ruta_235JB)
        {
            DataTable reservas_235JB = _dal_235JB.ListarActivas_235JB();
            _pdf_235JB.GenerarReporteReservasActivas_235JB(ruta_235JB, reservas_235JB);

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Reporte de reservas activas generado (" + reservas_235JB.Rows.Count + " reservas)", "Reservas", "Baja");
            return reservas_235JB.Rows.Count;
        }

       
        public void GenerarReporteReserva_235JB(string ruta_235JB, int codigoReserva_235JB)
        {
            DataTable activas_235JB = _dal_235JB.ListarActivas_235JB();
            DataRow[] elegida_235JB = activas_235JB.Select("Reserva = " + codigoReserva_235JB);
            if (elegida_235JB.Length == 0)
                throw new InvalidOperationException("La reserva ya no esta activa.");

            DataTable una_235JB = activas_235JB.Clone();
            una_235JB.ImportRow(elegida_235JB[0]);
            _pdf_235JB.GenerarReporteReserva_235JB(ruta_235JB, una_235JB);

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Reporte de la reserva " + codigoReserva_235JB + " generado", "Reservas", "Baja");
        }

        public Reserva ObtenerActivaPorDNI_235JB(int dni_235JB)
        {
            return _dal_235JB.ObtenerActivaPorDNI_235JB(dni_235JB);
        }

        
        public Reserva Reservar_235JB(Cliente cliente_235JB, int numeroMesa_235JB)
        {
            if (cliente_235JB == null) throw new ArgumentNullException("cliente");

            Cliente actual_235JB = _clienteDAL_235JB.ObtenerPorDNI_235JB(cliente_235JB.DNI);
            if (actual_235JB == null || !actual_235JB.Activo)
                throw new InvalidOperationException("El cliente no esta registrado o esta inactivo.");

            Pedido enCurso_235JB = _pedidoDAL_235JB.ObtenerPedidoEnCurso_235JB(cliente_235JB.DNI);
            if (enCurso_235JB != null)
            {
                InvalidOperationException error_235JB = new InvalidOperationException("msg_PedidoEnCurso");
                error_235JB.Data["CodigoPedido"] = enCurso_235JB.CodigoPedido;
                throw error_235JB;
            }

            Reserva existente_235JB = _dal_235JB.ObtenerActivaPorDNI_235JB(cliente_235JB.DNI);
            if (existente_235JB != null)
            {
                InvalidOperationException error_235JB = new InvalidOperationException("msg_ClienteConReserva");
                error_235JB.Data["NumeroMesa"] = existente_235JB.NumeroMesa;
                throw error_235JB;
            }

            Mesa mesa_235JB = _mesaDAL_235JB.ObtenerPorNumero_235JB(numeroMesa_235JB);
            if (mesa_235JB == null || mesa_235JB.Estado != "Libre")
                throw new InvalidOperationException("msg_MesaNoDisponible");

            DateTime ahora_235JB = DateTime.Now;
            Reserva reserva_235JB = new Reserva
            {
                DNI = cliente_235JB.DNI,
                NumeroMesa = numeroMesa_235JB,
                Fecha = ahora_235JB.Date,
                Hora = ahora_235JB.ToString("HH:mm:ss"),
                Estado = "Activa"
            };
            _dal_235JB.Reservar_235JB(reserva_235JB);

            _integridad_235JB.SincronizarFilasNuevas("Reserva");
            _integridad_235JB.SincronizarFilaPorClave("Mesa", numeroMesa_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Reserva " + reserva_235JB.CodigoReserva + ": mesa " + numeroMesa_235JB + " para el cliente " + cliente_235JB.DNI, "Reservas", "Media");

            
            QuitarDeEsperaDeReserva_235JB(cliente_235JB.DNI);
            return reserva_235JB;
        }

        private void QuitarDeEsperaDeReserva_235JB(int dni_235JB)
        {
            if (!_atencionDAL_235JB.EstaEnListaDeEspera_235JB(true, dni_235JB))
                return;
            
            if (_pedidoDAL_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(dni_235JB) != null)
                return;

            _atencionDAL_235JB.QuitarDeListaDeEspera_235JB(true, dni_235JB);
            _integridad_235JB.SincronizarFilasNuevas("ListaDeEsperaMesas");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Cliente " + dni_235JB + " sale de la lista de espera de mesas (consiguio su reserva)", "Reservas", "Baja");
        }

        
        public Reserva ReservarParaListaDeEspera_235JB(out string nombreCliente_235JB)
        {
            nombreCliente_235JB = null;
            ListaDeEspera primero_235JB = _atencionDAL_235JB.ObtenerPrimeroEnListaDeEsperaDeMesas_235JB(false);
            if (primero_235JB == null)
                return null;
            string nombre_235JB = primero_235JB.Nombre + " " + primero_235JB.Apellido;

            List<Mesa> libres_235JB = _atencionDAL_235JB.ObtenerMesasDisponibles_235JB();
            if (libres_235JB.Count == 0)
                throw new InvalidOperationException("msg_SinMesasLibres");

            Cliente cliente_235JB = _clienteDAL_235JB.ObtenerPorDNI_235JB(primero_235JB.DNI);
            try
            {
                if (cliente_235JB == null)
                    throw new InvalidOperationException("msg_EsperaClienteInvalido");
                Reserva reserva_235JB = Reservar_235JB(cliente_235JB, libres_235JB[0].NumeroMesa);
                nombreCliente_235JB = nombre_235JB;
                return reserva_235JB;
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message != "msg_MesaNoDisponible")
            {
                _atencionDAL_235JB.QuitarDeListaDeEspera_235JB(true, primero_235JB.DNI);
                _integridad_235JB.SincronizarFilasNuevas("ListaDeEsperaMesas");
                ex_235JB.Data["NombreEspera"] = nombre_235JB;
                throw;
            }
        }

       
        public int? UsarReserva_235JB(int dni_235JB)
        {
            Reserva reserva_235JB = _dal_235JB.ObtenerActivaPorDNI_235JB(dni_235JB);
            if (reserva_235JB == null) return null;

            _dal_235JB.Cumplir_235JB(reserva_235JB.CodigoReserva);

            _integridad_235JB.SincronizarFilaPorClave("Reserva", reserva_235JB.CodigoReserva);
            _integridad_235JB.SincronizarFilaPorClave("Mesa", reserva_235JB.NumeroMesa);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Reserva " + reserva_235JB.CodigoReserva + " cumplida: mesa " + reserva_235JB.NumeroMesa + " ocupada por el cliente " + dni_235JB, "Reservas", "Media");
            return reserva_235JB.NumeroMesa;
        }
        
        public void Cancelar_235JB(int codigoReserva_235JB)
        {
            int mesa_235JB = _dal_235JB.Cancelar_235JB(codigoReserva_235JB);

            _integridad_235JB.SincronizarFilaPorClave("Reserva", codigoReserva_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Mesa", mesa_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Reserva " + codigoReserva_235JB + " cancelada (mesa " + mesa_235JB + " liberada)", "Reservas", "Media");
        }
    }
}
