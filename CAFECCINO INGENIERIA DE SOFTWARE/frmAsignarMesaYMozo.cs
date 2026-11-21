using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;

namespace CAFECCINO_INGENIERIA_DE_SOFTWARE
{
   
    public partial class frmAsignarMesaYMozo : Form
    {
        private enum Etapa { Espera, Mesa, Mozo, Final }

        private readonly BLLAtencion _bllAtencion_235JB = new BLLAtencion();
        private readonly BLLPedido _bllPedido_235JB = new BLLPedido();
        private readonly BLLCliente _bllCliente_235JB = new BLLCliente();
        private Etapa _etapa_235JB;
        private DialogResult _pendiente_235JB = DialogResult.Cancel;

        // Mesa elegida de cada pedido que quedo esperando mozo (se pierde al cerrar el sistema)
        public static readonly Dictionary<int, int> MesaPorPedido_235JB = new Dictionary<int, int>();

        // Pedidos de clientes con reserva que quedaron esperando mozo: conservan la prioridad sobre un mozo
        public static readonly HashSet<int> PedidosConPrioridad_235JB = new HashSet<int>();

        // El cliente tiene (o tuvo) una reserva: puede tomar cualquier mozo libre
        public bool ConPrioridad { get; set; }

        // Estado de la atencion: entra por el constructor y el llamador lo lee al cerrar el CU
        public Cliente Cliente { get; private set; }
        public Pedido Pedido { get; private set; }
        public int? NumeroMesa { get; private set; }
        public ResumenAtencion Resumen { get; private set; }
        public bool PedidoCancelado { get; private set; }

        // Sin pedido ni cliente: se atiende al primero de la lista de espera
        public frmAsignarMesaYMozo(Cliente cliente_235JB, Pedido pedido_235JB, int? numeroMesa_235JB)
        {
            InitializeComponent();
            Cliente = cliente_235JB;
            Pedido = pedido_235JB;
            NumeroMesa = numeroMesa_235JB;
        }

       
        private void frmAsignarMesaYMozo_Load_235JB(object sender_235JB, EventArgs e_235JB)
        {
            traductor_235JB.TraducirFormulario(this, Singleton.Instancia.IdiomaActual);
            if (Pedido != null && PedidosConPrioridad_235JB.Contains(Pedido.CodigoPedido))
                ConPrioridad = true;
            CargarMesas_235JB();
            CargarMozos_235JB();

            if (Pedido == null)
                _etapa_235JB = Etapa.Espera;
            else
                _etapa_235JB = NumeroMesa.HasValue ? Etapa.Mozo : Etapa.Mesa;

            MostrarEncabezado_235JB();
            AplicarEtapa_235JB();
        }

       
        private void frmAsignarMesaYMozo_Shown_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (_etapa_235JB == Etapa.Mesa)
                ProcesarMesa_235JB();
            else if (_etapa_235JB == Etapa.Mozo)
                ProcesarMozo_235JB();
        }

        private void frmAsignarMesaYMozo_FormClosing_235JB(object sender_235JB, FormClosingEventArgs e_235JB)
        {
            if (Resultado == DialogResult.Cancel)
                Resultado = _pendiente_235JB;
        }

        private void AplicarEtapa_235JB()
        {
            btnAsignarMesa.Enabled = _etapa_235JB == Etapa.Mesa;
            btnMesaDesdeLista.Enabled = _etapa_235JB == Etapa.Espera;
            btnAsignarMozo.Enabled = _etapa_235JB == Etapa.Mozo;
            btnMozoDesdeLista.Enabled = _etapa_235JB == Etapa.Espera;
            btnListaEspera.Enabled = _etapa_235JB == Etapa.Mesa || _etapa_235JB == Etapa.Mozo;
            btnCancelarPedido.Enabled = Pedido != null && (_etapa_235JB == Etapa.Mesa || _etapa_235JB == Etapa.Mozo);
            btnContinuar.Visible = _etapa_235JB == Etapa.Final;
        }

        private void MostrarEncabezado_235JB()
        {
            txtResumen.Clear();
            if (Cliente != null && Pedido != null)
                AgregarResumen_235JB(string.Format(Msg_235JB("msg_EncabezadoPedido", "Cliente: {0} {1} - Pedido {2}."),
                    Cliente.Nombre, Cliente.Apellido, Pedido.CodigoPedido));
            if (NumeroMesa.HasValue)
                AgregarResumen_235JB(string.Format(Msg_235JB("msg_TextoMesaAsignada", "Mesa asignada: {0}."), NumeroMesa.Value));
        }

        private void AgregarResumen_235JB(string linea_235JB)
        {
            txtResumen.AppendText(linea_235JB + Environment.NewLine);
        }

      
        // CU05 paso 1: mesas disponibles (numero y capacidad)
        private void CargarMesas_235JB()
        {
            try
            {
                dgvMesas.DataSource = _bllAtencion_235JB.ObtenerMesasDisponibles_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCargarMesas", "Error al cargar las mesas: ", ex_235JB);
            }
        }

        private Mesa MesaSeleccionada_235JB()
        {
            return dgvMesas.CurrentRow == null ? null : dgvMesas.CurrentRow.DataBoundItem as Mesa;
        }

        // CU05 flujo 1.1: sin mesas libres -> se ofrece la lista de espera de mesas
        private void ProcesarMesa_235JB()
        {
            CargarMesas_235JB();
            List<Mesa> libres_235JB = dgvMesas.DataSource as List<Mesa>;
            if (libres_235JB == null || libres_235JB.Count == 0)
                OfrecerListaDeEspera_235JB(true);
        }

        // CU05 pasos 2-4: asigna la mesa elegida (queda Ocupada) y muestra numero y capacidad
        private void btnAsignarMesa_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mesa mesa_235JB = MesaSeleccionada_235JB();
            if (mesa_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMesa", "Seleccione de la grilla la mesa que desea asignar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _bllAtencion_235JB.AsignarMesa_235JB(mesa_235JB.NumeroMesa);
                NumeroMesa = mesa_235JB.NumeroMesa;

                AgregarResumen_235JB(string.Format(Msg_235JB("msg_TextoMesaAsignada", "Mesa asignada: {0}."), mesa_235JB.NumeroMesa)
                    + " " + string.Format(Msg_235JB("msg_TextoCapacidad", "Capacidad: {0}."), mesa_235JB.Capacidad));

                _etapa_235JB = Etapa.Mozo;
                AplicarEtapa_235JB();
                CargarMesas_235JB();
                ProcesarMozo_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaNoDisponible", "La mesa seleccionada ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMesas_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMesa", "Error al asignar la mesa: ", ex_235JB);
            }
        }

        
        // CU05 flujo 1.2: mesa liberada -> se asigna al primero de la lista de espera (Fecha y Hora)
        private void btnMesaDesdeLista_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mesa mesa_235JB = MesaSeleccionada_235JB();
            if (mesa_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMesaLibre", "Seleccione de la grilla la mesa libre que desea asignar al primer cliente de la lista de espera."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ListaDeEspera primero_235JB = _bllAtencion_235JB.AsignarMesaDeListaDeEspera_235JB(mesa_235JB.NumeroMesa);
                if (primero_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_ListaVacia", "La lista de espera está vacía."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                Pedido pedido_235JB = _bllPedido_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(primero_235JB.DNI);
                if (pedido_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_SinPedidoPendiente", "El cliente de la lista de espera no tiene un pedido confirmado pendiente."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    CargarMesas_235JB();
                    return;
                }

                Cliente = _bllCliente_235JB.ObtenerPorDNI_235JB(primero_235JB.DNI);
                Pedido = pedido_235JB;
                NumeroMesa = mesa_235JB.NumeroMesa;

                Aviso_235JB(string.Format(Msg_235JB("msg_MesaAsignadaEspera", "Se asignó la mesa {0} a {1} {2}, primero en la lista de espera. Continúe con la asignación de mozo."),
                        mesa_235JB.NumeroMesa, primero_235JB.Nombre, primero_235JB.Apellido),
                    "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);

                MostrarEncabezado_235JB();
                _etapa_235JB = Etapa.Mozo;
                AplicarEtapa_235JB();
                CargarMesas_235JB();
                ProcesarMozo_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MesaNoDisponible", "La mesa seleccionada ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMesas_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMesa", "Error al asignar la mesa: ", ex_235JB);
            }
        }

       
        // CU06 paso 1: mozos disponibles
        private void CargarMozos_235JB()
        {
            try
            {
                // Al atender la lista de espera (sin pedido) se muestran todos los mozos libres; la prioridad se verifica al asignar
                dgvMozos.DataSource = _bllAtencion_235JB.ObtenerMozosParaAsignar_235JB(ConPrioridad || Pedido == null);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCargarMozos", "Error al cargar los mozos: ", ex_235JB);
            }
        }

        private Mozo MozoSeleccionado_235JB()
        {
            return dgvMozos.CurrentRow == null ? null : dgvMozos.CurrentRow.DataBoundItem as Mozo;
        }

        // CU06 flujo 1.1: sin mozos disponibles -> se ofrece la lista de espera de mozos
        private void ProcesarMozo_235JB()
        {
            CargarMozos_235JB();
            List<Mozo> libres_235JB = dgvMozos.DataSource as List<Mozo>;
            if (libres_235JB == null || libres_235JB.Count == 0)
                OfrecerListaDeEspera_235JB(false);
        }

        // CU06 pasos 2-4: asigna el mozo (queda Ocupado) y registra la relacion cliente-pedido-mesa-mozo
        private void btnAsignarMozo_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mozo mozo_235JB = MozoSeleccionado_235JB();
            if (mozo_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMozo", "Seleccione de la grilla el mozo que desea asignar."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Resumen = _bllAtencion_235JB.AsignarMozo_235JB(mozo_235JB.DNIMozo, Pedido, NumeroMesa.Value, ConPrioridad);
                MesaPorPedido_235JB.Remove(Pedido.CodigoPedido);
                PedidosConPrioridad_235JB.Remove(Pedido.CodigoPedido);
                MostrarResumenAtencion_235JB(Resumen);
                LiberarMesasHuerfanas_235JB();
                Finalizar_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoNoDisponible", "El mozo seleccionado ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMozos_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMozo", "Error al asignar el mozo: ", ex_235JB);
            }
        }

        
        // Un cliente que retoma un pedido cuya mesa se habia perdido elige otra mesa: la anterior queda ocupada sin pedido.
        // Con la atencion ya registrada se liberan las mesas ocupadas que no pertenecen a nadie.
        private void LiberarMesasHuerfanas_235JB()
        {
            List<int> liberadas_235JB = BarrerMesasHuerfanas_235JB();
            if (liberadas_235JB.Count > 0)
                AgregarResumen_235JB(TextoMesasLiberadas_235JB(liberadas_235JB));
        }

        // Libera las mesas ocupadas sin pedido y devuelve cuales fueron. Si falla no lanza error: el pedido ya quedo cancelado
        // o la atencion ya quedo registrada, y esta limpieza no debe mostrarse como un error de esa operacion.
        private List<int> BarrerMesasHuerfanas_235JB()
        {
            try
            {
                return _bllAtencion_235JB.LiberarMesasHuerfanas_235JB(MesaPorPedido_235JB);
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }

        private string TextoMesasLiberadas_235JB(List<int> liberadas_235JB)
        {
            return string.Format(Msg_235JB("msg_MesasLiberadas", "Se liberó la mesa ocupada sin pedido: {0}."), string.Join(", ", liberadas_235JB));
        }

        // CU06 flujo 1.2: mozo liberado -> se asigna al primero de la lista de espera
        private void btnMozoDesdeLista_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Mozo mozo_235JB = MozoSeleccionado_235JB();
            if (mozo_235JB == null)
            {
                Aviso_235JB(Msg_235JB("msg_SeleccioneMozoLibre", "Seleccione de la grilla el mozo libre que desea asignar al primer cliente de la lista de espera."),
                    "msg_Validacion_Titulo", "Validación", MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ListaDeEspera primero_235JB = _bllAtencion_235JB.ObtenerPrimeroEnListaDeEspera_235JB(false);
                if (primero_235JB == null)
                {
                    Aviso_235JB(Msg_235JB("msg_ListaVacia", "La lista de espera está vacía."),
                        "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                    return;
                }

                Pedido pedido_235JB = _bllPedido_235JB.ObtenerPedidoConfirmadoSinAtencion_235JB(primero_235JB.DNI);
                if (pedido_235JB == null)
                {
                    _bllAtencion_235JB.QuitarDeListaDeEspera_235JB(false, primero_235JB.DNI);
                    Aviso_235JB(Msg_235JB("msg_SinPedidoPendiente", "El cliente de la lista de espera no tiene un pedido confirmado pendiente."),
                        "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                    return;
                }

                if (!MesaPorPedido_235JB.TryGetValue(pedido_235JB.CodigoPedido, out int mesa_235JB))
                {
                    
                    if (Preguntar_235JB(Msg_235JB("msg_MesaNoRecuperada", "No se pudo recuperar la mesa de este cliente (se pierde al cerrar el sistema). ¿Desea cancelar su pedido?"),
                            "msg_ListaEspera_Titulo", "Lista de espera"))
                    {
                        _bllPedido_235JB.CancelarPedido_235JB(pedido_235JB.CodigoPedido);

                        // Su mesa se habia perdido: se liberan las mesas ocupadas que ya no pertenecen a ningun pedido
                        List<int> liberadas_235JB = BarrerMesasHuerfanas_235JB();
                        string mensaje_235JB = Msg_235JB("msg_PedidoCancelado", "Pedido cancelado.");
                        if (liberadas_235JB.Count > 0)
                            mensaje_235JB += " " + TextoMesasLiberadas_235JB(liberadas_235JB);

                        Aviso_235JB(mensaje_235JB, "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                        CargarMozos_235JB();
                    }
                    return;
                }

                Cliente = _bllCliente_235JB.ObtenerPorDNI_235JB(primero_235JB.DNI);
                Pedido = pedido_235JB;
                NumeroMesa = mesa_235JB;
                Resumen = _bllAtencion_235JB.AsignarMozo_235JB(mozo_235JB.DNIMozo, pedido_235JB, mesa_235JB, PedidosConPrioridad_235JB.Contains(pedido_235JB.CodigoPedido));
                _bllAtencion_235JB.QuitarDeListaDeEspera_235JB(false, primero_235JB.DNI);
                MesaPorPedido_235JB.Remove(pedido_235JB.CodigoPedido);
                PedidosConPrioridad_235JB.Remove(pedido_235JB.CodigoPedido);

                MostrarEncabezado_235JB();
                MostrarResumenAtencion_235JB(Resumen);
                Finalizar_235JB();
            }
            catch (InvalidOperationException)
            {
                Aviso_235JB(Msg_235JB("msg_MozoNoDisponible", "El mozo seleccionado ya no está disponible."),
                    "msg_Aviso_Titulo", "Aviso", MessageBoxIcon.Warning);
                CargarMozos_235JB();
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorAsignarMozo", "Error al asignar el mozo: ", ex_235JB);
            }
        }

        // CU06 paso 5: resumen de atencion (pedido, mesa y mozo)
        private void MostrarResumenAtencion_235JB(ResumenAtencion resumen_235JB)
        {
            string texto_235JB = string.Format(
                Msg_235JB("msg_ResumenAtencion", "Resumen de atención - Pedido: {0} | Mesa: {1} (capacidad {2}) | Mozo: {3} {4}"),
                resumen_235JB.CodigoPedido, resumen_235JB.NumeroMesa, resumen_235JB.Capacidad,
                resumen_235JB.NombreMozo, resumen_235JB.ApellidoMozo);
            AgregarResumen_235JB(texto_235JB);
            Aviso_235JB(texto_235JB, "msg_Exito_Titulo", "Éxito", MessageBoxIcon.Information);
        }

        private void Finalizar_235JB()
        {
            _pendiente_235JB = DialogResult.OK;
            _etapa_235JB = Etapa.Final;
            AplicarEtapa_235JB();
            CargarMesas_235JB();
            CargarMozos_235JB();
        }

        private void btnListaEspera_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            RegistrarEnListaDeEspera_235JB(_etapa_235JB == Etapa.Mesa);
        }

        // CU05 y CU06 flujo 1.1: consulta si el cliente quiere esperar; si no acepta -> flujo 1.3
        private void OfrecerListaDeEspera_235JB(bool mesas_235JB)
        {
            string pregunta_235JB = mesas_235JB
                ? Msg_235JB("msg_SinMesas", "No hay mesas libres. ¿El cliente desea ingresar a la lista de espera de mesas?")
                : Msg_235JB("msg_SinMozos", "No hay mozos disponibles. ¿El cliente desea ingresar a la lista de espera de mozos?");

            if (Preguntar_235JB(pregunta_235JB, "msg_ListaEspera_Titulo", "Lista de espera"))
                RegistrarEnListaDeEspera_235JB(mesas_235JB);
            else if (Preguntar_235JB(Msg_235JB("msg_NoAceptaEspera", "El cliente no ingresará a la lista de espera. ¿Desea cancelar el pedido?"),
                         "msg_ListaEspera_Titulo", "Lista de espera"))
                CancelarPedidoActual_235JB();
        }

        // CU05 y CU06 flujo 1.1: registra al cliente en la lista (DNI, nombre, apellido, fecha y hora)
        private void RegistrarEnListaDeEspera_235JB(bool mesas_235JB)
        {
            if (Cliente == null || Pedido == null)
                return;

            try
            {
                _bllAtencion_235JB.RegistrarEnListaDeEspera_235JB(mesas_235JB, Cliente);
                if (!mesas_235JB && NumeroMesa.HasValue)
                    MesaPorPedido_235JB[Pedido.CodigoPedido] = NumeroMesa.Value;
                if (!mesas_235JB && ConPrioridad)
                    PedidosConPrioridad_235JB.Add(Pedido.CodigoPedido);

                // Con la mesa de este pedido ya guardada en memoria, una mesa anterior que se habia perdido queda sin duenio
                if (!mesas_235JB)
                    LiberarMesasHuerfanas_235JB();

                Aviso_235JB(mesas_235JB
                        ? Msg_235JB("msg_EnEsperaMesas", "El cliente quedó registrado en la lista de espera de mesas.")
                        : Msg_235JB("msg_EnEsperaMozos", "El cliente quedó registrado en la lista de espera de mozos."),
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
                Error_235JB("msg_ErrorListaEspera", "Error al registrar en la lista de espera: ", ex_235JB);
            }
        }

        private void btnCancelarPedido_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            if (Pedido == null)
                return;

            if (Preguntar_235JB(Msg_235JB("msg_ConfirmarCancelar", "¿Desea cancelar el pedido? Se devolverá el stock."),
                    "msg_Confirmar_Titulo", "Confirmar"))
                CancelarPedidoActual_235JB();
        }

        
        // CU05 y CU06 flujo 1.3 (CU01 12.3): cancela el pedido, devuelve stock y libera la mesa
        private void CancelarPedidoActual_235JB()
        {
            if (Pedido == null)
                return;

            try
            {
                _bllPedido_235JB.CancelarPedido_235JB(Pedido.CodigoPedido);
                if (NumeroMesa.HasValue)
                    _bllAtencion_235JB.LiberarMesa_235JB(NumeroMesa.Value);
                MesaPorPedido_235JB.Remove(Pedido.CodigoPedido);
                PedidosConPrioridad_235JB.Remove(Pedido.CodigoPedido);
                PedidoCancelado = true;

                // Si la mesa de este pedido se habia perdido (se retomo despues de cerrar el sistema), no se libero arriba:
                // se liberan las mesas ocupadas que ya no pertenecen a ningun pedido
                List<int> liberadas_235JB = BarrerMesasHuerfanas_235JB();
                string mensaje_235JB = Msg_235JB("msg_PedidoCancelado", "Pedido cancelado.");
                if (liberadas_235JB.Count > 0)
                    mensaje_235JB += " " + TextoMesasLiberadas_235JB(liberadas_235JB);

                Aviso_235JB(mensaje_235JB, "msg_Info_Titulo", "Información", MessageBoxIcon.Information);
                Terminar_235JB(DialogResult.Cancel);
            }
            catch (Exception ex_235JB)
            {
                Error_235JB("msg_ErrorCancelar", "No se pudo cancelar el pedido: ", ex_235JB);
            }
        }

        private void btnContinuar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(DialogResult.OK);
        }

        private void btnCerrar_Click_235JB(object sender_235JB, EventArgs e_235JB)
        {
            Terminar_235JB(_pendiente_235JB);
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
