using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
   
    public partial class frmAsignarMesaYMozo : FormularioCU
    {
        private enum Etapa { Espera, Mesa, Mozo, Final }

        private readonly BLLAtencion _bllAtencion_235JB = new BLLAtencion();
        private readonly BLLPedido _bllPedido_235JB = new BLLPedido();
        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();
        private readonly ContextoPedido _ctx_235JB;
        private Etapa _etapa_235JB;
        private ResultadoCU _pendiente_235JB = ResultadoCU.Cancelado;

        public frmAsignarMesaYMozo(ContextoPedido ctx_235JB)
        {
            InitializeComponent();
            _ctx_235JB = ctx_235JB;
        }

       
        private void frmAsignarMesaYMozo_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            TraducirAlAbrir_235JB();
            CargarMesas_235JB();
            CargarMozos_235JB();

            if (_ctx_235JB.Pedido == null)
                _etapa_235JB = Etapa.Espera;
            else
                _etapa_235JB = _ctx_235JB.NumeroMesa.HasValue ? Etapa.Mozo : Etapa.Mesa;

            MostrarEncabezado_235JB();
            AplicarEtapa_235JB();
        }

       
        private void frmAsignarMesaYMozo_Shown_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_etapa_235JB == Etapa.Mesa)
                ProcesarMesa_235JB();
            else if (_etapa_235JB == Etapa.Mozo)
                ProcesarMozo_235JB();
        }

        private void frmAsignarMesaYMozo_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (Resultado == ResultadoCU.Cancelado)
                Resultado = _pendiente_235JB;
        }

        private void AplicarEtapa_235JB()
        {
            btnAsignarMesa.Enabled = _etapa_235JB == Etapa.Mesa;
            btnMesaDesdeLista.Enabled = _etapa_235JB == Etapa.Espera;
            btnAsignarMozo.Enabled = _etapa_235JB == Etapa.Mozo;
            btnMozoDesdeLista.Enabled = _etapa_235JB == Etapa.Espera;
            btnListaEspera.Enabled = _etapa_235JB == Etapa.Mesa || _etapa_235JB == Etapa.Mozo;
            btnCancelarPedido.Enabled = _ctx_235JB.Pedido != null && (_etapa_235JB == Etapa.Mesa || _etapa_235JB == Etapa.Mozo);
            btnContinuar.Visible = _etapa_235JB == Etapa.Final;
        }

        private void MostrarEncabezado_235JB()
        {
            txtResumen.Clear();
            if (_ctx_235JB.Cliente != null && _ctx_235JB.Pedido != null)
                AgregarResumen_235JB(string.Format(Msg_235JB("msg_EncabezadoPedido", "Cliente: {0} {1} - Pedido {2}."),
                    _ctx_235JB.Cliente.Nombre, _ctx_235JB.Cliente.Apellido, _ctx_235JB.Pedido.CodigoPedido));
            if (_ctx_235JB.NumeroMesa.HasValue)
                AgregarResumen_235JB(string.Format(Msg_235JB("msg_TextoMesaAsignada", "Mesa asignada: {0}."), _ctx_235JB.NumeroMesa.Value));
        }

        private void AgregarResumen_235JB(string linea_235JB)
        {
            txtResumen.AppendText(linea_235JB + Environment.NewLine);
        }

      
        // CU05 paso 1: mesas disponibles (numero y capacidad)
        private void CargarMesas_235JB()
        {
            try
            {
                dgvMesas.DataSource = _bllAtencion_235JB.ObtenerMesasDisponibles_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCargarMesas", "Error al cargar las mesas: ", ex_235JB);
            }
        }

        private Mesa MesaSeleccionada_235JB()
        {
            return dgvMesas.CurrentRow == null ? null : dgvMesas.CurrentRow.DataBoundItem as Mesa;
        }

        // CU05 flujo 1.1: sin mesas libres -> se ofrece la lista de espera de mesas
        private void ProcesarMesa_235JB()
        {
            CargarMesas_235JB();
            List<Mesa> libres_235JB = dgvMesas.DataSource as List<Mesa>;
            if (libres_235JB == null || libres_235JB.Count == 0)
                OfrecerListaDeEspera_235JB(true);
        }

        // CU05 pasos 2-4: asigna la mesa elegida (queda Ocupada) y muestra numero y capacidad
        private void btnAsignarMesa_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mesa mesa_235JB = MesaSeleccionada_235JB();
            if (mesa_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMesa", "Seleccione de la grilla la mesa que desea asignar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bllAtencion_235JB.AsignarMesa_235JB(mesa_235JB.NumeroMesa);
                _ctx_235JB.NumeroMesa = mesa_235JB.NumeroMesa;

                AgregarResumen_235JB(string.Format(Msg_235JB("msg_TextoMesaAsignada", "Mesa asignada: {0}."), mesa_235JB.NumeroMesa)
                    + " " + string.Format(Msg_235JB("msg_TextoCapacidad", "Capacidad: {0}."), mesa_235JB.Capacidad));

                _etapa_235JB = Etapa.Mozo;
                AplicarEtapa_235JB();
                CargarMesas_235JB();
                ProcesarMozo_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaNoDisponible", "La mesa seleccionada ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMesas_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMesa", "Error al asignar la mesa: ", ex_235JB);
            }
        }

        
        // CU05 flujo 1.2: mesa liberada -> se asigna al primero de la lista de espera (Fecha y Hora)
        private void btnMesaDesdeLista_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mesa mesa_235JB = MesaSeleccionada_235JB();
            if (mesa_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMesaLibre", "Seleccione de la grilla la mesa libre que desea asignar al primer cliente de la lista de espera."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ListaDeEspera primero_235JB = _bllAtencion_235JB.AsignarMesaDeListaDeEspera_235JB(mesa_235JB.NumeroMesa);
                if (primero_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_ListaVacia", "La lista de espera está vacía."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                Pedido pedido_235JB = _bllPedido_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(primero_235JB.DNI);
                if (pedido_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_SinPedidoPendiente", "El cliente de la lista de espera no tiene un pedido confirmado pendiente."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    CargarMesas_235JB();
                    return;
                }

                _ctx_235JB.Cliente = _bllCliente_235JB.ObtenerPorDNI_235JB(primero_235JB.DNI);
                _ctx_235JB.Pedido = pedido_235JB;
                _ctx_235JB.Factura = _bllPedido_235JB.ObtenerFacturaPorPedido_235JB(pedido_235JB.CodigoPedido);
                _ctx_235JB.Detalle.Clear();
                _ctx_235JB.Detalle.AddRange(pedido_235JB.Detalles);
                _ctx_235JB.NumeroMesa = mesa_235JB.NumeroMesa;

                Aviso_235JB(string.Format(Msg_235JB("msg_MesaAsignadaEspera", "Se asignó la mesa {0} a {1} {2}, primero en la lista de espera. Continúe con la asignación de mozo."),
                        mesa_235JB.NumeroMesa, primero_235JB.Nombre, primero_235JB.Apellido),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);

                MostrarEncabezado_235JB();
                _etapa_235JB = Etapa.Mozo;
                AplicarEtapa_235JB();
                CargarMesas_235JB();
                ProcesarMozo_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaNoDisponible", "La mesa seleccionada ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMesas_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMesa", "Error al asignar la mesa: ", ex_235JB);
            }
        }

       
        // CU06 paso 1: mozos disponibles
        private void CargarMozos_235JB()
        {
            try
            {
                dgvMozos.DataSource = _bllAtencion_235JB.ObtenerMozosDisponibles_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCargarMozos", "Error al cargar los mozos: ", ex_235JB);
            }
        }

        private Mozo MozoSeleccionado_235JB()
        {
            return dgvMozos.CurrentRow == null ? null : dgvMozos.CurrentRow.DataBoundItem as Mozo;
        }

        // CU06 flujo 1.1: sin mozos disponibles -> se ofrece la lista de espera de mozos
        private void ProcesarMozo_235JB()
        {
            CargarMozos_235JB();
            List<Mozo> libres_235JB = dgvMozos.DataSource as List<Mozo>;
            if (libres_235JB == null || libres_235JB.Count == 0)
                OfrecerListaDeEspera_235JB(false);
        }

        // CU06 pasos 2-4: asigna el mozo (queda Ocupado) y registra la relacion cliente-pedido-factura-mesa-mozo
        private void btnAsignarMozo_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mozo mozo_235JB = MozoSeleccionado_235JB();
            if (mozo_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMozo", "Seleccione de la grilla el mozo que desea asignar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _ctx_235JB.Resumen = _bllAtencion_235JB.AsignarMozo_235JB(mozo_235JB.DNIMozo, _ctx_235JB.Pedido, _ctx_235JB.NumeroMesa.Value);
                ContextoPedido.MesaPorPedido_235JB.Remove(_ctx_235JB.Pedido.CodigoPedido);
                MostrarResumenAtencion_235JB(_ctx_235JB.Resumen);
                Finalizar_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoNoDisponible", "El mozo seleccionado ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMozos_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMozo", "Error al asignar el mozo: ", ex_235JB);
            }
        }

        
        // CU06 flujo 1.2: mozo liberado -> se asigna al primero de la lista de espera
        private void btnMozoDesdeLista_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mozo mozo_235JB = MozoSeleccionado_235JB();
            if (mozo_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMozoLibre", "Seleccione de la grilla el mozo libre que desea asignar al primer cliente de la lista de espera."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ListaDeEspera primero_235JB = _bllAtencion_235JB.ObtenerPrimeroEnListaDeEspera_235JB(false);
                if (primero_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_ListaVacia", "La lista de espera está vacía."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                Pedido pedido_235JB = _bllPedido_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(primero_235JB.DNI);
                if (pedido_235JB == null)
                {
                    _bllAtencion_235JB.QuitarDeListaDeEspera_235JB(false, primero_235JB.DNI);
                    Aviso_235JB(Msg_235JB("msg_SinPedidoPendiente", "El cliente de la lista de espera no tiene un pedido confirmado pendiente."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    return;
                }

                if (!ContextoPedido.MesaPorPedido_235JB.TryGetValue(pedido_235JB.CodigoPedido, out int mesa_235JB))
                {
                    
                    if (Preguntar_235JB(Msg_235JB("msg_MesaNoRecuperada", "No se pudo recuperar la mesa de este cliente (se pierde al cerrar el sistema). ¿Desea cancelar su pedido?"),
                            "msg_ListaEspera_Titulo", "Lista de espera"))
                    {
                        _bllPedido_235JB.CancelarPedido_235JB(pedido_235JB.CodigoPedido);
                        Aviso_235JB(Msg_235JB("msg_PedidoCancelado", "Pedido cancelado."),
                            "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                        CargarMozos_235JB();
                    }
                    return;
                }

                _ctx_235JB.Cliente = _bllCliente_235JB.ObtenerPorDNI_235JB(primero_235JB.DNI);
                _ctx_235JB.Pedido = pedido_235JB;
                _ctx_235JB.NumeroMesa = mesa_235JB;
                _ctx_235JB.Resumen = _bllAtencion_235JB.AsignarMozo_235JB(mozo_235JB.DNIMozo, pedido_235JB, mesa_235JB);
                _bllAtencion_235JB.QuitarDeListaDeEspera_235JB(false, primero_235JB.DNI);
                ContextoPedido.MesaPorPedido_235JB.Remove(pedido_235JB.CodigoPedido);

                MostrarEncabezado_235JB();
                MostrarResumenAtencion_235JB(_ctx_235JB.Resumen);
                Finalizar_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoNoDisponible", "El mozo seleccionado ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMozos_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMozo", "Error al asignar el mozo: ", ex_235JB);
            }
        }

        // CU06 paso 5: resumen de atencion (pedido, factura, mesa y mozo)
        private void MostrarResumenAtencion_235JB(ResumenAtencion resumen_235JB)
        {
            string texto_235JB = string.Format(
                Msg_235JB("msg_ResumenAtencion", "Resumen de atención - Pedido: {0} | Factura: {1} | Mesa: {2} (capacidad {3}) | Mozo: {4} {5}"),
                resumen_235JB.CodigoPedido, resumen_235JB.NumeroFactura, resumen_235JB.NumeroMesa, resumen_235JB.Capacidad,
                resumen_235JB.NombreMozo, resumen_235JB.ApellidoMozo);
            AgregarResumen_235JB(texto_235JB);
            Aviso_235JB(texto_235JB, "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
        }

        private void Finalizar_235JB()
        {
            _pendiente_235JB = ResultadoCU.Completado;
            _etapa_235JB = Etapa.Final;
            AplicarEtapa_235JB();
            CargarMesas_235JB();
            CargarMozos_235JB();
        }

        private void btnListaEspera_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            RegistrarEnListaDeEspera_235JB(_etapa_235JB == Etapa.Mesa);
        }

        // CU05 y CU06 flujo 1.1: consulta si el cliente quiere esperar; si no acepta -> flujo 1.3
        private void OfrecerListaDeEspera_235JB(bool mesas_235JB)
        {
            string pregunta_235JB = mesas_235JB
                ? Msg_235JB("msg_SinMesas", "No hay mesas libres. ¿El cliente desea ingresar a la lista de espera de mesas?")
                : Msg_235JB("msg_SinMozos", "No hay mozos disponibles. ¿El cliente desea ingresar a la lista de espera de mozos?");

            if (Preguntar_235JB(pregunta_235JB, "msg_ListaEspera_Titulo", "Lista de espera"))
                RegistrarEnListaDeEspera_235JB(mesas_235JB);
            else if (Preguntar_235JB(Msg_235JB("msg_NoAceptaEspera", "El cliente no ingresará a la lista de espera. ¿Desea cancelar el pedido?"),
                         "msg_ListaEspera_Titulo", "Lista de espera"))
                CancelarPedidoActual_235JB();
        }

        // CU05 y CU06 flujo 1.1: registra al cliente en la lista (DNI, nombre, apellido, fecha y hora)
        private void RegistrarEnListaDeEspera_235JB(bool mesas_235JB)
        {
            if (_ctx_235JB.Cliente == null || _ctx_235JB.Pedido == null)
                return;

            try
            {
                _bllAtencion_235JB.RegistrarEnListaDeEspera_235JB(mesas_235JB, _ctx_235JB.Cliente);
                if (!mesas_235JB && _ctx_235JB.NumeroMesa.HasValue)
                    ContextoPedido.MesaPorPedido_235JB[_ctx_235JB.Pedido.CodigoPedido] = _ctx_235JB.NumeroMesa.Value;

                Aviso_235JB(mesas_235JB
                        ? Msg_235JB("msg_EnEsperaMesas", "El cliente quedó registrado en la lista de espera de mesas.")
                        : Msg_235JB("msg_EnEsperaMozos", "El cliente quedó registrado en la lista de espera de mozos."),
                    "msg_Info_Titulo", "Información", MessageBoxIcon.Information);

                Terminar_235JB(ResultadoCU.EnEspera);
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_YaEnLista", "El cliente ya está en la lista de espera."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorListaEspera", "Error al registrar en la lista de espera: ", ex_235JB);
            }
        }

        private void btnCancelarPedido_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_ctx_235JB.Pedido == null)
                return;

            if (Preguntar_235JB(Msg_235JB("msg_ConfirmarCancelar", "¿Desea cancelar el pedido? Se devolverá el stock y se anulará la factura."),
                    "msg_Confirmar_Titulo", "Confirmar"))
                CancelarPedidoActual_235JB();
        }

        
        // CU05 y CU06 flujo 1.3 (CU01 12.3): cancela el pedido, devuelve stock, anula factura y libera la mesa
        private void CancelarPedidoActual_235JB()
        {
            if (_ctx_235JB.Pedido == null)
                return;

            try
            {
                _bllPedido_235JB.CancelarPedido_235JB(_ctx_235JB.Pedido.CodigoPedido);
                if (_ctx_235JB.NumeroMesa.HasValue)
                    _bllAtencion_235JB.LiberarMesa_235JB(_ctx_235JB.NumeroMesa.Value);
                ContextoPedido.MesaPorPedido_235JB.Remove(_ctx_235JB.Pedido.CodigoPedido);
                _ctx_235JB.PedidoCancelado = true;

                Aviso_235JB(Msg_235JB("msg_PedidoCancelado", "Pedido cancelado."),
                    "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                Terminar_235JB(ResultadoCU.Cancelado);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCancelar", "No se pudo cancelar el pedido: ", ex_235JB);
            }
        }

        private void btnContinuar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(ResultadoCU.Completado);
        }

        private void btnCerrar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(_pendiente_235JB);
        }
    }
}
