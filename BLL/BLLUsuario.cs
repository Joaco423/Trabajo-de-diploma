using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;

namespace BLL
{
    public class BLLUsuario
    {
       GenerarHash generarHash = new GenerarHash();

        DALUsuario DALUsuario = new DALUsuario();

        public bool Login(Usuario user)
        {
            user.Contraseña =
                Seguridad.GenerarHash(user.Contraseña);

            return DALUsuario.Login(user);
        }
        public void ActualizarPasswordHash(string usuario, string contraseña)
        {
            string hash = Seguridad.GenerarHash(contraseña);

            generarHash.ActualizarPasswordHash(usuario, hash);
        }
    }
}

