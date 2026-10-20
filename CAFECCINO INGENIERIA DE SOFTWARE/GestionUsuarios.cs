using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class GestionUsuarios : Form, IObserver
    {
        private BLLBitacoraUsuario _bllBitacora = new BLLBitacoraUsuario();
        BLLUsuario bllUsuario = new BLLUsuario();
        BLLPermiso _permisoBLL = new BLLPermiso();
        Traductor traductor = new Traductor();
       
        public GestionUsuarios()
        {
            InitializeComponent();
        }
        public void ActualizarIdioma(string idioma)
        {
            

            traductor.TraducirFormulario(this, int.Parse(idioma));
        }
        private void Agregar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out string mensajeError, out string tituloError))
            {
                MessageBox.Show(mensajeError, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int dni = int.Parse(textBox1.Text.Trim());
            int.TryParse(textBox7.Text.Trim(), out int telefono);

            try
            {

                Usuario usuario = new Usuario();
                usuario.DNI = dni;
                usuario.NombreUsuario = textBox2.Text.Trim();
                usuario.Contraseña = textBox3.Text.Trim(); 
                usuario.Nombre = textBox5.Text.Trim();
                usuario.Apellido = textBox6.Text.Trim();
                usuario.Mail = textBox4.Text.Trim();
                usuario.NumTelefono = telefono;
                usuario.Rol = CBRoles.Text;


                Perfil rolSeleccionado = (Perfil)CBRoles.SelectedItem;

                
                bllUsuario.RegistrarUsuario(usuario, rolSeleccionado.Id);
                bllUsuario.SincronizarDigitos();
               
                MessageBox.Show(
                    traductor.ObtenerMensaje("GestionUsuarios", "msg_UsuarioAgregado", "Usuario agregado y rol asignado correctamente."),
                    traductor.ObtenerMensaje("GestionUsuarios", "msg_Exito_Titulo", "Éxito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);


                LimpiarCampos();
                actualizarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    traductor.ObtenerMensaje("GestionUsuarios", "msg_ErrorRegistrar", "Error al registrar el usuario: ") + ex.Message,
                    traductor.ObtenerMensaje("GestionUsuarios", "msg_Error_Titulo", "Error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarCampos()
            {
            textBox1.Clear(); textBox2.Clear(); textBox3.Clear();
            textBox4.Clear(); textBox5.Clear(); textBox6.Clear(); textBox7.Clear();
            CBRoles.SelectedIndex = -1;
            chkActivo.Checked = true;
            }

        private bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool EsDniValido(string dni)
        {
            return Regex.IsMatch(dni, @"^\d{8}$");
        }

        private bool EsTelefonoValido(string telefono)
        {
            return Regex.IsMatch(telefono, @"^\d{6,15}$");
        }

        private bool EsNombreValido(string texto)
        {
            return Regex.IsMatch(texto, @"^[A-Za-zÀ-ÿ]+(\s[A-Za-zÀ-ÿ]+)*$");
        }

        private bool EsUsuarioValido(string usuario)
        {
            return Regex.IsMatch(usuario, @"^\S{3,}$");
        }

        /// <summary>
        /// Valida todos los campos obligatorios del formulario. Devuelve false y el mensaje/título
        /// correspondiente (ya traducidos) ante el primer problema encontrado.
        /// </summary>
        private bool ValidarCampos(out string mensajeError, out string tituloError)
        {
            tituloError = traductor.ObtenerMensaje("GestionUsuarios", "msg_ErrorFormato_Titulo", "Error de formato");

            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) || string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrWhiteSpace(textBox5.Text) || string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox7.Text) || CBRoles.SelectedItem == null)
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_CamposObligatorios",
                    "Por favor, complete todos los campos (DNI, Usuario, Contraseña, Email, Nombre, Apellido, Teléfono y Rol).");
                tituloError = traductor.ObtenerMensaje("GestionUsuarios", "msg_Validacion_Titulo", "Validación");
                return false;
            }

            if (!EsDniValido(textBox1.Text.Trim()))
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_DniInvalido", "El DNI debe tener exactamente 8 dígitos numéricos.");
                return false;
            }

            if (!EsUsuarioValido(textBox2.Text.Trim()))
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_UsuarioInvalido", "El nombre de usuario debe tener al menos 3 caracteres y no contener espacios.");
                return false;
            }

            if (!EsEmailValido(textBox4.Text.Trim()))
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_EmailInvalido", "El email ingresado no tiene un formato válido.");
                return false;
            }

            if (!EsNombreValido(textBox5.Text.Trim()))
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_NombreInvalido", "El nombre solo puede contener letras y espacios.");
                return false;
            }

            if (!EsNombreValido(textBox6.Text.Trim()))
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_ApellidoInvalido", "El apellido solo puede contener letras y espacios.");
                return false;
            }

            if (!EsTelefonoValido(textBox7.Text.Trim()))
            {
                mensajeError = traductor.ObtenerMensaje("GestionUsuarios", "msg_TelefonoInvalido", "El teléfono debe contener entre 6 y 15 dígitos numéricos.");
                return false;
            }

            mensajeError = null;
            return true;
        }


        private void Modificar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out string mensajeError, out string tituloError))
            {
                MessageBox.Show(mensajeError, tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Usuario usuario = new Usuario();
            Usuario anterior = bllUsuario.ObtenerPorDNI(Convert.ToInt32(textBox1.Text));

            usuario.DNI = Convert.ToInt32(textBox1.Text);
            usuario.NombreUsuario = textBox2.Text;
            usuario.Contraseña = textBox3.Text;
            usuario.Nombre = textBox5.Text;
            usuario.Apellido = textBox6.Text;
            usuario.Mail = textBox4.Text;
            usuario.NumTelefono = Convert.ToInt32(textBox7.Text);
            usuario.Rol = CBRoles.Text;

            bllUsuario.ModificarUsuario(usuario);
            bllUsuario.SincronizarDigitos();
            _bllBitacora.RegistrarModificacion(anterior, usuario, Singleton.Instancia.Usuario.NombreUsuario);

            MessageBox.Show(traductor.ObtenerMensaje("GestionUsuarios", "msg_UsuarioModificado", "Usuario modificado correctamente."));

            actualizarGrilla();
        }

        private void Eliminar_Click(object sender, EventArgs e)
        {
            Usuario usuarioActual = bllUsuario.ObtenerPorDNI(Convert.ToInt32(textBox1.Text));
            bool activarlo = !usuarioActual.Activo;

            string mensajeConfirmacion = activarlo
                ? traductor.ObtenerMensaje("GestionUsuarios", "msg_ConfirmarActivar", "¿Desea reactivar al usuario?")
                : traductor.ObtenerMensaje("GestionUsuarios", "msg_ConfirmarDesactivar", "¿Desea desactivar al usuario? No podrá volver a iniciar sesión hasta que se lo reactive.");

            if (MessageBox.Show(
                mensajeConfirmacion,
                traductor.ObtenerMensaje("GestionUsuarios", "msg_Confirmar_Titulo", "Confirmar"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bllUsuario.ActualizarActivo(usuarioActual.DNI, activarlo);
                bllUsuario.SincronizarDigitos();

                MessageBox.Show(activarlo
                    ? traductor.ObtenerMensaje("GestionUsuarios", "msg_UsuarioActivado", "Usuario activado correctamente.")
                    : traductor.ObtenerMensaje("GestionUsuarios", "msg_UsuarioDesactivado", "Usuario desactivado correctamente."));

                chkActivo.Checked = activarlo;
                ActualizarBotonActivar(activarlo);
                actualizarGrilla();
            }
        }

        private void ActualizarBotonActivar(bool activo)
        {
            Eliminar.Text = activo
                ? traductor.ObtenerMensaje("GestionUsuarios", "msg_BotonDesactivar", "Desactivar Usuario")
                : traductor.ObtenerMensaje("GestionUsuarios", "msg_BotonActivar", "Activar Usuario");
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || !int.TryParse(textBox1.Text.Trim(), out int dni))
            {
                MessageBox.Show(
                    traductor.ObtenerMensaje("GestionUsuarios", "msg_SeleccioneUsuarioDesbloquear", "Seleccione de la grilla el usuario que desea desbloquear."),
                    traductor.ObtenerMensaje("GestionUsuarios", "msg_Validacion_Titulo", "Validación"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bllUsuario.Desbloquear(dni);

            MessageBox.Show(traductor.ObtenerMensaje("GestionUsuarios", "msg_UsuarioDesbloqueado", "Usuario desbloqueado correctamente."));
            actualizarGrilla();
        }
        private void actualizarGrilla()
        {
            dataGridView1.DataSource = bllUsuario.ListarUsuarios();

        }

        private void GestionUsuarios_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
          

        }

        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            try
            {
                LlenarComboRoles();
                actualizarGrilla();
                
            }
            catch (Exception)
            {
                MessageBox.Show(traductor.ObtenerMensaje("GestionUsuarios", "msg_ErrorInicializar", "Error al inicializar la pantalla"));
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.IsNewRow)
                return;

            try
            {
                DataGridViewRow fila = dataGridView1.CurrentRow;

                textBox1.Text = ObtenerTexto(fila, "DNI");
                textBox2.Text = ObtenerTexto(fila, "Username");
                textBox3.Text = ObtenerTexto(fila, "Contraseña");
                textBox4.Text = ObtenerTexto(fila, "EMail");
                textBox5.Text = ObtenerTexto(fila, "Nombre");
                textBox6.Text = ObtenerTexto(fila, "Apellido");
                textBox7.Text = ObtenerTexto(fila, "NumTelefono");
                CBRoles.Text = ObtenerTexto(fila, "Rol");

                bool activo = fila.Cells["Activo"].Value != null && fila.Cells["Activo"].Value != DBNull.Value
                    && Convert.ToBoolean(fila.Cells["Activo"].Value);
                chkActivo.Checked = activo;
                ActualizarBotonActivar(activo);
            }
            catch (Exception)
            {
                MessageBox.Show("Error. ");
            }
        }

        private string ObtenerTexto(DataGridViewRow fila, string columna)
        {
            object valor = fila.Cells[columna].Value;
            return (valor == null || valor == DBNull.Value) ? string.Empty : valor.ToString();
        }
        private void LlenarComboRoles()
        {
            
            CBRoles.DataSource = _permisoBLL.ObtenerPerfiles();
            CBRoles.DisplayMember = "Nombre";
            CBRoles.ValueMember = "Id";
            CBRoles.SelectedIndex = -1;
        }

      

        private void v(object sender, EventArgs e)
        {

        }

        private void GestionUsuarios_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                
                traductor.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
            }
        }
    }
}