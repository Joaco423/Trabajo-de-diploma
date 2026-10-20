using System;
using System.Collections.Generic;

namespace BE
{
    public class Producto
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Precio { get; set; }
        public bool Disponible { get; set; }
        public int Stock { get; set; }
    }
}
