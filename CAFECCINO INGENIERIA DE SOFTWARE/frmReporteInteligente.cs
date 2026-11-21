using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    // Reportes > Reporte inteligente: elige un reporte, muestra su resultado y su conclusion, y lo exporta a PDF.
    public partial class frmReporteInteligente : Form, IObserver
    {
        private readonly BLLReporte _bll_235JB = new BLLReporte();
        private readonly Traductor traductor_235JB = new Traductor();
        private const string Form__235JB = "frmReporteInteligente";
        private ReporteInteligente _reporte_235JB;

        public frmReporteInteligente()
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
            MessageBox.Show(mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        private void frmReporteInteligente_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            cmbTipoReporte.DisplayMember = "Value";
            cmbTipoReporte.ValueMember = "Key";
            cmbTipoReporte.DataSource = new BindingSource(_bll_235JB.ListarTipos_235JB(), null);
            cmbTipoReporte.SelectedIndex = -1;
            dgvReporte.DataSource = null;
            _reporte_235JB = null;
            lblConclusion.Text = Msg_235JB("msg_ElijaReporte", "Elija un tipo de reporte para ver su resultado.");
        }

        private void frmReporteInteligente_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                if (cmbTipoReporte.SelectedIndex >= 0)
                    CargarReporte_235JB();   // los datos pueden haber cambiado mientras la pantalla estaba oculta
            }
        }

        private void frmReporteInteligente_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        private void cmbTipoReporte_SelectedIndexChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (cmbTipoReporte.SelectedIndex >= 0)
                CargarReporte_235JB();
        }

        private void CargarReporte_235JB()
        {
            try
            {
                KeyValuePair<int, string> tipo_235JB = (KeyValuePair<int, string>)cmbTipoReporte.SelectedItem;
                _reporte_235JB = _bll_235JB.ObtenerReporte_235JB(tipo_235JB.Key);
                dgvReporte.DataSource = _reporte_235JB.Datos;
                lblConclusion.Text = _reporte_235JB.Conclusion;
            }
            catch (Exception ex_235JB)
            {
                _reporte_235JB = null;
                Aviso_235JB(Msg_235JB("msg_ErrorCargar", "Error al obtener el reporte: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        // ---------------- Seleccion de filas: el PDF incluye solo las filas elegidas (sin seleccion, todo el reporte) ----------------

        // La grilla selecciona sola la primera fila al cargarse: se quita para que por defecto el reporte salga completo
        private void dgvReporte_DataBindingComplete_235JB(object sender_235JB, DataGridViewBindingCompleteEventArgs e_235JB)
        {
            dgvReporte.ClearSelection();
            dgvReporte.CurrentCell = null;
            ActualizarAvisoSeleccion_235JB();
        }

        private void dgvReporte_SelectionChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ActualizarAvisoSeleccion_235JB();
        }

        // Esc o clic en un espacio vacio de la grilla quitan la seleccion
        private void dgvReporte_KeyDown_235JB(object sender_235JB, KeyEventArgs e_235JB)
        {
            if (e_235JB.KeyCode == Keys.Escape)
            {
                dgvReporte.ClearSelection();
                e_235JB.Handled = true;
            }
        }

        private void dgvReporte_MouseDown_235JB(object sender_235JB, MouseEventArgs e_235JB)
        {
            if (dgvReporte.HitTest(e_235JB.X, e_235JB.Y).Type == DataGridViewHitTestType.None)
                dgvReporte.ClearSelection();
        }

        private void ActualizarAvisoSeleccion_235JB()
        {
            int elegidas_235JB = dgvReporte.SelectedRows.Count;
            lblSeleccion.Text = elegidas_235JB == 0
                ? Msg_235JB("msg_SinSeleccion", "Sin filas seleccionadas: el PDF incluye todo el reporte. Para incluir solo algunas, selecciónelas (Ctrl o Shift para varias).")
                : string.Format(Msg_235JB("msg_ConSeleccion", "{0} fila(s) seleccionada(s): el PDF incluye solo esas. Esc o clic en un espacio vacío para quitar la selección."), elegidas_235JB);

            // La conclusion de la pantalla es la misma que va a salir en el PDF (de la seleccion o del reporte completo)
            if (_reporte_235JB != null)
                lblConclusion.Text = _bll_235JB.FiltrarSeleccion_235JB(_reporte_235JB, FilasSeleccionadas_235JB()).Conclusion;
        }

        // Indices (en los datos del reporte) de las filas elegidas en la grilla
        private List<int> FilasSeleccionadas_235JB()
        {
            List<int> filas_235JB = new List<int>();
            foreach (DataGridViewRow fila_235JB in dgvReporte.SelectedRows)
            {
                System.Data.DataRowView vista_235JB = fila_235JB.DataBoundItem as System.Data.DataRowView;
                if (vista_235JB != null)
                    filas_235JB.Add(_reporte_235JB.Datos.Rows.IndexOf(vista_235JB.Row));
            }
            return filas_235JB;
        }

        private void btnGenerarReporte_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_reporte_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneReporte", "Seleccione el tipo de reporte que desea generar."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            // Solo las filas seleccionadas (o todo el reporte si no hay seleccion)
            ReporteInteligente aImprimir_235JB = _bll_235JB.FiltrarSeleccion_235JB(_reporte_235JB, FilasSeleccionadas_235JB());
            bool parcial_235JB = aImprimir_235JB != _reporte_235JB;

            using (SaveFileDialog dialogo_235JB = new SaveFileDialog())
            {
                dialogo_235JB.Title = Msg_235JB("msg_GuardarReporte_Titulo", "Guardar reporte");
                dialogo_235JB.Filter = "PDF (*.pdf)|*.pdf";
                dialogo_235JB.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dialogo_235JB.FileName = "Reporte_" + _reporte_235JB.Titulo.Replace(" ", "_").Replace("á", "a").Replace("é", "e")
                    .Replace("í", "i").Replace("ó", "o").Replace("ú", "u") + (parcial_235JB ? "_seleccion" : "") + "_" +
                    DateTime.Now.ToString("yyyyMMdd_HHmm") + ".pdf";
                if (dialogo_235JB.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    _bll_235JB.GenerarPDF_235JB(aImprimir_235JB, dialogo_235JB.FileName);
                    DialogResult abrir_235JB = MessageBox.Show(
                        string.Format(Msg_235JB("msg_ReporteGenerado", "Reporte generado: {0}\n\n¿Desea abrirlo?"), dialogo_235JB.FileName),
                        Msg_235JB("msg_Exito_Titulo", "Éxito"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (abrir_235JB == DialogResult.Yes && File.Exists(dialogo_235JB.FileName))
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(dialogo_235JB.FileName) { UseShellExecute = true });
                }
                catch (Exception ex_235JB)
                {
                    Aviso_235JB(Msg_235JB("msg_ErrorReporte", "No se pudo generar el reporte: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
                }
            }
        }
    }
}
