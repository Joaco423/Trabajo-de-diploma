using System;

namespace BE
{
    public class Reserva
    {
        public int CodigoReserva { get; set; }
        public int DNI { get; set; }
        public int NumeroMesa { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Estado { get; set; }
    }
}
