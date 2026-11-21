using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public partial class frmEventos : Form, IObserver
    {
        BLLEvento bllevento = new BLLEvento();
        Traductor traductor = new Traductor();

        // Mientras se recargan los combos no se deben disparar sus eventos
        private bool _cargandoFiltros_235JB;

        public frmEventos()
        {
            InitializeComponent();
        }

        public void ActualizarIdioma(string idioma)
        {
            traductor.TraducirFormulario(this, int.Parse(idioma));
        }

        private void frmEventos_Load(object sender, EventArgs e)
        {
            CargarFiltros_235JB();
            dataGridView1.DataSource = bllevento.ObtenerEventos();
        }

        // Los combos se arman con lo que hay guardado en la bitacora (modulos, criticidades y tipos de evento)
        private void CargarFiltros_235JB()
        {
            _cargandoFiltros_235JB = true;
            try
            {
                CargarCombo_235JB(cmbModulo, "Todos", bllevento.ObtenerModulos_235JB());
                CargarCombo_235JB(cmbCriticidad, "Todas", bllevento.ObtenerCriticidades_235JB());
                CargarTiposDeEvento_235JB();
            }
            finally
            {
                _cargandoFiltros_235JB = false;
            }
        }

        // Recarga los valores del combo y conserva lo que estaba seleccionado (si sigue existiendo)
        private void CargarCombo_235JB(ComboBox combo_235JB, string todos_235JB, List<string> valores_235JB)
        {
            string seleccionado_235JB = combo_235JB.SelectedItem as string;
            combo_235JB.Items.Clear();
            combo_235JB.Items.Add(todos_235JB);
            combo_235JB.Items.AddRange(valores_235JB.ToArray());

            int indice_235JB = seleccionado_235JB == null ? 0 : combo_235JB.Items.IndexOf(seleccionado_235JB);
            combo_235JB.SelectedIndex = indice_235JB < 0 ? 0 : indice_235JB;
        }

        // Los tipos de evento dependen del modulo elegido
        private void CargarTiposDeEvento_235JB()
        {
            CargarCombo_235JB(cmbEvento, "Todos", bllevento.ObtenerTiposDeEvento_235JB(ValorSeleccionado_235JB(cmbModulo)));
        }

        // "" cuando esta elegido "Todos"/"Todas" (la primera opcion)
        private string ValorSeleccionado_235JB(ComboBox combo_235JB)
        {
            return combo_235JB.SelectedIndex <= 0 ? "" : (string)combo_235JB.SelectedItem;
        }

        private void cmbModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_cargandoFiltros_235JB) return;

            _cargandoFiltros_235JB = true;
            try
            {
                CargarTiposDeEvento_235JB();
            }
            finally
            {
                _cargandoFiltros_235JB = false;
            }
        }

        private void AplicarFiltros_235JB()
        {
            // La fecha es opcional: si la casilla del selector no esta tildada se ven todas las fechas
            DateTime? fecha_235JB = dateTimePicker1.Checked ? dateTimePicker1.Value.Date : (DateTime?)null;

            dataGridView1.DataSource = bllevento.FiltrarEventos_235JB(
                ValorSeleccionado_235JB(cmbModulo),
                ValorSeleccionado_235JB(cmbCriticidad),
                ValorSeleccionado_235JB(cmbEvento),
                fecha_235JB);
        }

        private void Buscar_Click(object sender, EventArgs e)
        {
            AplicarFiltros_235JB();
        }

        // Cada evento (y cada modulo) nuevo se suma a los combos: se actualizan al volver a esta pantalla
        private void frmEventos_Activated(object sender, EventArgs e)
        {
            CargarFiltros_235JB();
        }

        private void frmEventos_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();

            }
        }

        private void frmEventos_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {

                traductor.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);

                // Se registran eventos nuevos mientras la pantalla esta oculta: se actualizan los combos y la grilla
                CargarFiltros_235JB();
                AplicarFiltros_235JB();
            }
        }
    }
}
