using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;

namespace BLL
{
    // Bitacora de cambios de producto (Producto_C): listar y filtrar los registros, y activar uno.
    // Los registros los crea el trigger de Producto; al activar, el trigger de Producto_C actualiza el producto.
    public class BLLProducto_C
    {
        private readonly DALProducto_C _dal_235JB = new DALProducto_C();
        private readonly BLLProducto _producto_235JB = new BLLProducto();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        public List<Producto_C> ListarCambios_235JB()
        {
            return _dal_235JB.ListarCambios_235JB();
        }

        // msg_RangoFechas: la fecha inicial es posterior a la final
        public List<Producto_C> FiltrarCambios_235JB(int? codigoProducto_235JB, string nombre_235JB, DateTime? fechaIni_235JB, DateTime? fechaFin_235JB)
        {
            if (fechaIni_235JB.HasValue && fechaFin_235JB.HasValue && fechaIni_235JB.Value.Date > fechaFin_235JB.Value.Date)
                throw new InvalidOperationException("msg_RangoFechas");
            return _dal_235JB.ObtenerCambiosSegunFiltros_235JB(codigoProducto_235JB, nombre_235JB, fechaIni_235JB, fechaFin_235JB);
        }

        public List<int> ListarCodigos_235JB()
        {
            return _dal_235JB.ObtenerCodigos_235JB();
        }

        public List<string> ListarNombres_235JB()
        {
            return _dal_235JB.ObtenerNombres_235JB();
        }

        // Errores (clave de traduccion en Message):
        //   msg_CambioInexistente  el registro ya no existe
        //   msg_CambioYaActivado   ese registro ya es el activo del producto (flujo alternativo 5.1)
        //   msg_ProductoExistente  otro producto ya usa el nombre que tenia en ese registro
        public void ActivarCambio_235JB(int codigoCambio_235JB)
        {
            Producto_C cambio_235JB = _dal_235JB.ObtenerCambio_235JB(codigoCambio_235JB);
            if (cambio_235JB == null)
                throw new InvalidOperationException("msg_CambioInexistente");

            if (cambio_235JB.Act)
            {
                InvalidOperationException yaActivado_235JB = new InvalidOperationException("msg_CambioYaActivado");
                yaActivado_235JB.Data["CodigoCambio"] = codigoCambio_235JB;
                throw yaActivado_235JB;
            }

            _producto_235JB.VerificarNombreLibre_235JB(cambio_235JB.Nombre, cambio_235JB.CodigoProducto);

            // Act = 1: el trigger desactiva los demas registros del producto y actualiza Producto con estos datos
            Producto version_235JB = _dal_235JB.ActivarCambio_235JB(codigoCambio_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Producto", version_235JB.Codigo);
            _producto_235JB.SincronizarBitacora_235JB();
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(),
                "Activacion del cambio " + codigoCambio_235JB + " del producto " + version_235JB.Codigo, "Maestros", "Media");
        }
    }
}
