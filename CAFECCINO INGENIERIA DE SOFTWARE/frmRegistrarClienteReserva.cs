using System;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    
    public partial class frmRegistrarClienteReserva : Form
    {
        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();
        private readonly Cliente _datos_235JB;

        
        public frmRegistrarClienteReserva(Cliente datos_235JB)
        {
            InitializeComponent();
            _datos_235JB = datos_235JB;
        }

        private void frmRegistrarClienteReserva_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
            if (_datos_235JB == null) return;

            txtResumen.Text =
                "DNI: " + _datos_235JB.DNI + Environment.NewLine +
                "Nombre" + ": " + _datos_235JB.Nombre + Environment.NewLine +
                "Apellido" + ": " + _datos_235JB.Apellido + Environment.NewLine +
                "Dirección" + ": " + _datos_235JB.Domicilio + Environment.NewLine +
                "Mail: " + _datos_235JB.Mail + Environment.NewLine +
                "Teléfono" + ": " + _datos_235JB.NumTelefono;
        }

        private void btnRegistrar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_datos_235JB == null)
                return;

            try
            {
                _bllCliente_235JB.Registrar_235JB(_datos_235JB);

                Aviso_235JB(string.Format(Msg_235JB("msg_ClienteRegistrado", "Cliente registrado correctamente. Código de cliente: {0}. Su cuenta quedó activa."), _datos_235JB.CodigoCliente),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                Terminar_235JB(DialogResult.OK);
            }
            catch (InvalidOperationException)
            {
                
                Aviso_235JB(Msg_235JB("msg_ClienteExistente", "Ya existe un cliente con ese DNI."),
                    "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
            }
            catch (Exception ex_235JB)
            {
                
                Error_235JB("msg_ErrorRegistrar", "Error al registrar el cliente: ", ex_235JB);
            }
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
