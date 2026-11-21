using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
   
    public class BLLAtencion
    {
        private readonly DALAtencion _dal_235JB = new DALAtencion();
        private readonly DALPedido _pedidoDAL_235JB = new DALPedido();
        private readonly DALReserva _reservaDAL_235JB = new DALReserva();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();
        private readonly GeneradorPDF _pdf_235JB = new GeneradorPDF();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        private static string TablaLista_235JB(bool mesas_235JB)
        {
            return mesas_235JB ? "ListaDeEsperaMesas" : "ListaDeEsperaMozos";
        }

       
        
        public List<Mesa> ObtenerMesasDisponibles_235JB()
        {
            return _dal_235JB.ObtenerMesasDisponibles_235JB();
        }

        
        
        public List<Mesa> ConsultarDisponibilidadMesas_235JB()
        {
            List<Mesa> mesas_235JB = _dal_235JB.ObtenerMesasDisponibles_235JB();
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Consulta de disponibilidad de mesas (" + mesas_235JB.Count + " libres)", "Reservas", "Baja");
            return mesas_235JB;
        }
        public void AsignarMesa_235JB(int numeroMesa_235JB)
        {
            if (!_dal_235JB.ObtenerMesasDisponibles_235JB().Any(m_235JB => m_235JB.NumeroMesa == numeroMesa_235JB))
                throw new InvalidOperationException("La mesa seleccionada ya no esta disponible.");

            _dal_235JB.ActualizarEstadoMesa_235JB(numeroMesa_235JB, "Ocupada");
            _integridad_235JB.SincronizarFilaPorClave("Mesa", numeroMesa_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Mesa " + numeroMesa_235JB + " asignada", "Pedidos", "Media");
        }

        
        public List<int> LiberarMesasHuerfanas_235JB(IDictionary<int, int> mesaPorPedidoEnMemoria_235JB)
        {
            List<int> liberadas_235JB = new List<int>();

            int pendientesConMesaPerdida_235JB = _pedidoDAL_235JB.ObtenerCodigosPedidosSinAtencion_235JB()
                .Count(codigo_235JB => !mesaPorPedidoEnMemoria_235JB.ContainsKey(codigo_235JB));
            if (pendientesConMesaPerdida_235JB > 0)
                return liberadas_235JB;

            foreach (int mesa_235JB in _dal_235JB.ObtenerMesasOcupadasSinAtencion_235JB())
            {
                if (mesaPorPedidoEnMemoria_235JB.Values.Contains(mesa_235JB))
                    continue;

                LiberarMesa_235JB(mesa_235JB);
                liberadas_235JB.Add(mesa_235JB);
            }
            return liberadas_235JB;
        }

        public ResumenAtencion FinalizarAtencion_235JB(int codigoPedido_235JB)
        {
            ResumenAtencion resumen_235JB = _dal_235JB.ObtenerResumen_235JB(codigoPedido_235JB);

            int numeroMesa_235JB, dniMozo_235JB;
            _pedidoDAL_235JB.FinalizarAtencion_235JB(codigoPedido_235JB, out numeroMesa_235JB, out dniMozo_235JB);

            _integridad_235JB.SincronizarFilaPorClave("Pedido", codigoPedido_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Mesa", numeroMesa_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Mozo", dniMozo_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Atencion del pedido " + codigoPedido_235JB + " finalizada (mesa " + numeroMesa_235JB + " liberada; el mozo " + dniMozo_235JB + " puede tomar otra mesa)", "Pedidos", "Media");

            return resumen_235JB;
        }

        
        public void LiberarMesa_235JB(int numeroMesa_235JB)
        {
            _dal_235JB.ActualizarEstadoMesa_235JB(numeroMesa_235JB, "Libre");
            _integridad_235JB.SincronizarFilaPorClave("Mesa", numeroMesa_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Mesa " + numeroMesa_235JB + " liberada", "Pedidos", "Media");
        }

       
        public ListaDeEspera AsignarMesaDeListaDeEspera_235JB(int numeroMesa_235JB)
        {
            
            ListaDeEspera primero_235JB = _dal_235JB.ObtenerPrimeroEnListaDeEsperaDeMesas_235JB(true);
            if (primero_235JB == null) return null;

            AsignarMesa_235JB(numeroMesa_235JB);
            _dal_235JB.QuitarDeListaDeEspera_235JB(true, primero_235JB.DNI);
            _integridad_235JB.SincronizarFilasNuevas(TablaLista_235JB(true));
            return primero_235JB;
        }

       
        
        public const int CapacidadMozo_235JB = 3;

        // CU06 paso 1: mozos que todavia pueden tomar una mesa (atienden menos de CapacidadMozo_235JB), primero los que menos atienden
        public List<Mozo> ObtenerMozosDisponibles_235JB()
        {
            return _dal_235JB.ObtenerMozosDisponibles_235JB(CapacidadMozo_235JB);
        }

        
        public List<Mozo> ObtenerMozosParaAsignar_235JB(bool conPrioridad_235JB)
        {
            List<Mozo> libres_235JB = ObtenerMozosDisponibles_235JB();
            if (conPrioridad_235JB) return libres_235JB;

            int lugaresLibres_235JB = libres_235JB.Sum(m_235JB => CapacidadMozo_235JB - m_235JB.MesasAtendiendo);
            int reservasActivas_235JB = _reservaDAL_235JB.ListarActivas_235JB().Rows.Count;
            return lugaresLibres_235JB > reservasActivas_235JB ? libres_235JB : new List<Mozo>();
        }

        public ResumenAtencion AsignarMozo_235JB(int dniMozo_235JB, Pedido pedido_235JB, int numeroMesa_235JB)
        {
            return AsignarMozo_235JB(dniMozo_235JB, pedido_235JB, numeroMesa_235JB, false);
        }

        public ResumenAtencion AsignarMozo_235JB(int dniMozo_235JB, Pedido pedido_235JB, int numeroMesa_235JB, bool conPrioridad_235JB)
        {
            if (!ObtenerMozosParaAsignar_235JB(conPrioridad_235JB).Any(m_235JB => m_235JB.DNIMozo == dniMozo_235JB))
                throw new InvalidOperationException("El mozo seleccionado ya no esta disponible.");

            _dal_235JB.GuardarAtencion_235JB(new Atencion
            {
                CodigoPedido = pedido_235JB.CodigoPedido,
                DNI = pedido_235JB.DNI,
                NumeroMesa = numeroMesa_235JB,
                DNIMozo = dniMozo_235JB
            });
            _integridad_235JB.SincronizarFilasNuevas("Atencion");

            
            int mesasAtendiendo_235JB = _dal_235JB.ContarAtencionesEnCurso_235JB(dniMozo_235JB);
            if (mesasAtendiendo_235JB >= CapacidadMozo_235JB)
            {
                _dal_235JB.ActualizarEstadoMozo_235JB(dniMozo_235JB, "Ocupado");
                _integridad_235JB.SincronizarFilaPorClave("Mozo", dniMozo_235JB);
            }

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Mozo " + dniMozo_235JB + " asignado al pedido " + pedido_235JB.CodigoPedido + " (atiende " + mesasAtendiendo_235JB + " de " + CapacidadMozo_235JB + " mesas)", "Pedidos", "Media");

            return _dal_235JB.ObtenerResumen_235JB(pedido_235JB.CodigoPedido);
        }

        
        public void GenerarReporteAtencion_235JB(string ruta_235JB, Cliente cliente_235JB, Pedido pedido_235JB, ResumenAtencion resumen_235JB)
        {
            _pdf_235JB.GenerarReporteAtencion_235JB(ruta_235JB, cliente_235JB, pedido_235JB, resumen_235JB);

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Reporte de atencion del pedido " + pedido_235JB.CodigoPedido + " generado", "Pedidos", "Baja");
        }

        public ListaDeEspera ObtenerPrimeroEnListaDeEspera_235JB(bool mesas_235JB)
        {
            return mesas_235JB
                ? _dal_235JB.ObtenerPrimeroEnListaDeEsperaDeMesas_235JB(true)
                : _dal_235JB.ObtenerPrimeroEnListaDeEspera_235JB(false);
        }

        public void QuitarDeListaDeEspera_235JB(bool mesas_235JB, int dni_235JB)
        {
            _dal_235JB.QuitarDeListaDeEspera_235JB(mesas_235JB, dni_235JB);
            _integridad_235JB.SincronizarFilasNuevas(TablaLista_235JB(mesas_235JB));
        }

        
        // CU05 y CU06 flujo 1.1: alta en la lista de espera (digito verificador + evento)
        public void RegistrarEnListaDeEspera_235JB(bool mesas_235JB, Cliente cliente_235JB)
        {
            if (_dal_235JB.EstaEnListaDeEspera_235JB(mesas_235JB, cliente_235JB.DNI))
                throw new InvalidOperationException("El cliente ya esta en la lista de espera.");

            DateTime ahora_235JB = DateTime.Now;
            _dal_235JB.GuardarEnListaDeEspera_235JB(mesas_235JB, new ListaDeEspera
            {
                DNI = cliente_235JB.DNI,
                Nombre = cliente_235JB.Nombre,
                Apellido = cliente_235JB.Apellido,
                Fecha = ahora_235JB.Date,
                Hora = ahora_235JB.ToString("HH:mm:ss")
            });

            _integridad_235JB.SincronizarFilasNuevas(TablaLista_235JB(mesas_235JB));
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Cliente " + cliente_235JB.DNI + " en lista de espera de " + (mesas_235JB ? "mesas" : "mozos"), "Pedidos", "Baja");
        }

       
        // CU01: grilla de lista de espera de la pantalla Gestion de pedido
        public System.Data.DataTable ListarClientesEnEspera_235JB()
        {
            return _dal_235JB.ListarTodasLasListasDeEspera_235JB();
        }

        
        public System.Data.DataTable ListarAtencionesEnCurso_235JB()
        {
            return _dal_235JB.ListarAtencionesEnCurso_235JB();
        }

        public System.Data.DataTable ListarListaDeEspera_235JB(bool mesas_235JB)
        {
            return _dal_235JB.ListarListaDeEspera_235JB(mesas_235JB);
        }
    }
}
