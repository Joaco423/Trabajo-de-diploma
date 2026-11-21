using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    
    public partial class frmGestionDePedido : Form, IObserver
    {
        private const string Form__235JB = "frmGestionDePedido";

        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();
        private readonly BLLPedido _bllPedido_235JB = new BLLPedido();
        private readonly BLLAtencion _bllAtencion_235JB = new BLLAtencion();
        private readonly BLLReserva _bllReserva_235JB = new BLLReserva();
        private readonly Traductor traductor_235JB = new Traductor();

        // Estado del pedido que se esta gestionando
        private Cliente _cliente_235JB;
        private List<DetallePedido> _detalle_235JB = new List<DetallePedido>();
        private Pedido _pedido_235JB;
        private int? _numeroMesa_235JB;
        private ResumenAtencion _resumen_235JB;
        private bool _pedidoCancelado_235JB;
        private bool _conPrioridad_235JB;

        // Datos del ultimo resumen de atencion mostrado: el estado de arriba se reinicia al terminar con el cliente,
        // pero el reporte en PDF se genera mientras el resumen siga a la vista
        private Cliente _clienteReporte_235JB;
        private Pedido _pedidoReporte_235JB;
        private ResumenAtencion _resumenReporte_235JB;

        public frmGestionDePedido()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(string idioma)
        {
            traductor_235JB.TraducirFormulario(this, int.Parse(idioma));
        }

        private string Msg_235JB(string clave_235JB, string original_235JB)
        {
            return traductor_235JB.ObtenerMensaje(Form__235JB, clave_235JB, original_235JB);
        }

        private void Aviso_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB, MessageBoxIcon icono_235JB)
        {
            MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        private void AgregarResumen_235JB(string linea_235JB)
        {
            txtResumen.AppendText(linea_235JB + Environment.NewLine);
        }

     
        private void RefrescarListaEspera_235JB()
        {
            try
            {
                dgvListaEspera.DataSource = _bllAtencion_235JB.ListarClientesEnEspera_235JB();
                dgvEnCurso.DataSource = _bllAtencion_235JB.ListarAtencionesEnCurso_235JB();

                // Reservas activas: esos clientes tienen prioridad sobre una mesa y un mozo
                dgvReservas.DataSource = _bllReserva_235JB.ListarActivas_235JB();
                if (dgvReservas.Columns["Fecha"] != null)
                    dgvReservas.Columns["Fecha"].Visible = false;
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorCargarListaEspera", "Error al cargar la lista de espera: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void frmGestionDePedido_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            NuevoPedido_235JB();
        }

        private void frmGestionDePedido_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                RefrescarListaEspera_235JB();
            }
        }

        // Las reservas se cargan desde otra pantalla: al volver a esta se actualiza la lista
        private void frmGestionDePedido_Activated_235JB(object sender_235JB, EventArgs e_235JB)
        {
            RefrescarListaEspera_235JB();
        }

        private void frmGestionDePedido_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        private void ReiniciarEstado_235JB()
        {
            _cliente_235JB = null;
            _detalle_235JB = new List<DetallePedido>();
            _pedido_235JB = null;
            _numeroMesa_235JB = null;
            _resumen_235JB = null;
            _pedidoCancelado_235JB = false;
            _conPrioridad_235JB = false;
        }

        private void LimpiarResumen_235JB()
        {
            txtResumen.Clear();
            _clienteReporte_235JB = null;
            _pedidoReporte_235JB = null;
            _resumenReporte_235JB = null;
            btnGenerarReporte.Enabled = false;
        }

        private void NuevoPedido_235JB()
        {
            ReiniciarEstado_235JB();
            txtDNI.Clear();
            txtDNI.Enabled = true;
            btnIniciar.Enabled = true;
            lblClienteInfo.Text = string.Empty;
            LimpiarResumen_235JB();
            RefrescarListaEspera_235JB();
        }

        private void btnNuevoPedido_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            NuevoPedido_235JB();
        }

       
        // CU01 pasos 2-4: se pide el DNI y se verifica que el cliente este registrado
        private void btnIniciar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            string texto_235JB = txtDNI.Text.Trim();
            if (!Regex.IsMatch(texto_235JB, @"^\d{8}$"))
            {
                Aviso_235JB(Msg_235JB("msg_DniInvalido", "El DNI debe tener exactamente 8 dígitos numéricos."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int dni_235JB = int.Parse(texto_235JB);
                ReiniciarEstado_235JB();
                LimpiarResumen_235JB();
                _cliente_235JB = _bllCliente_235JB.ObtenerPorDNI_235JB(dni_235JB);

                if (_cliente_235JB == null)
                {

                    // CU01 flujo 4.1: cliente no registrado -> CU02 RegistrarCliente
                    using (frmRegistrarCliente registrar_235JB = new frmRegistrarCliente(dni_235JB))
                    {
                        registrar_235JB.ShowDialog(this);
                        if (registrar_235JB.Resultado != DialogResult.OK)
                        {
                            AgregarResumen_235JB(Msg_235JB("msg_ClienteNoRegistradoFin", "El cliente no fue registrado. Se canceló la gestión del pedido."));
                            return;
                        }
                        _cliente_235JB = registrar_235JB.Cliente;
                    }
                }

                // Baja logica: un cliente inactivo no puede pedir
                if (!_cliente_235JB.Activo)
                {
                    Aviso_235JB(Msg_235JB("msg_ClienteInactivo", "El cliente está inactivo. Reactívelo desde Maestros > Clientes para poder registrar pedidos."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    return;
                }

                lblClienteInfo.Text = _cliente_235JB.Nombre + " " + _cliente_235JB.Apellido;
                btnIniciar.Enabled = false;
                txtDNI.Enabled = false;

               
                // CU01 flujo 12.1/12.2 (retoma): pedido confirmado sin mesa o mozo -> sigue en CU05/CU06
                Pedido pendiente_235JB = _bllPedido_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(dni_235JB);
                if (pendiente_235JB != null)
                {
                    _pedido_235JB = pendiente_235JB;
                    int mesa_235JB;
                    if (frmAsignarMesaYMozo.MesaPorPedido_235JB.TryGetValue(pendiente_235JB.CodigoPedido, out mesa_235JB))
                        _numeroMesa_235JB = mesa_235JB;

                    Aviso_235JB(Msg_235JB("msg_PedidoPendiente", "El cliente tiene un pedido confirmado pendiente de atención. Continúe con la asignación de mesa y mozo."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    AsignarMesaYMozo_235JB();
                    return;
                }

                
                // Regla: un cliente con pedido en curso no puede pedir otro
                Pedido enCurso_235JB = _bllPedido_235JB.ObtenerPedidoEnCurso_235JB(dni_235JB);
                if (enCurso_235JB != null)
                {
                    Aviso_235JB(string.Format(Msg_235JB("msg_PedidoEnCurso", "El cliente ya tiene un pedido en curso (pedido {0}). No puede realizar otro hasta que finalice su atención."),
                            enCurso_235JB.CodigoPedido),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    NuevoPedido_235JB();
                    return;
                }

                // Regla: un cliente que espera una mesa para reservar no puede ser atendido hasta conseguir su reserva
                if (_bllPedido_235JB.EstaEnEsperaDeReserva_235JB(dni_235JB))
                {
                    Aviso_235JB(Msg_235JB("msg_EnEsperaDeReserva", "El cliente está en la lista de espera para reservar una mesa y no puede ser atendido hasta que se le asigne su reserva."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    return;
                }

                SeleccionarYConfirmar_235JB();
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorBuscarCliente", "Error al buscar el cliente: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            finally
            {
                // El DNI solo se borra cuando el pedido ya esta en curso (mesa y mozo asignados) o se cancelo. Si el cliente quedo
                // en la lista de espera, la asignacion quedo pendiente o hubo un error, el DNI queda en el cuadro para retomar.
                bool pedidoEnCurso_235JB = _resumen_235JB != null && !_pedidoCancelado_235JB;
                bool pedidoCancelado_235JB = _pedidoCancelado_235JB;

                // El resumen queda a la vista, pero el DNI y el boton vuelven a habilitarse.
                ReiniciarEstado_235JB();
                if (pedidoEnCurso_235JB || pedidoCancelado_235JB)
                    txtDNI.Clear();
                txtDNI.Enabled = true;
                btnIniciar.Enabled = true;
                lblClienteInfo.Text = string.Empty;
                RefrescarListaEspera_235JB();
            }
        }

        
        // Fin de la atencion: finaliza el pedido y libera mesa y mozo (habilita la lista de espera)
        private void btnFinalizarAtencion_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (dgvEnCurso.CurrentRow == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccionePedidoEnCurso", "Seleccione de la grilla el pedido en curso que desea finalizar."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int codigoPedido_235JB = Convert.ToInt32(dgvEnCurso.CurrentRow.Cells["Pedido"].Value);

                if (MessageBox.Show(this,
                        string.Format(Msg_235JB("msg_ConfirmarFinalizar", "¿Desea finalizar la atención del pedido {0}? Se liberará la mesa y el mozo podrá tomar otra mesa."), codigoPedido_235JB),
                        Msg_235JB("msg_Confirmar_Titulo", "Confirmar"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                ResumenAtencion resumen_235JB = _bllAtencion_235JB.FinalizarAtencion_235JB(codigoPedido_235JB);
                string mensaje_235JB = string.Format(
                    Msg_235JB("msg_AtencionFinalizada", "Atención finalizada. Se liberó la mesa {0}; el mozo {1} {2} ya puede tomar otra mesa. Si hay clientes esperando, use Atender lista de espera."),
                    resumen_235JB.NumeroMesa, resumen_235JB.NombreMozo, resumen_235JB.ApellidoMozo);
                AgregarResumen_235JB(mensaje_235JB);
                Aviso_235JB(mensaje_235JB, "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorFinalizarAtencion", "No se pudo finalizar la atención: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            finally
            {
                RefrescarListaEspera_235JB();
            }
        }

        
        // CU01 pasos 5-11: CU03 SeleccionarProductos y CU04 ConfirmarPedido
        private void SeleccionarYConfirmar_235JB()
        {
            while (true)
            {
                // CU01 paso 7: CU03 SeleccionarProductos
                using (frmSeleccionarProductos seleccionar_235JB = new frmSeleccionarProductos(_cliente_235JB, _detalle_235JB))
                {
                    seleccionar_235JB.ShowDialog(this);
                    if (seleccionar_235JB.Resultado != DialogResult.OK)
                    {
                        AgregarResumen_235JB(Msg_235JB("msg_SinProductosFin", "No se seleccionaron productos. Se canceló la gestión del pedido."));
                        return;
                    }
                }

                DialogResult resultado_235JB;
                // CU01 paso 10: CU04 ConfirmarPedido
                using (frmConfirmarPedido confirmar_235JB = new frmConfirmarPedido(_cliente_235JB, _detalle_235JB))
                {
                    confirmar_235JB.ShowDialog(this);
                    resultado_235JB = confirmar_235JB.Resultado;
                    _pedido_235JB = confirmar_235JB.Pedido;
                }

                
                // CU01 flujo 9.1 / CU04 flujo 2.1: el cliente quiere modificar -> vuelve a CU03
                if (resultado_235JB == DialogResult.Retry)
                    continue;

                if (resultado_235JB != DialogResult.OK || _pedido_235JB == null)
                {
                    AgregarResumen_235JB(Msg_235JB("msg_PedidoNoConfirmadoFin", "El pedido no fue confirmado. Se canceló la gestión del pedido."));
                    return;
                }
                break;
            }

            AgregarResumen_235JB(string.Format(
                Msg_235JB("msg_TextoConfirmacion", "Pedido {0} confirmado. Fecha: {1} {2}. Total: ${3}. Estado: {4}."),
                _pedido_235JB.CodigoPedido, _pedido_235JB.Fecha.ToShortDateString(), _pedido_235JB.Hora, _pedido_235JB.Total, _pedido_235JB.Estado));
            AgregarResumen_235JB(string.Format(Msg_235JB("msg_TextoDetallePedido", "Detalle del pedido: {0}."),
                string.Join(", ", _detalle_235JB.Select(d_235JB => d_235JB.Nombre + " x" + d_235JB.Cantidad + " ($" + d_235JB.Subtotal + ")"))));

            AsignarMesaYMozo_235JB();
        }

      
        // CU01 paso 12: CU05 AsignarMesa y CU06 AsignarMozo (misma pantalla)
        private void AsignarMesaYMozo_235JB()
        {
            // Cliente con reserva (RFN2): ya tiene la mesa reservada y se le asigna un mozo disponible sin pasar
            // por la pantalla de asignacion. Si no hay mozos, sigue con la pantalla (que ofrece la lista de espera).
            if (!_numeroMesa_235JB.HasValue)
            {
                try
                {
                    int? reservada_235JB = _bllReserva_235JB.UsarReserva_235JB(_cliente_235JB.DNI);
                    if (reservada_235JB.HasValue)
                    {
                        _numeroMesa_235JB = reservada_235JB;
                        _conPrioridad_235JB = true;
                        AgregarResumen_235JB(string.Format(Msg_235JB("msg_MesaDeReserva", "El cliente tenía una reserva: se le asignó la mesa {0}."), reservada_235JB.Value));

                        List<Mozo> mozos_235JB = _bllAtencion_235JB.ObtenerMozosParaAsignar_235JB(true);
                        if (mozos_235JB.Count > 0)
                        {
                            _resumen_235JB = _bllAtencion_235JB.AsignarMozo_235JB(mozos_235JB[0].DNIMozo, _pedido_235JB, reservada_235JB.Value, true);
                            frmAsignarMesaYMozo.MesaPorPedido_235JB.Remove(_pedido_235JB.CodigoPedido);
                            RegistrarResultado_235JB(DialogResult.OK);
                            return;
                        }
                    }
                }
                catch (Exception ex_235JB)
                {
                    Aviso_235JB(Msg_235JB("msg_ErrorReservaPedido", "No se pudo usar la reserva del cliente: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
                }
            }
            DialogResult resultado_235JB;
            using (frmAsignarMesaYMozo asignar_235JB = new frmAsignarMesaYMozo(_cliente_235JB, _pedido_235JB, _numeroMesa_235JB) { ConPrioridad = _conPrioridad_235JB })
            {
                asignar_235JB.ShowDialog(this);
                resultado_235JB = asignar_235JB.Resultado;
                _numeroMesa_235JB = asignar_235JB.NumeroMesa;
                _resumen_235JB = asignar_235JB.Resumen;
                _pedidoCancelado_235JB = asignar_235JB.PedidoCancelado;
            }
            RegistrarResultado_235JB(resultado_235JB);
        }

        // CU01 pasos 13-15 y flujos 12.1-12.3: resumen de atencion / espera / cancelacion
        private void RegistrarResultado_235JB(DialogResult resultado_235JB)
        {
            if (_pedidoCancelado_235JB)
            {
                AgregarResumen_235JB(Msg_235JB("msg_PedidoCanceladoFin", "El pedido fue cancelado."));
            }
            else if (resultado_235JB == DialogResult.OK && _resumen_235JB != null)
            {
                AgregarResumen_235JB(string.Format(
                    Msg_235JB("msg_ResumenAtencion", "Resumen de atención - Pedido: {0} | Mesa: {1} (capacidad {2}) | Mozo: {3} {4}"),
                    _resumen_235JB.CodigoPedido, _resumen_235JB.NumeroMesa, _resumen_235JB.Capacidad,
                    _resumen_235JB.NombreMozo, _resumen_235JB.ApellidoMozo));

                // Con el resumen a la vista se puede generar el reporte en PDF
                if (_cliente_235JB != null && _pedido_235JB != null)
                {
                    _clienteReporte_235JB = _cliente_235JB;
                    _pedidoReporte_235JB = _pedido_235JB;
                    _resumenReporte_235JB = _resumen_235JB;
                    btnGenerarReporte.Enabled = true;
                }
            }
            else if (resultado_235JB == DialogResult.Yes)
            {
                AgregarResumen_235JB(Msg_235JB("msg_ClienteEnEsperaFin", "El cliente quedó en la lista de espera. Continúa desde Atender lista de espera cuando se libere una mesa o un mozo."));
            }
            else
            {
                AgregarResumen_235JB(Msg_235JB("msg_AsignacionPendienteFin", "La asignación de mesa y mozo quedó pendiente."));
            }
        }

        

        private void btnGenerarReporte_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_clienteReporte_235JB == null || _pedidoReporte_235JB == null || _resumenReporte_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SinResumenReporte", "No hay un resumen de atención para generar el reporte."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog dialogo_235JB = new SaveFileDialog())
            {
                dialogo_235JB.Title = Msg_235JB("msg_GuardarReporte_Titulo", "Guardar reporte");
                dialogo_235JB.Filter = "PDF (*.pdf)|*.pdf";
                dialogo_235JB.FileName = "Reporte_atencion_pedido_" + _pedidoReporte_235JB.CodigoPedido + ".pdf";
                if (dialogo_235JB.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _bllAtencion_235JB.GenerarReporteAtencion_235JB(dialogo_235JB.FileName, _clienteReporte_235JB, _pedidoReporte_235JB, _resumenReporte_235JB);
                    Aviso_235JB(string.Format(Msg_235JB("msg_ReporteGenerado", "Reporte generado: {0}"), dialogo_235JB.FileName),
                        "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                }
                catch (Exception ex_235JB)
                {
                    Aviso_235JB(Msg_235JB("msg_ErrorReporte", "No se pudo generar el reporte: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
                }
            }
        }

        // CU05 y CU06 flujo 1.2: se atiende al primero de la lista de espera
        private void btnListaEspera_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ReiniciarEstado_235JB();
            DialogResult resultado_235JB;
            using (frmAsignarMesaYMozo asignar_235JB = new frmAsignarMesaYMozo(null, null, null))
            {
                asignar_235JB.ShowDialog(this);
                resultado_235JB = asignar_235JB.Resultado;
                _cliente_235JB = asignar_235JB.Cliente;
                _pedido_235JB = asignar_235JB.Pedido;
                _numeroMesa_235JB = asignar_235JB.NumeroMesa;
                _resumen_235JB = asignar_235JB.Resumen;
                _pedidoCancelado_235JB = asignar_235JB.PedidoCancelado;
            }

            RefrescarListaEspera_235JB();
            if (_cliente_235JB != null)
                lblClienteInfo.Text = _cliente_235JB.Nombre + " " + _cliente_235JB.Apellido;
            if (_pedido_235JB != null)
                RegistrarResultado_235JB(resultado_235JB);

            
            if (_cliente_235JB != null && (_resumen_235JB != null || _pedidoCancelado_235JB)
                && txtDNI.Text.Trim() == _cliente_235JB.DNI.ToString())
                txtDNI.Clear();
        }

        
        private void dgvReservas_CellClick_235JB(object sender_235JB, DataGridViewCellEventArgs e_235JB)
        {
            if (e_235JB.RowIndex < 0 || !txtDNI.Enabled)
                return;

            object dni_235JB = dgvReservas.Rows[e_235JB.RowIndex].Cells["DNI"].Value;
            if (dni_235JB != null && dni_235JB != DBNull.Value)
                txtDNI.Text = dni_235JB.ToString();
        }
    }
}
