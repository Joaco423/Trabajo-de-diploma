using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    // Como termino la pantalla de un caso de uso.
    public enum ResultadoCU
    {
        Cancelado,   // el empleado salio sin completar el caso de uso
        Completado,  // el caso de uso termino bien y el flujo continua
        Reintentar,  // el cliente no esta conforme: se vuelve a SeleccionarProductos (flujo 2.1 de ConfirmarPedido)
        EnEspera     // el cliente quedo en una lista de espera
    }

    // Datos que van pasando de una pantalla a la siguiente mientras se gestiona un pedido.
    public class ContextoPedido
    {
        public Cliente Cliente { get; set; }
        public List<DetallePedido> Detalle { get; } = new List<DetallePedido>();
        public Pedido Pedido { get; set; }
        public Factura Factura { get; set; }
        public int? NumeroMesa { get; set; }
        public ResumenAtencion Resumen { get; set; }
        public bool PedidoCancelado { get; set; }

        // Mesa elegida por cada pedido que espera mozo. La mesa recien se guarda (en Atencion) al asignar el mozo,
        // asi que se recuerda mientras el sistema esta abierto.
        public static readonly Dictionary<int, int> MesaPorPedido_235JB = new Dictionary<int, int>();
    }

    // Base de las pantallas de los casos de uso (ventanas de dialogo): traduccion, mensajes y resultado.
    public class FormularioCU : Form
    {
        protected readonly Traductor traductor_235JB = new Traductor();

        public ResultadoCU Resultado { get; protected set; } = ResultadoCU.Cancelado;

        public FormularioCU()
        {
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
        }

        protected string Msg_235JB(string clave_235JB, string original_235JB)
        {
            return traductor_235JB.ObtenerMensaje(Name, clave_235JB, original_235JB);
        }

        protected void Aviso_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB, MessageBoxIcon icono_235JB)
        {
            MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        protected void Error_235JB(string clave_235JB, string original_235JB, Exception ex_235JB)
        {
            Aviso_235JB(Msg_235JB(clave_235JB, original_235JB) + ex_235JB.Message, "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
        }

        protected bool Preguntar_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB)
        {
            return MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        protected void TraducirAlAbrir_235JB()
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
        }

        protected void Terminar_235JB(ResultadoCU resultado_235JB)
        {
            Resultado = resultado_235JB;
            DialogResult = resultado_235JB == ResultadoCU.Cancelado ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }
    }
}
