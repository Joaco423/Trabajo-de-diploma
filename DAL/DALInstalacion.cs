using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace DAL
{

    public class DALInstalacion
    {
        private const string NombreScript_235JB = "cafeccino_instalador.sql";
        private const string NombreScriptActualizacion_235JB = "cafeccino_actualizacion.sql";
        private const string BaseDelScript_235JB = "CAFECCINO1";
        // Base auxiliar que se crea con el script durante la actualizacion, para tomar de ahi la configuracion que falte
        private const string SufijoBaseTemporal_235JB = "_ACTUALIZACION_TMP";
        // Misma intercalacion que CAFECCINO1, de donde sale el script
        private const string Intercalacion_235JB = "Modern_Spanish_CI_AS";

        // Clave donde el instalador 2.0 guarda la carpeta de instalacion (Path) y la fecha y hora de cada instalacion
        // (Instalacion). En HKCU el programa guarda la ultima instalacion que ya atendio (InstalacionAtendida).
        private const string ClaveRegistro_235JB = @"Software\RetailGenius\CAFECCINO";
        private const string ValorInstalacion_235JB = "Instalacion";
        private const string ValorAtendida_235JB = "InstalacionAtendida";

        public string NombreBaseDeDatos_235JB()
        {
            return Conexion.BaseDeDatos_235JB();
        }

        
        private static string ConexionMaster_235JB(out string nombreBase_235JB)
        {
            nombreBase_235JB = new SqlConnectionStringBuilder(Conexion.cadena).InitialCatalog;
            return ConexionMaster_235JB();
        }

        private static string ConexionMaster_235JB()
        {
            SqlConnectionStringBuilder conexion_235JB = new SqlConnectionStringBuilder(Conexion.cadena);
            conexion_235JB.InitialCatalog = "master";
            conexion_235JB.ConnectTimeout = 15;
            return conexion_235JB.ConnectionString;
        }

        private static bool Existe_235JB(SqlConnection cn_235JB, string nombreBase_235JB)
        {
            using (SqlCommand cmd_235JB = new SqlCommand("SELECT DB_ID(@base)", cn_235JB))
            {
                cmd_235JB.Parameters.AddWithValue("@base", nombreBase_235JB);
                return !(cmd_235JB.ExecuteScalar() is DBNull);
            }
        }

        
        public bool BaseDeDatosLista_235JB()
        {
            string nombreBase_235JB;
            using (SqlConnection cn_235JB = new SqlConnection(ConexionMaster_235JB(out nombreBase_235JB)))
            {
                cn_235JB.Open();
                if (!Existe_235JB(cn_235JB, nombreBase_235JB))
                    return false;

                using (SqlCommand cmd_235JB = new SqlCommand("SELECT OBJECT_ID(QUOTENAME(@base) + N'.dbo.Usuario', N'U')", cn_235JB))
                {
                    cmd_235JB.Parameters.AddWithValue("@base", nombreBase_235JB);
                    return !(cmd_235JB.ExecuteScalar() is DBNull);
                }
            }
        }

       
        public void CrearBaseDeDatos_235JB()
        {
            string nombreBase_235JB;
            ConexionMaster_235JB(out nombreBase_235JB);
            CrearBase_235JB(nombreBase_235JB);
        }

        private static void CrearBase_235JB(string nombreBase_235JB)
        {
            string script_235JB = LeerScript_235JB(NombreScript_235JB);
            using (SqlConnection cn_235JB = new SqlConnection(ConexionMaster_235JB()))
            {
                cn_235JB.Open();
                bool existiaAntes_235JB = Existe_235JB(cn_235JB, nombreBase_235JB);

                bool traeCreateDatabase_235JB = Regex.IsMatch(script_235JB, @"(?im)^\s*CREATE\s+DATABASE\b");
                string usarBase_235JB = "USE [" + nombreBase_235JB.Replace("]", "]]") + "]";
                if (!traeCreateDatabase_235JB)
                {
                    
                    script_235JB = Regex.Replace(script_235JB, @"(?im)^[ \t]*USE[ \t]+\[[^\]\r\n]*\][ \t]*\r?$", usarBase_235JB);

                    
                    foreach (Match uso_235JB in Regex.Matches(script_235JB, @"(?im)^[ \t]*USE\b.*$"))
                    {
                        if (uso_235JB.Value.Trim() != usarBase_235JB)
                            throw new InvalidOperationException("El script de la base de datos cambia a otra base (" + uso_235JB.Value.Trim() + ") y no se ejecuto.");
                    }
                }
                else if (nombreBase_235JB != BaseDelScript_235JB)
                {
                    script_235JB = script_235JB.Replace(BaseDelScript_235JB, nombreBase_235JB);
                }

                try
                {
                    if (!traeCreateDatabase_235JB)
                    {
                        if (!existiaAntes_235JB)
                        {
                            using (SqlCommand crear_235JB = new SqlCommand("CREATE DATABASE [" + nombreBase_235JB.Replace("]", "]]") + "] COLLATE " + Intercalacion_235JB, cn_235JB))
                            {
                                crear_235JB.CommandTimeout = 120;
                                crear_235JB.ExecuteNonQuery();
                            }
                        }
                        cn_235JB.ChangeDatabase(nombreBase_235JB);
                    }

                    foreach (string lote_235JB in Regex.Split(script_235JB, @"(?im)^\s*GO\s*$"))
                    {
                        if (string.IsNullOrWhiteSpace(lote_235JB))
                            continue;
                        using (SqlCommand cmd_235JB = new SqlCommand(lote_235JB, cn_235JB))
                        {
                            cmd_235JB.CommandTimeout = 300;
                            cmd_235JB.ExecuteNonQuery();
                        }
                    }
                }
                catch
                {
                    if (!existiaAntes_235JB)
                        BorrarBase_235JB(cn_235JB, nombreBase_235JB);
                    throw;
                }
            }
        }

        // Borra la base que usa el programa para crearla de nuevo (al instalar, si el usuario elige pisarla).
        // Por seguridad solo lo hace el programa instalado con el instalador 2.0: ejecutado desde Visual Studio nunca borra.
        public void BorrarBaseDeDatos_235JB()
        {
            string nombreBase_235JB;
            ConexionMaster_235JB(out nombreBase_235JB);
            if (!Conexion.EsProgramaInstalado_235JB())
                throw new InvalidOperationException("Por seguridad, solo el programa instalado puede borrar la base de datos " + nombreBase_235JB + ".");

            BorrarBasePorNombre_235JB(nombreBase_235JB);
        }

        private static void BorrarBasePorNombre_235JB(string nombreBase_235JB)
        {
            SqlConnection.ClearAllPools();
            using (SqlConnection cn_235JB = new SqlConnection(ConexionMaster_235JB()))
            {
                cn_235JB.Open();
                using (SqlCommand cmd_235JB = new SqlCommand(
                    "IF DB_ID(@base) IS NOT NULL BEGIN DECLARE @sql NVARCHAR(400) = N'ALTER DATABASE ' + QUOTENAME(@base) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ' + QUOTENAME(@base); EXEC(@sql); END", cn_235JB))
                {
                    cmd_235JB.CommandTimeout = 120;
                    cmd_235JB.Parameters.AddWithValue("@base", nombreBase_235JB);
                    cmd_235JB.ExecuteNonQuery();
                }
            }
        }

        // Al instalar, si el usuario elige conservar una base que ya existia (por ejemplo, la que creo el instalador 1.0),
        // se la lleva a la version 2.0 sin perder datos:
        //   1. estructura que le falte (cafeccino_actualizacion.sql: baja logica de productos, bitacora de cambios, etc.)
        //   2. configuracion que le falte (idiomas, controles, traducciones, permisos y sus relaciones), tomada del script
        //      de creacion. Solo se agrega lo que no esta: no se modifica ni se borra nada de lo que ya tenia.
        // Devuelve true si cambio algo (hay que recalcular los digitos verificadores).
        public bool ActualizarBaseDeDatos_235JB()
        {
            bool cambio_235JB;
            using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
            {
                cn_235JB.Open();
                string antes_235JB = Firma_235JB(cn_235JB);
                foreach (string lote_235JB in Lotes_235JB(LeerScript_235JB(NombreScriptActualizacion_235JB)))
                {
                    using (SqlCommand cmd_235JB = new SqlCommand(lote_235JB, cn_235JB))
                    {
                        cmd_235JB.CommandTimeout = 300;
                        cmd_235JB.ExecuteNonQuery();
                    }
                }
                cambio_235JB = Firma_235JB(cn_235JB) != antes_235JB;
            }

            string nombreBase_235JB;
            ConexionMaster_235JB(out nombreBase_235JB);
            string temporal_235JB = nombreBase_235JB + SufijoBaseTemporal_235JB;
            BorrarBasePorNombre_235JB(temporal_235JB);
            try
            {
                CrearBase_235JB(temporal_235JB);
                using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadena))
                {
                    cn_235JB.Open();
                    string antes_235JB = Firma_235JB(cn_235JB);
                    using (SqlTransaction tx_235JB = cn_235JB.BeginTransaction())
                    using (SqlCommand cmd_235JB = new SqlCommand(ConfiguracionFaltante_235JB(temporal_235JB), cn_235JB, tx_235JB))
                    {
                        cmd_235JB.CommandTimeout = 300;
                        cmd_235JB.ExecuteNonQuery();
                        tx_235JB.Commit();
                    }
                    cambio_235JB |= Firma_235JB(cn_235JB) != antes_235JB;
                }
            }
            finally
            {
                BorrarBasePorNombre_235JB(temporal_235JB);
            }
            return cambio_235JB;
        }

        // Estructura y cantidad de filas de configuracion: si cambia, la actualizacion modifico la base
        private static string Firma_235JB(SqlConnection cn_235JB)
        {
            using (SqlCommand cmd_235JB = new SqlCommand(
                "SELECT CONCAT((SELECT COUNT(*) FROM sys.objects WHERE is_ms_shipped = 0), '/', (SELECT COUNT(*) FROM sys.columns), '/', " +
                "(SELECT COUNT(*) FROM sys.indexes), '/', (SELECT COUNT(*) FROM dbo.Idioma), '/', (SELECT COUNT(*) FROM dbo.ControlIdioma), '/', " +
                "(SELECT COUNT(*) FROM dbo.Traduccion), '/', (SELECT COUNT(*) FROM dbo.Permiso), '/', (SELECT COUNT(*) FROM dbo.Permiso_Permiso))", cn_235JB))
                return (string)cmd_235JB.ExecuteScalar();
        }

        // Inserta en la base del programa la configuracion de la base temporal que no tenga. Se compara por los datos
        // (nombre del idioma, formulario y control, nombre del permiso), no por los codigos, que pueden no coincidir.
        private static string ConfiguracionFaltante_235JB(string temporal_235JB)
        {
            string t_235JB = "[" + temporal_235JB.Replace("]", "]]") + "].dbo.";
            return @"
INSERT INTO dbo.Idioma (Nombre)
SELECT o.Nombre FROM " + t_235JB + @"Idioma o
WHERE NOT EXISTS (SELECT 1 FROM dbo.Idioma d WHERE d.Nombre = o.Nombre COLLATE DATABASE_DEFAULT)
ORDER BY o.IdIdioma;

INSERT INTO dbo.ControlIdioma (Formulario, NombreControl, TextoOriginal)
SELECT o.Formulario, o.NombreControl, o.TextoOriginal FROM " + t_235JB + @"ControlIdioma o
WHERE NOT EXISTS (SELECT 1 FROM dbo.ControlIdioma d
                  WHERE d.Formulario = o.Formulario COLLATE DATABASE_DEFAULT AND d.NombreControl = o.NombreControl COLLATE DATABASE_DEFAULT)
ORDER BY o.IdControl;

INSERT INTO dbo.Traduccion (IdIdioma, IdControl, Texto)
SELECT di.IdIdioma, dc.IdControl, o.Texto
FROM " + t_235JB + @"Traduccion o
JOIN " + t_235JB + @"Idioma oi ON oi.IdIdioma = o.IdIdioma
JOIN dbo.Idioma di ON di.Nombre = oi.Nombre COLLATE DATABASE_DEFAULT
JOIN " + t_235JB + @"ControlIdioma oc ON oc.IdControl = o.IdControl
JOIN dbo.ControlIdioma dc ON dc.Formulario = oc.Formulario COLLATE DATABASE_DEFAULT AND dc.NombreControl = oc.NombreControl COLLATE DATABASE_DEFAULT
WHERE NOT EXISTS (SELECT 1 FROM dbo.Traduccion d WHERE d.IdIdioma = di.IdIdioma AND d.IdControl = dc.IdControl)
ORDER BY o.IdTraduccion;

INSERT INTO dbo.Permiso (Nombre, EsPerfil, EsFamilia)
SELECT o.Nombre, o.EsPerfil, o.EsFamilia FROM " + t_235JB + @"Permiso o
WHERE NOT EXISTS (SELECT 1 FROM dbo.Permiso d WHERE d.Nombre = o.Nombre COLLATE DATABASE_DEFAULT)
ORDER BY o.CodPermiso;

-- Relaciones entre permisos (Composite). No se agrega una que formaria un ciclo con las que ya tiene la base.
DECLARE @nuevas TABLE (CodPadre int, CodHijo int, PRIMARY KEY (CodPadre, CodHijo));
INSERT INTO @nuevas (CodPadre, CodHijo)
SELECT DISTINCT dp.CodPermiso, dh.CodPermiso
FROM " + t_235JB + @"Permiso_Permiso o
JOIN " + t_235JB + @"Permiso op ON op.CodPermiso = o.CodPadre
JOIN dbo.Permiso dp ON dp.Nombre = op.Nombre COLLATE DATABASE_DEFAULT
JOIN " + t_235JB + @"Permiso oh ON oh.CodPermiso = o.CodHijo
JOIN dbo.Permiso dh ON dh.Nombre = oh.Nombre COLLATE DATABASE_DEFAULT
WHERE NOT EXISTS (SELECT 1 FROM dbo.Permiso_Permiso d WHERE d.CodPadre = dp.CodPermiso AND d.CodHijo = dh.CodPermiso);

WITH Descendientes AS (
    SELECT n.CodPadre, n.CodHijo, n.CodHijo AS Nodo FROM @nuevas n
    UNION ALL
    SELECT d.CodPadre, d.CodHijo, pp.CodHijo FROM Descendientes d JOIN dbo.Permiso_Permiso pp ON pp.CodPadre = d.Nodo
)
DELETE n FROM @nuevas n
WHERE n.CodPadre = n.CodHijo
   OR EXISTS (SELECT 1 FROM Descendientes d WHERE d.CodPadre = n.CodPadre AND d.CodHijo = n.CodHijo AND d.Nodo = n.CodPadre)
OPTION (MAXRECURSION 1000);

INSERT INTO dbo.Permiso_Permiso (CodPadre, CodHijo)
SELECT CodPadre, CodHijo FROM @nuevas;";
        }

        private static string[] Lotes_235JB(string script_235JB)
        {
            return Array.FindAll(Regex.Split(script_235JB, @"(?im)^\s*GO\s*$"), l_235JB => !string.IsNullOrWhiteSpace(l_235JB));
        }

        // Fecha y hora de la ultima instalacion, tal como la dejo el instalador, solo si este .exe se ejecuta desde la
        // carpeta instalada. Devuelve null en cualquier otro caso (por ejemplo, al ejecutar desde Visual Studio).
        public string MarcaDeInstalacion_235JB()
        {
            string carpetaExe_235JB = NormalizarCarpeta_235JB(AppDomain.CurrentDomain.BaseDirectory);
            foreach (RegistryHive raiz_235JB in new[] { RegistryHive.LocalMachine, RegistryHive.CurrentUser })
            {
                foreach (RegistryView vista_235JB in new[] { RegistryView.Registry32, RegistryView.Registry64 })
                {
                    try
                    {
                        using (RegistryKey base_235JB = RegistryKey.OpenBaseKey(raiz_235JB, vista_235JB))
                        using (RegistryKey clave_235JB = base_235JB.OpenSubKey(ClaveRegistro_235JB))
                        {
                            if (clave_235JB == null)
                                continue;
                            string carpeta_235JB = clave_235JB.GetValue("Path") as string;
                            string marca_235JB = clave_235JB.GetValue(ValorInstalacion_235JB) as string;
                            if (!string.IsNullOrWhiteSpace(carpeta_235JB) && !string.IsNullOrWhiteSpace(marca_235JB) &&
                                string.Equals(NormalizarCarpeta_235JB(carpeta_235JB), carpetaExe_235JB, StringComparison.OrdinalIgnoreCase))
                                return marca_235JB;
                        }
                    }
                    catch
                    {
                        // Sin permiso de lectura o vista inexistente: se prueba la siguiente
                    }
                }
            }
            return null;
        }

        public string MarcaAtendida_235JB()
        {
            using (RegistryKey clave_235JB = Registry.CurrentUser.OpenSubKey(ClaveRegistro_235JB))
                return clave_235JB == null ? null : clave_235JB.GetValue(ValorAtendida_235JB) as string;
        }

        public void GuardarMarcaAtendida_235JB(string marca_235JB)
        {
            using (RegistryKey clave_235JB = Registry.CurrentUser.CreateSubKey(ClaveRegistro_235JB))
                clave_235JB.SetValue(ValorAtendida_235JB, marca_235JB ?? string.Empty);
        }

        private static string NormalizarCarpeta_235JB(string carpeta_235JB)
        {
            try { return Path.GetFullPath(carpeta_235JB.Trim()).TrimEnd('\\'); }
            catch { return carpeta_235JB.Trim().TrimEnd('\\'); }
        }

        private static void BorrarBase_235JB(SqlConnection cn_235JB, string nombreBase_235JB)
        {
            try
            {
                cn_235JB.ChangeDatabase("master");
                using (SqlCommand cmd_235JB = new SqlCommand(
                    "IF DB_ID(@base) IS NOT NULL BEGIN DECLARE @sql NVARCHAR(400) = N'ALTER DATABASE ' + QUOTENAME(@base) + N' SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ' + QUOTENAME(@base); EXEC(@sql); END", cn_235JB))
                {
                    cmd_235JB.Parameters.AddWithValue("@base", nombreBase_235JB);
                    cmd_235JB.ExecuteNonQuery();
                }
            }
            catch
            {
                
            }
        }

        // Script embebido en DAL.dll (el recurso se llama "DAL.<archivo>")
        private static string LeerScript_235JB(string nombreScript_235JB)
        {
            Assembly ensamblado_235JB = Assembly.GetExecutingAssembly();
            string nombreRecurso_235JB = null;
            foreach (string nombre_235JB in ensamblado_235JB.GetManifestResourceNames())
            {
                if (nombre_235JB.Equals(nombreScript_235JB, StringComparison.OrdinalIgnoreCase) ||
                    nombre_235JB.EndsWith("." + nombreScript_235JB, StringComparison.OrdinalIgnoreCase))
                {
                    nombreRecurso_235JB = nombre_235JB;
                    break;
                }
            }

            using (Stream recurso_235JB = nombreRecurso_235JB == null ? null : ensamblado_235JB.GetManifestResourceStream(nombreRecurso_235JB))
            {
                if (recurso_235JB == null)
                    throw new FileNotFoundException("La aplicacion no incluye el script de la base de datos (" + nombreScript_235JB + ").");

                using (StreamReader lector_235JB = new StreamReader(recurso_235JB, true))
                    return lector_235JB.ReadToEnd();
            }
        }
    }
}
