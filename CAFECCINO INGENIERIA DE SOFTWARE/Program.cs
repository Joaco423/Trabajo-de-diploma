using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!PrepararBaseDeDatos_235JB())
                return;

            Application.Run(new frmInicio());
        }

        // Primer arranque en una PC nueva: se deja lista la conexion con SQL Server (la instancia no esta fija: se lee de
        // conexion.ini, se detecta sola o se la pide al usuario) y, si la base de datos no existe, se crea con su
        // estructura y sus datos iniciales. Todavia no hay base de la que leer traducciones, por eso van en español.
        private static bool PrepararBaseDeDatos_235JB()
        {
            BLLInstalacion bll_235JB = new BLLInstalacion();

            if (!AsegurarServidor_235JB(bll_235JB))
                return false;

            try
            {
                // Primer arranque despues de instalar o reinstalar (solo el .exe instalado; desde Visual Studio nunca)
                string marca_235JB;
                bool primerArranque_235JB = bll_235JB.EsPrimerArranqueDeInstalacion_235JB(out marca_235JB);

                if (bll_235JB.BaseDeDatosLista_235JB())
                {
                    if (primerArranque_235JB)
                    {
                        // Ya existe la base que creo una instalacion anterior: se pregunta si conservarla o pisarla
                        if (PreguntarPisarBase_235JB(bll_235JB.NombreBaseDeDatos_235JB()))
                        {
                            using (Form espera_235JB = CrearVentanaEspera_235JB("Creando una base de datos nueva.\nPuede tardar unos segundos..."))
                            {
                                espera_235JB.Show();
                                Application.DoEvents();
                                bll_235JB.ReemplazarBaseDeDatos_235JB();
                            }
                        }
                        else
                        {
                            // Se conservan los datos; si la base es de una version anterior (instalador 1.0) se actualiza
                            using (Form espera_235JB = CrearVentanaEspera_235JB("Revisando la base de datos existente.\nPuede tardar unos segundos..."))
                            {
                                espera_235JB.Show();
                                Application.DoEvents();
                                bll_235JB.ConservarBaseDeDatos_235JB();
                            }
                        }
                        bll_235JB.MarcarInstalacionAtendida_235JB(marca_235JB);
                    }
                    return true;
                }

                using (Form espera_235JB = CrearVentanaEspera_235JB("Preparando la base de datos por primera vez.\nPuede tardar unos segundos..."))
                {
                    espera_235JB.Show();
                    Application.DoEvents();
                    bll_235JB.CrearBaseDeDatos_235JB();
                }
                if (primerArranque_235JB)
                    bll_235JB.MarcarInstalacionAtendida_235JB(marca_235JB);
                return true;
            }
            catch (Exception ex_235JB)
            {
                MessageBox.Show(
                    "No se pudo preparar la base de datos " + bll_235JB.NombreBaseDeDatos_235JB() + " en el servidor \"" + bll_235JB.ObtenerServidor_235JB() + "\".\n\n" +
                    "Verifique que SQL Server esté instalado y que su usuario de Windows tenga permiso para crear bases de datos. " +
                    "Si necesita usar otra instancia, edite el archivo:\n" + bll_235JB.RutaArchivoConfiguracion_235JB() +
                    "\n\nDetalle: " + ex_235JB.Message,
                    "CAFECCINO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // Comprueba que el servidor configurado responda. Si no, prueba las instancias de esta PC y, si ninguna sirve,
        // le pide al usuario que indique la instancia.
        private static bool AsegurarServidor_235JB(BLLInstalacion bll_235JB)
        {
            string error_235JB;
            if (bll_235JB.ProbarServidor_235JB(bll_235JB.ObtenerServidor_235JB(), out error_235JB))
                return true;

            if (bll_235JB.DetectarYGuardarServidor_235JB())
                return true;

            using (frmConfigurarConexion dialogo_235JB = new frmConfigurarConexion(bll_235JB, bll_235JB.ObtenerServidor_235JB(), error_235JB))
                return dialogo_235JB.ShowDialog() == DialogResult.OK;
        }

        // Pregunta al reinstalar. Devuelve true solo si el usuario elige pisar la base y lo confirma.
        // Si cierra la ventana o elige conservar, la base queda como esta.
        private static bool PreguntarPisarBase_235JB(string nombreBase_235JB)
        {
            using (Form pregunta_235JB = new Form
            {
                Text = "CAFECCINO - Base de datos existente",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = true,
                TopMost = true,
                ClientSize = new Size(500, 215)
            })
            {
                pregunta_235JB.Controls.Add(new Label
                {
                    Text = "Se instaló CAFECCINO y ya existe la base de datos " + nombreBase_235JB +
                           ", creada en una instalación anterior.\n\n" +
                           "¿Qué querés hacer con esa base de datos?\n\n" +
                           "• Conservarla: el programa sigue usando los datos que ya tiene (si es de la versión 1.0, se actualiza).\n" +
                           "• Pisarla: se borra y se crea una base nueva, solo con la configuración inicial.",
                    AutoSize = false,
                    Location = new Point(16, 12),
                    Size = new Size(468, 145)
                });
                Button conservar_235JB = new Button
                {
                    Text = "Conservarla",
                    DialogResult = DialogResult.No,
                    Location = new Point(196, 169),
                    Size = new Size(130, 32)
                };
                Button pisar_235JB = new Button
                {
                    Text = "Pisarla",
                    DialogResult = DialogResult.Yes,
                    Location = new Point(338, 169),
                    Size = new Size(130, 32)
                };
                pregunta_235JB.Controls.Add(conservar_235JB);
                pregunta_235JB.Controls.Add(pisar_235JB);
                pregunta_235JB.AcceptButton = conservar_235JB;
                pregunta_235JB.CancelButton = conservar_235JB;

                if (pregunta_235JB.ShowDialog() != DialogResult.Yes)
                    return false;
            }

            return MessageBox.Show(
                "Se van a borrar todos los datos de la base " + nombreBase_235JB +
                " (clientes, productos, pedidos, reservas, usuarios, etc.) y se va a crear una base nueva.\n\n" +
                "Esta acción no se puede deshacer. ¿Querés continuar?",
                "CAFECCINO - Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        private static Form CrearVentanaEspera_235JB(string texto_235JB)
        {
            Form ventana_235JB = new Form
            {
                Text = "CAFECCINO",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                ControlBox = false,
                ShowInTaskbar = false,
                TopMost = true,
                ClientSize = new Size(420, 90)
            };
            ventana_235JB.Controls.Add(new Label
            {
                Text = texto_235JB,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            });
            return ventana_235JB;
        }
    }
}
