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

        // Regla: no puede haber dos productos con el mismo nombre (el stock se cambia modificando el existente).
        private void VerificarNombreLibre_235JB(string nombre_235JB, int codigoExcluido_235JB)
        {
            int? existente_235JB = _dal_235JB.ObtenerCodigoPorNombre_235JB(nombre_235JB, codigoExcluido_235JB);
            if (existente_235JB.HasValue)
                throw new ProductoExistenteException(nombre_235JB, existente_235JB.Value);
        }

        public int Registrar_235JB(Producto producto_235JB)
        {
            if (producto_235JB == null) throw new ArgumentNullException("producto");
            VerificarNombreLibre_235JB(producto_235JB.Nombre, 0);

            int codigo_235JB = _dal_235JB.Insertar_235JB(producto_235JB);
            _integridad_235JB.SincronizarFilasNuevas("Producto");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de producto " + codigo_235JB, "Maestros", "Baja");
            return codigo_235JB;
        }

        // Devuelve false si los datos son iguales a los guardados (no se modifica nada).
        public bool Modificar_235JB(Producto producto_235JB)
        {
            if (producto_235JB == null) throw new ArgumentNullException("producto");
            Producto actual_235JB = _dal_235JB.ObtenerPorCodigo_235JB(producto_235JB.Codigo);
            if (actual_235JB == null)
                throw new InvalidOperationException("No existe un producto con ese codigo.");
            if (actual_235JB.Nombre == producto_235JB.Nombre.Trim() && actual_235JB.Precio == producto_235JB.Precio &&
                actual_235JB.Stock == producto_235JB.Stock && actual_235JB.Disponible == producto_235JB.Disponible)
                return false;
            VerificarNombreLibre_235JB(producto_235JB.Nombre, producto_235JB.Codigo);

            _dal_235JB.Modificar_235JB(producto_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Producto", producto_235JB.Codigo);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Modificacion de producto " + producto_235JB.Codigo, "Maestros", "Media");
            return true;
        }

        public void Eliminar_235JB(int codigo_235JB)
        {
            _dal_235JB.Eliminar_235JB(codigo_235JB);
            _integridad_235JB.SincronizarFilasNuevas("Producto");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Baja de producto " + codigo_235JB, "Maestros", "Alta");
        }
    }
}
