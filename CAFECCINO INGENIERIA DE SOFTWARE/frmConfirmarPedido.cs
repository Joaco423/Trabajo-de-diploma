using System;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
   
    public partial class frmConfirmarPedido : FormularioCU
    {
        private readonly BLLPedido _bllPedido_235JB = new BLLPedido();
        private readonly ContextoPedido _ctx_235JB;

        public frmConfirmarPedido(ContextoPedido ctx_235JB)
        {
            InitializeComponent();
            _ctx_235JB = ctx_235JB;
        }

        // CU04 paso 1: muestra el detalle del pedido con el total
        private void frmConfirmarPedido_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            TraducirAlAbrir_235JB();
            lblClienteInfo.Text = _ctx_235JB.Cliente == null ? string.Empty : _ctx_235JB.Cliente.Nombre + " " + _ctx_235JB.Cliente.Apellido;

            int total_235JB = _bllPedido_235JB.CalcularImporte_235JB(_ctx_235JB.Detalle);
            dgvDetalle.DataSource = _ctx_235JB.Detalle
                .Select(d_235JB => new { d_235JB.Codigo, d_235JB.Nombre, d_235JB.Precio, d_235JB.Cantidad, d_235JB.Subtotal })
                .ToList();
            lblTotalValor.Text = "$" + total_235JB;
            btnContinuar.Visible = false;
        }

        // CU04 pasos 2-6: conformidad del cliente, confirma el pedido (BLLPedido.ConfirmarPedido) y muestra pedido y factura
        private void btnConfirmar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            int total_235JB = _bllPedido_235JB.CalcularImporte_235JB(_ctx_235JB.Detalle);

            
            if (!Preguntar_235JB(string.Format(Msg_235JB("msg_ConformidadPedido", "Total del pedido: ${0}.\n¿El cliente está conforme con el pedido?"), total_235JB),
                    "msg_Conformidad_Titulo", "Conformidad del cliente"))
                return;

            try
            {
                _ctx_235JB.Factura = _bllPedido_235JB.ConfirmarPedido_235JB(_ctx_235JB.Cliente.DNI, _ctx_235JB.Detalle, out Pedido pedido_235JB);
                _ctx_235JB.Pedido = pedido_235JB;
            }
            // Regla: cliente con pedido en curso no puede confirmar otro
            catch (PedidoEnCursoException ex_235JB)
            {
         
                Aviso_235JB(string.Format(Msg_235JB("msg_PedidoEnCurso", "El cliente ya tiene un pedido en curso (pedido {0}). No puede realizar otro hasta que finalice su atención."),
                        ex_235JB.CodigoPedido),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                Terminar_235JB(ResultadoCU.Cancelado);
                return;
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorConfirmar", "No se pudo confirmar el pedido: ", ex_235JB);
                return;
            }

            txtResultado.Text =
                string.Format(Msg_235JB("msg_TextoConfirmacion", "Pedido {0} confirmado. Fecha: {1} {2}. Total: ${3}. Estado: {4}."),
                    _ctx_235JB.Pedido.CodigoPedido, _ctx_235JB.Pedido.Fecha.ToShortDateString(), _ctx_235JB.Pedido.Hora, _ctx_235JB.Pedido.Total, _ctx_235JB.Pedido.Estado)
                + Environment.NewLine +
                string.Format(Msg_235JB("msg_TextoFactura", "Factura N° {0} - Total: ${1} - Estado: {2}."),
                    _ctx_235JB.Factura.NumeroFactura, _ctx_235JB.Factura.Total, _ctx_235JB.Factura.Estado);

            Aviso_235JB(Msg_235JB("msg_PedidoConfirmado", "Pedido confirmado. Se generó la factura."),
                "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);

            
            btnConfirmar.Visible = false;
            btnModificar.Visible = false;
            btnCancelar.Visible = false;
            btnContinuar.Visible = true;
            Resultado = ResultadoCU.Completado;
        }

     
        // CU04 flujo 2.1: el cliente no esta conforme -> vuelve a CU03
        private void btnModificar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(ResultadoCU.Reintentar);
        }

        // CU04 paso 8: fin, continua CU01 (-> CU05)
        private void btnContinuar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(ResultadoCU.Completado);
        }

        private void btnCancelar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(ResultadoCU.Cancelado);
        }
    }
}
