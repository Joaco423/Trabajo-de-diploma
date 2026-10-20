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
    public partial class frmInicio : Form, IObserver
    {
        public frmInicio()
        {
            InitializeComponent();
        }
        public Login login = new Login();
        public CerrarSesion cerrarSesion = new CerrarSesion();
        public frmEventos eventos = new frmEventos();
        private GestionUsuarios gestionUsuarios;
        public AdministradorIdiomas administradorIdiomas = new AdministradorIdiomas();
        public GestionDePerfiles gestionDePerfiles = new GestionDePerfiles();
        public GestionDeFamilias gestionDeFamilias = new GestionDeFamilias();
        public frmBitacoraCambios bitacoraCambios = new frmBitacoraCambios();
        public frmCambiarContraseña cambiarContraseña = new frmCambiarContraseña();
        public frmVerificacionIntegridad verificacionIntegridad = new frmVerificacionIntegridad();
        public frmClientes clientes_235JB = new frmClientes();
        public frmProductos productos_235JB = new frmProductos();
        public frmMozos mozos_235JB = new frmMozos();
        public frmGestionDePedido gestionDePedido_235JB = new frmGestionDePedido();
        private frmInicio menuPrincipal;
        Traductor traductor = new Traductor();

        public void ActualizarIdioma(string idioma)
        {
            traductor.TraducirFormulario(this, int.Parse(idioma));
        }
        public frmInicio MenuPrincipal
        {
            get
            {
                if (menuPrincipal == null || menuPrincipal.IsDisposed)
                    menuPrincipal = this;
                return menuPrincipal;
            }
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            cerrarSesionToolStripMenuItem.Visible = false;
            administradorToolStripMenuItem.Visible = false;


            gestionUsuarios = new GestionUsuarios();
            gestionUsuarios.MdiParent = this;
            Singleton.Instancia.Suscribir(gestionUsuarios);

            
            eventos.MdiParent = this;
            Singleton.Instancia.Suscribir(eventos);

           
            administradorIdiomas.MdiParent = this;
            Singleton.Instancia.Suscribir(administradorIdiomas);

          
            gestionDePerfiles.MdiParent = this; 
            Singleton.Instancia.Suscribir(gestionDePerfiles);

            
            gestionDeFamilias.MdiParent = this;
            Singleton.Instancia.Suscribir(gestionDeFamilias);

            cambiarContraseña.MdiParent = this;
            Singleton.Instancia.Suscribir(cambiarContraseña);

            verificacionIntegridad.MdiParent = this;
            Singleton.Instancia.Suscribir(verificacionIntegridad);

            clientes_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(clientes_235JB);

            productos_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(productos_235JB);

            mozos_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(mozos_235JB);

            gestionDePedido_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(gestionDePedido_235JB);

            //Este metodo es para el frmInicio

            Singleton.Instancia.Suscribir(this);

            ValidarIntegridadInicial();
        }
        private void ValidarIntegridadInicial()
        {

            {
                BLLIntegridad bllIntegridad = new BLLIntegridad();
                bool integro = bllIntegridad.ValidarIntegridadGeneral();

                if (!integro)
                {
                    MessageBox.Show(
                        traductor.ObtenerMensaje("frmInicio", "msg_AlertaIntegridad",
                            "Se detectó que una o más tablas fueron alteradas fuera del sistema.\n\n" +
                            "Inicie sesión como administrador y utilice 'Administrador > Verificación de Integridad' " +
                            "para restaurar el último backup o recalcular los dígitos verificadores."),
                        traductor.ObtenerMensaje("frmInicio", "msg_AlertaIntegridad_Titulo", "Alerta de integridad"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }
        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login.MdiParent = this;
            login.Show();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
                    if (cerrarSesion == null || cerrarSesion.IsDisposed)
                    {
                                  cerrarSesion = new CerrarSesion();
                                  cerrarSesion.MdiParent = this;
                    }

                                   cerrarSesion.Show();
                                   cerrarSesion.BringToFront();

        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cambiarContraseña.MdiParent = this;
            cambiarContraseña.Show();
        }

        private void bitacoraDeEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {

            eventos.MdiParent = this;
            eventos.Show();
            eventos.WindowState = FormWindowState.Maximized;
            eventos.BringToFront();
        }

        private void gestionDeUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gestionUsuarios == null || gestionUsuarios.IsDisposed)
            {
                gestionUsuarios = new GestionUsuarios();
                gestionUsuarios.MdiParent = this;
            }
            gestionUsuarios.Show();
            gestionUsuarios.WindowState = FormWindowState.Maximized;
            gestionUsuarios.BringToFront();
        }



        private void gestionDeIdiomasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            administradorIdiomas.MdiParent = this;
            administradorIdiomas.Show();
            administradorIdiomas.WindowState = FormWindowState.Maximized;
            administradorIdiomas.BringToFront();
        }

        private void gestionDePerfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gestionDePerfiles.MdiParent = this;
            gestionDeFamilias.MdiParent = this;
            gestionDePerfiles.Show();
            gestionDePerfiles.WindowState = FormWindowState.Maximized;
            gestionDePerfiles.BringToFront();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public void AplicarPermisosUsuario(List<Perfil> componentesUsuario)
        {
            
            ApagarMenuCompleto(menuStrip1.Items);

           
            HabilitarControlesRecursivo(componentesUsuario);
        }


        private void ApagarMenuCompleto(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
            {
                
                
                if (item.Name == "usuarioToolStripMenuItem")
                {
                    item.Visible = true; 

                   
                    if (item is ToolStripMenuItem menuDesplegableUsuario)
                    {
                       
                        foreach (ToolStripItem subItem in menuDesplegableUsuario.DropDownItems)
                        {
                            subItem.Visible = true;
                        }
                    }

                    continue; 
                }

               
                item.Visible = false;

                if (item is ToolStripMenuItem menuDesplegable)
                {
                    ApagarMenuCompleto(menuDesplegable.DropDownItems);
                }
            }
        }

        private void HabilitarControlesRecursivo(List<Perfil> componentes)
        {
            foreach (var comp in componentes)
            {
                if (comp is Permiso) 
                {
                    
                    ToolStripItem controlEncontrado = BuscarControlMenu(menuStrip1.Items, comp.Nombre);

                    if (controlEncontrado != null)
                    {
                        controlEncontrado.Visible = true; 

                        
                        PrenderPadresOcultos(controlEncontrado);
                    }
                }
                else if (comp is Familia) 
                {
                    HabilitarControlesRecursivo(((Familia)comp).Hijos.ToList());
                }
            }
        }

        
        private ToolStripItem BuscarControlMenu(ToolStripItemCollection items, string nombreBuscar)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Name == nombreBuscar) return item;

                if (item is ToolStripMenuItem menuDesplegable)
                {
                    ToolStripItem encontrado = BuscarControlMenu(menuDesplegable.DropDownItems, nombreBuscar);
                    if (encontrado != null) return encontrado;
                }
            }
            return null;
        }

       
        private void PrenderPadresOcultos(ToolStripItem item)
        {
            if (item.OwnerItem != null)
            {
                item.OwnerItem.Visible = true;
                PrenderPadresOcultos(item.OwnerItem); 
            }
        }

        private void bitacoraDeCambiosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bitacoraCambios.MdiParent = this;
            bitacoraCambios.Show();
            bitacoraCambios.WindowState = FormWindowState.Maximized;
            bitacoraCambios.BringToFront();
        }

        private void clientesToolStripMenuItem_Click_235JB(object sender, EventArgs e)
        {
            clientes_235JB.MdiParent = this;
            clientes_235JB.Show();
            clientes_235JB.WindowState = FormWindowState.Maximized;
            clientes_235JB.BringToFront();
        }

        private void productosToolStripMenuItem_Click_235JB(object sender, EventArgs e)
        {
            productos_235JB.MdiParent = this;
            productos_235JB.Show();
            productos_235JB.WindowState = FormWindowState.Maximized;
            productos_235JB.BringToFront();
        }

        private void mozosToolStripMenuItem_Click_235JB(object sender, EventArgs e)
        {
            mozos_235JB.MdiParent = this;
            mozos_235JB.Show();
            mozos_235JB.WindowState = FormWindowState.Maximized;
            mozos_235JB.BringToFront();
        }

        // CU01 paso 1: el empleado elige "Gestion de pedido" del menu
        private void gestionDePedidoToolStripMenuItem_Click_235JB(object sender, EventArgs e)
        {
            gestionDePedido_235JB.MdiParent = this;
            gestionDePedido_235JB.Show();
            gestionDePedido_235JB.WindowState = FormWindowState.Maximized;
            gestionDePedido_235JB.BringToFront();
        }

       
        public void CerrarModuloCafeteria_235JB()
        {
            maestrosToolStripMenuItem.Visible = false;
            pedidosToolStripMenuItem.Visible = false;
            clientes_235JB.Hide();
            productos_235JB.Hide();
            mozos_235JB.Hide();
            gestionDePedido_235JB.Hide();
        }

        private void verificacionIntegridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            verificacionIntegridad.MdiParent = this;
            verificacionIntegridad.Show();
            verificacionIntegridad.WindowState = FormWindowState.Maximized;
            verificacionIntegridad.BringToFront();
        }
    }
}
