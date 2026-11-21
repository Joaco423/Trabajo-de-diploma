using System.Data;

namespace BE
{
    // Resultado de un reporte inteligente: los datos analizados y la conclusion que se saca de ellos.
    public class ReporteInteligente
    {
        public int Tipo { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Conclusion { get; set; }
        public DataTable Datos { get; set; }
    }
}
