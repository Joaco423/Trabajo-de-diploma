using System;
using System.Data.SqlClient;
using System.IO;

namespace DAL
{
    
    public class DALBackup
    {
        private static string _ruta_235JB;

        private static string NombreBaseDatos_235JB()
        {
            return Conexion.BaseDeDatos_235JB();
        }

        private static string RutaBackup_235JB()
        {
            if (_ruta_235JB != null)
                return _ruta_235JB;

            string carpeta_235JB = Conexion.RutaBackups_235JB();
            if (string.IsNullOrWhiteSpace(carpeta_235JB))
                carpeta_235JB = CarpetaBackupDelServidor_235JB();
            if (string.IsNullOrWhiteSpace(carpeta_235JB))
                carpeta_235JB = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "CAFECCINO", "Backups");

            _ruta_235JB = Path.Combine(carpeta_235JB, NombreBaseDatos_235JB() + ".bak");
            return _ruta_235JB;
        }

        private static string CarpetaBackupDelServidor_235JB()
        {
            try
            {
                using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadenaMaster_235JB))
                using (SqlCommand cmd_235JB = new SqlCommand("SELECT CONVERT(nvarchar(260), SERVERPROPERTY('InstanceDefaultBackupPath'))", cn_235JB))
                {
                    cn_235JB.Open();
                    object valor_235JB = cmd_235JB.ExecuteScalar();
                    return (valor_235JB == null || valor_235JB is DBNull) ? null : valor_235JB.ToString();
                }
            }
            catch
            {
                return null;
            }
        }

        public string ObtenerRutaBackup() => RutaBackup_235JB();

        
        public bool ExisteBackup()
        {
            string ruta_235JB = RutaBackup_235JB();
            try
            {
                if (File.Exists(ruta_235JB))
                    return true;
            }
            catch
            {
            }

            try
            {
                using (SqlConnection cn_235JB = new SqlConnection(Conexion.cadenaMaster_235JB))
                using (SqlCommand cmd_235JB = new SqlCommand(
                    "SELECT COUNT(*) FROM msdb.dbo.backupmediafamily f INNER JOIN msdb.dbo.backupset s ON s.media_set_id = f.media_set_id " +
                    "WHERE f.physical_device_name = @ruta AND s.database_name = @base", cn_235JB))
                {
                    cmd_235JB.Parameters.AddWithValue("@ruta", ruta_235JB);
                    cmd_235JB.Parameters.AddWithValue("@base", NombreBaseDatos_235JB());
                    cn_235JB.Open();
                    return Convert.ToInt32(cmd_235JB.ExecuteScalar()) > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public void GenerarBackup()
        {
            string ruta_235JB = RutaBackup_235JB();
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ruta_235JB));
            }
            catch
            {
                
            }

            using (SqlConnection cn = new SqlConnection(Conexion.cadenaMaster_235JB))
            using (SqlCommand cmd = new SqlCommand(
                $"BACKUP DATABASE [{NombreBaseDatos_235JB()}] TO DISK = @ruta WITH INIT, STATS = 10", cn))
            {
                cmd.Parameters.AddWithValue("@ruta", ruta_235JB);
                cmd.CommandTimeout = 120;
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void RestaurarBackup()
        {
            if (!ExisteBackup())
                throw new Exception("No existe un backup generado todavía. Use 'Recalcular' al menos una vez para generar uno.");

            string ruta_235JB = RutaBackup_235JB();
            string baseDatos_235JB = NombreBaseDatos_235JB();

            using (SqlConnection cn = new SqlConnection(Conexion.cadenaMaster_235JB))
            {
                cn.Open();

                EjecutarNonQuery(cn, $"ALTER DATABASE [{baseDatos_235JB}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");

                try
                {
                    using (SqlCommand cmd = new SqlCommand(
                        $"RESTORE DATABASE [{baseDatos_235JB}] FROM DISK = @ruta WITH REPLACE, STATS = 10", cn))
                    {
                        cmd.Parameters.AddWithValue("@ruta", ruta_235JB);
                        cmd.CommandTimeout = 180;
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    EjecutarNonQuery(cn, $"ALTER DATABASE [{baseDatos_235JB}] SET MULTI_USER");
                }
            }

            SqlConnection.ClearAllPools();
        }

        private void EjecutarNonQuery(SqlConnection cn, string sql)
        {
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
