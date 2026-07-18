using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Familia : Perfil
    {
        private IList<Perfil> _hijos;

        public Familia()
        {
            _hijos = new List<Perfil>();
        }

        public override IList<Perfil> Hijos
        {
            get
            {
                
                return _hijos.ToList();
            }
        }

        public override void EliminarHijo(int id)
        {
            Perfil aEliminar = _hijos.FirstOrDefault(h => h.Id == id);
            if (aEliminar != null)
                _hijos.Remove(aEliminar);
        }
        public override void VaciarHijos()
        {
            _hijos.Clear();
        }
        public override void AgregarHijo(Perfil p)
        {
            
            if (p != null && !_hijos.Any(x => x.Id == p.Id))
            {
                _hijos.Add(p);
            }
        }
    }
}
