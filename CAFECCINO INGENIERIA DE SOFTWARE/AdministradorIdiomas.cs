using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using SERVICIOS;
using BE;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class AdministradorIdiomas : Form, IObserver
    {
        public BLLIdioma bllIdioma = new BLLIdioma();
        Traductor traductor = new Traductor();
        public AdministradorIdiomas()
        {
            InitializeComponent();
          
        }
        public void ActualizarIdioma(string idioma)
        {
            traductor.TraducirFormulario(this, int.Parse(idioma));
        }
        private void ConfigurarGrilla()
        {
            dgvIdiomas.AutoGenerateColumns = false;
            dgvIdiomas.Columns.Clear();

           
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.Name = "IdControl";
            colId.HeaderText = "Id";
            colId.DataPropertyName = "IdControl";
            colId.Visible = false;
            dgvIdiomas.Columns.Add(colId);

            
            DataGridViewTextBoxColumn colControl = new DataGridViewTextBoxColumn();
            colControl.Name = "Control";
            colControl.HeaderText = "Control";
            colControl.DataPropertyName = "Control";
            colControl.ReadOnly = true;
            dgvIdiomas.Columns.Add(colControl);

            
            DataGridViewTextBoxColumn colTextoOriginal = new DataGridViewTextBoxColumn();
            colTextoOriginal.Name = "TextoOriginal";
            colTextoOriginal.HeaderText = "Texto Original";
            colTextoOriginal.DataPropertyName = "TextoOriginal";
            colTextoOriginal.ReadOnly = true;
            dgvIdiomas.Columns.Add(colTextoOriginal);

            
            DataGridViewTextBoxColumn colTraduccion = new DataGridViewTextBoxColumn();
            colTraduccion.Name = "Traduccion";
            colTraduccion.HeaderText = "Traducción";
            colTraduccion.DataPropertyName = "Traduccion";
            dgvIdiomas.Columns.Add(colTraduccion);
        }
        private void AdministradorIdiomas_Load(object sender, EventArgs e)
        {
            ConfigurarGrilla();

            cmbIdioma.DataSource = bllIdioma.ListarIdiomas();
            cmbIdioma.DisplayMember = "Nombre";
            cmbIdioma.ValueMember = "IdIdioma";

            
            cmbFormulario.Items.Clear();
            cmbFormulario.Items.Add("Login");
            cmbFormulario.Items.Add("GestionUsuarios");
            cmbFormulario.Items.Add("frmEventos");
            cmbFormulario.Items.Add("frmInicio");
            cmbFormulario.Items.Add("AdministradorIdiomas");
            cmbFormulario.Items.Add("GestionDePerfiles"); 
            cmbFormulario.Items.Add("GestionDeFamilias");
            cmbFormulario.Items.Add("frmBitacoraCambios");
            cmbFormulario.Items.Add("frmCambiarContraseña");
            cmbFormulario.Items.Add("frmVerificacionIntegridad");
            cmbFormulario.Items.Add("frmClientes");
            cmbFormulario.Items.Add("frmProductos");
            cmbFormulario.Items.Add("frmMozos");
            cmbFormulario.Items.Add("frmGestionDePedido");
            cmbFormulario.Items.Add("frmRegistrarCliente");
            cmbFormulario.Items.Add("frmSeleccionarProductos");
            cmbFormulario.Items.Add("frmConfirmarPedido");
            cmbFormulario.Items.Add("frmAsignarMesaYMozo");
            cmbFormulario.SelectedIndex = -1;

      
        }

        
        private void CargarGrilla()
        {
            if (cmbFormulario.SelectedIndex == -1 || cmbIdioma.SelectedIndex == -1)
                return;

            dgvIdiomas.DataSource = bllIdioma.ObtenerTraducciones(
                cmbFormulario.Text,
                Convert.ToInt32(cmbIdioma.SelectedValue));
        }
        private void cmbFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFormulario.SelectedIndex == -1) return;

            RegistrarControlesFormulario(cmbFormulario.Text);

            
            if (cmbIdioma.SelectedIndex != -1)
                CargarGrilla();
            else
                dgvIdiomas.DataSource = bllIdioma.ObtenerControles(cmbFormulario.Text);
        }
        private void RegistrarControlesFormulario(string formulario)
        {
            switch (formulario)
            {
                case "Login":
                    bllIdioma.RegistrarControl("Login", "lblUsuario", "Usuario");
                    bllIdioma.RegistrarControl("Login", "lblContraseña", "Contraseña");
                    bllIdioma.RegistrarControl("Login", "btnLogin", "Ingresar");
                    bllIdioma.RegistrarControl("Login", "btnCancelar", "Cancelar");
                    break;

                case "GestionUsuarios":
                    bllIdioma.RegistrarControl("GestionUsuarios", "label1", "DNI");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label2", "Usuario");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label3", "Contraseña");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label4", "Email");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label5", "Nombre");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label6", "Apellido");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label7", "NumTelefono");
                    bllIdioma.RegistrarControl("GestionUsuarios", "label8", "Rol");
                    bllIdioma.RegistrarControl("GestionUsuarios", "Agregar", "Agregar");
                    bllIdioma.RegistrarControl("GestionUsuarios", "Modificar", "Modificar");
                    bllIdioma.RegistrarControl("GestionUsuarios", "Eliminar", "Desactivar Usuario");
                    bllIdioma.RegistrarControl("GestionUsuarios", "chkActivo", "Activo");
                    bllIdioma.RegistrarControl("GestionUsuarios", "btnDesbloquear", "Desbloquear");
                    break;

                case "frmEventos":
                    bllIdioma.RegistrarControl("frmEventos", "label1", "Fecha");
                    bllIdioma.RegistrarControl("frmEventos", "label2", "Modulo");
                    bllIdioma.RegistrarControl("frmEventos", "label3", "Criticidad");
                    bllIdioma.RegistrarControl("frmEventos", "label4", "Evento");
                    bllIdioma.RegistrarControl("frmEventos", "Buscar", "Buscar");
                    break;

                case "frmInicio":
                    bllIdioma.RegistrarControl("frmInicio", "iniciarSesionToolStripMenuItem", "Iniciar Sesión");
                    bllIdioma.RegistrarControl("frmInicio", "cerrarSesionToolStripMenuItem", "Cerrar Sesión");
                    bllIdioma.RegistrarControl("frmInicio", "administradorToolStripMenuItem", "Administrador");
                    bllIdioma.RegistrarControl("frmInicio", "gestionDeUsuariosToolStripMenuItem", "Gestión de Usuarios");
                    bllIdioma.RegistrarControl("frmInicio", "gestionDePerfilesToolStripMenuItem", "Gestión de Perfiles");
                    bllIdioma.RegistrarControl("frmInicio", "gestionDeIdiomasToolStripMenuItem", "Gestión de Idiomas");
                    bllIdioma.RegistrarControl("frmInicio", "usuarioToolStripMenuItem", "Usuario");
                    bllIdioma.RegistrarControl("frmInicio", "bitacoraDeEventosToolStripMenuItem", "Eventos");
                    bllIdioma.RegistrarControl("frmInicio", "bitacoraDeCambiosToolStripMenuItem", "Cambios");
                    bllIdioma.RegistrarControl("frmInicio", "cambiarContraseñaToolStripMenuItem", "Cambiar Contraseña");
                    bllIdioma.RegistrarControl("frmInicio", "verificacionIntegridadToolStripMenuItem", "Verificacion de Integridad");
                    bllIdioma.RegistrarControl("frmInicio", "maestrosToolStripMenuItem", "Maestros");
                    bllIdioma.RegistrarControl("frmInicio", "clientesToolStripMenuItem", "Clientes");
                    bllIdioma.RegistrarControl("frmInicio", "productosToolStripMenuItem", "Productos");
                    bllIdioma.RegistrarControl("frmInicio", "mozosToolStripMenuItem", "Mozos");
                    bllIdioma.RegistrarControl("frmInicio", "pedidosToolStripMenuItem", "Pedidos");
                    bllIdioma.RegistrarControl("frmInicio", "gestionDePedidoToolStripMenuItem", "Gestion de pedido");
                    break;

                case "frmCambiarContraseña":
                    bllIdioma.RegistrarControl("frmCambiarContraseña", "lblUsuario", "Usuario");
                    bllIdioma.RegistrarControl("frmCambiarContraseña", "lblNuevaContraseña", "Nueva contraseña");
                    bllIdioma.RegistrarControl("frmCambiarContraseña", "lblConfirmarContraseña", "Confirmar contraseña");
                    bllIdioma.RegistrarControl("frmCambiarContraseña", "btnCambiar", "Cambiar");
                    break;

                case "frmVerificacionIntegridad":
                    bllIdioma.RegistrarControl("frmVerificacionIntegridad", "lblTitulo", "Verificación de Integridad");
                    bllIdioma.RegistrarControl("frmVerificacionIntegridad", "btnRecalcular", "Recalcular");
                    bllIdioma.RegistrarControl("frmVerificacionIntegridad", "btnRestaurar", "Restaurar");
                    bllIdioma.RegistrarControl("frmVerificacionIntegridad", "btnSalir", "Salir");
                    break;

                case "frmClientes":
                    bllIdioma.RegistrarControl("frmClientes", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmClientes", "lblNombre", "Nombre");
                    bllIdioma.RegistrarControl("frmClientes", "lblApellido", "Apellido");
                    bllIdioma.RegistrarControl("frmClientes", "lblMail", "Mail");
                    bllIdioma.RegistrarControl("frmClientes", "lblTelefono", "Télefono");
                    bllIdioma.RegistrarControl("frmClientes", "lblDomicilio", "Domicilio");
                    bllIdioma.RegistrarControl("frmClientes", "btnAgregar", "Agregar");
                    bllIdioma.RegistrarControl("frmClientes", "btnModificar", "Modificar");
                    bllIdioma.RegistrarControl("frmClientes", "btnEliminar", "Desactivar");
                    bllIdioma.RegistrarControl("frmClientes", "chkActivo", "Activo");
                    bllIdioma.RegistrarControl("frmClientes", "btnLimpiar", "Limpiar");
                    break;

                case "frmMozos":
                    bllIdioma.RegistrarControl("frmMozos", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmMozos", "lblNombre", "Nombre");
                    bllIdioma.RegistrarControl("frmMozos", "lblApellido", "Apellido");
                    bllIdioma.RegistrarControl("frmMozos", "lblMail", "Mail");
                    bllIdioma.RegistrarControl("frmMozos", "lblTelefono", "Télefono");
                    bllIdioma.RegistrarControl("frmMozos", "btnAgregar", "Agregar");
                    bllIdioma.RegistrarControl("frmMozos", "btnModificar", "Modificar");
                    bllIdioma.RegistrarControl("frmMozos", "btnEliminar", "Desactivar");
                    bllIdioma.RegistrarControl("frmMozos", "chkActivo", "Activo");
                    bllIdioma.RegistrarControl("frmMozos", "btnLimpiar", "Limpiar");
                    break;
                case "frmProductos":
                    bllIdioma.RegistrarControl("frmProductos", "lblCodigo", "Código");
                    bllIdioma.RegistrarControl("frmProductos", "lblNombre", "Nombre");
                    bllIdioma.RegistrarControl("frmProductos", "lblPrecio", "Precio");
                    bllIdioma.RegistrarControl("frmProductos", "lblStock", "Stock");
                    bllIdioma.RegistrarControl("frmProductos", "chkDisponible", "Disponible");
                    bllIdioma.RegistrarControl("frmProductos", "btnAgregar", "Agregar");
                    bllIdioma.RegistrarControl("frmProductos", "btnModificar", "Modificar");
                    bllIdioma.RegistrarControl("frmProductos", "btnEliminar", "Eliminar");
                    bllIdioma.RegistrarControl("frmProductos", "btnLimpiar", "Limpiar");
                    break;

                case "frmGestionDePedido":
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbCliente", "Cliente");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnIniciar", "Iniciar pedido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnListaEspera", "Atender lista de espera");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbResumen", "Resumen");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbListaEspera", "Lista de espera");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnNuevoPedido", "Nuevo pedido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnFinalizarAtencion", "Finalizar atención");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbEnCurso", "Pedidos en curso");
                    break;

                case "frmRegistrarCliente":
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "lblNombre", "Nombre");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "lblApellido", "Apellido");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "lblMail", "Mail");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "lblTelefono", "Télefono");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "lblDomicilio", "Domicilio");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "btnRegistrar", "Registrar");
                    bllIdioma.RegistrarControl("frmRegistrarCliente", "btnCancelar", "Cancelar");
                    break;

                case "frmSeleccionarProductos":
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "lblProducto", "Producto");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "lblCantidad", "Cantidad");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "btnAgregarProducto", "Agregar producto");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "btnQuitarProducto", "Quitar producto");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "lblTotal", "Total");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "btnContinuar", "Continuar");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "btnCancelar", "Cancelar");
                    break;

                case "frmConfirmarPedido":
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "lblTotal", "Total");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnConfirmar", "Confirmar pedido");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnModificar", "Modificar pedido");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnCancelar", "Cancelar");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnContinuar", "Continuar");
                    break;

                case "frmAsignarMesaYMozo":
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "gbMesa", "Mesa");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnAsignarMesa", "Asignar mesa");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnMesaDesdeLista", "Mesa liberada: asignar desde lista de espera");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "gbMozo", "Mozo");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnAsignarMozo", "Asignar mozo");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnMozoDesdeLista", "Mozo liberado: asignar desde lista de espera");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "gbResumen", "Resumen");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnListaEspera", "Ingresar a lista de espera");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnCancelarPedido", "Cancelar pedido");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnContinuar", "Continuar");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "btnCerrar", "Cerrar");
                    break;
                case "AdministradorIdiomas":
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "label1", "Formulario");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "label2", "Idioma");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "label3", "Nuevo Idioma");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "btnGuardar", "Guardar");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "btnNuevoIdioma", "Agregar Idioma");
                    break;
                
                case "GestionDePerfiles":
                    bllIdioma.RegistrarControl("GestionDePerfiles", "label1", "Perfiles");
                    bllIdioma.RegistrarControl("GestionDePerfiles", "label2", "Familias");
                    bllIdioma.RegistrarControl("GestionDePerfiles", "label3", "Permisos");
                    bllIdioma.RegistrarControl("GestionDePerfiles", "label4", "Nombre del perfil"); 
                    bllIdioma.RegistrarControl("GestionDePerfiles", "BTNAgregarFamilia", "Agregar Familia");
                    bllIdioma.RegistrarControl("GestionDePerfiles", "BTNAgregarPermiso", "Agregar Permiso");
                    bllIdioma.RegistrarControl("GestionDePerfiles", "BTNCrearPerfil", "Agregar Perfil");              
                    bllIdioma.RegistrarControl("GestionDePerfiles", "BTNEliminarPerfil", "Eliminar Perfil");
                    bllIdioma.RegistrarControl("GestionDePerfiles", "BTNAplicar", "Aplicar"); 
                    bllIdioma.RegistrarControl("GestionDePerfiles", "BTNConfigurarFamilias", "Configurar familias");
                    break;

                case "GestionDeFamilias":
                    bllIdioma.RegistrarControl("GestionDeFamilias", "label1", "Nombre de la familia");
                    bllIdioma.RegistrarControl("GestionDeFamilias", "label2", "Familias a editar");
                    bllIdioma.RegistrarControl("GestionDeFamilias", "label3", "Permisos");
                    bllIdioma.RegistrarControl("GestionDeFamilias", "label4", "Familias");
                    bllIdioma.RegistrarControl("GestionDeFamilias", "BTNAgregarFamilia", "Agregar Familia");
                    bllIdioma.RegistrarControl("GestionDeFamilias", "BTNAgregarPermiso", "Agregar permiso");              
                    bllIdioma.RegistrarControl("GestionDeFamilias", "BTNCrearFamilia", "Crear Familia");
                    bllIdioma.RegistrarControl("GestionDeFamilias", "BTNEliminarFamilia", "Eliminar Familia"); 
                    bllIdioma.RegistrarControl("GestionDeFamilias", "BTNAplicar", "Aplicar");
                    break;
                case "frmBitacoraCambios":
                  
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "button2", "Restaurar");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "button1", "Ver Todos");
                    break;
            }
           
        }
        private void cmbIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            CargarGrilla();
        }

        

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (cmbIdioma.SelectedIndex == -1)
            {
                MessageBox.Show(traductor.ObtenerMensaje("AdministradorIdiomas", "msg_SeleccioneIdioma", "Seleccione un idioma."));
                return;
            }

            foreach (DataGridViewRow fila in dgvIdiomas.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                if (fila.Cells["Traduccion"].Value == null)
                    continue;

                if (string.IsNullOrWhiteSpace(fila.Cells["Traduccion"].Value.ToString()))
                    continue;

                Traduccion traduccion = new Traduccion();

                traduccion.Idioma = Convert.ToInt32(cmbIdioma.SelectedValue);
                traduccion.Control = Convert.ToInt32(fila.Cells["IdControl"].Value);
                traduccion.Texto = fila.Cells["Traduccion"].Value.ToString();

                bllIdioma.GuardarTraduccion(traduccion);
            }

            MessageBox.Show(traductor.ObtenerMensaje("AdministradorIdiomas", "msg_TraduccionesGuardadas", "Traducciones guardadas correctamente."));
        }
        

        private void dgvIdiomas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNuevoIdioma_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNuevoIdioma.Text))
            {
                MessageBox.Show(traductor.ObtenerMensaje("AdministradorIdiomas", "msg_IngreseNombreIdioma", "Ingrese un nombre para el idioma."));
                return;
            }

            bllIdioma.AgregarIdioma(txtNuevoIdioma.Text.Trim());

            cmbIdioma.DataSource = null;
            cmbIdioma.DataSource = bllIdioma.ListarIdiomas();
            cmbIdioma.DisplayMember = "Nombre";
            cmbIdioma.ValueMember = "IdIdioma";

            txtNuevoIdioma.Clear();

            
            frmInicio parentUI = this.MdiParent as frmInicio;
            if (parentUI != null)
                parentUI.login.RecargarIdiomas();

            MessageBox.Show(traductor.ObtenerMensaje("AdministradorIdiomas", "msg_IdiomaAgregado", "Idioma agregado correctamente."));
        }

        private void AdministradorIdiomas_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();

            }
        }

        private void AdministradorIdiomas_VisibleChanged(object sender, EventArgs e)
        {
           
        }
    }
}
