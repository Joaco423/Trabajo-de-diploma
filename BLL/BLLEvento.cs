using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLEvento
    {
        DALEvento eventoDAL = new DALEvento();

        public void RegistrarEvento(string usuario,
                                    string evento,
                                    string modulo,
                                    string criticidad)
        {
            BEEvento ev = new BEEvento();

            ev.NombreUsuario = usuario;
            ev.Fecha = DateTime.Now;
            ev.Evento = evento;
            ev.Modulo = modulo;
            ev.Criticidad = criticidad;

            eventoDAL.RegistrarEvento(ev);
        }
        public DataTable ObtenerEventos()
        {
            return eventoDAL.ListarEventos();
        }

        public DataTable FiltrarEventos(string modulo, string criticidad, string evento, string fecha)
        {
            return eventoDAL.FiltrarEventos(modulo, criticidad, evento, fecha);
        }
    }
}
