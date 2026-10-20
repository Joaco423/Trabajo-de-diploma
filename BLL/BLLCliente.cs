using BE;
using DAL;
using SERVICIOS;
using System;
using System.Data;

namespace BLL
{
    public class BLLCliente
    {
        private readonly DALCliente _dal_235JB = new DALCliente();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();
        private readonly DALPedido _pedidoDAL_235JB = new DALPedido();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        public DataTable Listar_235JB()
        {
            return _dal_235JB.Listar_235JB();
        }

        public Cliente ObtenerPorDNI_235JB(int dni_235JB)
        {
            return _dal_235JB.ObtenerPorDNI_235JB(dni_235JB);
        }

        
        public void Registrar_235JB(Cliente cliente_235JB)
        {
            if (cliente_235JB == null) throw new ArgumentNullException("cliente");
            if (_dal_235JB.ObtenerPorDNI_235JB(cliente_235JB.DNI) != null)
                throw new InvalidOperationException("Ya existe un cliente con ese DNI.");

            _dal_235JB.Insertar_235JB(cliente_235JB);
            _integridad_235JB.SincronizarFilasNuevas("Cliente");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de cliente " + cliente_235JB.DNI, "Maestros", "Baja");
        }

        // Devuelve false si los datos son iguales a los guardados (no se modifica nada).
        public bool Modificar_235JB(Cliente cliente_235JB)
        {
            if (cliente_235JB == null) throw new ArgumentNullException("cliente");
            Cliente actual_235JB = _dal_235JB.ObtenerPorDNI_235JB(cliente_235JB.DNI);
            if (actual_235JB == null)
                throw new InvalidOperationException("No existe un cliente con ese DNI.");
            if (actual_235JB.Nombre == cliente_235JB.Nombre && actual_235JB.Apellido == cliente_235JB.Apellido && actual_235JB.Mail == cliente_235JB.Mail &&
                actual_235JB.NumTelefono == cliente_235JB.NumTelefono && actual_235JB.Domicilio == cliente_235JB.Domicilio && actual_235JB.Activo == cliente_235JB.Activo)
                return false;

            if (!cliente_235JB.Activo) VerificarSinPedidoEnCurso_235JB(cliente_235JB.DNI);

            _dal_235JB.Modificar_235JB(cliente_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Cliente", cliente_235JB.DNI);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Modificacion de cliente " + cliente_235JB.DNI, "Maestros", "Media");
            return true;
        }

        private void VerificarSinPedidoEnCurso_235JB(int dni_235JB)
        {
            if (_pedidoDAL_235JB.ObtenerPedidoEnCurso_235JB(dni_235JB) != null)
                throw new EntidadEnUsoException("El cliente tiene un pedido en curso.");
        }

        
        public void Desactivar_235JB(int dni_235JB)
        {
            VerificarSinPedidoEnCurso_235JB(dni_235JB);
            _dal_235JB.Desactivar_235JB(dni_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Cliente", dni_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Baja logica de cliente " + dni_235JB, "Maestros", "Alta");
        }
    }
}