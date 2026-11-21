using System;

namespace BE
{
    // Un registro de la bitacora de cambios de producto (tabla Producto_C). Lo crea el trigger TR_Producto_RegistrarCambio
    // cada vez que cambian los datos del producto; Act = true marca la version activa (una sola por producto).
    public class Producto_C
    {
        public int CodigoCambio { get; set; }
        public int CodigoProducto { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public bool Disponible { get; set; }
        public int Stock { get; set; }
        public bool Act { get; set; }
    }
}
