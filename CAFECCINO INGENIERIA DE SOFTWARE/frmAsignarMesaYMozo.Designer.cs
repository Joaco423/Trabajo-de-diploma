namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    partial class frmAsignarMesaYMozo
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
            this.gbMesa = new System.Windows.Forms.GroupBox();
            this.dgvMesas = new System.Windows.Forms.DataGridView();
            this.btnAsignarMesa = new System.Windows.Forms.Button();
            this.btnMesaDesdeLista = new System.Windows.Forms.Button();
            this.gbMozo = new System.Windows.Forms.GroupBox();
            this.dgvMozos = new System.Windows.Forms.DataGridView();
            this.btnAsignarMozo = new System.Windows.Forms.Button();
            this.btnMozoDesdeLista = new System.Windows.Forms.Button();
            this.gbResumen = new System.Windows.Forms.GroupBox();
            this.txtResumen = new System.Windows.Forms.TextBox();
            this.btnListaEspera = new System.Windows.Forms.Button();
            this.btnCancelarPedido = new System.Windows.Forms.Button();
            this.btnContinuar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.gbMesa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas)).BeginInit();
            this.gbMozo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMozos)).BeginInit();
            this.gbResumen.SuspendLayout();
            this.SuspendLayout();
            //
            // gbMesa
            //
            this.gbMesa.Controls.Add(this.btnMesaDesdeLista);
            this.gbMesa.Controls.Add(this.btnAsignarMesa);
            this.gbMesa.Controls.Add(this.dgvMesas);
            this.gbMesa.Location = new System.Drawing.Point(10, 10);
            this.gbMesa.Name = "gbMesa";
            this.gbMesa.Size = new System.Drawing.Size(435, 190);
            this.gbMesa.TabIndex = 0;
            this.gbMesa.TabStop = false;
            this.gbMesa.Text = "Mesa";
            //
            // dgvMesas
            //
            this.dgvMesas.AllowUserToAddRows = false;
            this.dgvMesas.AllowUserToDeleteRows = false;
            this.dgvMesas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMesas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMesas.Location = new System.Drawing.Point(15, 25);
            this.dgvMesas.MultiSelect = false;
            this.dgvMesas.Name = "dgvMesas";
            this.dgvMesas.ReadOnly = true;
            this.dgvMesas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMesas.Size = new System.Drawing.Size(405, 105);
            this.dgvMesas.TabIndex = 0;
            //
            // btnAsignarMesa
            //
            this.btnAsignarMesa.Location = new System.Drawing.Point(15, 142);
            this.btnAsignarMesa.Name = "btnAsignarMesa";
            this.btnAsignarMesa.Size = new System.Drawing.Size(140, 32);
            this.btnAsignarMesa.TabIndex = 1;
            this.btnAsignarMesa.Text = "Asignar mesa";
            this.btnAsignarMesa.UseVisualStyleBackColor = true;
            this.btnAsignarMesa.Click += new System.EventHandler(this.btnAsignarMesa_Click_235JB);
            //
            // btnMesaDesdeLista
            //
            this.btnMesaDesdeLista.Location = new System.Drawing.Point(165, 142);
            this.btnMesaDesdeLista.Name = "btnMesaDesdeLista";
            this.btnMesaDesdeLista.Size = new System.Drawing.Size(255, 32);
            this.btnMesaDesdeLista.TabIndex = 2;
            this.btnMesaDesdeLista.Text = "Mesa liberada: asignar desde lista de espera";
            this.btnMesaDesdeLista.UseVisualStyleBackColor = true;
            this.btnMesaDesdeLista.Click += new System.EventHandler(this.btnMesaDesdeLista_Click_235JB);
            //
            // gbMozo
            //
            this.gbMozo.Controls.Add(this.btnMozoDesdeLista);
            this.gbMozo.Controls.Add(this.btnAsignarMozo);
            this.gbMozo.Controls.Add(this.dgvMozos);
            this.gbMozo.Location = new System.Drawing.Point(455, 10);
            this.gbMozo.Name = "gbMozo";
            this.gbMozo.Size = new System.Drawing.Size(435, 190);
            this.gbMozo.TabIndex = 1;
            this.gbMozo.TabStop = false;
            this.gbMozo.Text = "Mozo";
            //
            // dgvMozos
            //
            this.dgvMozos.AllowUserToAddRows = false;
            this.dgvMozos.AllowUserToDeleteRows = false;
            this.dgvMozos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMozos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMozos.Location = new System.Drawing.Point(15, 25);
            this.dgvMozos.MultiSelect = false;
            this.dgvMozos.Name = "dgvMozos";
            this.dgvMozos.ReadOnly = true;
            this.dgvMozos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMozos.Size = new System.Drawing.Size(405, 105);
            this.dgvMozos.TabIndex = 0;
            //
            // btnAsignarMozo
            //
            this.btnAsignarMozo.Location = new System.Drawing.Point(15, 142);
            this.btnAsignarMozo.Name = "btnAsignarMozo";
            this.btnAsignarMozo.Size = new System.Drawing.Size(140, 32);
            this.btnAsignarMozo.TabIndex = 1;
            this.btnAsignarMozo.Text = "Asignar mozo";
            this.btnAsignarMozo.UseVisualStyleBackColor = true;
            this.btnAsignarMozo.Click += new System.EventHandler(this.btnAsignarMozo_Click_235JB);
            //
            // btnMozoDesdeLista
            //
            this.btnMozoDesdeLista.Location = new System.Drawing.Point(165, 142);
            this.btnMozoDesdeLista.Name = "btnMozoDesdeLista";
            this.btnMozoDesdeLista.Size = new System.Drawing.Size(255, 32);
            this.btnMozoDesdeLista.TabIndex = 2;
            this.btnMozoDesdeLista.Text = "Mozo liberado: asignar desde lista de espera";
            this.btnMozoDesdeLista.UseVisualStyleBackColor = true;
            this.btnMozoDesdeLista.Click += new System.EventHandler(this.btnMozoDesdeLista_Click_235JB);
            //
            // gbResumen
            //
            this.gbResumen.Controls.Add(this.txtResumen);
            this.gbResumen.Location = new System.Drawing.Point(10, 210);
            this.gbResumen.Name = "gbResumen";
            this.gbResumen.Size = new System.Drawing.Size(880, 220);
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
            this.txtResumen.Size = new System.Drawing.Size(850, 185);
            this.txtResumen.TabIndex = 0;
            //
            // btnListaEspera
            //
            this.btnListaEspera.Location = new System.Drawing.Point(10, 445);
            this.btnListaEspera.Name = "btnListaEspera";
            this.btnListaEspera.Size = new System.Drawing.Size(180, 34);
            this.btnListaEspera.TabIndex = 3;
            this.btnListaEspera.Text = "Ingresar a lista de espera";
            this.btnListaEspera.UseVisualStyleBackColor = true;
            this.btnListaEspera.Click += new System.EventHandler(this.btnListaEspera_Click_235JB);
            //
            // btnCancelarPedido
            //
            this.btnCancelarPedido.Location = new System.Drawing.Point(200, 445);
            this.btnCancelarPedido.Name = "btnCancelarPedido";
            this.btnCancelarPedido.Size = new System.Drawing.Size(180, 34);
            this.btnCancelarPedido.TabIndex = 4;
            this.btnCancelarPedido.Text = "Cancelar pedido";
            this.btnCancelarPedido.UseVisualStyleBackColor = true;
            this.btnCancelarPedido.Click += new System.EventHandler(this.btnCancelarPedido_Click_235JB);
            //
            // btnContinuar
            //
            this.btnContinuar.Location = new System.Drawing.Point(600, 445);
            this.btnContinuar.Name = "btnContinuar";
            this.btnContinuar.Size = new System.Drawing.Size(140, 34);
            this.btnContinuar.TabIndex = 5;
            this.btnContinuar.Text = "Continuar";
            this.btnContinuar.UseVisualStyleBackColor = true;
            this.btnContinuar.Visible = false;
            this.btnContinuar.Click += new System.EventHandler(this.btnContinuar_Click_235JB);
            //
            // btnCerrar
            //
            this.btnCerrar.Location = new System.Drawing.Point(750, 445);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(140, 34);
            this.btnCerrar.TabIndex = 6;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click_235JB);
            //
            // frmAsignarMesaYMozo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chocolate;
            this.ClientSize = new System.Drawing.Size(900, 495);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnContinuar);
            this.Controls.Add(this.btnCancelarPedido);
            this.Controls.Add(this.btnListaEspera);
            this.Controls.Add(this.gbResumen);
            this.Controls.Add(this.gbMozo);
            this.Controls.Add(this.gbMesa);
            this.Name = "frmAsignarMesaYMozo";
            this.Text = "frmAsignarMesaYMozo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAsignarMesaYMozo_FormClosing_235JB);
            this.Load += new System.EventHandler(this.frmAsignarMesaYMozo_Load_235JB);
            this.Shown += new System.EventHandler(this.frmAsignarMesaYMozo_Shown_235JB);
            this.gbMesa.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas)).EndInit();
            this.gbMozo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMozos)).EndInit();
            this.gbResumen.ResumeLayout(false);
            this.gbResumen.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbMesa;
        private System.Windows.Forms.DataGridView dgvMesas;
        private System.Windows.Forms.Button btnAsignarMesa;
        private System.Windows.Forms.Button btnMesaDesdeLista;
        private System.Windows.Forms.GroupBox gbMozo;
        private System.Windows.Forms.DataGridView dgvMozos;
        private System.Windows.Forms.Button btnAsignarMozo;
        private System.Windows.Forms.Button btnMozoDesdeLista;
        private System.Windows.Forms.GroupBox gbResumen;
        private System.Windows.Forms.TextBox txtResumen;
        private System.Windows.Forms.Button btnListaEspera;
        private System.Windows.Forms.Button btnCancelarPedido;
        private System.Windows.Forms.Button btnContinuar;
        private System.Windows.Forms.Button btnCerrar;
    }
}
