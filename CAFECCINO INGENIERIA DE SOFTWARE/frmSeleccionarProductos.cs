using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{

    public partial class frmSeleccionarProductos : Form
    {
        private readonly BLLPedido _bllPedido_235JB = new BLLPedido();
        private readonly Cliente _cliente_235JB;
        private readonly List<DetallePedido> _detalle_235JB;

        // El detalle es la lista del pedido en armado: se completa por referencia
        public frmSeleccionarProductos(Cliente cliente_235JB, List<DetallePedido> detalle_235JB)
        {
            InitializeComponent();
            _cliente_235JB = cliente_235JB;
            _detalle_235JB = detalle_235JB;
            cboProducto.Format += cboProducto_Format_235JB;
        }

        private void frmSeleccionarProductos_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
            lblClienteInfo.Text = _cliente_235JB == null ? string.Empty : _cliente_235JB.Nombre + " " + _cliente_235JB.Apellido;
            CargarProductos_235JB();
            RefrescarDetalle_235JB();
        }

        private void cboProducto_Format_235JB(object sender_235JB, ListControlConvertEventArgs e_235JB)
        {
            Producto p_235JB = e_235JB.ListItem as Producto;
            if (p_235JB != null)
                e_235JB.Value = p_235JB.Nombre + " - $" + p_235JB.Precio;
        }

        // CU03 pasos 1 y 3: productos disponibles (codigo, nombre, precio)
        private void CargarProductos_235JB()
        {
            try
            {
                cboProducto.DataSource = _bllPedido_235JB.ObtenerProductosDisponibles_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCargarProductos", "Error al cargar los productos: ", ex_235JB);
            }
        }

        // CU03 paso 6: detalle del pedido con subtotales y total
        private void RefrescarDetalle_235JB()
        {
            int total_235JB = _bllPedido_235JB.CalcularImporte_235JB(_detalle_235JB);
            dgvDetalle.DataSource = null;
            dgvDetalle.DataSource = _detalle_235JB
                .Select(d_235JB => new { d_235JB.Codigo, d_235JB.Nombre, d_235JB.Precio, d_235JB.Cantidad, d_235JB.Subtotal })
                .ToList();
            lblTotalValor.Text = "$" + total_235JB;
            btnContinuar.Enabled = _detalle_235JB.Count > 0;
        }

        // CU03 pasos 2, 4 y 5: producto y cantidad -> BLLPedido.SeleccionarProducto valida y calcula
        private void btnAgregarProducto_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Producto producto_235JB = cboProducto.SelectedItem as Producto;
            if (producto_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneProducto", "Seleccione un producto."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bllPedido_235JB.SeleccionarProducto_235JB(_detalle_235JB, producto_235JB.Codigo, (int)nudCantidad.Value);
                RefrescarDetalle_235JB();
                nudCantidad.Value = 1;
            }
            // CU03 flujo 4.1: producto no disponible -> vuelve al paso 2
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_ProductoNoDisponible")
            {
              
                Aviso_235JB(string.Format(Msg_235JB("msg_ProductoNoDisponible", "El producto {0} no está disponible. Elija otro producto."), ex_235JB.Data["Nombre"]),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarProductos_235JB();
            }
            // CU03 flujo 4.2: stock insuficiente -> vuelve al paso 2
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_StockInsuficiente")
            {
                
                Aviso_235JB(string.Format(Msg_235JB("msg_StockInsuficiente", "No hay stock suficiente de {0} (stock disponible: {1}). Modifique la cantidad o elija otro producto."),
                        ex_235JB.Data["Nombre"], ex_235JB.Data["Stock"]),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarProductos_235JB();
            }
            // Integridad: si falla la verificacion, "Sistema caido" (el administrador queda exento)
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_SistemaCaido")
            {
                Aviso_235JB(Msg_235JB("msg_SistemaCaido", "Sistema caído. Contacte al administrador."),
                    "msg_ErrorSistema_Titulo", "Error del sistema", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAgregarProducto", "Error al agregar el producto: ", ex_235JB);
            }
        }

        private void btnQuitarProducto_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (dgvDetalle.CurrentRow == null || dgvDetalle.CurrentRow.IsNewRow)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneLinea", "Seleccione de la grilla el producto que desea quitar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            int codigo_235JB = Convert.ToInt32(dgvDetalle.CurrentRow.Cells["Codigo"].Value);
            _detalle_235JB.RemoveAll(d_235JB => d_235JB.Codigo == codigo_235JB);
            RefrescarDetalle_235JB();
        }

        // CU03 paso 7: fin, continua CU01 (-> CU04)
        private void btnContinuar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_detalle_235JB.Count == 0)
            {
                Aviso_235JB(Msg_235JB("msg_PedidoVacio", "El pedido no tiene productos."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }
            Terminar_235JB(DialogResult.OK);
        }

        private void btnCancelar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(DialogResult.Cancel);
        }

        private readonly Traductor traductor_235JB = new Traductor();

        public DialogResult Resultado { get; private set; } = DialogResult.Cancel;

        private string Msg_235JB(string clave_235JB, string original_235JB)
        {
            return traductor_235JB.ObtenerMensaje(Name, clave_235JB, original_235JB);
        }

        private void Aviso_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB, MessageBoxIcon icono_235JB)
        {
            MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        private void Error_235JB(string clave_235JB, string original_235JB, Exception ex_235JB)
        {
            Aviso_235JB(Msg_235JB(clave_235JB, original_235JB) + ex_235JB.Message, "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
        }

        private void Terminar_235JB(DialogResult resultado_235JB)
        {
            Resultado = resultado_235JB;
            DialogResult = resultado_235JB == DialogResult.Cancel ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }
    }
}
