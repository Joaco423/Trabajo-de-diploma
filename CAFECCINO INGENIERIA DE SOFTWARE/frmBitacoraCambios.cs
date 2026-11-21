using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    // Bitacora de cambios de producto (Maestros > Bitacora de cambios): muestra Producto_C, la filtra por codigo de producto,
    // nombre y rango de fechas, y activa el registro elegido (el producto vuelve a esos datos).
    public partial class frmBitacoraCambios : Form, IObserver
    {
        private readonly BLLProducto_C _bll_235JB = new BLLProducto_C();
        private readonly Traductor traductor_235JB = new Traductor();
        private const string Form__235JB = "frmBitacoraCambios";
        private const string Todos_235JB = "Todos";

        public frmBitacoraCambios()
        {
            InitializeComponent();

            // Cada cambio de producto agrega registros en la bitacora: la grilla se actualiza sola, sin apretar Aplicar
            BLLProducto.ProductosCambiados_235JB += ProductosCambiados_235JB;
            Disposed += (s_235JB, e_235JB) => BLLProducto.ProductosCambiados_235JB -= ProductosCambiados_235JB;
        }

        private void ProductosCambiados_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (IsDisposed || !IsHandleCreated)
                return;
            // Se actualiza cuando termina la operacion que hizo el cambio (por ejemplo, el alta desde Productos)
            BeginInvoke((MethodInvoker)RefrescarAutomaticamente_235JB);
        }

        // Recarga los filtros y la grilla con los filtros que estan elegidos. Si la ventana esta oculta no hace falta:
        // se recarga al volver a mostrarse. Sin mensajes de error: no se interrumpe lo que el usuario esta haciendo.
        private void RefrescarAutomaticamente_235JB()
        {
            if (IsDisposed || !Visible)
                return;
            CargarFiltros_235JB(false);
            MostrarCambios_235JB(false);
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
            MessageBox.Show(mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        private void frmBitacoraCambios_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            CargarFiltros_235JB();
            MostrarCambios_235JB();
        }

        private void frmBitacoraCambios_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                CargarFiltros_235JB();
                MostrarCambios_235JB();
            }
        }

        private void frmBitacoraCambios_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        // Listas de los filtros: "Todos" + los codigos y los nombres que aparecen en la bitacora.
        // Al recargarlas se conserva lo que estaba elegido (si todavia existe).
        private void CargarFiltros_235JB(bool avisarErrores_235JB = true)
        {
            try
            {
                CargarCombo_235JB(cmbCodProd, _bll_235JB.ListarCodigos_235JB().Select(c_235JB => c_235JB.ToString()).ToList());
                CargarCombo_235JB(cmbNombre, _bll_235JB.ListarNombres_235JB());
            }
            catch (Exception ex_235JB)
            {
                if (avisarErrores_235JB)
                    Aviso_235JB(Msg_235JB("msg_ErrorCargar", "Error al cargar los cambios: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private static void CargarCombo_235JB(ComboBox combo_235JB, List<string> valores_235JB)
        {
            string seleccionado_235JB = combo_235JB.SelectedIndex > 0 ? combo_235JB.SelectedItem as string : null;
            combo_235JB.Items.Clear();
            combo_235JB.Items.Add(Todos_235JB);
            combo_235JB.Items.AddRange(valores_235JB.ToArray());
            int indice_235JB = seleccionado_235JB == null ? 0 : combo_235JB.Items.IndexOf(seleccionado_235JB);
            combo_235JB.SelectedIndex = indice_235JB < 0 ? 0 : indice_235JB;
        }

        private bool HayFiltros_235JB()
        {
            return cmbCodProd.SelectedIndex > 0 || cmbNombre.SelectedIndex > 0 || dtpFechaIni.Checked || dtpFechaFin.Checked;
        }

        // Pasos 2 y 4 del caso de uso: sin filtros muestra todos los registros; con filtros, solo los que coinciden
        private void MostrarCambios_235JB(bool avisarErrores_235JB = true)
        {
            try
            {
                List<Producto_C> cambios_235JB = HayFiltros_235JB()
                    ? _bll_235JB.FiltrarCambios_235JB(
                        cmbCodProd.SelectedIndex > 0 ? int.Parse((string)cmbCodProd.SelectedItem) : (int?)null,
                        cmbNombre.SelectedIndex > 0 ? (string)cmbNombre.SelectedItem : "",
                        dtpFechaIni.Checked ? dtpFechaIni.Value.Date : (DateTime?)null,
                        dtpFechaFin.Checked ? dtpFechaFin.Value.Date : (DateTime?)null)
                    : _bll_235JB.ListarCambios_235JB();

                dgvCambios.DataSource = cambios_235JB;
                // Flujo alternativo 3.1: no hay cambios que cumplan con los filtros
                lblSinResultados.Visible = cambios_235JB.Count == 0 && HayFiltros_235JB();
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_RangoFechas")
            {
                if (avisarErrores_235JB)
                    Aviso_235JB(Msg_235JB("msg_RangoFechas", "La fecha inicial no puede ser posterior a la fecha final."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (Exception ex_235JB)
            {
                if (avisarErrores_235JB)
                    Aviso_235JB(Msg_235JB("msg_ErrorCargar", "Error al cargar los cambios: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            MostrarCambios_235JB();
        }

        private void btnLimpiar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (cmbCodProd.Items.Count > 0) cmbCodProd.SelectedIndex = 0;
            if (cmbNombre.Items.Count > 0) cmbNombre.SelectedIndex = 0;
            dtpFechaIni.Value = DateTime.Today;
            dtpFechaIni.Checked = false;
            dtpFechaFin.Value = DateTime.Today;
            dtpFechaFin.Checked = false;
            MostrarCambios_235JB();
        }

        private void btnSalir_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Hide();
        }

        private void dgvCambios_DataBindingComplete_235JB(object sender_235JB, DataGridViewBindingCompleteEventArgs e_235JB)
        {
            if (dgvCambios.Columns.Contains("CodigoCambio"))
                dgvCambios.Columns["CodigoCambio"].Visible = false;
            if (dgvCambios.Columns.Contains("Fecha"))
                dgvCambios.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }

        // Pasos 5 a 7 del caso de uso
        private void btnActivar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Producto_C cambio_235JB = dgvCambios.CurrentRow == null ? null : dgvCambios.CurrentRow.DataBoundItem as Producto_C;
            if (cambio_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneCambio", "Seleccione de la tabla el cambio que desea activar."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bll_235JB.ActivarCambio_235JB(cambio_235JB.CodigoCambio);
                Aviso_235JB(Msg_235JB("msg_CambioActivado", "La operación fue exitosa: el producto volvió a los datos del cambio seleccionado."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_CambioYaActivado")
            {
                // Flujo alternativo 5.1
                Aviso_235JB(Msg_235JB("msg_CambioYaActivado", "El cambio seleccionado ya se encuentra activado."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Information);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_CambioInexistente")
            {
                Aviso_235JB(Msg_235JB("msg_CambioInexistente", "El cambio seleccionado ya no existe. Vuelva a cargar la tabla."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_ProductoExistente")
            {
                Aviso_235JB(Msg_235JB("msg_ProductoExistente", "No se puede activar: ya existe otro producto con el nombre de este cambio."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorActivar", "Error al activar el cambio: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }

            CargarFiltros_235JB();
            MostrarCambios_235JB();
        }
    }
}
