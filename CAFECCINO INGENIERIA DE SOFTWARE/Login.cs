using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class Login : Form
    {
        BLLUsuario NegociosUsuario = new BLLUsuario();
        BLLEvento bitacora = new BLLEvento();
        public Login()
        {
            InitializeComponent();
        }
      

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtContraseña.Text))
            {
                MessageBox.Show("Llene todos los espacios");
                return;
            }
            if (Singleton.ObtenerInstancia().Usuario != null)
            {
                MessageBox.Show("Ya hay una sesión activa.");
                return;
            }
            Usuario user = new Usuario
            {
                Nombre = txtNombre.Text,
                Contraseña = txtContraseña.Text
            };
            bool loginExitoso = NegociosUsuario.Login(user);
            if (loginExitoso)
            {
             
                Singleton.ObtenerInstancia().IniciarSesion(user);
                

                bitacora.RegistrarEvento(
                    txtNombre.Text,
                    "Inicio de sesión exitoso",
                    "Login",
                    "Baja");

                ModificarMenu();
                MessageBox.Show("Inicio de sesión exitoso.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
            else
            {
                
                MessageBox.Show("Nombre de usuario o contraseña incorrectos.", "Error de autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();

            }
        }
        public void ModificarMenu()
        {
            frmInicio parent = this.MdiParent as frmInicio;
            parent.cerrarSesionToolStripMenuItem.Visible = true;
            parent.administradorToolStripMenuItem.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BLLUsuario bll = new BLLUsuario();

            bll.ActualizarPasswordHash(
                txtNombre.Text,
                txtContraseña.Text);

            MessageBox.Show("Contraseña encriptada");
        }
    }
}
