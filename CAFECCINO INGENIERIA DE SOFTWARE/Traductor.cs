using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    public class Traductor
    {
        private readonly BLLIdioma bllIdioma = new BLLIdioma();

        

        /// <summary>
        /// Traduce un mensaje (ej. de MessageBox) que no es un control fijo del formulario.
        /// Usa el mismo mecanismo que los controles: "formulario" + "clave" identifican la fila en
        /// ControlIdioma/Traduccion. Si no hay traducción cargada para el idioma actual, devuelve el
        /// texto original en español.
        /// </summary>
        public string ObtenerMensaje(string formulario, string clave, string textoOriginal)
        {
            int idIdioma = Singleton.Instancia.IdiomaActual;
            string traducido = bllIdioma.ObtenerTexto(idIdioma, formulario, clave);
            return string.IsNullOrEmpty(traducido) ? textoOriginal : traducido;
        }

        public void TraducirFormulario(Form formulario, int idIdioma)
        {
            TraducirControles(formulario.Controls, formulario.Name, idIdioma);

            
            foreach (Control control in formulario.Controls)
            {
                if (control is MenuStrip menuStrip)
                    TraducirMenuItems(menuStrip.Items, formulario.Name, idIdioma);
            }
        }

        private void TraducirControles(Control.ControlCollection controles,
                                       string nombreFormulario,
                                       int idIdioma)
        {
            foreach (Control control in controles)
            {
                // Las pantallas hijas de un contenedor MDI (frmInicio) cuelgan de su MdiClient: cada una se traduce a si misma,
                // con su propio nombre de formulario. Recorrerlas desde el padre las retraduciria con el nombre equivocado.
                if (control is Form)
                    continue;

                string texto = bllIdioma.ObtenerTexto(idIdioma, nombreFormulario, control.Name);
                if (!string.IsNullOrEmpty(texto))
                    control.Text = texto;

                DataGridView grilla_235JB = control as DataGridView;
                if (grilla_235JB != null)
                    PrepararGrilla_235JB(grilla_235JB, nombreFormulario, idIdioma);

                if (control.HasChildren)
                    TraducirControles(control.Controls, nombreFormulario, idIdioma);
            }
        }

        // Grillas: los encabezados se traducen con la clave "grilla.columna". Las columnas se crean cada vez que se asigna el
        // DataSource, por eso ademas de traducir las que ya existen se engancha DataBindingComplete (una sola vez por grilla)
        // para traducirlas en cada recarga, con el idioma que este activo en ese momento.
        private readonly HashSet<DataGridView> _grillas_235JB = new HashSet<DataGridView>();

        private void PrepararGrilla_235JB(DataGridView grilla_235JB, string nombreFormulario_235JB, int idIdioma_235JB)
        {
            
            Form duenio_235JB = grilla_235JB.FindForm();
            if (duenio_235JB != null && duenio_235JB.Name != nombreFormulario_235JB)
                return;

            TraducirColumnas_235JB(grilla_235JB, nombreFormulario_235JB, idIdioma_235JB);

            if (_grillas_235JB.Add(grilla_235JB))
            {
                grilla_235JB.DataBindingComplete += (sender_235JB, e_235JB) =>
                    TraducirColumnas_235JB(grilla_235JB, nombreFormulario_235JB, Singleton.Instancia.IdiomaActual);
            }
        }

        private void TraducirColumnas_235JB(DataGridView grilla_235JB, string nombreFormulario_235JB, int idIdioma_235JB)
        {
            if (grilla_235JB.Columns.Count == 0)
                return;

            Dictionary<string, string> textos_235JB = bllIdioma.ObtenerTextosColumnas_235JB(idIdioma_235JB, nombreFormulario_235JB);
            foreach (DataGridViewColumn columna_235JB in grilla_235JB.Columns)
            {
                // El encabezado original se guarda para poder volver a el si no hay traduccion en el idioma activo
                string original_235JB = columna_235JB.Tag as string;
                if (original_235JB == null)
                {
                    original_235JB = columna_235JB.HeaderText;
                    columna_235JB.Tag = original_235JB;
                }

                string traducido_235JB;
                columna_235JB.HeaderText = textos_235JB.TryGetValue(grilla_235JB.Name + "." + columna_235JB.Name, out traducido_235JB)
                    && !string.IsNullOrEmpty(traducido_235JB) ? traducido_235JB : original_235JB;
            }
        }

        private void TraducirMenuItems(ToolStripItemCollection items,
                                       string nombreFormulario,
                                       int idIdioma)
        {
            foreach (ToolStripItem item in items)
            {
                string texto = bllIdioma.ObtenerTexto(idIdioma, nombreFormulario, item.Name);
                if (!string.IsNullOrEmpty(texto))
                    item.Text = texto;

                
                if (item is ToolStripMenuItem menuItem && menuItem.DropDownItems.Count > 0)
                    TraducirMenuItems(menuItem.DropDownItems, nombreFormulario, idIdioma);
            }
        }
    }
}
