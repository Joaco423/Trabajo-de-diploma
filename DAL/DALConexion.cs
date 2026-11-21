using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Win32;

namespace DAL
{
    
    
    public class DALConexion
    {
        public string ObtenerServidor_235JB()
        {
            return Conexion.Servidor_235JB();
        }

        public string RutaArchivoConfiguracion_235JB()
        {
            return Conexion.RutaArchivoConfiguracion_235JB();
        }

        public void GuardarServidor_235JB(string servidor_235JB)
        {
            Conexion.GuardarServidor_235JB(servidor_235JB);
        }

        
        public bool ProbarServidor_235JB(string servidor_235JB, out string error_235JB)
        {
            error_235JB = null;
            if (string.IsNullOrWhiteSpace(servidor_235JB))
            {
                error_235JB = "Indique el nombre del servidor.";
                return false;
            }

            try
            {
                SqlConnectionStringBuilder constructor_235JB =
                    new SqlConnectionStringBuilder(Conexion.Armar_235JB(servidor_235JB.Trim(), "master"));
                constructor_235JB.ConnectTimeout = 8;
                using (SqlConnection cn_235JB = new SqlConnection(constructor_235JB.ConnectionString))
                {
                    cn_235JB.Open();
                    return true;
                }
            }
            catch (Exception ex_235JB)
            {
                error_235JB = ex_235JB.Message;
                return false;
            }
        }

        
        public List<string> ListarInstanciasLocales_235JB()
        {
            List<string> servidores_235JB = new List<string>();
            foreach (RegistryView vista_235JB in new[] { RegistryView.Registry64, RegistryView.Registry32 })
            {
                try
                {
                    using (RegistryKey raiz_235JB = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, vista_235JB))
                    using (RegistryKey clave_235JB = raiz_235JB.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"))
                    {
                        if (clave_235JB == null)
                            continue;
                        foreach (string instancia_235JB in clave_235JB.GetValueNames())
                        {
                            string servidor_235JB = string.Equals(instancia_235JB, "MSSQLSERVER", StringComparison.OrdinalIgnoreCase)
                                ? "localhost"
                                : @".\" + instancia_235JB;
                            if (!servidores_235JB.Contains(servidor_235JB))
                                servidores_235JB.Add(servidor_235JB);
                        }
                    }
                }
                catch
                {
                    
                }
            }
            return servidores_235JB;
        }

        
        public bool DetectarServidor_235JB(out string servidor_235JB)
        {
            List<string> candidatos_235JB = ListarInstanciasLocales_235JB();
            foreach (string habitual_235JB in new[] { "localhost", @".\SQLEXPRESS", @"localhost\SQLEXPRESS", "(local)" })
                if (!candidatos_235JB.Contains(habitual_235JB))
                    candidatos_235JB.Add(habitual_235JB);

            foreach (string candidato_235JB in candidatos_235JB)
            {
                string error_235JB;
                if (ProbarServidor_235JB(candidato_235JB, out error_235JB))
                {
                    servidor_235JB = candidato_235JB;
                    return true;
                }
            }
            servidor_235JB = null;
            return false;
        }
    }
}
