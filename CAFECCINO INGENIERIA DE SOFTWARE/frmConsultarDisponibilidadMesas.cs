using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
    
    public partial class frmConsultarDisponibilidadMesas : Form
    {
        private readonly BLLAtencion _bllAtencion_235JB = new BLLAtencion();
        private readonly BLLReserva _bllReserva_235JB = new BLLReserva();
        private readonly Cliente _cliente_235JB;

        
        public int? NumeroMesaReservada { get; private set; }

        public frmConsultarDisponibilidadMesas(Cliente cliente_235JB)
        {
            InitializeComponent();
            _cliente_235JB = cliente_235JB;
        }

        private void frmConsultarDisponibilidadMesas_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
        }

        
        private void frmConsultarDisponibilidadMesas_Shown_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ConsultarMesas_235JB();
        }

        private void btnConsultar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            ConsultarMesas_235JB();
        }

        private void ConsultarMesas_235JB()
        {
            try
            {
                List<Mesa> mesas_235JB = _bllAtencion_235JB.ConsultarDisponibilidadMesas_235JB();

                if (mesas_235JB.Count > 0)
                {
                    dgvMesas.DataSource = mesas_235JB;
                    Resultado = DialogResult.OK;
                    return;
                }

                
                dgvMesas.DataSource = null;
                if (!Preguntar_235JB(Msg_235JB("msg_SinMesas", "No hay mesas disponibles. ¿El cliente desea ingresar a la lista de espera de mesas?"),
                        "msg_ListaEspera_Titulo", "Lista de espera"))
                    return;

                _bllAtencion_235JB.RegistrarEnListaDeEspera_235JB(true, _cliente_235JB);
                Aviso_235JB(Msg_235JB("msg_EnListaDeEspera", "El cliente quedó registrado en la lista de espera de mesas."),
                    "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                Terminar_235JB(DialogResult.Yes);
            }
            catch (InvalidOperationException)
            {
                
                Aviso_235JB(Msg_235JB("msg_YaEnLista", "El cliente ya está en la lista de espera."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorMesas", "Error al consultar las mesas: ", ex_235JB);
            }
        }

        
        private void btnReservar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mesa mesa_235JB = dgvMesas.CurrentRow == null ? null : dgvMesas.CurrentRow.DataBoundItem as Mesa;
            if (mesa_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMesa", "Seleccione de la grilla la mesa que desea reservar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Reserva reserva_235JB = _bllReserva_235JB.Reservar_235JB(_cliente_235JB, mesa_235JB.NumeroMesa);
                NumeroMesaReservada = reserva_235JB.NumeroMesa;
                Aviso_235JB(string.Format(Msg_235JB("msg_MesaReservada", "Mesa {0} reservada para {1} {2}. Código de reserva: {3}."),
                        reserva_235JB.NumeroMesa, _cliente_235JB.Nombre, _cliente_235JB.Apellido, reserva_235JB.CodigoReserva),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
                Terminar_235JB(DialogResult.OK);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_PedidoEnCurso")
            {
                Aviso_235JB(string.Format(Msg_235JB("msg_ClienteConPedidoEnCurso", "El cliente ya está siendo atendido (pedido {0}). No puede reservar una mesa hasta que finalice su atención."), ex_235JB.Data["CodigoPedido"]),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex_235JB) when (ex_235JB.Message == "msg_ClienteConReserva")
            {
                Aviso_235JB(string.Format(Msg_235JB("msg_ClienteConReserva", "El cliente ya tiene una reserva activa (mesa {0}). Cancélela para reservar otra."), ex_235JB.Data["NumeroMesa"]),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaNoDisponible", "La mesa seleccionada ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                ConsultarMesas_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorReservar", "Error al reservar la mesa: ", ex_235JB);
            }
        }
        private void btnCerrar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(Resultado);
        }

        private readonly Traductor traductor_235JB = new Traductor();

        public DialogResult Resultado { get; private set; } = DialogResult.Cancel;

        private string Msg_235JB(string clave_235JB, string original_235JB)
        {
            return traductor_235JB.ObtenerMensaje(Name, clave_235JB, original_235JB);
        }

        private void Aviso_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB, MessageBoxIcon icono_235JB)
        {
            MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB), MessageBoxButtons.OK, icono_235JB);
        }

        private void Error_235JB(string clave_235JB, string original_235JB, Exception ex_235JB)
        {
            Aviso_235JB(Msg_235JB(clave_235JB, original_235JB) + ex_235JB.Message, "msg_Error_Titulo", "Error", MessageBoxIcon.Error);
        }

        private bool Preguntar_235JB(string mensaje_235JB, string tituloClave_235JB, string tituloOriginal_235JB)
        {
            return MessageBox.Show(this, mensaje_235JB, Msg_235JB(tituloClave_235JB, tituloOriginal_235JB),
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void Terminar_235JB(DialogResult resultado_235JB)
        {
            Resultado = resultado_235JB;
            DialogResult = resultado_235JB == DialogResult.Cancel ? DialogResult.Cancel : DialogResult.OK;
            Close();
        }
    }
}
