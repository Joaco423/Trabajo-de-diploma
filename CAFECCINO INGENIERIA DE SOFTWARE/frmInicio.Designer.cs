namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    partial class frmInicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInicio));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.usuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iniciarSesionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cambiarContraseñaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.administradorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDeUsuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDePerfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDeIdiomasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacoraDeEventosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacoraDeCambiosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verificacionIntegridadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maestrosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mozosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mesasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pedidosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionDePedidoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usuarioToolStripMenuItem,
            this.administradorToolStripMenuItem,
            this.maestrosToolStripMenuItem,
            this.pedidosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // usuarioToolStripMenuItem
            // 
            this.usuarioToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iniciarSesionToolStripMenuItem,
            this.cerrarSesionToolStripMenuItem,
            this.cambiarContraseñaToolStripMenuItem});
            this.usuarioToolStripMenuItem.Name = "usuarioToolStripMenuItem";
            this.usuarioToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.usuarioToolStripMenuItem.Text = "Usuario";
            // 
            // iniciarSesionToolStripMenuItem
            // 
            this.iniciarSesionToolStripMenuItem.Name = "iniciarSesionToolStripMenuItem";
            this.iniciarSesionToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.iniciarSesionToolStripMenuItem.Text = "Iniciar Sesion";
            this.iniciarSesionToolStripMenuItem.Click += new System.EventHandler(this.iniciarSesionToolStripMenuItem_Click);
            // 
            // cerrarSesionToolStripMenuItem
            // 
            this.cerrarSesionToolStripMenuItem.Name = "cerrarSesionToolStripMenuItem";
            this.cerrarSesionToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.cerrarSesionToolStripMenuItem.Text = "Cerrar Sesion";
            this.cerrarSesionToolStripMenuItem.Click += new System.EventHandler(this.cerrarSesionToolStripMenuItem_Click);
            //
            // cambiarContraseñaToolStripMenuItem
            //
            this.cambiarContraseñaToolStripMenuItem.Name = "cambiarContraseñaToolStripMenuItem";
            this.cambiarContraseñaToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.cambiarContraseñaToolStripMenuItem.Text = "Cambiar Contraseña";
            this.cambiarContraseñaToolStripMenuItem.Click += new System.EventHandler(this.cambiarContraseñaToolStripMenuItem_Click);
            //
            // administradorToolStripMenuItem
            // 
            this.administradorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionDeUsuariosToolStripMenuItem,
            this.gestionDePerfilesToolStripMenuItem,
            this.gestionDeIdiomasToolStripMenuItem,
            this.bitacoraDeEventosToolStripMenuItem,
            this.bitacoraDeCambiosToolStripMenuItem,
            this.verificacionIntegridadToolStripMenuItem});
            this.administradorToolStripMenuItem.Name = "administradorToolStripMenuItem";
            this.administradorToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.administradorToolStripMenuItem.Text = "Administrador";
            // 
            // gestionDeUsuariosToolStripMenuItem
            // 
            this.gestionDeUsuariosToolStripMenuItem.Name = "gestionDeUsuariosToolStripMenuItem";
            this.gestionDeUsuariosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionDeUsuariosToolStripMenuItem.Text = "Gestion de usuarios";
            this.gestionDeUsuariosToolStripMenuItem.Click += new System.EventHandler(this.gestionDeUsuariosToolStripMenuItem_Click);
            // 
            // gestionDePerfilesToolStripMenuItem
            // 
            this.gestionDePerfilesToolStripMenuItem.Name = "gestionDePerfilesToolStripMenuItem";
            this.gestionDePerfilesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionDePerfilesToolStripMenuItem.Text = "Gestion de perfiles";
            this.gestionDePerfilesToolStripMenuItem.Click += new System.EventHandler(this.gestionDePerfilesToolStripMenuItem_Click);
            // 
            // gestionDeIdiomasToolStripMenuItem
            // 
            this.gestionDeIdiomasToolStripMenuItem.Name = "gestionDeIdiomasToolStripMenuItem";
            this.gestionDeIdiomasToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionDeIdiomasToolStripMenuItem.Text = "Gestion de idiomas";
            this.gestionDeIdiomasToolStripMenuItem.Click += new System.EventHandler(this.gestionDeIdiomasToolStripMenuItem_Click);
            // 
            // bitacoraDeEventosToolStripMenuItem
            // 
            this.bitacoraDeEventosToolStripMenuItem.Name = "bitacoraDeEventosToolStripMenuItem";
            this.bitacoraDeEventosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.bitacoraDeEventosToolStripMenuItem.Text = "Bitacora de eventos";
            this.bitacoraDeEventosToolStripMenuItem.Click += new System.EventHandler(this.bitacoraDeEventosToolStripMenuItem_Click);
            // 
            // bitacoraDeCambiosToolStripMenuItem
            // 
            this.bitacoraDeCambiosToolStripMenuItem.Name = "bitacoraDeCambiosToolStripMenuItem";
            this.bitacoraDeCambiosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.bitacoraDeCambiosToolStripMenuItem.Text = "Bitacora de cambios";
            this.bitacoraDeCambiosToolStripMenuItem.Click += new System.EventHandler(this.bitacoraDeCambiosToolStripMenuItem_Click);
            //
            // verificacionIntegridadToolStripMenuItem
            //
            this.verificacionIntegridadToolStripMenuItem.Name = "verificacionIntegridadToolStripMenuItem";
            this.verificacionIntegridadToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.verificacionIntegridadToolStripMenuItem.Text = "Verificacion de Integridad";
            this.verificacionIntegridadToolStripMenuItem.Visible = false;
            this.verificacionIntegridadToolStripMenuItem.Click += new System.EventHandler(this.verificacionIntegridadToolStripMenuItem_Click);
            //
            // maestrosToolStripMenuItem
            //
            this.maestrosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientesToolStripMenuItem,
            this.productosToolStripMenuItem,
            this.mozosToolStripMenuItem,
            this.mesasToolStripMenuItem});
            this.maestrosToolStripMenuItem.Name = "maestrosToolStripMenuItem";
            this.maestrosToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.maestrosToolStripMenuItem.Text = "Maestros";
            this.maestrosToolStripMenuItem.Visible = false;
            //
            // clientesToolStripMenuItem
            //
            this.clientesToolStripMenuItem.Name = "clientesToolStripMenuItem";
            this.clientesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.clientesToolStripMenuItem.Text = "Clientes";
            this.clientesToolStripMenuItem.Click += new System.EventHandler(this.clientesToolStripMenuItem_Click_235JB);
            //
            // productosToolStripMenuItem
            //
            this.productosToolStripMenuItem.Name = "productosToolStripMenuItem";
            this.productosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.productosToolStripMenuItem.Text = "Productos";
            this.productosToolStripMenuItem.Click += new System.EventHandler(this.productosToolStripMenuItem_Click_235JB);
            //
            // mozosToolStripMenuItem
            //
            this.mozosToolStripMenuItem.Name = "mozosToolStripMenuItem";
            this.mozosToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.mozosToolStripMenuItem.Text = "Mozos";
            this.mozosToolStripMenuItem.Click += new System.EventHandler(this.mozosToolStripMenuItem_Click_235JB);
            //
            // mesasToolStripMenuItem
            //
            this.mesasToolStripMenuItem.Name = "mesasToolStripMenuItem";
            this.mesasToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.mesasToolStripMenuItem.Text = "Mesas";
            this.mesasToolStripMenuItem.Click += new System.EventHandler(this.mesasToolStripMenuItem_Click_235JB);
            //
            // pedidosToolStripMenuItem
            //
            this.pedidosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionDePedidoToolStripMenuItem});
            this.pedidosToolStripMenuItem.Name = "pedidosToolStripMenuItem";
            this.pedidosToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.pedidosToolStripMenuItem.Text = "Pedidos";
            this.pedidosToolStripMenuItem.Visible = false;
            //
            // gestionDePedidoToolStripMenuItem
            //
            this.gestionDePedidoToolStripMenuItem.Name = "gestionDePedidoToolStripMenuItem";
            this.gestionDePedidoToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gestionDePedidoToolStripMenuItem.Text = "Gestion de pedido";
            this.gestionDePedidoToolStripMenuItem.Click += new System.EventHandler(this.gestionDePedidoToolStripMenuItem_Click_235JB);
            //
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // frmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::CAFECCINO_INGENIERIA_DE_SOFTWARE.Properties.Resources.Fondo;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmInicio";
            this.Text = "Inicio";
            this.Load += new System.EventHandler(this.frmInicio_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem usuarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iniciarSesionToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public System.Windows.Forms.ToolStripMenuItem cerrarSesionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cambiarContraseñaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitacoraDeEventosToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem administradorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionDeUsuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionDeIdiomasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionDePerfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitacoraDeCambiosToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem verificacionIntegridadToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem maestrosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mozosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mesasToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem pedidosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestionDePedidoToolStripMenuItem;
    }
}

