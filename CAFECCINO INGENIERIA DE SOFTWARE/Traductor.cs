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
                string texto = bllIdioma.ObtenerTexto(idIdioma, nombreFormulario, control.Name);
                if (!string.IsNullOrEmpty(texto))
                    control.Text = texto;
                if (control.HasChildren)
                    TraducirControles(control.Controls, nombreFormulario, idIdioma);
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
