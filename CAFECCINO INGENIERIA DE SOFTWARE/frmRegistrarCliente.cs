using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    
    public partial class frmRegistrarCliente : Form
    {
        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();

        // Cliente registrado (se completa al terminar el CU)
        public Cliente Cliente { get; private set; }

        public frmRegistrarCliente(int dni_235JB)
        {
            InitializeComponent();
            txtDNI.Text = dni_235JB.ToString();
        }

        // CU02 paso 1: se solicitan los datos del cliente
        private void frmRegistrarCliente_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
            Aviso_235JB(Msg_235JB("msg_ClienteNoRegistrado", "El cliente no está registrado. Complete sus datos para registrarlo."),
                "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
        }

        private void btnRegistrar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            // CU02 paso 3 / flujo 3.1: datos incompletos o con formato incorrecto -> vuelve al paso 2
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                Aviso_235JB(Msg_235JB("msg_NombreApellidoObligatorios", "Complete el nombre y el apellido del cliente."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMail.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text) || string.IsNullOrWhiteSpace(txtDomicilio.Text))
            {
                Aviso_235JB(Msg_235JB("msg_DatosContactoObligatorios", "Complete el mail, el teléfono y el domicilio del cliente."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtMail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Aviso_235JB(Msg_235JB("msg_MailInvalido", "El mail ingresado no tiene un formato válido."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            int telefono_235JB;
            if (!Regex.IsMatch(txtTelefono.Text.Trim(), @"^\d{6,10}$") || !int.TryParse(txtTelefono.Text.Trim(), out telefono_235JB))
            {
                Aviso_235JB(Msg_235JB("msg_TelefonoInvalido", "El teléfono debe ser un número de entre 6 y 10 dígitos (hasta 2147483647)."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            const string patron_235JB = @"^[A-Za-zÀ-ÿ]+(\s[A-Za-zÀ-ÿ]+)*$";
            if (!Regex.IsMatch(txtNombre.Text.Trim(), patron_235JB) || !Regex.IsMatch(txtApellido.Text.Trim(), patron_235JB))
            {
                Aviso_235JB(Msg_235JB("msg_NombreApellidoInvalidos", "El nombre y el apellido solo pueden contener letras y espacios."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Cliente nuevo_235JB = new Cliente
                {
                    DNI = int.Parse(txtDNI.Text),
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Mail = txtMail.Text.Trim(),
                    NumTelefono = telefono_235JB,
                    Domicilio = txtDomicilio.Text.Trim(),
                    Activo = true
                };
                // CU02 paso 4: guarda el cliente (BLLCliente: digito verificador + evento)
                _bllCliente_235JB.Registrar_235JB(nuevo_235JB);
                Cliente = nuevo_235JB;

                // CU02 pasos 5-6: informa que quedo registrado y continua CU01
                Aviso_235JB(Msg_235JB("msg_ClienteRegistrado", "Cliente registrado correctamente."),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                Terminar_235JB(DialogResult.OK);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorRegistrarCliente", "Error al registrar el cliente: ", ex_235JB);
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
