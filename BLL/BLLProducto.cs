using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    public class BLLProducto
    {
        private readonly DALProducto _dal_235JB = new DALProducto();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();

        // Se dispara cada vez que cambian los datos de los productos: alta, modificacion o baja, stock de un pedido confirmado
        // o cancelado, y activacion de un cambio de la bitacora (el producto vuelve a esos datos). Cada uno de esos cambios
        // tambien agrega registros en la bitacora. Las pantallas de Productos y de la bitacora se suscriben para actualizarse solas.
        public static event EventHandler ProductosCambiados_235JB;

        internal static void AvisarProductosCambiados_235JB()
        {
            EventHandler suscriptores_235JB = ProductosCambiados_235JB;
            if (suscriptores_235JB != null)
                suscriptores_235JB(null, EventArgs.Empty);
        }

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        public DataTable Listar_235JB()
        {
            return _dal_235JB.Listar_235JB();
        }

        public List<Producto> ListarDisponibles_235JB()
        {
            return _dal_235JB.ListarDisponibles_235JB();
        }

        public Producto ObtenerPorCodigo_235JB(int codigo_235JB)
        {
            return _dal_235JB.ObtenerPorCodigo_235JB(codigo_235JB);
        }

        // Otro producto activo (no borrado) ya usa ese nombre -> msg_ProductoExistente. Lo usa tambien la bitacora al activar.
        internal void VerificarNombreLibre_235JB(string nombre_235JB, int codigoExcluido_235JB)
        {
            int? existente_235JB = _dal_235JB.ObtenerCodigoPorNombre_235JB(nombre_235JB, codigoExcluido_235JB);
            if (existente_235JB.HasValue)
                throw new InvalidOperationException("msg_ProductoExistente");
        }

        public int Registrar_235JB(Producto producto_235JB)
        {
            if (producto_235JB == null) throw new ArgumentNullException("producto");
            VerificarNombreLibre_235JB(producto_235JB.Nombre, 0);

            int codigo_235JB = _dal_235JB.Insertar_235JB(producto_235JB);
            _integridad_235JB.SincronizarFilasNuevas("Producto");
            SincronizarBitacora_235JB();
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de producto " + codigo_235JB, "Maestros", "Baja");
            return codigo_235JB;
        }


        public bool Modificar_235JB(Producto producto_235JB)
        {
            if (producto_235JB == null) throw new ArgumentNullException("producto");
            Producto actual_235JB = _dal_235JB.ObtenerPorCodigo_235JB(producto_235JB.Codigo);
            if (actual_235JB == null)
                throw new InvalidOperationException("No existe un producto con ese codigo.");
            if (actual_235JB.Nombre == producto_235JB.Nombre.Trim() && actual_235JB.Precio == producto_235JB.Precio &&
                actual_235JB.Stock == producto_235JB.Stock && actual_235JB.Disponible == producto_235JB.Disponible)
                return false;

            ActualizarProducto_235JB(producto_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Modificacion de producto " + producto_235JB.Codigo, "Maestros", "Media");
            return true;
        }

        // Guarda los datos del producto y recalcula los digitos verificadores (del producto y de la bitacora, donde el trigger
        // agrego el registro nuevo)
        public void ActualizarProducto_235JB(Producto producto_235JB)
        {
            if (producto_235JB == null) throw new ArgumentNullException("producto");
            VerificarNombreLibre_235JB(producto_235JB.Nombre, producto_235JB.Codigo);

            _dal_235JB.ActualizarProducto_235JB(producto_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Producto", producto_235JB.Codigo);
            SincronizarBitacora_235JB();
        }

        // Baja logica (Bit_Lo_Bo = 1)
        public void Eliminar_235JB(int codigo_235JB)
        {
            _dal_235JB.Eliminar_235JB(codigo_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Producto", codigo_235JB);
            SincronizarBitacora_235JB();
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Baja de producto " + codigo_235JB, "Maestros", "Alta");
        }

        // Cada cambio en Producto hace que el trigger agregue un registro en Producto_C y desactive el anterior: se recalcula
        // el digito verificador de toda la tabla (no solo de la fila nueva) y se avisa a las pantallas que muestran productos
        internal void SincronizarBitacora_235JB()
        {
            _integridad_235JB.SincronizarTabla("Producto_C");
            AvisarProductosCambiados_235JB();
        }
    }
}
