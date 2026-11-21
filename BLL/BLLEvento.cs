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
        BLLIntegridad integridadBLL = new BLLIntegridad();

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

            integridadBLL.SincronizarFilasNuevas("Evento");
        }
        public DataTable ObtenerEventos()
        {
            return eventoDAL.ListarEventos();
        }

        public List<string> ObtenerModulos_235JB()
        {
            return eventoDAL.ListarValoresDistintos_235JB("Modulo", "");
        }

        
        public List<string> ObtenerCriticidades_235JB()
        {
            return new List<string> { "Alta", "Media", "Baja" };
        }

        
        public List<string> ObtenerTiposDeEvento_235JB(string modulo_235JB)
        {
            return eventoDAL.ListarValoresDistintos_235JB("Evento", modulo_235JB)
                .Select(e_235JB => System.Text.RegularExpressions.Regex.Replace(e_235JB, @"\d+", "*"))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(e_235JB => e_235JB, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        
        public DataTable FiltrarEventos_235JB(string modulo_235JB, string criticidad_235JB, string tipoEvento_235JB, DateTime? fecha_235JB)
        {
            string patron_235JB = string.IsNullOrEmpty(tipoEvento_235JB)
                ? ""
                : tipoEvento_235JB.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_").Replace("[", "\\[").Replace("*", "%");
            string fechaTexto_235JB = fecha_235JB.HasValue ? fecha_235JB.Value.ToString("yyyy-MM-dd") : "";
            return eventoDAL.FiltrarEventos_235JB(modulo_235JB, criticidad_235JB, patron_235JB, fechaTexto_235JB);
        }

        public DataTable FiltrarEventos(string modulo, string criticidad, string evento, string fecha)
        {
            return eventoDAL.FiltrarEventos(modulo, criticidad, evento, fecha);
        }
    }
}
