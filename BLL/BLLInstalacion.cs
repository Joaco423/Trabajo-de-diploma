using DAL;

namespace BLL
{
    
    public class BLLInstalacion
    {
        private readonly DALInstalacion _dal_235JB = new DALInstalacion();
        private readonly DALConexion _conexion_235JB = new DALConexion();

        public bool BaseDeDatosLista_235JB()
        {
            return _dal_235JB.BaseDeDatosLista_235JB();
        }

        public void CrearBaseDeDatos_235JB()
        {
            _dal_235JB.CrearBaseDeDatos_235JB();

            
            try
            {
                new BLLIntegridad().SincronizarTodasLasTablas();
            }
            catch
            {
                
            }
        }

        public string NombreBaseDeDatos_235JB()
        {
            return _dal_235JB.NombreBaseDeDatos_235JB();
        }

        // True si es el primer arranque despues de instalar (o reinstalar) el programa con el instalador 2.0.
        // Solo vale para el .exe de la carpeta instalada; ejecutado desde Visual Studio siempre devuelve false.
        public bool EsPrimerArranqueDeInstalacion_235JB(out string marca_235JB)
        {
            marca_235JB = _dal_235JB.MarcaDeInstalacion_235JB();
            return marca_235JB != null && marca_235JB != _dal_235JB.MarcaAtendida_235JB();
        }

        // Despues de atender la instalacion (base creada, conservada o reemplazada) no se vuelve a preguntar hasta la proxima
        public void MarcarInstalacionAtendida_235JB(string marca_235JB)
        {
            _dal_235JB.GuardarMarcaAtendida_235JB(marca_235JB);
        }

        // Al reinstalar, si el usuario elige pisar la base: se borra y se crea de nuevo con el script (sin datos de uso)
        public void ReemplazarBaseDeDatos_235JB()
        {
            _dal_235JB.BorrarBaseDeDatos_235JB();
            CrearBaseDeDatos_235JB();
            RegistrarEventoInstalacion_235JB("Base de datos " + NombreBaseDeDatos_235JB() + " reemplazada por una nueva al instalar", "Alta");
        }

        // Al instalar, si el usuario elige conservar la base que ya existia (la del instalador 1.0 o una instalacion anterior):
        // se mantienen sus datos y se le agrega lo que le falte de la version 2.0. Si cambio algo, se recalculan los digitos
        // verificadores (la estructura nueva cambia el calculo, por ejemplo la columna de baja logica de Producto).
        public void ConservarBaseDeDatos_235JB()
        {
            bool actualizada_235JB = _dal_235JB.ActualizarBaseDeDatos_235JB();
            if (actualizada_235JB)
                new BLLIntegridad().SincronizarTodasLasTablas();

            RegistrarEventoInstalacion_235JB("Se conservó la base de datos " + NombreBaseDeDatos_235JB() + " al instalar" +
                (actualizada_235JB ? " y se actualizó a la versión 2.0" : ""), "Baja");
        }

        private static void RegistrarEventoInstalacion_235JB(string evento_235JB, string criticidad_235JB)
        {
            try
            {
                new BLLEvento().RegistrarEvento("sistema", evento_235JB, "Instalacion", criticidad_235JB);
            }
            catch
            {
                // La bitacora no debe impedir que el programa arranque
            }
        }

        public string ObtenerServidor_235JB()
        {
            return _conexion_235JB.ObtenerServidor_235JB();
        }

        public string RutaArchivoConfiguracion_235JB()
        {
            return _conexion_235JB.RutaArchivoConfiguracion_235JB();
        }

        public bool ProbarServidor_235JB(string servidor_235JB, out string error_235JB)
        {
            return _conexion_235JB.ProbarServidor_235JB(servidor_235JB, out error_235JB);
        }

        public void GuardarServidor_235JB(string servidor_235JB)
        {
            _conexion_235JB.GuardarServidor_235JB(servidor_235JB);
        }

        public System.Collections.Generic.List<string> ListarInstanciasLocales_235JB()
        {
            return _conexion_235JB.ListarInstanciasLocales_235JB();
        }

        
        public bool DetectarYGuardarServidor_235JB()
        {
            string servidor_235JB;
            if (!_conexion_235JB.DetectarServidor_235JB(out servidor_235JB))
                return false;
            _conexion_235JB.GuardarServidor_235JB(servidor_235JB);
            return true;
        }
    }
}
