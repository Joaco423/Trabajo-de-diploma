using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    
    public partial class frmIniciarRegistro : Form
    {
        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();

        public int DNI { get; private set; }

        // null si el cliente todavia no esta registrado
        public Cliente ClienteExistente { get; private set; }

        public frmIniciarRegistro()
        {
            InitializeComponent();
        }

        private void frmIniciarRegistro_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
        }

        private void btnConsultarDNI_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            string texto_235JB = txtDNI.Text.Trim();
            if (!Regex.IsMatch(texto_235JB, @"^\d{8}$"))
            {
                
                Aviso_235JB(Msg_235JB("msg_DniInvalido", "El DNI debe tener exactamente 8 dígitos numéricos."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int dni_235JB = int.Parse(texto_235JB);
                Cliente existente_235JB = _bllCliente_235JB.ConsultarRegistro_235JB(dni_235JB);
                DNI = dni_235JB;

                if (existente_235JB == null)
                {

                    ClienteExistente = null;
                    Aviso_235JB(Msg_235JB("msg_ClienteNoRegistrado", "El cliente no está registrado. A continuación se cargan sus datos personales."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    Terminar_235JB(DialogResult.OK);
                    return;
                }

                if (!existente_235JB.Activo)
                {
                    
                    Aviso_235JB(Msg_235JB("msg_ClienteInactivo", "El cliente está registrado pero su cuenta está inactiva. Reactívela desde Maestros > Clientes."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    Terminar_235JB(DialogResult.Cancel);
                    return;
                }

                
                ClienteExistente = existente_235JB;
                Aviso_235JB(Msg_235JB("msg_ClienteYaRegistrado", "El cliente ya está registrado y cuenta con una cuenta activa."),
                    "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                Terminar_235JB(DialogResult.OK);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorConsultar", "Error al consultar el cliente: ", ex_235JB);
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
