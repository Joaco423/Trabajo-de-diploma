using System;
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
        private readonly Traductor traductor_235JB = new Traductor();
        private ContextoPedido _ctx_235JB;

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

        private void frmGestionDePedido_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        private void NuevoPedido_235JB()
        {
            _ctx_235JB = null;
            txtDNI.Clear();
            txtDNI.Enabled = true;
            btnIniciar.Enabled = true;
            lblClienteInfo.Text = string.Empty;
            txtResumen.Clear();
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
                _ctx_235JB = new ContextoPedido();
                txtResumen.Clear();
                _ctx_235JB.Cliente = _bllCliente_235JB.ObtenerPorDNI_235JB(dni_235JB);

                if (_ctx_235JB.Cliente == null)
                {
                    
                    // CU01 flujo 4.1: cliente no registrado -> CU02 RegistrarCliente
                    using (frmRegistrarCliente registrar_235JB = new frmRegistrarCliente(_ctx_235JB, dni_235JB))
                    {
                        registrar_235JB.ShowDialog(this);
                        if (registrar_235JB.Resultado != ResultadoCU.Completado)
                        {
                            AgregarResumen_235JB(Msg_235JB("msg_ClienteNoRegistradoFin", "El cliente no fue registrado. Se canceló la gestión del pedido."));
                            return;
                        }
                    }
                }

                // Baja logica: un cliente inactivo no puede pedir
                if (!_ctx_235JB.Cliente.Activo)
                {
                    Aviso_235JB(Msg_235JB("msg_ClienteInactivo", "El cliente está inactivo. Reactívelo desde Maestros > Clientes para poder registrar pedidos."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    NuevoPedido_235JB();
                    return;
                }

                lblClienteInfo.Text = _ctx_235JB.Cliente.Nombre + " " + _ctx_235JB.Cliente.Apellido;
                btnIniciar.Enabled = false;
                txtDNI.Enabled = false;

               
                // CU01 flujo 12.1/12.2 (retoma): pedido confirmado sin mesa o mozo -> sigue en CU05/CU06
                Pedido pendiente_235JB = _bllPedido_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(dni_235JB);
                if (pendiente_235JB != null)
                {
                    _ctx_235JB.Pedido = pendiente_235JB;
                    _ctx_235JB.Factura = _bllPedido_235JB.ObtenerFacturaPorPedido_235JB(pendiente_235JB.CodigoPedido);
                    _ctx_235JB.Detalle.AddRange(pendiente_235JB.Detalles);
                    int mesa_235JB;
                    if (ContextoPedido.MesaPorPedido_235JB.TryGetValue(pendiente_235JB.CodigoPedido, out mesa_235JB))
                        _ctx_235JB.NumeroMesa = mesa_235JB;

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

                SeleccionarYConfirmar_235JB();
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorBuscarCliente", "Error al buscar el cliente: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            finally
            {
                
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
                        string.Format(Msg_235JB("msg_ConfirmarFinalizar", "¿Desea finalizar la atención del pedido {0}? Se liberarán la mesa y el mozo."), codigoPedido_235JB),
                        Msg_235JB("msg_Confirmar_Titulo", "Confirmar"),
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                ResumenAtencion resumen_235JB = _bllAtencion_235JB.FinalizarAtencion_235JB(codigoPedido_235JB);
                string mensaje_235JB = string.Format(
                    Msg_235JB("msg_AtencionFinalizada", "Atención finalizada. Se liberaron la mesa {0} y el mozo {1} {2}. Si hay clientes esperando, use Atender lista de espera."),
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
                using (frmSeleccionarProductos seleccionar_235JB = new frmSeleccionarProductos(_ctx_235JB))
                {
                    seleccionar_235JB.ShowDialog(this);
                    if (seleccionar_235JB.Resultado != ResultadoCU.Completado)
                    {
                        AgregarResumen_235JB(Msg_235JB("msg_SinProductosFin", "No se seleccionaron productos. Se canceló la gestión del pedido."));
                        return;
                    }
                }

                ResultadoCU resultado_235JB;
                // CU01 paso 10: CU04 ConfirmarPedido
                using (frmConfirmarPedido confirmar_235JB = new frmConfirmarPedido(_ctx_235JB))
                {
                    confirmar_235JB.ShowDialog(this);
                    resultado_235JB = confirmar_235JB.Resultado;
                }

                
                // CU01 flujo 9.1 / CU04 flujo 2.1: el cliente quiere modificar -> vuelve a CU03
                if (resultado_235JB == ResultadoCU.Reintentar)
                    continue;

                if (resultado_235JB != ResultadoCU.Completado || _ctx_235JB.Pedido == null)
                {
                    AgregarResumen_235JB(Msg_235JB("msg_PedidoNoConfirmadoFin", "El pedido no fue confirmado. Se canceló la gestión del pedido."));
                    return;
                }
                break;
            }

            AgregarResumen_235JB(string.Format(
                Msg_235JB("msg_TextoConfirmacion", "Pedido {0} confirmado. Fecha: {1} {2}. Total: ${3}. Estado: {4}."),
                _ctx_235JB.Pedido.CodigoPedido, _ctx_235JB.Pedido.Fecha.ToShortDateString(), _ctx_235JB.Pedido.Hora, _ctx_235JB.Pedido.Total, _ctx_235JB.Pedido.Estado));
            AgregarResumen_235JB(string.Format(Msg_235JB("msg_TextoFactura", "Factura N° {0} - Total: ${1} - Estado: {2}."),
                _ctx_235JB.Factura.NumeroFactura, _ctx_235JB.Factura.Total, _ctx_235JB.Factura.Estado));

            AsignarMesaYMozo_235JB();
        }

      
        // CU01 paso 12: CU05 AsignarMesa y CU06 AsignarMozo (misma pantalla)
        private void AsignarMesaYMozo_235JB()
        {
            ResultadoCU resultado_235JB;
            using (frmAsignarMesaYMozo asignar_235JB = new frmAsignarMesaYMozo(_ctx_235JB))
            {
                asignar_235JB.ShowDialog(this);
                resultado_235JB = asignar_235JB.Resultado;
            }
            RegistrarResultado_235JB(resultado_235JB);
        }

        // CU01 pasos 13-15 y flujos 12.1-12.3: resumen de atencion / espera / cancelacion
        private void RegistrarResultado_235JB(ResultadoCU resultado_235JB)
        {
            if (_ctx_235JB.PedidoCancelado)
            {
                AgregarResumen_235JB(Msg_235JB("msg_PedidoCanceladoFin", "El pedido fue cancelado."));
            }
            else if (resultado_235JB == ResultadoCU.Completado && _ctx_235JB.Resumen != null)
            {
                AgregarResumen_235JB(string.Format(
                    Msg_235JB("msg_ResumenAtencion", "Resumen de atención - Pedido: {0} | Factura: {1} | Mesa: {2} (capacidad {3}) | Mozo: {4} {5}"),
                    _ctx_235JB.Resumen.CodigoPedido, _ctx_235JB.Resumen.NumeroFactura, _ctx_235JB.Resumen.NumeroMesa, _ctx_235JB.Resumen.Capacidad,
                    _ctx_235JB.Resumen.NombreMozo, _ctx_235JB.Resumen.ApellidoMozo));
            }
            else if (resultado_235JB == ResultadoCU.EnEspera)
            {
                AgregarResumen_235JB(Msg_235JB("msg_ClienteEnEsperaFin", "El cliente quedó en la lista de espera. Continúa desde Atender lista de espera cuando se libere una mesa o un mozo."));
            }
            else
            {
                AgregarResumen_235JB(Msg_235JB("msg_AsignacionPendienteFin", "La asignación de mesa y mozo quedó pendiente."));
            }
        }

        
        // CU05 y CU06 flujo 1.2: se atiende al primero de la lista de espera
        private void btnListaEspera_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ContextoPedido vacio_235JB = new ContextoPedido();
            ResultadoCU resultado_235JB;
            using (frmAsignarMesaYMozo asignar_235JB = new frmAsignarMesaYMozo(vacio_235JB))
            {
                asignar_235JB.ShowDialog(this);
                resultado_235JB = asignar_235JB.Resultado;
            }

            RefrescarListaEspera_235JB();
            _ctx_235JB = vacio_235JB;
            if (_ctx_235JB.Cliente != null)
                lblClienteInfo.Text = _ctx_235JB.Cliente.Nombre + " " + _ctx_235JB.Cliente.Apellido;
            if (_ctx_235JB.Pedido != null)
                RegistrarResultado_235JB(resultado_235JB);
        }
    }
}
