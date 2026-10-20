using BE;
using DAL;
using SERVICIOS;
using System;
using System.Data;

namespace BLL
{
    public class BLLMozo
    {
        private readonly DALMozo _dal_235JB = new DALMozo();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        public DataTable Listar_235JB()
        {
            return _dal_235JB.Listar_235JB();
        }

        public Mozo ObtenerPorDNI_235JB(int dni_235JB)
        {
            return _dal_235JB.ObtenerPorDNI_235JB(dni_235JB);
        }

        public void Registrar_235JB(Mozo mozo_235JB)
        {
            if (mozo_235JB == null) throw new ArgumentNullException("mozo");
            if (_dal_235JB.ObtenerPorDNI_235JB(mozo_235JB.DNIMozo) != null)
                throw new InvalidOperationException("Ya existe un mozo con ese DNI.");

            _dal_235JB.Insertar_235JB(mozo_235JB);
            _integridad_235JB.SincronizarFilasNuevas("Mozo");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de mozo " + mozo_235JB.DNIMozo, "Maestros", "Baja");
        }

        // Devuelve false si los datos son iguales a los guardados (no se modifica nada).
        public bool Modificar_235JB(Mozo mozo_235JB)
        {
            if (mozo_235JB == null) throw new ArgumentNullException("mozo");
            Mozo actual_235JB = _dal_235JB.ObtenerPorDNI_235JB(mozo_235JB.DNIMozo);
            if (actual_235JB == null)
                throw new InvalidOperationException("No existe un mozo con ese DNI.");
            if (actual_235JB.Nombre == mozo_235JB.Nombre && actual_235JB.Apellido == mozo_235JB.Apellido && actual_235JB.Mail == mozo_235JB.Mail &&
                actual_235JB.NumTelefono == mozo_235JB.NumTelefono && actual_235JB.Activo == mozo_235JB.Activo)
                return false;

            if (!mozo_235JB.Activo) VerificarNoOcupado_235JB(mozo_235JB.DNIMozo);

            _dal_235JB.Modificar_235JB(mozo_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Mozo", mozo_235JB.DNIMozo);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Modificacion de mozo " + mozo_235JB.DNIMozo, "Maestros", "Media");
            return true;
        }

        private void VerificarNoOcupado_235JB(int dni_235JB)
        {
            Mozo actual_235JB = _dal_235JB.ObtenerPorDNI_235JB(dni_235JB);
            if (actual_235JB != null && actual_235JB.Estado == "Ocupado")
                throw new EntidadEnUsoException("El mozo esta atendiendo un pedido.");
        }

        
        public void Desactivar_235JB(int dni_235JB)
        {
            VerificarNoOcupado_235JB(dni_235JB);
            _dal_235JB.Desactivar_235JB(dni_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Mozo", dni_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Baja logica de mozo " + dni_235JB, "Maestros", "Alta");
        }
    }
}