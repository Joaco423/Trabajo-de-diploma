using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;

namespace BLL
{
    
    public class BLLReporte
    {
        public const int ProductosMasOrdenados_235JB = 1;
        public const int ClienteQueMasPidioCadaProducto_235JB = 2;
        public const int ClientesQueMasGastaron_235JB = 3;
        public const int MozosConMasPedidos_235JB = 4;
        public const int HorariosConMasPedidos_235JB = 5;
        public const int ProductosMenosVendidos_235JB = 6;
        public const int VentasPorDiaSemana_235JB = 7;
        public const int MesasQueMasFacturaron_235JB = 8;
        public const int MesasMasReservadas_235JB = 9;
        public const int ClientesQueMasReservaron_235JB = 10;
        public const int HorariosConMasReservas_235JB = 11;

        private readonly DALReporte _dal_235JB = new DALReporte();
        private readonly BLLEvento _evento_235JB = new BLLEvento();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }


        public Dictionary<int, string> ListarTipos_235JB()
        {
            return new Dictionary<int, string>
            {
                { ProductosMasOrdenados_235JB, "Productos más ordenados" },
                { ClienteQueMasPidioCadaProducto_235JB, "Cliente que más pidió cada producto" },
                { ClientesQueMasGastaron_235JB, "Clientes que más gastaron" },
                { MozosConMasPedidos_235JB, "Mozos con más pedidos atendidos" },
                { HorariosConMasPedidos_235JB, "Horarios con más pedidos" },
                { ProductosMenosVendidos_235JB, "Productos menos vendidos" },
                { VentasPorDiaSemana_235JB, "Ventas por día de la semana" },
                { MesasQueMasFacturaron_235JB, "Mesas que más facturaron" },
                { MesasMasReservadas_235JB, "Mesas más reservadas" },
                { ClientesQueMasReservaron_235JB, "Clientes que más reservaron" },
                { HorariosConMasReservas_235JB, "Horarios con más reservas" }
            };
        }

        public ReporteInteligente ObtenerReporte_235JB(int tipo_235JB)
        {
            ReporteInteligente r_235JB = new ReporteInteligente { Tipo = tipo_235JB };
            Dictionary<int, string> tipos_235JB = ListarTipos_235JB();
            if (!tipos_235JB.ContainsKey(tipo_235JB))
                throw new InvalidOperationException("msg_TipoReporteInvalido");
            r_235JB.Titulo = tipos_235JB[tipo_235JB];

            switch (tipo_235JB)
            {
                case ProductosMasOrdenados_235JB:
                    r_235JB.Descripcion = "Ranking de productos según las unidades vendidas (sin pedidos cancelados).";
                    r_235JB.Datos = _dal_235JB.ProductosMasOrdenados_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "El producto más ordenado es " + f_235JB["Producto"] + ", con " + f_235JB["Unidades"] + " unidades en " +
                        f_235JB["Pedidos"] + " pedidos ($" + f_235JB["Facturado ($)"] + " facturados).");
                    break;
                case ClienteQueMasPidioCadaProducto_235JB:
                    r_235JB.Descripcion = "Para cada producto, el cliente que más unidades pidió y qué porcentaje de las ventas del producto representa.";
                    r_235JB.Datos = _dal_235JB.ClienteQueMasPidioCadaProducto_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "El cliente que más pidió " + f_235JB["Producto"] + " (el producto más vendido) es " + f_235JB["Cliente"] +
                        ", con " + f_235JB["Unidades"] + " unidades: el " + f_235JB["% del producto"] + "% de lo vendido de ese producto.");
                    break;
                case ClientesQueMasGastaron_235JB:
                    r_235JB.Descripcion = "Los 10 clientes con mayor gasto total, con su cantidad de pedidos y ticket promedio.";
                    r_235JB.Datos = _dal_235JB.ClientesQueMasGastaron_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "El cliente que más gastó es " + f_235JB["Cliente"] + ", con $" + f_235JB["Total gastado ($)"] + " en " +
                        f_235JB["Pedidos"] + " pedidos (ticket promedio $" + f_235JB["Ticket promedio ($)"] + ").");
                    break;
                case MozosConMasPedidos_235JB:
                    r_235JB.Descripcion = "Mozos ordenados por cantidad de pedidos atendidos, con lo facturado en sus mesas.";
                    r_235JB.Datos = _dal_235JB.MozosConMasPedidos_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "El mozo con más pedidos atendidos es " + f_235JB["Mozo"] + ", con " + f_235JB["Pedidos atendidos"] +
                        " pedidos y $" + f_235JB["Total facturado ($)"] + " facturados.");
                    break;
                case HorariosConMasPedidos_235JB:
                    r_235JB.Descripcion = "Cantidad de pedidos por franja horaria, para saber cuándo hay más movimiento.";
                    r_235JB.Datos = _dal_235JB.HorariosConMasPedidos_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "La franja con más pedidos es de " + f_235JB["Franja horaria"] + ", con " + f_235JB["Pedidos"] +
                        " pedidos ($" + f_235JB["Total facturado ($)"] + " facturados).");
                    break;
                case ProductosMenosVendidos_235JB:
                    r_235JB.Descripcion = "Productos vigentes ordenados de menos a más vendidos, incluidos los que nunca se vendieron (sin pedidos cancelados).";
                    r_235JB.Datos = _dal_235JB.ProductosMenosVendidos_235JB();
                    r_235JB.Conclusion = r_235JB.Datos.Rows.Count == 0 ? "No hay productos cargados."
                        : "El producto menos vendido es " + FraseFila_235JB(tipo_235JB, r_235JB.Datos.Rows[0]);
                    break;
                case VentasPorDiaSemana_235JB:
                    r_235JB.Descripcion = "Pedidos y facturación según el día de la semana, para saber qué días hay más movimiento.";
                    r_235JB.Datos = _dal_235JB.VentasPorDiaSemana_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "El día con más pedidos es el " + f_235JB["Día"].ToString().ToLower() + ", con " + f_235JB["Pedidos"] + " pedidos y $" +
                        f_235JB["Total facturado ($)"] + " facturados (promedio $" + f_235JB["Promedio por pedido ($)"] + " por pedido).");
                    break;
                case MesasQueMasFacturaron_235JB:
                    r_235JB.Descripcion = "Mesas ordenadas por lo facturado en los pedidos atendidos en ellas.";
                    r_235JB.Datos = _dal_235JB.MesasQueMasFacturaron_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "La mesa que más facturó es la " + f_235JB["Mesa"] + " (" + f_235JB["Capacidad"] + " lugares), con $" +
                        f_235JB["Total facturado ($)"] + " en " + f_235JB["Pedidos"] + " pedidos.");
                    break;
                case MesasMasReservadas_235JB:
                    r_235JB.Descripcion = "Mesas ordenadas por cantidad de reservas; las canceladas se muestran aparte y no suman al ranking.";
                    r_235JB.Datos = _dal_235JB.MesasMasReservadas_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "La mesa más reservada es la " + f_235JB["Mesa"] + " (" + f_235JB["Capacidad"] + " lugares), con " + f_235JB["Reservas"] +
                        " reservas, de las cuales " + f_235JB["Cumplidas"] + " se cumplieron.", "Todavía no hay mesas cargadas.");
                    break;
                case ClientesQueMasReservaron_235JB:
                    r_235JB.Descripcion = "Los 10 clientes con más reservas (sin contar las canceladas), con la mesa que más eligieron.";
                    r_235JB.Datos = _dal_235JB.ClientesQueMasReservaron_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "El cliente que más reservó es " + f_235JB["Cliente"] + ", con " + f_235JB["Reservas"] + " reservas (" +
                        f_235JB["Cumplidas"] + " cumplidas); su mesa preferida es la " + f_235JB["Mesa preferida"] + ".",
                        "Todavía no hay reservas para analizar.");
                    break;
                case HorariosConMasReservas_235JB:
                    r_235JB.Descripcion = "Cantidad de reservas por franja horaria (sin contar las canceladas).";
                    r_235JB.Datos = _dal_235JB.HorariosConMasReservas_235JB();
                    r_235JB.Conclusion = Primera_235JB(r_235JB.Datos, f_235JB =>
                        "La franja con más reservas es de " + f_235JB["Franja horaria"] + ", con " + f_235JB["Reservas"] + " reservas (" +
                        f_235JB["Cumplidas"] + " cumplidas).", "Todavía no hay reservas para analizar.");
                    break;
            }
            return r_235JB;
        }

        private static string Primera_235JB(DataTable datos_235JB, Func<DataRow, string> frase_235JB,
            string sinDatos_235JB = "Todavía no hay pedidos para analizar.")
        {
            return datos_235JB.Rows.Count == 0 ? sinDatos_235JB : frase_235JB(datos_235JB.Rows[0]);
        }

        
        public ReporteInteligente FiltrarSeleccion_235JB(ReporteInteligente reporte_235JB, IList<int> filas_235JB)
        {
            if (reporte_235JB == null) throw new ArgumentNullException("reporte");
            if (filas_235JB == null || filas_235JB.Count == 0 || filas_235JB.Count == reporte_235JB.Datos.Rows.Count)
                return reporte_235JB;

            List<int> orden_235JB = new List<int>(filas_235JB);
            orden_235JB.Sort();
            DataTable datos_235JB = reporte_235JB.Datos.Clone();
            foreach (int i_235JB in orden_235JB)
                datos_235JB.ImportRow(reporte_235JB.Datos.Rows[i_235JB]);

            string frase_235JB = FraseFila_235JB(reporte_235JB.Tipo, datos_235JB.Rows[0]);
            return new ReporteInteligente
            {
                Tipo = reporte_235JB.Tipo,
                Titulo = reporte_235JB.Titulo,
                Descripcion = reporte_235JB.Descripcion + " Incluye solo las " + datos_235JB.Rows.Count + " filas seleccionadas de " +
                              reporte_235JB.Datos.Rows.Count + ".",
                Conclusion = datos_235JB.Rows.Count == 1 ? frase_235JB
                             : "Se seleccionaron " + datos_235JB.Rows.Count + " filas. La primera según el reporte: " + frase_235JB,
                Datos = datos_235JB
            };
        }

        
        private static string FraseFila_235JB(int tipo_235JB, DataRow f_235JB)
        {
            switch (tipo_235JB)
            {
                case ProductosMasOrdenados_235JB:
                    return f_235JB["Producto"] + " (puesto " + f_235JB["Puesto"] + "): " + f_235JB["Unidades"] + " unidades en " +
                           f_235JB["Pedidos"] + " pedidos ($" + f_235JB["Facturado ($)"] + " facturados).";
                case ClienteQueMasPidioCadaProducto_235JB:
                    return "El cliente que más pidió " + f_235JB["Producto"] + " es " + f_235JB["Cliente"] + ", con " + f_235JB["Unidades"] +
                           " unidades: el " + f_235JB["% del producto"] + "% de lo vendido de ese producto.";
                case ClientesQueMasGastaron_235JB:
                    return f_235JB["Cliente"] + " (puesto " + f_235JB["Puesto"] + ") gastó $" + f_235JB["Total gastado ($)"] + " en " +
                           f_235JB["Pedidos"] + " pedidos (ticket promedio $" + f_235JB["Ticket promedio ($)"] + ").";
                case MozosConMasPedidos_235JB:
                    return f_235JB["Mozo"] + " (puesto " + f_235JB["Puesto"] + ") atendió " + f_235JB["Pedidos atendidos"] + " pedidos y facturó $" +
                           f_235JB["Total facturado ($)"] + ".";
                case HorariosConMasPedidos_235JB:
                    return "De " + f_235JB["Franja horaria"] + " (puesto " + f_235JB["Puesto"] + ") hubo " + f_235JB["Pedidos"] + " pedidos ($" +
                           f_235JB["Total facturado ($)"] + " facturados).";
                case ProductosMenosVendidos_235JB:
                    return Convert.ToInt32(f_235JB["Unidades"]) == 0
                        ? f_235JB["Producto"] + " (puesto " + f_235JB["Puesto"] + "): no se vendió nunca" +
                          (f_235JB["Disponible"].ToString() == "No" ? " (está marcado como no disponible)." : ".")
                        : f_235JB["Producto"] + " (puesto " + f_235JB["Puesto"] + "): " + f_235JB["Unidades"] + " unidades en " +
                          f_235JB["Pedidos"] + " pedidos ($" + f_235JB["Facturado ($)"] + " facturados).";
                case VentasPorDiaSemana_235JB:
                    return "El " + f_235JB["Día"].ToString().ToLower() + " (puesto " + f_235JB["Puesto"] + ") hubo " + f_235JB["Pedidos"] +
                           " pedidos y $" + f_235JB["Total facturado ($)"] + " facturados.";
                case MesasQueMasFacturaron_235JB:
                    return "La mesa " + f_235JB["Mesa"] + " (puesto " + f_235JB["Puesto"] + ") facturó $" + f_235JB["Total facturado ($)"] +
                           " en " + f_235JB["Pedidos"] + " pedidos.";
                case MesasMasReservadas_235JB:
                    return "La mesa " + f_235JB["Mesa"] + " (puesto " + f_235JB["Puesto"] + ") tuvo " + f_235JB["Reservas"] + " reservas (" +
                           f_235JB["Cumplidas"] + " cumplidas; " + f_235JB["Canceladas"] + " canceladas aparte).";
                case ClientesQueMasReservaron_235JB:
                    return f_235JB["Cliente"] + " (puesto " + f_235JB["Puesto"] + ") hizo " + f_235JB["Reservas"] + " reservas (" +
                           f_235JB["Cumplidas"] + " cumplidas); mesa preferida: " + f_235JB["Mesa preferida"] + ".";
                case HorariosConMasReservas_235JB:
                    return "De " + f_235JB["Franja horaria"] + " (puesto " + f_235JB["Puesto"] + ") hubo " + f_235JB["Reservas"] + " reservas (" +
                           f_235JB["Cumplidas"] + " cumplidas).";
                default:
                    return string.Empty;
            }
        }

        public void GenerarPDF_235JB(ReporteInteligente reporte_235JB, string ruta_235JB)
        {
            if (reporte_235JB == null) throw new ArgumentNullException("reporte");
            new GeneradorPDF().GenerarReporteInteligente_235JB(ruta_235JB, reporte_235JB.Titulo, reporte_235JB.Descripcion,
                reporte_235JB.Conclusion, reporte_235JB.Datos);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Reporte inteligente: " + reporte_235JB.Titulo +
                " (" + reporte_235JB.Datos.Rows.Count + " filas)", "Reportes", "Baja");
        }
    }
}
