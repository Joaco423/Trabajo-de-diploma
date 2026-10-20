using System;
using System.Collections.Generic;

namespace BE
{
    public class DetallePedido
    {
        public int CodigoPedido { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public int Cantidad { get; set; }
        public int Subtotal { get; set; }
    }
}
