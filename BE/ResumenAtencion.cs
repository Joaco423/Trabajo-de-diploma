using System;
using System.Collections.Generic;

namespace BE
{
    public class ResumenAtencion
    {
        public int CodigoPedido { get; set; }
        public int NumeroFactura { get; set; }
        public int NumeroMesa { get; set; }
        public int Capacidad { get; set; }
        public string NombreMozo { get; set; }
        public string ApellidoMozo { get; set; }
    }
}
