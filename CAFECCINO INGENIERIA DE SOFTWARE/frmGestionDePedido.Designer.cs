namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    partial class frmGestionDePedido
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            this.gbCliente = new System.Windows.Forms.GroupBox();
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnFinalizarAtencion = new System.Windows.Forms.Button();
            this.lblClienteInfo = new System.Windows.Forms.Label();
            this.btnListaEspera = new System.Windows.Forms.Button();
            this.gbResumen = new System.Windows.Forms.GroupBox();
            this.txtResumen = new System.Windows.Forms.TextBox();
            this.btnNuevoPedido = new System.Windows.Forms.Button();
            this.gbListaEspera = new System.Windows.Forms.GroupBox();
            this.dgvListaEspera = new System.Windows.Forms.DataGridView();
            this.gbEnCurso = new System.Windows.Forms.GroupBox();
            this.dgvEnCurso = new System.Windows.Forms.DataGridView();
            this.gbEnCurso.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnCurso)).BeginInit();
            this.gbCliente.SuspendLayout();
            this.gbResumen.SuspendLayout();
            this.gbListaEspera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaEspera)).BeginInit();
            this.SuspendLayout();
            //
            // gbCliente
            //
            this.gbCliente.Controls.Add(this.lblClienteInfo);
            this.gbCliente.Controls.Add(this.btnIniciar);
            this.gbCliente.Controls.Add(this.txtDNI);
            this.gbCliente.Controls.Add(this.lblDNI);
            this.gbCliente.Location = new System.Drawing.Point(10, 10);
            this.gbCliente.Name = "gbCliente";
            this.gbCliente.Size = new System.Drawing.Size(740, 70);
            this.gbCliente.TabIndex = 0;
            this.gbCliente.TabStop = false;
            this.gbCliente.Text = "Cliente";
            //
            // lblDNI
            //
            this.lblDNI.AutoSize = true;
            this.lblDNI.Location = new System.Drawing.Point(15, 32);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(26, 13);
            this.lblDNI.TabIndex = 0;
            this.lblDNI.Text = "DNI";
            //
            // txtDNI
            //
            this.txtDNI.Location = new System.Drawing.Point(70, 29);
            this.txtDNI.MaxLength = 8;
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(110, 20);
            this.txtDNI.TabIndex = 1;
            //
            // btnIniciar
            //
            this.btnIniciar.Location = new System.Drawing.Point(195, 26);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(140, 28);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text = "Iniciar pedido";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click_235JB);
            //
            // btnFinalizarAtencion
            //
            this.btnFinalizarAtencion.Location = new System.Drawing.Point(15, 110);
            this.btnFinalizarAtencion.Name = "btnFinalizarAtencion";
            this.btnFinalizarAtencion.Size = new System.Drawing.Size(200, 28);
            this.btnFinalizarAtencion.TabIndex = 1;
            this.btnFinalizarAtencion.Text = "Finalizar atención";
            this.btnFinalizarAtencion.UseVisualStyleBackColor = true;
            this.btnFinalizarAtencion.Click += new System.EventHandler(this.btnFinalizarAtencion_Click_235JB);
            //
            // lblClienteInfo
            //
            this.lblClienteInfo.AutoSize = true;
            this.lblClienteInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblClienteInfo.Location = new System.Drawing.Point(355, 32);
            this.lblClienteInfo.Name = "lblClienteInfo";
            this.lblClienteInfo.Size = new System.Drawing.Size(0, 15);
            this.lblClienteInfo.TabIndex = 3;
            //
            // btnListaEspera
            //
            this.btnListaEspera.Location = new System.Drawing.Point(10, 92);
            this.btnListaEspera.Name = "btnListaEspera";
            this.btnListaEspera.Size = new System.Drawing.Size(260, 32);
            this.btnListaEspera.TabIndex = 1;
            this.btnListaEspera.Text = "Atender lista de espera";
            this.btnListaEspera.UseVisualStyleBackColor = true;
            this.btnListaEspera.Click += new System.EventHandler(this.btnListaEspera_Click_235JB);
            //
            // gbResumen
            //
            this.gbResumen.Controls.Add(this.txtResumen);
            this.gbResumen.Location = new System.Drawing.Point(10, 135);
            this.gbResumen.Name = "gbResumen";
            this.gbResumen.Size = new System.Drawing.Size(740, 100);
            this.gbResumen.TabIndex = 2;
            this.gbResumen.TabStop = false;
            this.gbResumen.Text = "Resumen";
            //
            // txtResumen
            //
            this.txtResumen.Location = new System.Drawing.Point(15, 22);
            this.txtResumen.Multiline = true;
            this.txtResumen.Name = "txtResumen";
            this.txtResumen.ReadOnly = true;
            this.txtResumen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResumen.Size = new System.Drawing.Size(710, 65);
            this.txtResumen.TabIndex = 0;
            //
            // btnNuevoPedido
            //
            this.btnNuevoPedido.Location = new System.Drawing.Point(590, 547);
            this.btnNuevoPedido.Name = "btnNuevoPedido";
            this.btnNuevoPedido.Size = new System.Drawing.Size(160, 32);
            this.btnNuevoPedido.TabIndex = 3;
            this.btnNuevoPedido.Text = "Nuevo pedido";
            this.btnNuevoPedido.UseVisualStyleBackColor = true;
            this.btnNuevoPedido.Click += new System.EventHandler(this.btnNuevoPedido_Click_235JB);
            //
            // gbListaEspera
            //
            this.gbListaEspera.Controls.Add(this.dgvListaEspera);
            this.gbListaEspera.Location = new System.Drawing.Point(10, 245);
            this.gbListaEspera.Name = "gbListaEspera";
            this.gbListaEspera.Size = new System.Drawing.Size(740, 130);
            this.gbListaEspera.TabIndex = 4;
            this.gbListaEspera.TabStop = false;
            this.gbListaEspera.Text = "Lista de espera";
            //
            // dgvListaEspera
            //
            this.dgvListaEspera.AllowUserToAddRows = false;
            this.dgvListaEspera.AllowUserToDeleteRows = false;
            this.dgvListaEspera.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvListaEspera.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListaEspera.Location = new System.Drawing.Point(15, 22);
            this.dgvListaEspera.MultiSelect = false;
            this.dgvListaEspera.Name = "dgvListaEspera";
            this.dgvListaEspera.ReadOnly = true;
            this.dgvListaEspera.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListaEspera.Size = new System.Drawing.Size(710, 96);
            this.dgvListaEspera.TabIndex = 0;
            //
            // gbEnCurso
            //
            this.gbEnCurso.Controls.Add(this.btnFinalizarAtencion);
            this.gbEnCurso.Controls.Add(this.dgvEnCurso);
            this.gbEnCurso.Location = new System.Drawing.Point(10, 385);
            this.gbEnCurso.Name = "gbEnCurso";
            this.gbEnCurso.Size = new System.Drawing.Size(740, 152);
            this.gbEnCurso.TabIndex = 5;
            this.gbEnCurso.TabStop = false;
            this.gbEnCurso.Text = "Pedidos en curso";
            //
            // dgvEnCurso
            //
            this.dgvEnCurso.AllowUserToAddRows = false;
            this.dgvEnCurso.AllowUserToDeleteRows = false;
            this.dgvEnCurso.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEnCurso.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEnCurso.Location = new System.Drawing.Point(15, 22);
            this.dgvEnCurso.MultiSelect = false;
            this.dgvEnCurso.Name = "dgvEnCurso";
            this.dgvEnCurso.ReadOnly = true;
            this.dgvEnCurso.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnCurso.Size = new System.Drawing.Size(710, 80);
            this.dgvEnCurso.TabIndex = 0;
            //
            // frmGestionDePedido
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chocolate;
            this.ClientSize = new System.Drawing.Size(760, 592);
            this.Controls.Add(this.gbEnCurso);
            this.Controls.Add(this.gbListaEspera);
            this.Controls.Add(this.btnNuevoPedido);
            this.Controls.Add(this.gbResumen);
            this.Controls.Add(this.btnListaEspera);
            this.Controls.Add(this.gbCliente);
            this.Name = "frmGestionDePedido";
            this.Text = "frmGestionDePedido";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGestionDePedido_FormClosing_235JB);
            this.Load += new System.EventHandler(this.frmGestionDePedido_Load_235JB);
            this.VisibleChanged += new System.EventHandler(this.frmGestionDePedido_VisibleChanged_235JB);
            this.gbCliente.ResumeLayout(false);
            this.gbCliente.PerformLayout();
            this.gbResumen.ResumeLayout(false);
            this.gbResumen.PerformLayout();
            this.gbListaEspera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListaEspera)).EndInit();
            this.gbEnCurso.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnCurso)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCliente;
        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnFinalizarAtencion;
        private System.Windows.Forms.Label lblClienteInfo;
        private System.Windows.Forms.Button btnListaEspera;
        private System.Windows.Forms.GroupBox gbResumen;
        private System.Windows.Forms.TextBox txtResumen;
        private System.Windows.Forms.Button btnNuevoPedido;
        private System.Windows.Forms.GroupBox gbListaEspera;
        private System.Windows.Forms.DataGridView dgvListaEspera;
        private System.Windows.Forms.GroupBox gbEnCurso;
        private System.Windows.Forms.DataGridView dgvEnCurso;
    }
}
