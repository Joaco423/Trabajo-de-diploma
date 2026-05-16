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
        DALUsuario DALUsuario = new DALUsuario();

        public bool Login(Usuario user)
        {
            return DALUsuario.Login(user);
        }
    }
}
