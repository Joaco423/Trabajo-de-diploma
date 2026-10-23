using BE;
using DAL;
using SERVICIOS;
using System;
using System.Data;

namespace BLL
{
    public class BLLMesa
    {
        private readonly DALMesa _dal_235JB = new DALMesa();
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

        public Mesa ObtenerPorNumero_235JB(int numeroMesa_235JB)
        {
            return _dal_235JB.ObtenerPorNumero_235JB(numeroMesa_235JB);
        }

        public void Registrar_235JB(Mesa mesa_235JB)
        {
            if (mesa_235JB == null) throw new ArgumentNullException("mesa");
            if (_dal_235JB.ObtenerPorNumero_235JB(mesa_235JB.NumeroMesa) != null)
                throw new InvalidOperationException("Ya existe una mesa con ese numero.");

            _dal_235JB.Insertar_235JB(mesa_235JB);
            _integridad_235JB.SincronizarFilasNuevas("Mesa");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de mesa " + mesa_235JB.NumeroMesa, "Maestros", "Baja");
        }

        // Devuelve false si la capacidad es igual a la guardada (no se modifica nada).
        public bool Modificar_235JB(Mesa mesa_235JB)
        {
            if (mesa_235JB == null) throw new ArgumentNullException("mesa");
            Mesa actual_235JB = _dal_235JB.ObtenerPorNumero_235JB(mesa_235JB.NumeroMesa);
            if (actual_235JB == null)
                throw new InvalidOperationException("No existe una mesa con ese numero.");
            if (actual_235JB.Capacidad == mesa_235JB.Capacidad)
                return false;

            _dal_235JB.Modificar_235JB(mesa_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Mesa", mesa_235JB.NumeroMesa);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Modificacion de mesa " + mesa_235JB.NumeroMesa, "Maestros", "Media");
            return true;
        }

        public void Eliminar_235JB(int numeroMesa_235JB)
        {
            Mesa actual_235JB = _dal_235JB.ObtenerPorNumero_235JB(numeroMesa_235JB);
            if (actual_235JB != null && actual_235JB.Estado == "Ocupada")
                throw new EntidadEnUsoException("La mesa esta ocupada.");

            _dal_235JB.Eliminar_235JB(numeroMesa_235JB);
            // Tras una baja hay que recalcular el DVV de la tabla (cambio la cantidad de filas).
            _integridad_235JB.SincronizarFilasNuevas("Mesa");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Baja de mesa " + numeroMesa_235JB, "Maestros", "Alta");
        }
    }
}
