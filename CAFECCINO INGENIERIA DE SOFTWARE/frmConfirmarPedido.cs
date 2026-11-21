using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
   
    public partial class frmConfirmarPedido : Form
    {
        private readonly BLLPedido _bllPedido_235JB = new BLLPedido();
        private readonly Cliente _cliente_235JB;
        private readonly List<DetallePedido> _detalle_235JB;

        // Se completan al confirmar el pedido
        public Pedido Pedido { get; private set; }

        public frmConfirmarPedido(Cliente cliente_235JB, List<DetallePedido> detalle_235JB)
        {
            InitializeComponent();
            _cliente_235JB = cliente_235JB;
            _detalle_235JB = detalle_235JB;
        }

        
        private void frmConfirmarPedido_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
            lblClienteInfo.Text = _cliente_235JB == null ? string.Empty : _cliente_235JB.Nombre + " " + _cliente_235JB.Apellido;

            int total_235JB = _bllPedido_235JB.CalcularImporte_235JB(_detalle_235JB);
            dgvDetalle.DataSource = _detalle_235JB
                .Select(d_235JB => new { d_235JB.Codigo, d_235JB.Nombre, d_235JB.Precio, d_235JB.Cantidad, d_235JB.Subtotal })
                .ToList();
            lblTotalValor.Text = "$" + total_235JB;
            btnContinuar.Visible = false;
        }

        
        private void btnConfirmar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            int total_235JB = _bllPedido_235JB.CalcularImporte_235JB(_detalle_235JB);

            
            if (!Preguntar_235JB(string.Format(Msg_235JB("msg_ConformidadPedido", "Total del pedido: ${0}.\n¿El cliente está conforme con el pedido?"), total_235JB),
                    "msg_Conformidad_Titulo", "Conformidad del cliente"))
                return;

            try
            {
                Pedido = _bllPedido_235JB.ConfirmarPedido_235JB(_cliente_235JB.DNI, _detalle_235JB);
            }
            
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_PedidoEnCurso")
            {
         
                Aviso_235JB(string.Format(Msg_235JB("msg_PedidoEnCurso", "El cliente ya tiene un pedido en curso (pedido {0}). No puede realizar otro hasta que finalice su atención."),
                        ex_235JB.Data["CodigoPedido"]),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                Terminar_235JB(DialogResult.Cancel);
                return;
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_EnEsperaDeReserva")
            {
                Aviso_235JB(Msg_235JB("msg_EnEsperaDeReserva", "El cliente está en la lista de espera para reservar una mesa y no puede ser atendido hasta que se le asigne su reserva."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                Terminar_235JB(DialogResult.Cancel);
                return;
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorConfirmar", "No se pudo confirmar el pedido: ", ex_235JB);
                return;
            }

            txtResultado.Text =
                string.Format(Msg_235JB("msg_TextoConfirmacion", "Pedido {0} confirmado. Fecha: {1} {2}. Total: ${3}. Estado: {4}."),
                    Pedido.CodigoPedido, Pedido.Fecha.ToShortDateString(), Pedido.Hora, Pedido.Total, Pedido.Estado)
                + Environment.NewLine +
                string.Format(Msg_235JB("msg_TextoDetallePedido", "Detalle del pedido: {0}."),
                    string.Join(", ", _detalle_235JB.Select(d_235JB => d_235JB.Nombre + " x" + d_235JB.Cantidad + " ($" + d_235JB.Subtotal + ")")));

            Aviso_235JB(Msg_235JB("msg_PedidoConfirmado", "Pedido confirmado."),
                "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);

            
            btnConfirmar.Visible = false;
            btnModificar.Visible = false;
            btnCancelar.Visible = false;
            btnContinuar.Visible = true;
            Resultado = DialogResult.OK;
        }

     
        
        private void btnModificar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(DialogResult.Retry);
        }

        
        private void btnContinuar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
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

        private bool Preguntar_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB)
        {
            return MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void Terminar_235JB(DialogResult resultado_235JB)
        {
            Resultado = resultado_235JB;
            DialogResult = resultado_235JB == DialogResult.Cancel ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }
    }
}
