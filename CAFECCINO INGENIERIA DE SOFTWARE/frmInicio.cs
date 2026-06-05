using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
        }
        public Login login = new Login();
        public CerrarSesion cerrarSesion = new CerrarSesion();
        public frmEventos eventos = new frmEventos();

        private void frmInicio_Load(object sender, EventArgs e)
        {
            cerrarSesionToolStripMenuItem.Visible = false;
            administradorToolStripMenuItem.Visible = false;
        }

        private void iniciarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login.MdiParent = this;
            login.Show();
        }

        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cerrarSesion.MdiParent = this;
            cerrarSesion.Show();

        }

        private void bitacoraDeEventosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eventos.MdiParent = this;
            eventos.Show();
        }
    }
}
