using System;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    
     
    
    public partial class frmGestionDeReservas : Form, IObserver
    {
        private const string Form__235JB = "frmGestionDeReservas";

        private readonly Traductor traductor_235JB = new Traductor();
        private readonly BLLReserva _bllReserva_235JB = new BLLReserva();
        private readonly ToolTip _ayuda_235JB = new ToolTip();

        public frmGestionDeReservas()
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

        private void AgregarResumen_235JB(string linea_235JB)
        {
            txtResumen.AppendText(linea_235JB + Environment.NewLine);
        }

        private void frmGestionDeReservas_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                _ayuda_235JB.SetToolTip(btnGenerarReporte, Msg_235JB("msg_AyudaReporte",
                    "Con una reserva seleccionada genera el reporte de esa reserva; sin selección, el de todas. Para quitar la selección: clic en un espacio vacío de la grilla o Esc."));
                RefrescarReservas_235JB();
            }
        }

        
        private void frmGestionDeReservas_Activated_235JB(object sender_235JB, EventArgs e_235JB)
        {
            RefrescarReservas_235JB();
        }

        private void frmGestionDeReservas_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        private void Aviso_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB, MessageBoxIcon icono_235JB)
        {
            MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        private void RefrescarReservas_235JB()
        {
            try
            {
                int? seleccionada_235JB = ReservaSeleccionada_235JB();
                dgvReservas.DataSource = _bllReserva_235JB.ListarActivas_235JB();
                SeleccionarReserva_235JB(seleccionada_235JB);
                dgvEspera.DataSource = _bllReserva_235JB.ListarEsperaDeReservas_235JB();
                dgvEspera.ClearSelection();
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorCargarReservas", "Error al cargar las reservas: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        
        private void btnCancelarReserva_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (dgvReservas.CurrentRow == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneReserva", "Seleccione de la grilla la reserva que desea cancelar."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            int codigo_235JB = Convert.ToInt32(dgvReservas.CurrentRow.Cells["Reserva"].Value);
            if (MessageBox.Show(this,
                    string.Format(Msg_235JB("msg_ConfirmarCancelarReserva", "¿Desea cancelar la reserva {0}? La mesa quedará libre."), codigo_235JB),
                    Msg_235JB("msg_Confirmar_Titulo", "Confirmar"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _bllReserva_235JB.Cancelar_235JB(codigo_235JB);
                AgregarResumen_235JB(string.Format(Msg_235JB("msg_ReservaCancelada", "Reserva {0} cancelada. La mesa quedó libre."), codigo_235JB));
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorCancelarReserva", "No se pudo cancelar la reserva: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            finally
            {
                RefrescarReservas_235JB();
            }
        }

        // RFN2 paso 5: hay una mesa libre -> se reserva para el primer cliente de la lista de espera (por Fecha y Hora)
        private void btnReservarDeLista_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            try
            {
                string nombre_235JB;
                Reserva reserva_235JB = _bllReserva_235JB.ReservarParaListaDeEspera_235JB(out nombre_235JB);
                if (reserva_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_ListaReservasVacia", "No hay clientes esperando una mesa para reservar."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                string texto_235JB = string.Format(Msg_235JB("msg_ReservadaDesdeLista", "Mesa {0} reservada para {1}, primero en la lista de espera. Código de reserva: {2}."),
                    reserva_235JB.NumeroMesa, nombre_235JB, reserva_235JB.CodigoReserva);
                AgregarResumen_235JB(texto_235JB);
                Aviso_235JB(texto_235JB, "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_SinMesasLibres")
            {
                Aviso_235JB(Msg_235JB("msg_SinMesasLibres", "No hay mesas libres para reservar. El cliente sigue en la lista de espera."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_MesaNoDisponible")
            {
                Aviso_235JB(Msg_235JB("msg_MesaNoDisponible", "La mesa seleccionada ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_PedidoEnCurso" || ex_235JB.Message == "msg_ClienteConReserva" || ex_235JB.Message == "msg_EsperaClienteInvalido")
            {
                Aviso_235JB(string.Format(Msg_235JB("msg_EsperaSaleDeLista", "{0} ya no puede reservar (ya tiene una reserva, está siendo atendido o no está registrado) y se lo quitó de la lista de espera. Vuelva a intentarlo para atender al siguiente."),
                    ex_235JB.Data["NombreEspera"]),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorReservarLista", "No se pudo reservar desde la lista de espera: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            finally
            {
                RefrescarReservas_235JB();
            }
        }

        private int? ReservaSeleccionada_235JB()
        {
            if (dgvReservas.SelectedRows.Count == 0)
                return null;
            return Convert.ToInt32(dgvReservas.SelectedRows[0].Cells["Reserva"].Value);
        }

        private void QuitarSeleccion_235JB()
        {
            dgvReservas.ClearSelection();
            dgvReservas.CurrentCell = null;
        }

        
        private void SeleccionarReserva_235JB(int? codigo_235JB)
        {
            QuitarSeleccion_235JB();
            if (!codigo_235JB.HasValue)
                return;

            foreach (DataGridViewRow fila_235JB in dgvReservas.Rows)
            {
                if (Convert.ToInt32(fila_235JB.Cells["Reserva"].Value) == codigo_235JB.Value)
                {
                    dgvReservas.CurrentCell = fila_235JB.Cells[0];
                    fila_235JB.Selected = true;
                    break;
                }
            }
        }

        
        private void dgvReservas_MouseDown_235JB(object sender_235JB, MouseEventArgs e_235JB)
        {
            if (dgvReservas.HitTest(e_235JB.X, e_235JB.Y).Type == DataGridViewHitTestType.None)
                QuitarSeleccion_235JB();
        }

        private void dgvReservas_KeyDown_235JB(object sender_235JB, KeyEventArgs e_235JB)
        {
            if (e_235JB.KeyCode == Keys.Escape)
                QuitarSeleccion_235JB();
        }

        
        private void btnGenerarReporte_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            
            int? codigo_235JB = ReservaSeleccionada_235JB();

            if (!codigo_235JB.HasValue && dgvReservas.Rows.Count == 0)
            {
                Aviso_235JB(Msg_235JB("msg_SinReservasReporte", "No hay reservas activas para generar el reporte."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog dialogo_235JB = new SaveFileDialog())
            {
                dialogo_235JB.Title = Msg_235JB("msg_GuardarReporte_Titulo", "Guardar reporte");
                dialogo_235JB.Filter = "PDF (*.pdf)|*.pdf";
                dialogo_235JB.FileName = codigo_235JB.HasValue
                    ? "Reporte_reserva_" + codigo_235JB.Value + ".pdf"
                    : "Reporte_reservas_activas_" + DateTime.Now.ToString("yyyyMMdd") + ".pdf";
                if (dialogo_235JB.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    if (codigo_235JB.HasValue)
                    {
                        _bllReserva_235JB.GenerarReporteReserva_235JB(dialogo_235JB.FileName, codigo_235JB.Value);
                        AgregarResumen_235JB(string.Format(Msg_235JB("msg_ReporteReservaGenerado", "Reporte de la reserva {0} generado."), codigo_235JB.Value));
                    }
                    else
                    {
                        int cantidad_235JB = _bllReserva_235JB.GenerarReporteReservasActivas_235JB(dialogo_235JB.FileName);
                        AgregarResumen_235JB(string.Format(Msg_235JB("msg_ReporteReservasGenerado", "Reporte de reservas activas generado ({0} reservas)."), cantidad_235JB));
                    }
                    Aviso_235JB(string.Format(Msg_235JB("msg_ReporteGenerado", "Reporte generado: {0}"), dialogo_235JB.FileName),
                        "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                }
                catch (Exception ex_235JB)
                {
                    Aviso_235JB(Msg_235JB("msg_ErrorReporte", "No se pudo generar el reporte: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
                }
                finally
                {
                    RefrescarReservas_235JB();
                }
            }
        }
        private void btnLimpiar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            txtResumen.Clear();
        }

        private void btnIniciar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            txtResumen.Clear();
            int dni_235JB;
            Cliente cliente_235JB;


            using (frmIniciarRegistro iniciar_235JB = new frmIniciarRegistro())
            {
                iniciar_235JB.ShowDialog(this);
                if (iniciar_235JB.Resultado != DialogResult.OK)
                {
                    AgregarResumen_235JB(Msg_235JB("msg_RegistroCancelado", "No se pudo completar el registro del cliente."));
                    return;
                }
                dni_235JB = iniciar_235JB.DNI;
                cliente_235JB = iniciar_235JB.ClienteExistente;
            }

            if (cliente_235JB != null)
            {
                AgregarResumen_235JB(string.Format(Msg_235JB("msg_ResumenYaRegistrado", "Cliente ya registrado: {0} {1} (código {2})."),
                    cliente_235JB.Nombre, cliente_235JB.Apellido, cliente_235JB.CodigoCliente));
            }
            else
            {

                using (frmIngresarDatosCliente ingresar_235JB = new frmIngresarDatosCliente(dni_235JB))
                {
                    ingresar_235JB.ShowDialog(this);
                    if (ingresar_235JB.Resultado != DialogResult.OK)
                    {
                        AgregarResumen_235JB(Msg_235JB("msg_RegistroCancelado", "No se pudo completar el registro del cliente."));
                        return;
                    }
                    cliente_235JB = ingresar_235JB.Datos;
                }


                using (frmRegistrarClienteReserva registrar_235JB = new frmRegistrarClienteReserva(cliente_235JB))
                {
                    registrar_235JB.ShowDialog(this);
                    if (registrar_235JB.Resultado != DialogResult.OK)
                    {
                        AgregarResumen_235JB(Msg_235JB("msg_RegistroCancelado", "No se pudo completar el registro del cliente."));
                        return;
                    }
                }

                AgregarResumen_235JB(string.Format(Msg_235JB("msg_ResumenRegistrado", "Cliente registrado: {0} {1} (código {2})."),
                    cliente_235JB.Nombre, cliente_235JB.Apellido, cliente_235JB.CodigoCliente));
            }


            using (frmConsultarDisponibilidadMesas mesas_235JB = new frmConsultarDisponibilidadMesas(cliente_235JB))
            {
                mesas_235JB.ShowDialog(this);
                if (mesas_235JB.Resultado == DialogResult.Yes)
                    AgregarResumen_235JB(Msg_235JB("msg_ResumenEnEspera", "El cliente quedó en la lista de espera de mesas."));
                else if (mesas_235JB.NumeroMesaReservada.HasValue)
                    AgregarResumen_235JB(string.Format(Msg_235JB("msg_ResumenMesaReservada", "Mesa {0} reservada para {1} {2}."), mesas_235JB.NumeroMesaReservada.Value, cliente_235JB.Nombre, cliente_235JB.Apellido));
                else
                    AgregarResumen_235JB(Msg_235JB("msg_ResumenMesasConsultadas", "Se consultó la disponibilidad de mesas."));
            }

            RefrescarReservas_235JB();
        }

        private void frmGestionDeReservas_Load(object sender, EventArgs e)
        {

        }
    }
}
