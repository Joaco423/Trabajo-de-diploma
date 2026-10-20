using System;
using System.Collections.Generic;

namespace BE
{
    public class Pedido
    {
        public int CodigoPedido { get; set; }
        public int DNI { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int Total { get; set; }
        /// <summary>"Confirmado" o "Cancelado".</summary>
        public string Estado { get; set; }
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
    }
}
