namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    partial class frmGestionDeReservas
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
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.gbResumen = new System.Windows.Forms.GroupBox();
            this.txtResumen = new System.Windows.Forms.TextBox();
            this.gbReservas = new System.Windows.Forms.GroupBox();
            this.btnGenerarReporte = new System.Windows.Forms.Button();
            this.btnCancelarReserva = new System.Windows.Forms.Button();
            this.btnReservarDeLista = new System.Windows.Forms.Button();
            this.dgvReservas = new System.Windows.Forms.DataGridView();
            this.gbEspera = new System.Windows.Forms.GroupBox();
            this.dgvEspera = new System.Windows.Forms.DataGridView();
            this.gbResumen.SuspendLayout();
            this.gbReservas.SuspendLayout();
            this.gbEspera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEspera)).BeginInit();
            this.SuspendLayout();
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(20, 20);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(260, 36);
            this.btnIniciar.TabIndex = 0;
            this.btnIniciar.Text = "Iniciar registro de cliente";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click_235JB);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(590, 20);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(160, 36);
            this.btnLimpiar.TabIndex = 1;
            this.btnLimpiar.Text = "Limpiar resumen";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click_235JB);
            // 
            // gbResumen
            // 
            this.gbResumen.Controls.Add(this.txtResumen);
            this.gbResumen.Location = new System.Drawing.Point(20, 70);
            this.gbResumen.Name = "gbResumen";
            this.gbResumen.Size = new System.Drawing.Size(730, 150);
            this.gbResumen.TabIndex = 2;
            this.gbResumen.TabStop = false;
            this.gbResumen.Text = "Resumen";
            // 
            // txtResumen
            // 
            this.txtResumen.Location = new System.Drawing.Point(15, 25);
            this.txtResumen.Multiline = true;
            this.txtResumen.Name = "txtResumen";
            this.txtResumen.ReadOnly = true;
            this.txtResumen.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResumen.Size = new System.Drawing.Size(700, 110);
            this.txtResumen.TabIndex = 0;
            // 
            // gbReservas
            // 
            this.gbReservas.Controls.Add(this.btnReservarDeLista);
            this.gbReservas.Controls.Add(this.btnGenerarReporte);
            this.gbReservas.Controls.Add(this.btnCancelarReserva);
            this.gbReservas.Controls.Add(this.dgvReservas);
            this.gbReservas.Location = new System.Drawing.Point(20, 230);
            this.gbReservas.Name = "gbReservas";
            this.gbReservas.Size = new System.Drawing.Size(730, 215);
            this.gbReservas.TabIndex = 3;
            this.gbReservas.TabStop = false;
            this.gbReservas.Text = "Reservas activas";
            // 
            // btnGenerarReporte
            // 
            this.btnGenerarReporte.Location = new System.Drawing.Point(235, 172);
            this.btnGenerarReporte.Name = "btnGenerarReporte";
            this.btnGenerarReporte.Size = new System.Drawing.Size(210, 30);
            this.btnGenerarReporte.TabIndex = 2;
            this.btnGenerarReporte.Text = "Generar reporte";
            this.btnGenerarReporte.UseVisualStyleBackColor = true;
            this.btnGenerarReporte.Click += new System.EventHandler(this.btnGenerarReporte_Click_235JB);
            //
            // btnReservarDeLista
            //
            this.btnReservarDeLista.Location = new System.Drawing.Point(455, 172);
            this.btnReservarDeLista.Name = "btnReservarDeLista";
            this.btnReservarDeLista.Size = new System.Drawing.Size(260, 30);
            this.btnReservarDeLista.TabIndex = 3;
            this.btnReservarDeLista.Text = "Reservar para lista de espera";
            this.btnReservarDeLista.UseVisualStyleBackColor = true;
            this.btnReservarDeLista.Click += new System.EventHandler(this.btnReservarDeLista_Click_235JB);
            //
            // btnCancelarReserva
            //
            this.btnCancelarReserva.Location = new System.Drawing.Point(15, 172);
            this.btnCancelarReserva.Name = "btnCancelarReserva";
            this.btnCancelarReserva.Size = new System.Drawing.Size(210, 30);
            this.btnCancelarReserva.TabIndex = 1;
            this.btnCancelarReserva.Text = "Cancelar reserva seleccionada";
            this.btnCancelarReserva.UseVisualStyleBackColor = true;
            this.btnCancelarReserva.Click += new System.EventHandler(this.btnCancelarReserva_Click_235JB);
            // 
            // dgvReservas
            // 
            this.dgvReservas.AllowUserToAddRows = false;
            this.dgvReservas.AllowUserToDeleteRows = false;
            this.dgvReservas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReservas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReservas.Location = new System.Drawing.Point(15, 25);
            this.dgvReservas.MultiSelect = false;
            this.dgvReservas.Name = "dgvReservas";
            this.dgvReservas.ReadOnly = true;
            this.dgvReservas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservas.Size = new System.Drawing.Size(700, 140);
            this.dgvReservas.TabIndex = 0;
            this.dgvReservas.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvReservas_KeyDown_235JB);
            this.dgvReservas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvReservas_MouseDown_235JB);
            //
            // gbEspera
            //
            this.gbEspera.Controls.Add(this.dgvEspera);
            this.gbEspera.Location = new System.Drawing.Point(20, 455);
            this.gbEspera.Name = "gbEspera";
            this.gbEspera.Size = new System.Drawing.Size(730, 150);
            this.gbEspera.TabIndex = 4;
            this.gbEspera.TabStop = false;
            this.gbEspera.Text = "Lista de espera de mesas";
            //
            // dgvEspera
            //
            this.dgvEspera.AllowUserToAddRows = false;
            this.dgvEspera.AllowUserToDeleteRows = false;
            this.dgvEspera.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEspera.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEspera.Location = new System.Drawing.Point(15, 25);
            this.dgvEspera.MultiSelect = false;
            this.dgvEspera.Name = "dgvEspera";
            this.dgvEspera.ReadOnly = true;
            this.dgvEspera.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEspera.Size = new System.Drawing.Size(700, 110);
            this.dgvEspera.TabIndex = 0;
            //
            // frmGestionDeReservas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chocolate;
            this.ClientSize = new System.Drawing.Size(770, 625);
            this.Controls.Add(this.gbEspera);
            this.Controls.Add(this.gbReservas);
            this.Controls.Add(this.gbResumen);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnIniciar);
            this.Name = "frmGestionDeReservas";
            this.Text = "frmGestionDeReservas";
            this.Activated += new System.EventHandler(this.frmGestionDeReservas_Activated_235JB);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGestionDeReservas_FormClosing_235JB);
            this.Load += new System.EventHandler(this.frmGestionDeReservas_Load);
            this.VisibleChanged += new System.EventHandler(this.frmGestionDeReservas_VisibleChanged_235JB);
            this.gbResumen.ResumeLayout(false);
            this.gbResumen.PerformLayout();
            this.gbReservas.ResumeLayout(false);
            this.gbEspera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEspera)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.GroupBox gbResumen;
        private System.Windows.Forms.TextBox txtResumen;
        private System.Windows.Forms.GroupBox gbReservas;
        private System.Windows.Forms.DataGridView dgvReservas;
        private System.Windows.Forms.Button btnCancelarReserva;
        private System.Windows.Forms.Button btnGenerarReporte;
        private System.Windows.Forms.Button btnReservarDeLista;
        private System.Windows.Forms.GroupBox gbEspera;
        private System.Windows.Forms.DataGridView dgvEspera;
    }
}
