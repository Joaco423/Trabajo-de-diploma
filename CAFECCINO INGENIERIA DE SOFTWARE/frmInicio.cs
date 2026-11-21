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
            CrearMenuAyuda_235JB();
        }

        // ---------------- Menu Ayuda: guias rapidas en PDF (carpeta Guias junto al .exe) ----------------
        // Cada guia esta asociada al item del menu de su pantalla: se muestra solo si el usuario puede abrir esa pantalla,
        // asi respeta los permisos de cada perfil (administrador, empleado, reservista) sin duplicarlos.
        private ToolStripMenuItem ayudaToolStripMenuItem;
        private readonly Dictionary<ToolStripMenuItem, ToolStripMenuItem> _guiaPantalla_235JB = new Dictionary<ToolStripMenuItem, ToolStripMenuItem>();

        private void CrearMenuAyuda_235JB()
        {
            ayudaToolStripMenuItem = new ToolStripMenuItem { Name = "ayudaToolStripMenuItem", Text = "Ayuda" };
            ayudaToolStripMenuItem.DropDownOpening += ayudaToolStripMenuItem_DropDownOpening_235JB;

            object[,] guias_235JB =
            {
                { "guiaInicioSesionToolStripMenuItem",        "Inicio de sesión",               "01_Inicio_de_sesion.pdf",           iniciarSesionToolStripMenuItem },
                { "guiaCerrarSesionToolStripMenuItem",        "Cerrar sesión",                  "02_Cerrar_sesion.pdf",              cerrarSesionToolStripMenuItem },
                { "guiaCambiarContrasenaToolStripMenuItem",   "Cambiar contraseña",             "03_Cambiar_contrasena.pdf",         cambiarContraseñaToolStripMenuItem },
                { "-", null, null, null },
                { "guiaGestionUsuariosToolStripMenuItem",     "Gestión de usuarios",            "04_Gestion_de_usuarios.pdf",        gestionDeUsuariosToolStripMenuItem },
                { "guiaGestionPerfilesToolStripMenuItem",     "Gestión de perfiles y familias", "05_Gestion_de_perfiles.pdf",        gestionDePerfilesToolStripMenuItem },
                { "guiaGestionIdiomasToolStripMenuItem",      "Gestión de idiomas",             "06_Gestion_de_idiomas.pdf",         gestionDeIdiomasToolStripMenuItem },
                { "guiaBitacoraEventosToolStripMenuItem",     "Bitácora de eventos",            "07_Bitacora_de_eventos.pdf",        bitacoraDeEventosToolStripMenuItem },
                { "guiaVerificacionIntegridadToolStripMenuItem", "Verificación de integridad",  "08_Verificacion_de_integridad.pdf", verificacionIntegridadToolStripMenuItem },
                { "-", null, null, null },
                { "guiaClientesToolStripMenuItem",            "Clientes",                       "09_Clientes.pdf",                   clientesToolStripMenuItem },
                { "guiaProductosToolStripMenuItem",           "Productos",                      "10_Productos.pdf",                  productosToolStripMenuItem },
                { "guiaMozosToolStripMenuItem",               "Mozos",                          "11_Mozos.pdf",                      mozosToolStripMenuItem },
                { "guiaMesasToolStripMenuItem",               "Mesas",                          "12_Mesas.pdf",                      mesasToolStripMenuItem },
                { "guiaBitacoraCambiosToolStripMenuItem",     "Bitácora de cambios",            "13_Bitacora_de_cambios.pdf",        bitacoraDeCambiosToolStripMenuItem },
                { "-", null, null, null },
                { "guiaGestionPedidoToolStripMenuItem",       "Gestión de pedido",              "14_Gestion_de_pedido.pdf",          gestionDePedidoToolStripMenuItem },
                { "guiaGestionReservasToolStripMenuItem",     "Gestión de reservas",            "15_Gestion_de_reservas.pdf",        gestionDeReservasToolStripMenuItem },
                { "-", null, null, null },
                { "guiaReporteInteligenteToolStripMenuItem",  "Reporte inteligente",            "16_Reporte_inteligente.pdf",        reporteInteligenteToolStripMenuItem }
            };

            for (int i_235JB = 0; i_235JB < guias_235JB.GetLength(0); i_235JB++)
            {
                if ((string)guias_235JB[i_235JB, 0] == "-")
                {
                    ayudaToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                    continue;
                }
                ToolStripMenuItem guia_235JB = new ToolStripMenuItem
                {
                    Name = (string)guias_235JB[i_235JB, 0],
                    Text = (string)guias_235JB[i_235JB, 1],
                    Tag = guias_235JB[i_235JB, 2]
                };
                guia_235JB.Click += guiaToolStripMenuItem_Click_235JB;
                _guiaPantalla_235JB[guia_235JB] = (ToolStripMenuItem)guias_235JB[i_235JB, 3];
                ayudaToolStripMenuItem.DropDownItems.Add(guia_235JB);
            }

            menuStrip1.Items.Add(ayudaToolStripMenuItem);
        }

        // La pantalla esta habilitada si su item y todos sus menus padres estan disponibles para el usuario
        private static bool PantallaHabilitada_235JB(ToolStripItem item_235JB)
        {
            for (ToolStripItem actual_235JB = item_235JB; actual_235JB != null; actual_235JB = actual_235JB.OwnerItem)
                if (!actual_235JB.Available) return false;
            return true;
        }

        private void ayudaToolStripMenuItem_DropDownOpening_235JB(object sender_235JB, EventArgs e_235JB)
        {
            foreach (KeyValuePair<ToolStripMenuItem, ToolStripMenuItem> par_235JB in _guiaPantalla_235JB)
                par_235JB.Key.Available = PantallaHabilitada_235JB(par_235JB.Value);

            // Separadores: uno solo entre dos grupos que tengan guias visibles (los grupos vacios no dejan rayas de mas)
            bool hayGuiaAntes_235JB = false;
            ToolStripItem separadorPendiente_235JB = null;
            foreach (ToolStripItem item_235JB in ayudaToolStripMenuItem.DropDownItems)
            {
                if (item_235JB is ToolStripSeparator)
                {
                    item_235JB.Available = false;
                    separadorPendiente_235JB = item_235JB;
                }
                else if (item_235JB.Available)
                {
                    if (separadorPendiente_235JB != null && hayGuiaAntes_235JB)
                        separadorPendiente_235JB.Available = true;
                    separadorPendiente_235JB = null;
                    hayGuiaAntes_235JB = true;
                }
            }
        }

        private void guiaToolStripMenuItem_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ToolStripMenuItem guia_235JB = (ToolStripMenuItem)sender_235JB;
            string ruta_235JB = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Guias", (string)guia_235JB.Tag);
            if (!System.IO.File.Exists(ruta_235JB))
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmInicio", "msg_GuiaNoEncontrada", "No se encontró la guía: ") + ruta_235JB,
                    traductor.ObtenerMensaje("frmInicio", "msg_Ayuda_Titulo", "Ayuda"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(ruta_235JB) { UseShellExecute = true });
            }
            catch (Exception ex_235JB)
            {
                MessageBox.Show(traductor.ObtenerMensaje("frmInicio", "msg_ErrorAbrirGuia", "No se pudo abrir la guía: ") + ex_235JB.Message,
                    traductor.ObtenerMensaje("frmInicio", "msg_Ayuda_Titulo", "Ayuda"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        public frmMesas mesas_235JB = new frmMesas();
        public frmGestionDeReservas reservas_235JB = new frmGestionDeReservas();
        public frmGestionDePedido gestionDePedido_235JB = new frmGestionDePedido();
        public frmReporteInteligente reporteInteligente_235JB = new frmReporteInteligente();
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

            mesas_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(mesas_235JB);

            reservas_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(reservas_235JB);

            gestionDePedido_235JB.MdiParent = this;
            Singleton.Instancia.Suscribir(gestionDePedido_235JB);

            

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
                    }

                                   // Siempre como hija de esta ventana: CerrarSesion.ModificarMenu usa el MdiParent para ocultar los menus
                                   cerrarSesion.MdiParent = this;
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

                // Ayuda siempre visible: sus guias se filtran al abrir el menu segun lo que el usuario puede ver
                if (item.Name == "ayudaToolStripMenuItem")
                {
                    item.Visible = true;
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

       
        private void mesasToolStripMenuItem_Click_235JB(object sender, EventArgs e)
        {
            mesas_235JB.MdiParent = this;
            mesas_235JB.Show();
            mesas_235JB.WindowState = FormWindowState.Maximized;
            mesas_235JB.BringToFront();
        }

     

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
            mesas_235JB.Hide();
            bitacoraCambios.Hide();
            reservas_235JB.Hide();
            reportesToolStripMenuItem.Visible = false;
            reporteInteligente_235JB.Hide();
            reservasToolStripMenuItem.Visible = false;
            gestionDePedido_235JB.Hide();
        }

        private void reporteInteligenteToolStripMenuItem_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            reporteInteligente_235JB.MdiParent = this;
            reporteInteligente_235JB.Show();
            reporteInteligente_235JB.WindowState = FormWindowState.Maximized;
            reporteInteligente_235JB.BringToFront();
        }

        private void verificacionIntegridadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirVerificacionIntegridad_235JB();
        }

        
        public void AbrirVerificacionIntegridad_235JB()
        {
            verificacionIntegridad.MdiParent = this;
            verificacionIntegridad.Show();
            verificacionIntegridad.WindowState = FormWindowState.Maximized;
            verificacionIntegridad.BringToFront();
        }

        private void pedidosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void gestionDeReservasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reservas_235JB.MdiParent = this;
            reservas_235JB.Show();
            reservas_235JB.WindowState = FormWindowState.Maximized;
            reservas_235JB.BringToFront();
        }
    }
}
