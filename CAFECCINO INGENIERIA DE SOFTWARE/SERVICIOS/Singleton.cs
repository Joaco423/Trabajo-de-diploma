using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace SERVICIOS
{
    public class Singleton
    {
        private static readonly Singleton instance = new Singleton();
        private static Usuario _user;


        private Singleton() { }



        public static Singleton ObtenerInstancia()
        {
            return instance;
        }


        public void IniciarSesion(Usuario userNuevo)
        {
            _user = userNuevo;
        }





        public Usuario Usuario
        {
            get { return _user; }
        }

    }
}
