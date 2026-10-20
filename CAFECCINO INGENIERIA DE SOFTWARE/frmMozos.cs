using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{

    public partial class frmMozos : Form, IObserver
    {
        private readonly BLLMozo _bllMozo_235JB = new BLLMozo();
        private readonly Traductor traductor_235JB = new Traductor();
        private const string Form__235JB = "frmMozos";

        public frmMozos()
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

        private void frmMozos_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ActualizarGrilla_235JB();
        }

        private void frmMozos_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                ActualizarGrilla_235JB();
            }
        }

        private void frmMozos_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        private void ActualizarGrilla_235JB()
        {
            try
            {
                dgvMozos.DataSource = _bllMozo_235JB.Listar_235JB();
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorInicializar", "Error al cargar los mozos: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos_235JB()
        {
            txtDNI.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtMail.Clear();
            txtTelefono.Clear();
            chkActivo.Checked = true;
            txtDNI.Enabled = true;
            dgvMozos.ClearSelection();
        }

        private static bool EsNombreValido_235JB(string texto_235JB)
        {
            return Regex.IsMatch(texto_235JB, @"^[A-Za-zÀ-ÿ]+(\s[A-Za-zÀ-ÿ]+)*$");
        }

        private bool ValidarCampos_235JB(out string mensajeError_235JB, out int telefono_235JB)
        {
            telefono_235JB = 0;

            if (string.IsNullOrWhiteSpace(txtDNI.Text) || string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) || string.IsNullOrWhiteSpace(txtMail.Text) ||
                string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                mensajeError_235JB = Msg_235JB("msg_CamposObligatorios", "Por favor, complete todos los campos (DNI, Nombre, Apellido, Mail y Teléfono).");
                return false;
            }

            if (!Regex.IsMatch(txtDNI.Text.Trim(), @"^\d{8}$"))
            {
                mensajeError_235JB = Msg_235JB("msg_DniInvalido", "El DNI debe tener exactamente 8 dígitos numéricos.");
                return false;
            }

            if (!EsNombreValido_235JB(txtNombre.Text.Trim()))
            {
                mensajeError_235JB = Msg_235JB("msg_NombreInvalido", "El nombre solo puede contener letras y espacios.");
                return false;
            }

            if (!EsNombreValido_235JB(txtApellido.Text.Trim()))
            {
                mensajeError_235JB = Msg_235JB("msg_ApellidoInvalido", "El apellido solo puede contener letras y espacios.");
                return false;
            }

            if (!Regex.IsMatch(txtMail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                mensajeError_235JB = Msg_235JB("msg_MailInvalido", "El mail ingresado no tiene un formato válido.");
                return false;
            }

            if (!Regex.IsMatch(txtTelefono.Text.Trim(), @"^\d{6,10}$") || !int.TryParse(txtTelefono.Text.Trim(), out telefono_235JB))
            {
                mensajeError_235JB = Msg_235JB("msg_TelefonoInvalido", "El teléfono debe ser un número de entre 6 y 10 dígitos (hasta 2147483647).");
                return false;
            }

            mensajeError_235JB = null;
            return true;
        }

        private Mozo LeerMozo_235JB(int telefono_235JB)
        {
            return new Mozo
            {
                DNIMozo = int.Parse(txtDNI.Text.Trim()),
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Mail = txtMail.Text.Trim(),
                NumTelefono = telefono_235JB,
                Activo = chkActivo.Checked
            };
        }

        private void btnAgregar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!ValidarCampos_235JB(out string error_235JB, out int telefono_235JB))
            {
                Aviso_235JB(error_235JB, "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bllMozo_235JB.Registrar_235JB(LeerMozo_235JB(telefono_235JB));
                Aviso_235JB(Msg_235JB("msg_MozoAgregado", "Mozo agregado correctamente. Ya está disponible para asignarlo."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoExistente", "Ya existe un mozo con ese DNI."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorRegistrar", "Error al registrar el mozo: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!ValidarCampos_235JB(out string error_235JB, out int telefono_235JB))
            {
                Aviso_235JB(error_235JB, "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!_bllMozo_235JB.Modificar_235JB(LeerMozo_235JB(telefono_235JB)))
                {
                    Aviso_235JB(Msg_235JB("msg_SinCambios", "No hay cambios para guardar: los datos son iguales a los ya registrados."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }
                Aviso_235JB(Msg_235JB("msg_MozoModificado", "Mozo modificado correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (EntidadEnUsoException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoEnUso", "No se puede desactivar el mozo porque está atendiendo un pedido."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoInexistente", "No existe un mozo con ese DNI. Seleccione uno de la grilla."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorModificar", "Error al modificar el mozo: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!int.TryParse(txtDNI.Text.Trim(), out int dni_235JB))
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMozo", "Seleccione de la grilla el mozo que desea desactivar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(Msg_235JB("msg_ConfirmarEliminar", "¿Desea desactivar al mozo? Se conserva su historial y no se lo podrá asignar hasta que se lo reactive."),
                    Msg_235JB("msg_Confirmar_Titulo", "Confirmar"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _bllMozo_235JB.Desactivar_235JB(dni_235JB);
                Aviso_235JB(Msg_235JB("msg_MozoDesactivado", "Mozo desactivado correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoEnUso", "No se puede desactivar el mozo porque está atendiendo un pedido."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorEliminar", "Error al desactivar el mozo: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            LimpiarCampos_235JB();
        }

        private void dgvMozos_CellClick_235JB(object sender_235JB, DataGridViewCellEventArgs e_235JB)
        {
            if (e_235JB.RowIndex < 0 || dgvMozos.CurrentRow == null || dgvMozos.CurrentRow.IsNewRow)
                return;

            try
            {
                DataGridViewRow fila_235JB = dgvMozos.CurrentRow;
                txtDNI.Text = ObtenerTexto_235JB(fila_235JB, "DNIMozo");
                txtNombre.Text = ObtenerTexto_235JB(fila_235JB, "Nombre");
                txtApellido.Text = ObtenerTexto_235JB(fila_235JB, "Apellido");
                txtMail.Text = ObtenerTexto_235JB(fila_235JB, "Mail");
                txtTelefono.Text = ObtenerTexto_235JB(fila_235JB, "NumTelefono");
                chkActivo.Checked = ObtenerTexto_235JB(fila_235JB, "Activo") == "True";
                txtDNI.Enabled = false;
            }
            catch (Exception)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorSeleccion", "No se pudo leer la fila seleccionada."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private static string ObtenerTexto_235JB(DataGridViewRow fila_235JB, string columna_235JB)
        {
            object valor_235JB = fila_235JB.Cells[columna_235JB].Value;
            return (valor_235JB == null || valor_235JB == DBNull.Value) ? string.Empty : valor_235JB.ToString();
        }
    }
}
