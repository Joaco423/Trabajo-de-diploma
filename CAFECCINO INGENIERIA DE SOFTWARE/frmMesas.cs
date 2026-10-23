using System;
using System.Data;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class frmMesas : Form, IObserver
    {
        private readonly BLLMesa _bllMesa_235JB = new BLLMesa();
        private readonly Traductor traductor_235JB = new Traductor();
        private const string Form__235JB = "frmMesas";

        public frmMesas()
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

        private void frmMesas_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ActualizarGrilla_235JB();
        }

        private void frmMesas_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                ActualizarGrilla_235JB();
            }
        }

        private void frmMesas_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
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
                dgvMesas.DataSource = _bllMesa_235JB.Listar_235JB();
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorInicializar", "Error al cargar las mesas: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos_235JB()
        {
            txtNumero.Clear();
            txtCapacidad.Clear();
            txtNumero.Enabled = true;
            dgvMesas.ClearSelection();
        }

        private bool ValidarCampos_235JB(out string mensajeError_235JB, out int numero_235JB, out int capacidad_235JB)
        {
            numero_235JB = 0;
            capacidad_235JB = 0;

            if (string.IsNullOrWhiteSpace(txtNumero.Text) || string.IsNullOrWhiteSpace(txtCapacidad.Text))
            {
                mensajeError_235JB = Msg_235JB("msg_CamposObligatorios", "Por favor, complete todos los campos (Número de mesa y Capacidad).");
                return false;
            }

            if (!int.TryParse(txtNumero.Text.Trim(), out numero_235JB) || numero_235JB < 1 || numero_235JB > 999)
            {
                mensajeError_235JB = Msg_235JB("msg_NumeroInvalido", "El número de mesa debe ser un entero entre 1 y 999.");
                return false;
            }

            if (!int.TryParse(txtCapacidad.Text.Trim(), out capacidad_235JB) || capacidad_235JB < 1 || capacidad_235JB > 20)
            {
                mensajeError_235JB = Msg_235JB("msg_CapacidadInvalida", "La capacidad debe ser un entero entre 1 y 20.");
                return false;
            }

            mensajeError_235JB = null;
            return true;
        }

        private void btnAgregar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            // Con una mesa elegida de la grilla no se agrega otra: se la modifica.
            if (!txtNumero.Enabled)
            {
                Aviso_235JB(Msg_235JB("msg_MesaSeleccionada", "Hay una mesa seleccionada. Use Modificar para cambiar su capacidad, o Limpiar para cargar una nueva."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos_235JB(out string error_235JB, out int numero_235JB, out int capacidad_235JB))
            {
                Aviso_235JB(error_235JB, "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bllMesa_235JB.Registrar_235JB(new Mesa { NumeroMesa = numero_235JB, Capacidad = capacidad_235JB });
                Aviso_235JB(Msg_235JB("msg_MesaAgregada", "Mesa agregada correctamente. Ya está libre para asignarla."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaExistente", "Ya existe una mesa con ese número."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorRegistrar", "Error al registrar la mesa: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!ValidarCampos_235JB(out string error_235JB, out int numero_235JB, out int capacidad_235JB))
            {
                Aviso_235JB(error_235JB, "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!_bllMesa_235JB.Modificar_235JB(new Mesa { NumeroMesa = numero_235JB, Capacidad = capacidad_235JB }))
                {
                    Aviso_235JB(Msg_235JB("msg_SinCambios", "No hay cambios para guardar: los datos son iguales a los ya registrados."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                Aviso_235JB(Msg_235JB("msg_MesaModificada", "Mesa modificada correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaInexistente", "No existe una mesa con ese número. Seleccione una de la grilla."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorModificar", "Error al modificar la mesa: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!int.TryParse(txtNumero.Text.Trim(), out int numero_235JB) || txtNumero.Enabled)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMesa", "Seleccione de la grilla la mesa que desea eliminar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(Msg_235JB("msg_ConfirmarEliminar", "¿Desea eliminar la mesa?"),
                    Msg_235JB("msg_Confirmar_Titulo", "Confirmar"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _bllMesa_235JB.Eliminar_235JB(numero_235JB);
                Aviso_235JB(Msg_235JB("msg_MesaEliminada", "Mesa eliminada correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (EntidadEnUsoException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaOcupada", "No se puede eliminar la mesa porque está ocupada."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaConAtenciones", "No se puede eliminar la mesa porque tiene atenciones registradas."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorEliminar", "Error al eliminar la mesa: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            LimpiarCampos_235JB();
        }

        private void dgvMesas_CellClick_235JB(object sender_235JB, DataGridViewCellEventArgs e_235JB)
        {
            if (e_235JB.RowIndex < 0 || dgvMesas.CurrentRow == null || dgvMesas.CurrentRow.IsNewRow)
                return;

            try
            {
                DataGridViewRow fila_235JB = dgvMesas.CurrentRow;
                txtNumero.Text = ObtenerTexto_235JB(fila_235JB, "NumeroMesa");
                txtCapacidad.Text = ObtenerTexto_235JB(fila_235JB, "Capacidad");
                txtNumero.Enabled = false;
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
