using System;
using DAL;

namespace BLL
{
    public class BLLBackup
    {
        private readonly DALBackup _dal = new DALBackup();
        private readonly BLLIntegridad _integridadBLL = new BLLIntegridad();

        public bool ExisteBackupDisponible() => _dal.ExisteBackup();

        public string ObtenerRutaBackup() => _dal.ObtenerRutaBackup();

     
        public void Recalcular()
        {
            _integridadBLL.SincronizarTodasLasTablas();
            _dal.GenerarBackup();
        }

        public void Restaurar()
        {
            _dal.RestaurarBackup();
        }
    }
}
