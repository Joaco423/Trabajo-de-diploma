namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    partial class frmReporteInteligente
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTipoReporte = new System.Windows.Forms.Label();
            this.cmbTipoReporte = new System.Windows.Forms.ComboBox();
            this.dgvReporte = new System.Windows.Forms.DataGridView();
            this.lblConclusion = new System.Windows.Forms.Label();
            this.lblSeleccion = new System.Windows.Forms.Label();
            this.btnGenerarReporte = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            this.SuspendLayout();
            //
            // lblTipoReporte
            //
            this.lblTipoReporte.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTipoReporte.AutoSize = true;
            this.lblTipoReporte.Location = new System.Drawing.Point(255, 23);
            this.lblTipoReporte.Name = "lblTipoReporte";
            this.lblTipoReporte.Size = new System.Drawing.Size(85, 13);
            this.lblTipoReporte.TabIndex = 0;
            this.lblTipoReporte.Text = "Tipo de reporte:";
            //
            // cmbTipoReporte
            //
            this.cmbTipoReporte.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbTipoReporte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoReporte.FormattingEnabled = true;
            this.cmbTipoReporte.Location = new System.Drawing.Point(350, 19);
            this.cmbTipoReporte.Name = "cmbTipoReporte";
            this.cmbTipoReporte.Size = new System.Drawing.Size(260, 21);
            this.cmbTipoReporte.TabIndex = 1;
            this.cmbTipoReporte.SelectedIndexChanged += new System.EventHandler(this.cmbTipoReporte_SelectedIndexChanged_235JB);
            //
            // dgvReporte
            //
            this.dgvReporte.AllowUserToAddRows = false;
            this.dgvReporte.AllowUserToDeleteRows = false;
            this.dgvReporte.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvReporte.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReporte.Location = new System.Drawing.Point(75, 50);
            this.dgvReporte.MultiSelect = true;
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.ReadOnly = true;
            this.dgvReporte.RowHeadersVisible = false;
            this.dgvReporte.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporte.Size = new System.Drawing.Size(710, 268);
            this.dgvReporte.TabIndex = 2;
            this.dgvReporte.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvReporte_DataBindingComplete_235JB);
            this.dgvReporte.SelectionChanged += new System.EventHandler(this.dgvReporte_SelectionChanged_235JB);
            this.dgvReporte.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvReporte_KeyDown_235JB);
            this.dgvReporte.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvReporte_MouseDown_235JB);
            //
            // lblSeleccion
            //
            this.lblSeleccion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSeleccion.ForeColor = System.Drawing.Color.White;
            this.lblSeleccion.Location = new System.Drawing.Point(75, 321);
            this.lblSeleccion.Name = "lblSeleccion";
            this.lblSeleccion.Size = new System.Drawing.Size(710, 16);
            this.lblSeleccion.TabIndex = 5;
            this.lblSeleccion.Text = "Sin filas seleccionadas: el PDF incluye todo el reporte. Para incluir solo algunas, selecciónelas (Ctrl o Shift para varias).";
            this.lblSeleccion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblConclusion
            //
            this.lblConclusion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConclusion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConclusion.ForeColor = System.Drawing.Color.White;
            this.lblConclusion.Location = new System.Drawing.Point(75, 339);
            this.lblConclusion.Name = "lblConclusion";
            this.lblConclusion.Size = new System.Drawing.Size(710, 40);
            this.lblConclusion.TabIndex = 3;
            this.lblConclusion.Text = "Elija un tipo de reporte para ver su resultado.";
            this.lblConclusion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // btnGenerarReporte
            //
            this.btnGenerarReporte.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGenerarReporte.Font = new System.Drawing.Font("Microsoft Sans Serif", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerarReporte.Location = new System.Drawing.Point(240, 385);
            this.btnGenerarReporte.Name = "btnGenerarReporte";
            this.btnGenerarReporte.Size = new System.Drawing.Size(385, 105);
            this.btnGenerarReporte.TabIndex = 4;
            this.btnGenerarReporte.Text = "Generar reporte";
            this.btnGenerarReporte.UseVisualStyleBackColor = true;
            this.btnGenerarReporte.Click += new System.EventHandler(this.btnGenerarReporte_Click_235JB);
            //
            // frmReporteInteligente
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chocolate;
            this.ClientSize = new System.Drawing.Size(860, 510);
            this.Controls.Add(this.btnGenerarReporte);
            this.Controls.Add(this.lblSeleccion);
            this.Controls.Add(this.lblConclusion);
            this.Controls.Add(this.dgvReporte);
            this.Controls.Add(this.cmbTipoReporte);
            this.Controls.Add(this.lblTipoReporte);
            this.Name = "frmReporteInteligente";
            this.Text = "Reporte inteligente";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReporteInteligente_FormClosing_235JB);
            this.Load += new System.EventHandler(this.frmReporteInteligente_Load_235JB);
            this.VisibleChanged += new System.EventHandler(this.frmReporteInteligente_VisibleChanged_235JB);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTipoReporte;
        private System.Windows.Forms.ComboBox cmbTipoReporte;
        private System.Windows.Forms.DataGridView dgvReporte;
        private System.Windows.Forms.Label lblConclusion;
        private System.Windows.Forms.Label lblSeleccion;
        private System.Windows.Forms.Button btnGenerarReporte;
    }
}
