using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class frmVerificacionIntegridad : Form, IObserver
    {
        private readonly BLLIntegridad _integridadBLL = new BLLIntegridad();
        private readonly BLLBackup _backupBLL = new BLLBackup();
        private Traductor traductor = new Traductor();

        public frmVerificacionIntegridad()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(string idioma)
        {
            traductor.TraducirFormulario(this, int.Parse(idioma));
        }

        private void frmVerificacionIntegridad_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                traductor.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                ActualizarEstado();
            }
        }

        private void ActualizarEstado()
        {
            bool esAdmin = Singleton.Instancia.EsAdministrador();

            btnRecalcular.Enabled = esAdmin;
            btnRestaurar.Enabled = esAdmin && _backupBLL.ExisteBackupDisponible();

            lblAviso.Text = esAdmin
                ? ""
                : traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_RequiereAdmin", "Debe iniciar sesión como administrador para restaurar o recalcular.");

            try
            {
                var tablasAlteradas = _integridadBLL.ObtenerTablasAlteradas();

                if (tablasAlteradas.Count == 0)
                {
                    lblEstado.Text = traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_EstadoOK", "Estado: OK. Ninguna tabla presenta alteraciones.");
                }
                else
                {
                    var lineas = tablasAlteradas.Select(kv => kv.Key + ": " + string.Join(", ", kv.Value));
                    lblEstado.Text = traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_EstadoAlterado", "Estado: ALTERADO.") + "\n" + string.Join("\n", lineas);
                }
            }
            catch (Exception ex)
            {
                lblEstado.Text = traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ErrorVerificar", "No se pudo verificar el estado: ") + ex.Message;
            }
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            if (!Singleton.Instancia.EsAdministrador())
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_SoloAdminRecalcular", "Solo un administrador puede recalcular los dígitos verificadores."));
                return;
            }

            DialogResult confirm = MessageBox.Show(
                traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ConfirmarRecalculo",
                    "Esto acepta el estado actual de la tabla Usuario como válido y genera un nuevo backup de la base de datos.\n\n¿Desea continuar?"),
                traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ConfirmarRecalculo_Titulo", "Confirmar recálculo"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _backupBLL.Recalcular();
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_RecalculoOK", "Dígitos recalculados y backup generado correctamente."));
            }
            catch (Exception ex)
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ErrorRecalcular", "Error al recalcular: ") + ex.Message);
            }

            ActualizarEstado();
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            if (!Singleton.Instancia.EsAdministrador())
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_SoloAdminRestaurar", "Solo un administrador puede restaurar la base de datos."));
                return;
            }

            if (!_backupBLL.ExisteBackupDisponible())
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_SinBackup", "No hay ningún backup generado todavía. Use 'Recalcular' primero para generar uno."));
                return;
            }

            DialogResult confirm = MessageBox.Show(
                traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ConfirmarRestauracion",
                    "Esto va a REEMPLAZAR toda la base de datos actual por el último backup generado.") + " (" + _backupBLL.ObtenerRutaBackup() + ").\n" +
                traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ConfirmarRestauracion2",
                    "Se perderá cualquier cambio posterior a ese backup y se cerrará la sesión actual.\n\n¿Desea continuar?"),
                traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ConfirmarRestauracion_Titulo", "Confirmar restauración"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _backupBLL.Restaurar();
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_RestauracionOK", "Base de datos restaurada correctamente desde el backup. Debe iniciar sesión nuevamente."));
                CerrarSesionYPedirLogin();
            }
            catch (Exception ex)
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmVerificacionIntegridad", "msg_ErrorRestaurar", "Error al restaurar: ") + ex.Message);
                ActualizarEstado();
            }
        }

        private void CerrarSesionYPedirLogin()
        {
            Singleton.Instancia.CerrarSesion();

            frmInicio parent = this.MdiParent as frmInicio;
            if (parent != null)
            {
                parent.cerrarSesionToolStripMenuItem.Visible = false;
                parent.administradorToolStripMenuItem.Visible = false;
                parent.verificacionIntegridadToolStripMenuItem.Visible = false;

                parent.gestionDeFamilias.Hide();
                parent.gestionDePerfiles.Hide();
                parent.administradorIdiomas.Hide();

                Hide();

                parent.login.MdiParent = parent;
                parent.login.Show();
            }
            else
            {
                Hide();
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void frmVerificacionIntegridad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void frmVerificacionIntegridad_Load(object sender, EventArgs e)
        {

        }
    }
}
