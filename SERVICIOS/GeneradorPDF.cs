using System;
using System.Collections.Generic;
using System.Data;
using BE;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace SERVICIOS
{
    
    public class GeneradorPDF
    {
        private const double Margen_235JB = 50;
        private const double AltoFila_235JB = 18;

        
        private PdfDocument _documento_235JB;
        private PdfPage _pagina_235JB;
        private XGraphics _gfx_235JB;
        private double _y_235JB;
        private int _numeroPagina_235JB;
        private string _pie_235JB;

        private XFont _fuenteTitulo_235JB;
        private XFont _fuenteSeccion_235JB;
        private XFont _fuenteNormal_235JB;
        private XFont _fuenteNegrita_235JB;
        private XFont _fuentePie_235JB;

        public void GenerarReporteAtencion_235JB(string ruta_235JB, Cliente cliente_235JB, Pedido pedido_235JB, ResumenAtencion resumen_235JB)
        {
            if (string.IsNullOrWhiteSpace(ruta_235JB))
                throw new ArgumentException("Falta la ruta del archivo.");
            if (cliente_235JB == null || pedido_235JB == null || resumen_235JB == null)
                throw new ArgumentException("No hay datos para generar el reporte.");

            IniciarDocumento_235JB("Reporte de atención - Pedido " + pedido_235JB.CodigoPedido,
                "CAFECCINO - Reporte de atención del pedido " + pedido_235JB.CodigoPedido);
            Encabezado_235JB("Reporte de atención");

            
            Seccion_235JB("Cliente");
            Campo_235JB("Nombre", cliente_235JB.Nombre + " " + cliente_235JB.Apellido);
            Campo_235JB("DNI", cliente_235JB.DNI.ToString());

            
            Seccion_235JB("Pedido");
            Campo_235JB("Código", pedido_235JB.CodigoPedido.ToString());
            Campo_235JB("Fecha", pedido_235JB.Fecha.ToString("dd/MM/yyyy"));
            Campo_235JB("Hora", pedido_235JB.Hora);
            Campo_235JB("Estado", pedido_235JB.Estado);

            
            Seccion_235JB("Detalle del pedido");
            double[] columnas_235JB = { Margen_235JB, Margen_235JB + 60, Margen_235JB + 290, Margen_235JB + 360, Margen_235JB + 430 };
            string[] titulos_235JB = { "Código", "Producto", "Precio", "Cantidad", "Subtotal" };
            EncabezadoTabla_235JB(columnas_235JB, titulos_235JB);

            int total_235JB = 0;
            List<DetallePedido> detalle_235JB = pedido_235JB.Detalles ?? new List<DetallePedido>();
            foreach (DetallePedido d_235JB in detalle_235JB)
            {
                if (_y_235JB > _pagina_235JB.Height.Point - 90)
                {
                    NuevaPagina_235JB();
                    EncabezadoTabla_235JB(columnas_235JB, titulos_235JB);
                }

                int subtotal_235JB = d_235JB.Subtotal > 0 ? d_235JB.Subtotal : d_235JB.Precio * d_235JB.Cantidad;
                total_235JB += subtotal_235JB;
                string nombre_235JB = d_235JB.Nombre ?? string.Empty;
                if (nombre_235JB.Length > 34) nombre_235JB = nombre_235JB.Substring(0, 31) + "...";

                _gfx_235JB.DrawString(d_235JB.Codigo.ToString(), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[0], _y_235JB + 12);
                _gfx_235JB.DrawString(nombre_235JB, _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[1], _y_235JB + 12);
                _gfx_235JB.DrawString("$" + d_235JB.Precio, _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[2], _y_235JB + 12);
                _gfx_235JB.DrawString(d_235JB.Cantidad.ToString(), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[3], _y_235JB + 12);
                _gfx_235JB.DrawString("$" + subtotal_235JB, _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[4], _y_235JB + 12);
                _y_235JB += AltoFila_235JB;
            }

            _y_235JB += 4;
            Linea_235JB();
            _y_235JB += 4;
            string textoTotal_235JB = "Total: $" + (pedido_235JB.Total > 0 ? pedido_235JB.Total : total_235JB);
            XRect zonaTotal_235JB = new XRect(Margen_235JB, _y_235JB, _pagina_235JB.Width.Point - 2 * Margen_235JB, AltoFila_235JB);
            _gfx_235JB.DrawString(textoTotal_235JB, _fuenteSeccion_235JB, XBrushes.Black, zonaTotal_235JB, XStringFormats.TopRight);
            _y_235JB += 30;

            
            if (_y_235JB > _pagina_235JB.Height.Point - 140)
                NuevaPagina_235JB();
            Seccion_235JB("Resumen de atención");
            Campo_235JB("Mesa", resumen_235JB.NumeroMesa + " (capacidad " + resumen_235JB.Capacidad + ")");
            Campo_235JB("Mozo", resumen_235JB.NombreMozo + " " + resumen_235JB.ApellidoMozo);

            GuardarDocumento_235JB(ruta_235JB);
        }

   
        public void GenerarReporteReservasActivas_235JB(string ruta_235JB, DataTable reservas_235JB)
        {
            GenerarReporteReservas_235JB(ruta_235JB, reservas_235JB, "Reporte de reservas activas", "Reservas activas: " + (reservas_235JB != null ? reservas_235JB.Rows.Count : 0));
        }

        
        public void GenerarReporteReserva_235JB(string ruta_235JB, DataTable reserva_235JB)
        {
            GenerarReporteReservas_235JB(ruta_235JB, reserva_235JB, "Reporte de reserva", "Reserva seleccionada");
        }

        private void GenerarReporteReservas_235JB(string ruta_235JB, DataTable reservas_235JB, string titulo_235JB, string seccion_235JB)
        {
            if (string.IsNullOrWhiteSpace(ruta_235JB))
                throw new ArgumentException("Falta la ruta del archivo.");
            if (reservas_235JB == null)
                throw new ArgumentException("No hay datos para generar el reporte.");

            IniciarDocumento_235JB(titulo_235JB, "CAFECCINO - " + titulo_235JB);
            Encabezado_235JB(titulo_235JB);

            Seccion_235JB(seccion_235JB);
            double[] columnas_235JB = { Margen_235JB, Margen_235JB + 55, Margen_235JB + 120, Margen_235JB + 280, Margen_235JB + 335, Margen_235JB + 425 };
            string[] titulos_235JB = { "Reserva", "DNI", "Cliente", "Mesa", "Fecha", "Hora" };
            EncabezadoTabla_235JB(columnas_235JB, titulos_235JB);

            if (reservas_235JB.Rows.Count == 0)
                _gfx_235JB.DrawString("No hay reservas activas.", _fuenteNormal_235JB, XBrushes.Black, Margen_235JB, _y_235JB + 12);

            foreach (DataRow fila_235JB in reservas_235JB.Rows)
            {
                if (_y_235JB > _pagina_235JB.Height.Point - 70)
                {
                    NuevaPagina_235JB();
                    EncabezadoTabla_235JB(columnas_235JB, titulos_235JB);
                }

                string cliente_235JB = fila_235JB["Nombre"] + " " + fila_235JB["Apellido"];
                if (cliente_235JB.Length > 26) cliente_235JB = cliente_235JB.Substring(0, 23) + "...";
                DateTime fecha_235JB = Convert.ToDateTime(fila_235JB["Fecha"]);

                _gfx_235JB.DrawString(fila_235JB["Reserva"].ToString(), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[0], _y_235JB + 12);
                _gfx_235JB.DrawString(fila_235JB["DNI"].ToString(), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[1], _y_235JB + 12);
                _gfx_235JB.DrawString(cliente_235JB, _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[2], _y_235JB + 12);
                _gfx_235JB.DrawString(fila_235JB["Mesa"].ToString(), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[3], _y_235JB + 12);
                _gfx_235JB.DrawString(fecha_235JB.ToString("dd/MM/yyyy"), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[4], _y_235JB + 12);
                _gfx_235JB.DrawString(fila_235JB["Hora"].ToString(), _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[5], _y_235JB + 12);
                _y_235JB += AltoFila_235JB;
            }

            GuardarDocumento_235JB(ruta_235JB);
        }

        // Reporte inteligente: titulo, descripcion, conclusion destacada y la tabla con todas las columnas del resultado
        // Si todas las columnas entran, el espacio sobrante se reparte en proporcion; si no, las columnas angostas conservan
        // su ancho y solo las mas anchas comparten (y recortan) lo que queda.
        private static double[] RepartirAnchos_235JB(double[] necesario_235JB, double disponible_235JB)
        {
            int n_235JB = necesario_235JB.Length;
            double[] anchos_235JB = new double[n_235JB];
            double total_235JB = 0;
            foreach (double v_235JB in necesario_235JB) total_235JB += v_235JB;
            if (total_235JB <= disponible_235JB)
            {
                for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
                    anchos_235JB[c_235JB] = necesario_235JB[c_235JB] * disponible_235JB / total_235JB;
                return anchos_235JB;
            }
            bool[] fija_235JB = new bool[n_235JB];
            double resto_235JB = disponible_235JB;
            int libres_235JB = n_235JB;
            bool cambio_235JB = true;
            while (cambio_235JB && libres_235JB > 0)
            {
                cambio_235JB = false;
                double parte_235JB = resto_235JB / libres_235JB;
                for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
                    if (!fija_235JB[c_235JB] && necesario_235JB[c_235JB] <= parte_235JB)
                    {
                        fija_235JB[c_235JB] = true;
                        anchos_235JB[c_235JB] = necesario_235JB[c_235JB];
                        resto_235JB -= necesario_235JB[c_235JB];
                        libres_235JB--;
                        cambio_235JB = true;
                    }
            }
            for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
                if (!fija_235JB[c_235JB])
                    anchos_235JB[c_235JB] = resto_235JB / libres_235JB;
            return anchos_235JB;
        }

        public void GenerarReporteInteligente_235JB(string ruta_235JB, string titulo_235JB, string descripcion_235JB, string conclusion_235JB, DataTable datos_235JB)
        {
            if (string.IsNullOrWhiteSpace(ruta_235JB))
                throw new ArgumentException("Falta la ruta del archivo.");
            if (datos_235JB == null)
                throw new ArgumentException("No hay datos para generar el reporte.");

            IniciarDocumento_235JB("Reporte inteligente - " + titulo_235JB, "CAFECCINO - Reporte inteligente: " + titulo_235JB);
            Encabezado_235JB("Reporte inteligente: " + titulo_235JB);

            double ancho_235JB = _pagina_235JB.Width.Point - 2 * Margen_235JB;
            PdfSharp.Drawing.Layout.XTextFormatter formato_235JB = new PdfSharp.Drawing.Layout.XTextFormatter(_gfx_235JB);
            _y_235JB += 10;
            formato_235JB.DrawString(descripcion_235JB ?? string.Empty, _fuenteNormal_235JB, XBrushes.Black, new XRect(Margen_235JB, _y_235JB, ancho_235JB, 30));
            _y_235JB += 30;

            // Conclusion en un recuadro
            Seccion_235JB("Conclusión");
            XRect recuadro_235JB = new XRect(Margen_235JB, _y_235JB, ancho_235JB, 42);
            _gfx_235JB.DrawRectangle(new XPen(XColor.FromArgb(210, 105, 30), 1), new XSolidBrush(XColor.FromArgb(251, 243, 236)), recuadro_235JB);
            formato_235JB.DrawString(conclusion_235JB ?? string.Empty, _fuenteNegrita_235JB, XBrushes.Black,
                new XRect(Margen_235JB + 8, _y_235JB + 7, ancho_235JB - 16, 32));
            _y_235JB += 50;

            Seccion_235JB("Detalle");
            int n_235JB = datos_235JB.Columns.Count;
            if (n_235JB == 0)
            {
                GuardarDocumento_235JB(ruta_235JB);
                return;
            }

            // Ancho que necesita cada columna (texto mas ancho medido, encabezado incluido)
            double[] necesario_235JB = new double[n_235JB];
            for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
            {
                double medida_235JB = _gfx_235JB.MeasureString(datos_235JB.Columns[c_235JB].ColumnName, _fuenteNegrita_235JB).Width;
                foreach (DataRow f_235JB in datos_235JB.Rows)
                    medida_235JB = Math.Max(medida_235JB, _gfx_235JB.MeasureString(Convert.ToString(f_235JB[c_235JB]), _fuenteNormal_235JB).Width);
                necesario_235JB[c_235JB] = medida_235JB + 10;
            }
            double[] anchos_235JB = RepartirAnchos_235JB(necesario_235JB, ancho_235JB);
            double[] columnas_235JB = new double[n_235JB];
            double x_235JB = Margen_235JB;
            for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
            {
                columnas_235JB[c_235JB] = x_235JB;
                x_235JB += anchos_235JB[c_235JB];
            }
            string[] titulos_235JB = new string[n_235JB];
            for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
                titulos_235JB[c_235JB] = Recortar_235JB(datos_235JB.Columns[c_235JB].ColumnName, anchos_235JB[c_235JB], _fuenteNegrita_235JB);
            EncabezadoTabla_235JB(columnas_235JB, titulos_235JB);

            if (datos_235JB.Rows.Count == 0)
                _gfx_235JB.DrawString("Sin datos.", _fuenteNormal_235JB, XBrushes.Black, Margen_235JB, _y_235JB + 12);

            foreach (DataRow f_235JB in datos_235JB.Rows)
            {
                if (_y_235JB > _pagina_235JB.Height.Point - 70)
                {
                    NuevaPagina_235JB();
                    EncabezadoTabla_235JB(columnas_235JB, titulos_235JB);
                }
                for (int c_235JB = 0; c_235JB < n_235JB; c_235JB++)
                    _gfx_235JB.DrawString(Recortar_235JB(Convert.ToString(f_235JB[c_235JB]), anchos_235JB[c_235JB], _fuenteNormal_235JB),
                        _fuenteNormal_235JB, XBrushes.Black, columnas_235JB[c_235JB], _y_235JB + 12);
                _y_235JB += AltoFila_235JB;
            }

            GuardarDocumento_235JB(ruta_235JB);
        }

        private string Recortar_235JB(string texto_235JB, double ancho_235JB, XFont fuente_235JB)
        {
            texto_235JB = texto_235JB ?? string.Empty;
            if (_gfx_235JB.MeasureString(texto_235JB, fuente_235JB).Width <= ancho_235JB - 6)
                return texto_235JB;
            while (texto_235JB.Length > 1 && _gfx_235JB.MeasureString(texto_235JB + "...", fuente_235JB).Width > ancho_235JB - 6)
                texto_235JB = texto_235JB.Substring(0, texto_235JB.Length - 1);
            return texto_235JB + "...";
        }

        private void IniciarDocumento_235JB(string titulo_235JB, string pie_235JB)
        {
            _pie_235JB = pie_235JB;
            _numeroPagina_235JB = 0;
            _fuenteTitulo_235JB = new XFont("Arial", 20, XFontStyle.Bold);
            _fuenteSeccion_235JB = new XFont("Arial", 12, XFontStyle.Bold);
            _fuenteNormal_235JB = new XFont("Arial", 10, XFontStyle.Regular);
            _fuenteNegrita_235JB = new XFont("Arial", 10, XFontStyle.Bold);
            _fuentePie_235JB = new XFont("Arial", 8, XFontStyle.Regular);

            _documento_235JB = new PdfDocument();
            _documento_235JB.Info.Title = titulo_235JB;
            _documento_235JB.Info.Creator = "CAFECCINO";
            _gfx_235JB = null;
            NuevaPagina_235JB();
        }

        private void Encabezado_235JB(string subtitulo_235JB)
        {
            Texto_235JB("CAFECCINO", _fuenteTitulo_235JB, Margen_235JB);
            _y_235JB += 28;
            Texto_235JB(subtitulo_235JB, _fuenteSeccion_235JB, Margen_235JB);
            TextoDerecha_235JB("Generado: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), _fuenteNormal_235JB);
            _y_235JB += 22;
            Linea_235JB();
        }

        private void GuardarDocumento_235JB(string ruta_235JB)
        {
            _gfx_235JB.Dispose();
            _documento_235JB.Save(ruta_235JB);
            _documento_235JB.Close();
        }

        private void NuevaPagina_235JB()
        {
            if (_gfx_235JB != null)
                _gfx_235JB.Dispose();

            _pagina_235JB = _documento_235JB.AddPage();
            _pagina_235JB.Size = PageSize.A4;
            _gfx_235JB = XGraphics.FromPdfPage(_pagina_235JB);
            _numeroPagina_235JB++;
            _y_235JB = Margen_235JB;

            XRect pie_235JB = new XRect(Margen_235JB, _pagina_235JB.Height.Point - 35, _pagina_235JB.Width.Point - 2 * Margen_235JB, 14);
            _gfx_235JB.DrawString(_pie_235JB + " - Página " + _numeroPagina_235JB,
                _fuentePie_235JB, XBrushes.Gray, pie_235JB, XStringFormats.TopCenter);
        }

        private void Texto_235JB(string texto_235JB, XFont fuente_235JB, double x_235JB)
        {
            _gfx_235JB.DrawString(texto_235JB, fuente_235JB, XBrushes.Black, x_235JB, _y_235JB + fuente_235JB.Size);
        }

        private void TextoDerecha_235JB(string texto_235JB, XFont fuente_235JB)
        {
            XRect zona_235JB = new XRect(Margen_235JB, _y_235JB, _pagina_235JB.Width.Point - 2 * Margen_235JB, fuente_235JB.Height);
            _gfx_235JB.DrawString(texto_235JB, fuente_235JB, XBrushes.Black, zona_235JB, XStringFormats.TopRight);
        }

        private void Linea_235JB()
        {
            _gfx_235JB.DrawLine(XPens.Black, Margen_235JB, _y_235JB, _pagina_235JB.Width.Point - Margen_235JB, _y_235JB);
        }

        private void Seccion_235JB(string titulo_235JB)
        {
            _y_235JB += 20;
            Texto_235JB(titulo_235JB, _fuenteSeccion_235JB, Margen_235JB);
            _y_235JB += 22;
        }

        private void Campo_235JB(string etiqueta_235JB, string valor_235JB)
        {
            _gfx_235JB.DrawString(etiqueta_235JB + ":", _fuenteNegrita_235JB, XBrushes.Black, Margen_235JB, _y_235JB + 12);
            _gfx_235JB.DrawString(valor_235JB ?? string.Empty, _fuenteNormal_235JB, XBrushes.Black, Margen_235JB + 80, _y_235JB + 12);
            _y_235JB += AltoFila_235JB;
        }

        private void EncabezadoTabla_235JB(double[] columnas_235JB, string[] titulos_235JB)
        {
            for (int i_235JB = 0; i_235JB < titulos_235JB.Length; i_235JB++)
                _gfx_235JB.DrawString(titulos_235JB[i_235JB], _fuenteNegrita_235JB, XBrushes.Black, columnas_235JB[i_235JB], _y_235JB + 12);
            _y_235JB += AltoFila_235JB;
            Linea_235JB();
            _y_235JB += 2;
        }
    }
}
