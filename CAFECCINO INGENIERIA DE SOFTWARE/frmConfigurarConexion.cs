using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    // Se muestra al arrancar solo cuando no se logra conectar con SQL Server: permite escribir o elegir la instancia
    // (localhost, .\SQLEXPRESS, NOMBREPC\SQLEXPRESS...), probarla y guardarla. Todavia no hay base de la que leer
    // traducciones, por eso los textos van en español.
    public class frmConfigurarConexion : Form
    {
        private readonly BLLInstalacion _bll_235JB;
        private readonly ComboBox cboServidor = new ComboBox();
        private readonly Label lblEstado = new Label();
        private readonly Button btnProbar = new Button();
        private readonly Button btnAceptar = new Button();
        private readonly Button btnSalir = new Button();

        public frmConfigurarConexion(BLLInstalacion bll_235JB, string servidorActual_235JB, string motivo_235JB)
        {
            _bll_235JB = bll_235JB;

            Text = "CAFECCINO - Conexión con SQL Server";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = true;
            ClientSize = new Size(540, 330);

            Label intro_235JB = new Label
            {
                Text = "No se pudo conectar con SQL Server en el servidor \"" + servidorActual_235JB + "\".\n" +
                       "Indique el nombre del servidor o de la instancia a usar (se usa su usuario de Windows).",
                Location = new Point(15, 12),
                Size = new Size(510, 40)
            };

            Label etiqueta_235JB = new Label { Text = "Servidor / instancia:", Location = new Point(15, 62), AutoSize = true };

            cboServidor.Location = new Point(15, 82);
            cboServidor.Size = new Size(380, 24);
            cboServidor.DropDownStyle = ComboBoxStyle.DropDown;
            foreach (string instancia_235JB in CandidatosDeServidor_235JB())
                cboServidor.Items.Add(instancia_235JB);
            cboServidor.Text = servidorActual_235JB;

            btnProbar.Text = "Probar conexión";
            btnProbar.Location = new Point(405, 80);
            btnProbar.Size = new Size(120, 28);
            btnProbar.Click += btnProbar_Click_235JB;

            Label ejemplos_235JB = new Label
            {
                Text = "Ejemplos:  localhost   |   .\\SQLEXPRESS   |   NOMBREPC\\SQLEXPRESS",
                Location = new Point(15, 112),
                AutoSize = true,
                ForeColor = Color.DimGray
            };

            lblEstado.Location = new Point(15, 140);
            lblEstado.Size = new Size(510, 75);
            lblEstado.Text = string.IsNullOrWhiteSpace(motivo_235JB) ? string.Empty : "Detalle del error:\n" + motivo_235JB;
            lblEstado.ForeColor = Color.Firebrick;

            Label archivo_235JB = new Label
            {
                Text = "La elección se guarda en:\n" + _bll_235JB.RutaArchivoConfiguracion_235JB(),
                Location = new Point(15, 225),
                Size = new Size(510, 45),
                ForeColor = Color.DimGray
            };

            btnAceptar.Text = "Aceptar";
            btnAceptar.Location = new Point(310, 285);
            btnAceptar.Size = new Size(100, 30);
            btnAceptar.Click += btnAceptar_Click_235JB;

            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(425, 285);
            btnSalir.Size = new Size(100, 30);
            btnSalir.DialogResult = DialogResult.Cancel;

            AcceptButton = btnAceptar;
            CancelButton = btnSalir;
            Controls.AddRange(new Control[] { intro_235JB, etiqueta_235JB, cboServidor, btnProbar, ejemplos_235JB, lblEstado, archivo_235JB, btnAceptar, btnSalir });
        }

        // Instancias detectadas en esta PC y los nombres habituales de la sala
        private System.Collections.Generic.List<string> CandidatosDeServidor_235JB()
        {
            System.Collections.Generic.List<string> lista_235JB = _bll_235JB.ListarInstanciasLocales_235JB();
            foreach (string habitual_235JB in new[] { "localhost", @".\SQLEXPRESS", @"localhost\SQLEXPRESS", "(local)" })
                if (!lista_235JB.Contains(habitual_235JB))
                    lista_235JB.Add(habitual_235JB);
            return lista_235JB;
        }

        private bool Probar_235JB()
        {
            string error_235JB;
            Cursor = Cursors.WaitCursor;
            btnProbar.Enabled = false;
            btnAceptar.Enabled = false;
            lblEstado.ForeColor = Color.DimGray;
            lblEstado.Text = "Probando la conexión...";
            Application.DoEvents();

            bool ok_235JB;
            try
            {
                ok_235JB = _bll_235JB.ProbarServidor_235JB(cboServidor.Text, out error_235JB);
            }
            finally
            {
                Cursor = Cursors.Default;
                btnProbar.Enabled = true;
                btnAceptar.Enabled = true;
            }

            if (ok_235JB)
            {
                lblEstado.ForeColor = Color.ForestGreen;
                lblEstado.Text = "Conexión exitosa con \"" + cboServidor.Text.Trim() + "\".";
            }
            else
            {
                lblEstado.ForeColor = Color.Firebrick;
                lblEstado.Text = "No se pudo conectar:\n" + error_235JB;
            }
            return ok_235JB;
        }

        private void btnProbar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Probar_235JB();
        }

        private void btnAceptar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (!Probar_235JB())
                return;

            try
            {
                _bll_235JB.GuardarServidor_235JB(cboServidor.Text);
            }
            catch (Exception ex_235JB)
            {
                lblEstado.ForeColor = Color.Firebrick;
                lblEstado.Text = "No se pudo guardar la configuración:\n" + ex_235JB.Message;
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
