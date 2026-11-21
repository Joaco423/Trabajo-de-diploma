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
            cmbFormulario.Items.Add("frmMesas");
            cmbFormulario.Items.Add("frmGestionDeReservas");
            cmbFormulario.Items.Add("frmIniciarRegistro");
            cmbFormulario.Items.Add("frmIngresarDatosCliente");
            cmbFormulario.Items.Add("frmRegistrarClienteReserva");
            cmbFormulario.Items.Add("frmConsultarDisponibilidadMesas");
            cmbFormulario.Items.Add("frmGestionDePedido");
            cmbFormulario.Items.Add("frmRegistrarCliente");
            cmbFormulario.Items.Add("frmSeleccionarProductos");
            cmbFormulario.Items.Add("frmConfirmarPedido");
            cmbFormulario.Items.Add("frmAsignarMesaYMozo");
            cmbFormulario.Items.Add("frmReporteInteligente");
            cmbFormulario.SelectedIndex = -1;

            // Las columnas de la grilla se crean recien aca (ConfigurarGrilla), despues de cualquier traduccion anterior:
            // se traducen sus encabezados con el idioma activo
            traductor.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
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
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.DNI", "DNI");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Username", "Usuario");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Contraseña", "Contraseña");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Email", "Email");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.NumTelefono", "Teléfono");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Rol", "Rol");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.DVH", "DVH");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Activo", "Activo");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.Bloqueado", "Bloqueado");
                    bllIdioma.RegistrarControl("GestionUsuarios", "dataGridView1.IntentosFallidos", "Intentos fallidos");
                    break;

                case "frmEventos":
                    bllIdioma.RegistrarControl("frmEventos", "label1", "Fecha");
                    bllIdioma.RegistrarControl("frmEventos", "label2", "Modulo");
                    bllIdioma.RegistrarControl("frmEventos", "label3", "Criticidad");
                    bllIdioma.RegistrarControl("frmEventos", "label4", "Evento");
                    bllIdioma.RegistrarControl("frmEventos", "Buscar", "Buscar");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.CodEvento", "Código de evento");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.NombreUsuario", "Usuario");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.Fecha", "Fecha");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.Evento", "Evento");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.Modulo", "Módulo");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.Criticidad", "Criticidad");
                    bllIdioma.RegistrarControl("frmEventos", "dataGridView1.DVH", "DVH");
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
                    bllIdioma.RegistrarControl("frmInicio", "mesasToolStripMenuItem", "Mesas");
                    bllIdioma.RegistrarControl("frmInicio", "reservasToolStripMenuItem", "Reservas");
                    bllIdioma.RegistrarControl("frmInicio", "gestionDeReservasToolStripMenuItem", "Gestión de reservas");
                    bllIdioma.RegistrarControl("frmInicio", "pedidosToolStripMenuItem", "Pedidos");
                    bllIdioma.RegistrarControl("frmInicio", "gestionDePedidoToolStripMenuItem", "Gestion de pedido");
                    bllIdioma.RegistrarControl("frmInicio", "reportesToolStripMenuItem", "Reportes");
                    bllIdioma.RegistrarControl("frmInicio", "reporteInteligenteToolStripMenuItem", "Reporte inteligente");
                    bllIdioma.RegistrarControl("frmInicio", "ayudaToolStripMenuItem", "Ayuda");
                    bllIdioma.RegistrarControl("frmInicio", "guiaInicioSesionToolStripMenuItem", "Inicio de sesión");
                    bllIdioma.RegistrarControl("frmInicio", "guiaCerrarSesionToolStripMenuItem", "Cerrar sesión");
                    bllIdioma.RegistrarControl("frmInicio", "guiaCambiarContrasenaToolStripMenuItem", "Cambiar contraseña");
                    bllIdioma.RegistrarControl("frmInicio", "guiaGestionUsuariosToolStripMenuItem", "Gestión de usuarios");
                    bllIdioma.RegistrarControl("frmInicio", "guiaGestionPerfilesToolStripMenuItem", "Gestión de perfiles y familias");
                    bllIdioma.RegistrarControl("frmInicio", "guiaGestionIdiomasToolStripMenuItem", "Gestión de idiomas");
                    bllIdioma.RegistrarControl("frmInicio", "guiaBitacoraEventosToolStripMenuItem", "Bitácora de eventos");
                    bllIdioma.RegistrarControl("frmInicio", "guiaVerificacionIntegridadToolStripMenuItem", "Verificación de integridad");
                    bllIdioma.RegistrarControl("frmInicio", "guiaClientesToolStripMenuItem", "Clientes");
                    bllIdioma.RegistrarControl("frmInicio", "guiaProductosToolStripMenuItem", "Productos");
                    bllIdioma.RegistrarControl("frmInicio", "guiaMozosToolStripMenuItem", "Mozos");
                    bllIdioma.RegistrarControl("frmInicio", "guiaMesasToolStripMenuItem", "Mesas");
                    bllIdioma.RegistrarControl("frmInicio", "guiaBitacoraCambiosToolStripMenuItem", "Bitácora de cambios");
                    bllIdioma.RegistrarControl("frmInicio", "guiaGestionPedidoToolStripMenuItem", "Gestión de pedido");
                    bllIdioma.RegistrarControl("frmInicio", "guiaGestionReservasToolStripMenuItem", "Gestión de reservas");
                    bllIdioma.RegistrarControl("frmInicio", "guiaReporteInteligenteToolStripMenuItem", "Reporte inteligente");
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
                    bllIdioma.RegistrarControl("frmClientes", "btnSerializar", "Serializar");
                    bllIdioma.RegistrarControl("frmClientes", "btnDeserializar", "Deserializar");
                    bllIdioma.RegistrarControl("frmClientes", "lblSerializado", "Archivo serializado");
                    bllIdioma.RegistrarControl("frmClientes", "lblDeserializado", "Archivo deserializado");
                    bllIdioma.RegistrarControl("frmClientes", "lblFormato", "Tipo de serialización");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.CodigoCliente", "Código de cliente");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.DNI", "DNI");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.Mail", "Mail");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.NumTelefono", "Teléfono");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.Domicilio", "Domicilio");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.FechaAlta", "Fecha de alta");
                    bllIdioma.RegistrarControl("frmClientes", "dgvClientes.Activo", "Activo");
                    break;

                case "frmGestionDeReservas":
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "btnIniciar", "Iniciar registro de cliente");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "btnLimpiar", "Limpiar resumen");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "gbResumen", "Resumen");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "gbReservas", "Reservas activas");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "btnCancelarReserva", "Cancelar reserva seleccionada");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "btnGenerarReporte", "Generar reporte");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "btnReservarDeLista", "Reservar para lista de espera");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "gbEspera", "Lista de espera de mesas");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvEspera.DNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvEspera.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvEspera.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvEspera.Fecha", "Fecha");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvEspera.Hora", "Hora");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.Reserva", "Reserva");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.DNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.Mesa", "Mesa");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.Fecha", "Fecha");
                    bllIdioma.RegistrarControl("frmGestionDeReservas", "dgvReservas.Hora", "Hora");
                    break;

                case "frmIniciarRegistro":
                    bllIdioma.RegistrarControl("frmIniciarRegistro", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmIniciarRegistro", "btnConsultarDNI", "Consultar DNI");
                    bllIdioma.RegistrarControl("frmIniciarRegistro", "btnCancelar", "Cancelar");
                    break;

                case "frmIngresarDatosCliente":
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "lblNombre", "Nombre");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "lblApellido", "Apellido");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "lblDomicilio", "Dirección");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "lblMail", "Mail");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "lblTelefono", "Teléfono");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "btnIngresarDatos", "Validar datos");
                    bllIdioma.RegistrarControl("frmIngresarDatosCliente", "btnCancelar", "Cancelar");
                    break;

                case "frmRegistrarClienteReserva":
                    bllIdioma.RegistrarControl("frmRegistrarClienteReserva", "lblDatos", "Datos a registrar");
                    bllIdioma.RegistrarControl("frmRegistrarClienteReserva", "btnRegistrar", "Registrar cliente");
                    bllIdioma.RegistrarControl("frmRegistrarClienteReserva", "btnCancelar", "Cancelar");
                    break;

                case "frmConsultarDisponibilidadMesas":
                    bllIdioma.RegistrarControl("frmConsultarDisponibilidadMesas", "btnReservar", "Reservar mesa seleccionada");
                    bllIdioma.RegistrarControl("frmConsultarDisponibilidadMesas", "btnConsultar", "Consultar nuevamente");
                    bllIdioma.RegistrarControl("frmConsultarDisponibilidadMesas", "btnCerrar", "Cerrar");
                    bllIdioma.RegistrarControl("frmConsultarDisponibilidadMesas", "dgvMesas.NumeroMesa", "Número de mesa");
                    bllIdioma.RegistrarControl("frmConsultarDisponibilidadMesas", "dgvMesas.Capacidad", "Capacidad");
                    bllIdioma.RegistrarControl("frmConsultarDisponibilidadMesas", "dgvMesas.Estado", "Estado");
                    break;

                case "frmMesas":
                    bllIdioma.RegistrarControl("frmMesas", "lblNumero", "Número de mesa");
                    bllIdioma.RegistrarControl("frmMesas", "lblCapacidad", "Capacidad");
                    bllIdioma.RegistrarControl("frmMesas", "btnAgregar", "Agregar");
                    bllIdioma.RegistrarControl("frmMesas", "btnModificar", "Modificar");
                    bllIdioma.RegistrarControl("frmMesas", "btnEliminar", "Eliminar");
                    bllIdioma.RegistrarControl("frmMesas", "btnLimpiar", "Limpiar");
                    bllIdioma.RegistrarControl("frmMesas", "dgvMesas.NumeroMesa", "Número de mesa");
                    bllIdioma.RegistrarControl("frmMesas", "dgvMesas.Capacidad", "Capacidad");
                    bllIdioma.RegistrarControl("frmMesas", "dgvMesas.Estado", "Estado");
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
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.DNIMozo", "DNI del mozo");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.Mail", "Mail");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.NumTelefono", "Teléfono");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.Estado", "Estado");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.Activo", "Activo");
                    bllIdioma.RegistrarControl("frmMozos", "dgvMozos.MesasAtendiendo", "Mesas que atiende");
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
                    bllIdioma.RegistrarControl("frmProductos", "dgvProductos.Codigo", "Código");
                    bllIdioma.RegistrarControl("frmProductos", "dgvProductos.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmProductos", "dgvProductos.Precio", "Precio");
                    bllIdioma.RegistrarControl("frmProductos", "dgvProductos.Disponible", "Disponible");
                    bllIdioma.RegistrarControl("frmProductos", "dgvProductos.Stock", "Stock");
                    break;

                case "frmGestionDePedido":
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbCliente", "Cliente");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "lblDNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnIniciar", "Iniciar pedido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnListaEspera", "Atender lista de espera");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbResumen", "Resumen");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnGenerarReporte", "Generar reporte");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbListaEspera", "Lista de espera");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnNuevoPedido", "Nuevo pedido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "btnFinalizarAtencion", "Finalizar atención");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbEnCurso", "Pedidos en curso");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "gbReservasActivas", "Reservas activas (prioridad de mesa y mozo)");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvListaEspera.Lista", "Lista");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvListaEspera.DNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvListaEspera.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvListaEspera.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvListaEspera.Fecha", "Fecha");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvListaEspera.Hora", "Hora");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.Pedido", "Pedido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.DNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.Mesa", "Mesa");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.Mozo", "Mozo");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvEnCurso.Total", "Total");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.Reserva", "Reserva");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.DNI", "DNI");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.Mesa", "Mesa");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.Fecha", "Fecha");
                    bllIdioma.RegistrarControl("frmGestionDePedido", "dgvReservas.Hora", "Hora");
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
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "dgvDetalle.Codigo", "Código");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "dgvDetalle.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "dgvDetalle.Precio", "Precio");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "dgvDetalle.Cantidad", "Cantidad");
                    bllIdioma.RegistrarControl("frmSeleccionarProductos", "dgvDetalle.Subtotal", "Subtotal");
                    break;

                case "frmConfirmarPedido":
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "lblTotal", "Total");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnConfirmar", "Confirmar pedido");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnModificar", "Modificar pedido");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnCancelar", "Cancelar");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "btnContinuar", "Continuar");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "dgvDetalle.Codigo", "Código");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "dgvDetalle.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "dgvDetalle.Precio", "Precio");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "dgvDetalle.Cantidad", "Cantidad");
                    bllIdioma.RegistrarControl("frmConfirmarPedido", "dgvDetalle.Subtotal", "Subtotal");
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
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMesas.NumeroMesa", "Número de mesa");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMesas.Capacidad", "Capacidad");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMesas.Estado", "Estado");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.DNIMozo", "DNI del mozo");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.Apellido", "Apellido");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.Mail", "Mail");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.NumTelefono", "Teléfono");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.Estado", "Estado");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.Activo", "Activo");
                    bllIdioma.RegistrarControl("frmAsignarMesaYMozo", "dgvMozos.MesasAtendiendo", "Mesas que atiende");
                    break;
                case "AdministradorIdiomas":
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "label1", "Formulario");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "label2", "Idioma");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "label3", "Nuevo Idioma");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "btnGuardar", "Guardar");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "btnNuevoIdioma", "Agregar Idioma");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "dgvIdiomas.IdControl", "Id");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "dgvIdiomas.Control", "Control");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "dgvIdiomas.TextoOriginal", "Texto original");
                    bllIdioma.RegistrarControl("AdministradorIdiomas", "dgvIdiomas.Traduccion", "Traducción");
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
                case "frmReporteInteligente":
                    bllIdioma.RegistrarControl("frmReporteInteligente", "lblTipoReporte", "Tipo de reporte:");
                    bllIdioma.RegistrarControl("frmReporteInteligente", "btnGenerarReporte", "Generar reporte");
                    break;
                case "frmBitacoraCambios":
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "lblCodProd", "Cod. Prod.");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "lblNombre", "Nombre");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "lblFechaIni", "Fecha Ini.");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "lblFechaFin", "Fecha Fin");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "btnAplicar", "Aplicar");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "btnSalir", "Salir");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "btnLimpiar", "Limpiar");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "btnActivar", "Activar");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "lblSinResultados", "No se encontraron cambios con los filtros ingresados.");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.CodigoProducto", "Cod. Prod.");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Fecha", "Fecha");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Hora", "Hora");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Nombre", "Nombre");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Precio", "Precio");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Disponible", "Disponible");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Stock", "Exist.");
                    bllIdioma.RegistrarControl("frmBitacoraCambios", "dgvCambios.Act", "Act.");
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
            if (Visible)
                traductor.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
        }
    }
}
