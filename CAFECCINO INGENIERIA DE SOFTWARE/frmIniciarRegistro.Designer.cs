namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    partial class frmIniciarRegistro
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
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.btnConsultarDNI = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblDNI
            //
            this.lblDNI.AutoSize = true;
            this.lblDNI.Location = new System.Drawing.Point(30, 35);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(26, 13);
            this.lblDNI.TabIndex = 0;
            this.lblDNI.Text = "DNI";
            //
            // txtDNI
            //
            this.txtDNI.Location = new System.Drawing.Point(90, 32);
            this.txtDNI.MaxLength = 8;
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(150, 20);
            this.txtDNI.TabIndex = 1;
            //
            // btnConsultarDNI
            //
            this.btnConsultarDNI.Location = new System.Drawing.Point(90, 80);
            this.btnConsultarDNI.Name = "btnConsultarDNI";
            this.btnConsultarDNI.Size = new System.Drawing.Size(140, 30);
            this.btnConsultarDNI.TabIndex = 2;
            this.btnConsultarDNI.Text = "Consultar DNI";
            this.btnConsultarDNI.UseVisualStyleBackColor = true;
            this.btnConsultarDNI.Click += new System.EventHandler(this.btnConsultarDNI_Click_235JB);
            //
            // btnCancelar
            //
            this.btnCancelar.Location = new System.Drawing.Point(245, 80);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(100, 30);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click_235JB);
            //
            // frmIniciarRegistro
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Chocolate;
            this.ClientSize = new System.Drawing.Size(390, 140);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConsultarDNI);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.lblDNI);
            this.Name = "frmIniciarRegistro";
            this.Text = "frmIniciarRegistro";
            this.Load += new System.EventHandler(this.frmIniciarRegistro_Load_235JB);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.Button btnConsultarDNI;
        private System.Windows.Forms.Button btnCancelar;
    }
}
