using BE;
using System;

namespace BLL
{
    
    public class EntidadEnUsoException : InvalidOperationException
    {
        public EntidadEnUsoException(string mensaje_235JB) : base(mensaje_235JB) { }
    }

    // Ya hay un producto con ese nombre: para cambiar su stock se lo modifica, no se agrega otro.
    public class ProductoExistenteException : InvalidOperationException
    {
        public int CodigoExistente { get; private set; }

        public ProductoExistenteException(string nombre_235JB, int codigoExistente_235JB)
            : base("Ya existe el producto " + nombre_235JB + " (codigo " + codigoExistente_235JB + ").")
        {
            CodigoExistente = codigoExistente_235JB;
        }
    }

    
    public class ProductoNoDisponibleException : Exception
    {
        public Producto Producto { get; private set; }

        public ProductoNoDisponibleException(Producto producto_235JB)
            : base("El producto " + producto_235JB.Nombre + " no esta disponible.")
        {
            Producto = producto_235JB;
        }
    }

    
    public class StockInsuficienteException : Exception
    {
        public Producto Producto { get; private set; }
        public int CantidadPedida { get; private set; }

        public StockInsuficienteException(Producto producto_235JB, int cantidadPedida_235JB)
            : base("No hay stock suficiente de " + producto_235JB.Nombre + ". Stock: " + producto_235JB.Stock + ", pedido: " + cantidadPedida_235JB + ".")
        {
            Producto = producto_235JB;
            CantidadPedida = cantidadPedida_235JB;
        }
    }

     
    public class PedidoEnCursoException : Exception
    {
        public int CodigoPedido { get; private set; }

        public PedidoEnCursoException(int codigoPedido_235JB)
            : base("El cliente ya tiene un pedido en curso (pedido " + codigoPedido_235JB + ").")
        {
            CodigoPedido = codigoPedido_235JB;
        }
    }


    public class SistemaNoDisponibleException : Exception
    {
        public SistemaNoDisponibleException()
            : base("Sistema caido. Contacte al administrador.")
        {
        }
    }
}
