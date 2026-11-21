using System;
using System.Data;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{

    public partial class frmProductos : Form, IObserver
    {
        private readonly BLLProducto _bllProducto_235JB = new BLLProducto();
        private readonly Traductor traductor_235JB = new Traductor();
        private const string Form__235JB = "frmProductos";

        public frmProductos()
        {
            InitializeComponent();

            // Los productos tambien cambian desde otras pantallas (activar un cambio en la bitacora, el stock de un pedido):
            // la grilla se actualiza sola
            BLLProducto.ProductosCambiados_235JB += ProductosCambiados_235JB;
            Disposed += (s_235JB, e_235JB) => BLLProducto.ProductosCambiados_235JB -= ProductosCambiados_235JB;
        }

        private void ProductosCambiados_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (IsDisposed || !IsHandleCreated)
                return;
            // Se actualiza cuando termina la operacion que hizo el cambio
            BeginInvoke((MethodInvoker)RefrescarAutomaticamente_235JB);
        }

        // Recarga la grilla. Si hay un producto elegido, sus campos pasan a mostrar los datos actuales (por ejemplo, los del
        // cambio anterior activado en la bitacora), asi Modificar no vuelve a guardar los datos viejos. Si la ventana esta
        // oculta no hace falta: se recarga al volver a mostrarse.
        private void RefrescarAutomaticamente_235JB()
        {
            if (IsDisposed || !Visible)
                return;

            int codigo_235JB;
            bool haySeleccionado_235JB = int.TryParse(txtCodigo.Text.Trim(), out codigo_235JB);
            if (!ActualizarGrilla_235JB(false))
                return;

            DataGridViewRow fila_235JB = haySeleccionado_235JB ? BuscarFila_235JB(codigo_235JB) : null;
            if (fila_235JB == null)
            {
                // Sin producto elegido (o ya no esta en la lista): como queda despues de Limpiar
                if (haySeleccionado_235JB)
                    LimpiarCampos_235JB();
                else
                    dgvProductos.ClearSelection();
                return;
            }

            DataGridViewColumn visible_235JB = dgvProductos.Columns.GetFirstColumn(DataGridViewElementStates.Visible);
            if (visible_235JB != null)
                dgvProductos.CurrentCell = fila_235JB.Cells[visible_235JB.Index];
            CargarCampos_235JB(fila_235JB);
        }

        private DataGridViewRow BuscarFila_235JB(int codigo_235JB)
        {
            foreach (DataGridViewRow fila_235JB in dgvProductos.Rows)
                if (!fila_235JB.IsNewRow && ObtenerTexto_235JB(fila_235JB, "Codigo") == codigo_235JB.ToString())
                    return fila_235JB;
            return null;
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

        private void frmProductos_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ActualizarGrilla_235JB();
        }

        private void frmProductos_VisibleChanged_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Visible)
            {
                traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
                ActualizarGrilla_235JB();
            }
        }

        private void frmProductos_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (e_235JB.CloseReason == CloseReason.UserClosing)
            {
                e_235JB.Cancel = true;
                Hide();
            }
        }

        private bool ActualizarGrilla_235JB(bool avisarErrores_235JB = true)
        {
            try
            {
                dgvProductos.DataSource = _bllProducto_235JB.Listar_235JB();
                return true;
            }
            catch (Exception ex_235JB)
            {
                if (avisarErrores_235JB)
                    Aviso_235JB(Msg_235JB("msg_ErrorInicializar", "Error al cargar los productos: ") + ex_235JB.Message,
                        "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
                return false;
            }
        }

        private void LimpiarCampos_235JB()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            chkDisponible.Checked = true;
            dgvProductos.ClearSelection();
        }

        private bool ValidarCampos_235JB(out string mensajeError_235JB, out int precio_235JB, out int stock_235JB)
        {
            precio_235JB = 0;
            stock_235JB = 0;

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                mensajeError_235JB = Msg_235JB("msg_CamposObligatorios", "Por favor, complete todos los campos (Nombre, Precio y Stock).");
                return false;
            }

            if (!int.TryParse(txtPrecio.Text.Trim(), out precio_235JB) || precio_235JB <= 0)
            {
                mensajeError_235JB = Msg_235JB("msg_PrecioInvalido", "El precio debe ser un número entero mayor a cero.");
                return false;
            }

            if (!int.TryParse(txtStock.Text.Trim(), out stock_235JB) || stock_235JB < 0)
            {
                mensajeError_235JB = Msg_235JB("msg_StockInvalido", "El stock debe ser un número entero mayor o igual a cero.");
                return false;
            }

            mensajeError_235JB = null;
            return true;
        }

        private void btnAgregar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            // Con un producto elegido de la grilla no se agrega otro: se lo modifica (si no, se duplicaria).
            if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                Aviso_235JB(Msg_235JB("msg_ProductoSeleccionado", "Hay un producto seleccionado. Use Modificar para cambiar su precio o stock, o Limpiar para cargar uno nuevo."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos_235JB(out string error_235JB, out int precio_235JB, out int stock_235JB))
            {
                Aviso_235JB(error_235JB, "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bllProducto_235JB.Registrar_235JB(new Producto
                {
                    Nombre = txtNombre.Text.Trim(),
                    Precio = precio_235JB,
                    Stock = stock_235JB,
                    Disponible = chkDisponible.Checked
                });
                Aviso_235JB(Msg_235JB("msg_ProductoAgregado", "Producto agregado correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_ProductoExistente")
            {
                Aviso_235JB(Msg_235JB("msg_ProductoExistente", "Ya existe un producto con ese nombre. Para cambiar su stock, selecciónelo en la grilla y use Modificar."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorRegistrar", "Error al registrar el producto: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!int.TryParse(txtCodigo.Text.Trim(), out int codigo_235JB))
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneProducto", "Seleccione de la grilla el producto que desea modificar o eliminar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarCampos_235JB(out string error_235JB, out int precio_235JB, out int stock_235JB))
            {
                Aviso_235JB(error_235JB, "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!_bllProducto_235JB.Modificar_235JB(new Producto
                {
                    Codigo = codigo_235JB,
                    Nombre = txtNombre.Text.Trim(),
                    Precio = precio_235JB,
                    Stock = stock_235JB,
                    Disponible = chkDisponible.Checked
                }))
                {
                    Aviso_235JB(Msg_235JB("msg_SinCambios", "No hay cambios para guardar: los datos son iguales a los ya registrados."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                Aviso_235JB(Msg_235JB("msg_ProductoModificado", "Producto modificado correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                LimpiarCampos_235JB();
                ActualizarGrilla_235JB();
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_ProductoExistente")
            {
                Aviso_235JB(Msg_235JB("msg_ProductoExistente", "Ya existe un producto con ese nombre. Para cambiar su stock, selecciónelo en la grilla y use Modificar."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorModificar", "Error al modificar el producto: ") + ex_235JB.Message,
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        

        private void btnLimpiar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            LimpiarCampos_235JB();
        }

        private void dgvProductos_CellClick_235JB(object sender_235JB, DataGridViewCellEventArgs e_235JB)
        {
            if (e_235JB.RowIndex < 0 || dgvProductos.CurrentRow == null || dgvProductos.CurrentRow.IsNewRow)
                return;

            try
            {
                CargarCampos_235JB(dgvProductos.CurrentRow);
            }
            catch (Exception)
            {
                Aviso_235JB(Msg_235JB("msg_ErrorSeleccion", "No se pudo leer la fila seleccionada."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
        }

        private void CargarCampos_235JB(DataGridViewRow fila_235JB)
        {
            txtCodigo.Text = ObtenerTexto_235JB(fila_235JB, "Codigo");
            txtNombre.Text = ObtenerTexto_235JB(fila_235JB, "Nombre");
            txtPrecio.Text = ObtenerTexto_235JB(fila_235JB, "Precio");
            txtStock.Text = ObtenerTexto_235JB(fila_235JB, "Stock");
            object disponible_235JB = fila_235JB.Cells["Disponible"].Value;
            chkDisponible.Checked = disponible_235JB != null && disponible_235JB != DBNull.Value && Convert.ToBoolean(disponible_235JB);
        }

        private static string ObtenerTexto_235JB(DataGridViewRow fila_235JB, string columna_235JB)
        {
            object valor_235JB = fila_235JB.Cells[columna_235JB].Value;
            return (valor_235JB == null || valor_235JB == DBNull.Value) ? string.Empty : valor_235JB.ToString();
        }
    }
}
