using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace DAL
{
    
    internal static class Conexion
    {
        private const string NombreArchivo_235JB = "conexion.ini";
        private const string ClaveRegistro_235JB = @"Software\RetailGenius\CAFECCINO";
        private const string ServidorPorDefecto_235JB = "localhost";
        private const string ModeloCadena_235JB = @"Data Source=localhost;Initial Catalog=CAFECCINO1;Integrated Security=True";

        private static readonly object _bloqueo_235JB = new object();
        private static Dictionary<string, string> _valores_235JB;

        
        internal static string cadena
        {
            get { return Armar_235JB(Servidor_235JB(), null); }
        }

        
        internal static string cadenaMaster_235JB
        {
            get { return Armar_235JB(Servidor_235JB(), "master"); }
        }

        
        internal static string BaseDeDatos_235JB()
        {
            string base_235JB;
            if (Valores_235JB().TryGetValue("BaseDeDatos", out base_235JB) && !string.IsNullOrWhiteSpace(base_235JB))
                return base_235JB.Trim();
            return BaseDeDatosDesarrollo_235JB();
        }

    
        internal static string BaseDeDatosDesarrollo_235JB()
        {
            return new SqlConnectionStringBuilder(ModeloCadena_235JB).InitialCatalog;
        }

        internal static string Armar_235JB(string servidor_235JB, string baseDeDatos_235JB)
        {
            SqlConnectionStringBuilder constructor_235JB = new SqlConnectionStringBuilder(ModeloCadena_235JB);
            constructor_235JB.DataSource = servidor_235JB;
            constructor_235JB.InitialCatalog = baseDeDatos_235JB ?? BaseDeDatos_235JB();
            return constructor_235JB.ConnectionString;
        }

        internal static string Servidor_235JB()
        {
            string servidor_235JB;
            if (Valores_235JB().TryGetValue("Servidor", out servidor_235JB) && !string.IsNullOrWhiteSpace(servidor_235JB))
                return servidor_235JB.Trim();
            return ServidorPorDefecto_235JB;
        }

        
        internal static string RutaBackups_235JB()
        {
            string ruta_235JB;
            if (Valores_235JB().TryGetValue("RutaBackups", out ruta_235JB) && !string.IsNullOrWhiteSpace(ruta_235JB))
                return ruta_235JB.Trim();
            return null;
        }

        internal static string RutaArchivoUsuario_235JB()
        {
            string carpeta_235JB = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrEmpty(carpeta_235JB))
                carpeta_235JB = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(carpeta_235JB, "CAFECCINO", NombreArchivo_235JB);
        }

        // conexion.ini de la carpeta del .exe. El instalador 2.0 lo deja ahi con la base (BaseDeDatos=CAFECCINO1, la misma
        // que crea el instalador 1.0) y la instancia de SQL Server elegida en la pantalla de conexion (Servidor=...).
        internal static string RutaArchivoPrograma_235JB()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NombreArchivo_235JB);
        }

        // Programa instalado con el instalador 2.0: su conexion.ini define la base de datos. Entonces usa solo ese archivo
        // (la instancia elegida al instalar) y no el del usuario, que es el del programa ejecutado desde Visual Studio.
        internal static bool EsProgramaInstalado_235JB()
        {
            Dictionary<string, string> programa_235JB = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            Leer_235JB(RutaArchivoPrograma_235JB(), programa_235JB);
            return programa_235JB.ContainsKey("BaseDeDatos");
        }

        // Archivo en el que se lee y se guarda la configuracion de este programa
        internal static string RutaArchivoConfiguracion_235JB()
        {
            return EsProgramaInstalado_235JB() ? RutaArchivoPrograma_235JB() : RutaArchivoUsuario_235JB();
        }


        internal static void GuardarServidor_235JB(string servidor_235JB)
        {
            lock (_bloqueo_235JB)
            {
                Dictionary<string, string> valores_235JB = Valores_235JB();
                valores_235JB["Servidor"] = servidor_235JB.Trim();

                bool instalado_235JB = EsProgramaInstalado_235JB();
                string ruta_235JB = RutaArchivoConfiguracion_235JB();
                Directory.CreateDirectory(Path.GetDirectoryName(ruta_235JB));

                string rutaBackups_235JB;
                valores_235JB.TryGetValue("RutaBackups", out rutaBackups_235JB);

                StringBuilder texto_235JB = new StringBuilder();
                texto_235JB.AppendLine("; Configuracion de CAFECCINO. Se puede editar a mano con el Bloc de notas.");
                texto_235JB.AppendLine("; Servidor: instancia de SQL Server (localhost, .\\SQLEXPRESS, NOMBREPC\\SQLEXPRESS, etc.)");
                texto_235JB.AppendLine("; RutaBackups: carpeta de los backups (opcional; si se deja vacia se usa la carpeta de backups de SQL Server)");
                if (instalado_235JB)
                {
                    // Misma seccion que escribe el instalador, para que al reparar o desinstalar encuentre los valores
                    texto_235JB.AppendLine("; BaseDeDatos: base de datos del programa instalado");
                    texto_235JB.AppendLine("[CAFECCINO]");
                    texto_235JB.AppendLine("BaseDeDatos=" + valores_235JB["BaseDeDatos"]);
                }
                texto_235JB.AppendLine("Servidor=" + valores_235JB["Servidor"]);
                texto_235JB.AppendLine("RutaBackups=" + (rutaBackups_235JB ?? string.Empty));
                File.WriteAllText(ruta_235JB, texto_235JB.ToString(), new UTF8Encoding(false));

                if (instalado_235JB)
                    GuardarServidorEnRegistro_235JB(valores_235JB["Servidor"]);

                _valores_235JB = null;
            }
            SqlConnection.ClearAllPools();
        }

        // El instalador recuerda la instancia elegida en el registro (valor Servidor) y la vuelve a escribir en
        // conexion.ini al reparar. Se actualiza para que una reparacion no vuelva a la instancia anterior.
        private static void GuardarServidorEnRegistro_235JB(string servidor_235JB)
        {
            try
            {
                using (RegistryKey clave_235JB = Registry.CurrentUser.OpenSubKey(ClaveRegistro_235JB, true))
                {
                    if (clave_235JB != null && clave_235JB.GetValue("Servidor") != null)
                        clave_235JB.SetValue("Servidor", servidor_235JB, RegistryValueKind.String);
                }
            }
            catch
            {
                // Si no se puede, solo afecta a una reparacion futura del programa
            }
        }

        private static Dictionary<string, string> Valores_235JB()
        {
            lock (_bloqueo_235JB)
            {
                if (_valores_235JB == null)
                {
                    Dictionary<string, string> valores_235JB = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                    Leer_235JB(RutaArchivoPrograma_235JB(), valores_235JB);
                    if (!valores_235JB.ContainsKey("BaseDeDatos"))
                        Leer_235JB(RutaArchivoUsuario_235JB(), valores_235JB);
                    _valores_235JB = valores_235JB;
                }
                return _valores_235JB;
            }
        }

        private static void Leer_235JB(string ruta_235JB, Dictionary<string, string> destino_235JB)
        {
            try
            {
                if (!File.Exists(ruta_235JB))
                    return;

                foreach (string linea_235JB in File.ReadAllLines(ruta_235JB, Encoding.UTF8))
                {
                    string texto_235JB = linea_235JB.Trim();
                    if (texto_235JB.Length == 0 || texto_235JB.StartsWith(";") || texto_235JB.StartsWith("#") || texto_235JB.StartsWith("["))
                        continue;

                    int igual_235JB = texto_235JB.IndexOf('=');
                    if (igual_235JB <= 0)
                        continue;

                    string valor_235JB = texto_235JB.Substring(igual_235JB + 1).Trim();
                    if (valor_235JB.Length > 0)
                        destino_235JB[texto_235JB.Substring(0, igual_235JB).Trim()] = valor_235JB;
                }
            }
            catch
            {
                
            }
        }
    }
}
