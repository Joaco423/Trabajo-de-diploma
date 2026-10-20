using System;
using System.Collections.Generic;

namespace BE
{
    public class Cliente
    {
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public int NumTelefono { get; set; }
        public string Domicilio { get; set; }
        public bool Activo { get; set; }
    }
}
