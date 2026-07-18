using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace SERVICIOS
{
    public sealed class GestorIdiomas : Subject
    {
        private static GestorIdiomas instancia;

        public static GestorIdiomas Instancia
        {
            get
            {
                if (instancia == null)
                    instancia = new GestorIdiomas();

                return instancia;
            }
        }

        private GestorIdiomas()
        {

        }

        public Idioma IdiomaActual { get; private set; }

        public void CambiarIdioma(Idioma idioma)
        {
            IdiomaActual = idioma;

            Notificar();
        }
    }
}
