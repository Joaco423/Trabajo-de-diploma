using System;
using System.Collections.Generic;

namespace BE
{
    public class Mozo
    {
        public int DNIMozo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public int NumTelefono { get; set; }
       
        public string Estado { get; set; }
        public bool Activo { get; set; }
    }
}
