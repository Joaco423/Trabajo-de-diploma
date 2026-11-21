using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BLL
{
    
    public class BLLIntegridad
    {
        private readonly DALIntegridad _dal = new DALIntegridad();
        private readonly DALDigitoVerificador _dvDAL = new DALDigitoVerificador();

        public IReadOnlyList<string> ObtenerTablasControladas() => _dal.ObtenerTablasControladas();

        public void SincronizarTabla(string tabla)
        {
            string[] pk = _dal.ObtenerClavePrimaria(tabla);
            DataTable filas = _dal.ObtenerFilas(tabla);

            List<int> dvhs = new List<int>();

            foreach (DataRow fila in filas.Rows)
            {
                int dvh = DigitVerifierService.CalcularDVHGenerico(fila, filas.Columns);
                dvhs.Add(dvh);
                _dal.ActualizarDVH(tabla, pk, fila, dvh);
            }

            int dvv = DigitVerifierService.CalcularDVV(dvhs);
            _dvDAL.Guardar(new DigitoVerificador { Tabla = tabla, DVV = dvv.ToString() });
        }

        public void SincronizarTodasLasTablas()
        {
            foreach (string tabla in ObtenerTablasControladas())
                SincronizarTabla(tabla);
        }

       
        public void SincronizarFilasNuevas(string tabla)
        {
            string[] pk = _dal.ObtenerClavePrimaria(tabla);
            DataTable filas = _dal.ObtenerFilas(tabla);

            foreach (DataRow fila in filas.Rows)
            {
                object valorGuardado = filas.Columns.Contains("DVH") ? fila["DVH"] : null;
                bool esFilaNueva = valorGuardado == null || valorGuardado is DBNull;

                if (esFilaNueva)
                {
                    int dvh = DigitVerifierService.CalcularDVHGenerico(fila, filas.Columns);
                    _dal.ActualizarDVH(tabla, pk, fila, dvh);
                }
            }

            ActualizarDVVConEstadoActual(tabla);
        }

        
        public void SincronizarFilaPorClave(string tabla, params object[] valoresClavePrimaria)
        {
            string[] pk = _dal.ObtenerClavePrimaria(tabla);
            DataTable filas = _dal.ObtenerFilas(tabla);

            foreach (DataRow fila in filas.Rows)
            {
                bool esLaFila = true;
                for (int i = 0; i < pk.Length; i++)
                {
                    if (fila[pk[i]].ToString() != valoresClavePrimaria[i].ToString())
                    {
                        esLaFila = false;
                        break;
                    }
                }

                if (esLaFila)
                {
                    int dvh = DigitVerifierService.CalcularDVHGenerico(fila, filas.Columns);
                    _dal.ActualizarDVH(tabla, pk, fila, dvh);
                    break;
                }
            }

            ActualizarDVVConEstadoActual(tabla);
        }

        private void ActualizarDVVConEstadoActual(string tabla)
        {
            DataTable filas = _dal.ObtenerFilas(tabla);

            var digestos = filas.Rows.Cast<DataRow>()
                .Select(f => DigitVerifierService.CalcularDVHGenerico(f, filas.Columns))
                .ToList();

            int dvv = DigitVerifierService.CalcularDVV(digestos);
            _dvDAL.Guardar(new DigitoVerificador { Tabla = tabla, DVV = dvv.ToString() });
        }

       
        public Dictionary<string, List<string>> ObtenerTablasAlteradas()
        {
            var resultado = new Dictionary<string, List<string>>();

            foreach (string tabla in ObtenerTablasControladas())
            {
                DigitoVerificador dvGuardado = _dvDAL.ObtenerPorTabla(tabla);
                if (dvGuardado == null) continue;

                string[] pk = _dal.ObtenerClavePrimaria(tabla);
                DataTable filas = _dal.ObtenerFilas(tabla);

                List<int> dvhActual = new List<int>();
                List<string> filasAlteradas = new List<string>();

                foreach (DataRow fila in filas.Rows)
                {
                    int dvhCalculado = DigitVerifierService.CalcularDVHGenerico(fila, filas.Columns);
                    dvhActual.Add(dvhCalculado);

                    object valorGuardado = filas.Columns.Contains("DVH") ? fila["DVH"] : null;
                    int dvhGuardado = (valorGuardado == null || valorGuardado is DBNull)
                        ? int.MinValue
                        : Convert.ToInt32(valorGuardado);

                    if (dvhCalculado != dvhGuardado)
                    {
                        filasAlteradas.Add(DescribirFila(fila, filas.Columns, pk));
                    }
                }

                int dvvActual = DigitVerifierService.CalcularDVV(dvhActual);
                int dvvGuardado = dvGuardado.ObtenerDVV();

                if (dvvActual != dvvGuardado && filasAlteradas.Count == 0)
                {
                    filasAlteradas.Add("(cambió la cantidad de filas)");
                }

                if (filasAlteradas.Count > 0)
                {
                    resultado[tabla] = filasAlteradas;
                }
            }

            return resultado;
        }

      
        private string DescribirFila(DataRow fila, DataColumnCollection columnas, string[] pk)
        {
            string clave = string.Join("-", pk.Select(c => fila[c].ToString()));

            var otrasColumnas = columnas.Cast<DataColumn>()
                .Where(c => !string.Equals(c.ColumnName, "DVH", StringComparison.OrdinalIgnoreCase)
                         && !pk.Contains(c.ColumnName))
                .Select(c => c.ColumnName + "=" + TruncarValor(fila[c]));

            return clave + " (" + string.Join(", ", otrasColumnas) + ")";
        }

        private string TruncarValor(object valor)
        {
            string texto = (valor == null || valor is DBNull) ? "NULL" : valor.ToString();
            return texto.Length > 40 ? texto.Substring(0, 40) + "..." : texto;
        }

        public bool ValidarIntegridadGeneral()
        {
            return ObtenerTablasAlteradas().Count == 0;
        }
    }
}
