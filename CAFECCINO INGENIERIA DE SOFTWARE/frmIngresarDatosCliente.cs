using System;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    // CU IngresarDatosCliente (RFN2): se cargan los datos personales y se validan. Todavia no se guarda el cliente.
    public partial class frmIngresarDatosCliente : Form
    {
        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();
        private readonly int _dni_235JB;

        // Datos ya validados; se usan en el CU RegistrarCliente
        public Cliente Datos { get; private set; }

        public frmIngresarDatosCliente(int dni_235JB)
        {
            InitializeComponent();
            _dni_235JB = dni_235JB;
            txtDNI.Text = dni_235JB.ToString();
        }

        private void frmIngresarDatosCliente_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
        }

        private static string TextoDeError_235JB(string clave_235JB)
        {
            switch (clave_235JB)
            {
                case "msg_DniInvalido": return "El DNI debe tener exactamente 8 dígitos numéricos.";
                case "msg_NombreInvalido": return "El nombre solo puede contener letras y espacios.";
                case "msg_ApellidoInvalido": return "El apellido solo puede contener letras y espacios.";
                case "msg_MailInvalido": return "El mail ingresado no tiene un formato válido.";
                case "msg_TelefonoInvalido": return "El teléfono debe ser un número de entre 6 y 10 dígitos (hasta 2147483647).";
                default: return "Por favor, complete todos los campos (Nombre, Apellido, Dirección, Mail y Teléfono).";
            }
        }

        private void btnIngresarDatos_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            int telefono_235JB;
            if (!int.TryParse(txtTelefono.Text.Trim(), out telefono_235JB))
                telefono_235JB = 0;

            Cliente cliente_235JB = new Cliente
            {
                DNI = _dni_235JB,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Mail = txtMail.Text.Trim(),
                NumTelefono = telefono_235JB,
                Domicilio = txtDomicilio.Text.Trim(),
                Activo = true
            };

            try
            {
                string claveError_235JB;
                if (!_bllCliente_235JB.IngresarDatos_235JB(cliente_235JB, out claveError_235JB))
                {
                    // Flujo 4.1: falta un dato o es incorrecto: se indica cual corregir y se vuelve al paso 3.
                    Aviso_235JB(Msg_235JB(claveError_235JB, TextoDeError_235JB(claveError_235JB)),
                        "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                    return;
                }

                Datos = cliente_235JB;
                Terminar_235JB(DialogResult.OK);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorValidar", "Error al validar los datos: ", ex_235JB);
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
