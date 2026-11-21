using BE;
using DAL;
using SERVICIOS;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace BLL
{
    public class BLLCliente
    {
        private readonly DALCliente _dal_235JB = new DALCliente();
        private readonly BLLIntegridad _integridad_235JB = new BLLIntegridad();
        private readonly BLLEvento _evento_235JB = new BLLEvento();
        private readonly DALPedido _pedidoDAL_235JB = new DALPedido();
        private readonly EncriptacionReversible _cripto_235JB = new EncriptacionReversible();
        private readonly Serializador _serializador_235JB = new Serializador();

        private static string UsuarioActual_235JB()
        {
            return Singleton.Instancia.Usuario != null ? Singleton.Instancia.Usuario.NombreUsuario : "sistema";
        }

        
        public DataTable Listar_235JB()
        {
            DataTable tabla_235JB = _dal_235JB.Listar_235JB();
            foreach (DataRow fila_235JB in tabla_235JB.Rows)
                fila_235JB["Domicilio"] = _cripto_235JB.Desencriptar_235JB(fila_235JB["Domicilio"] as string);
            return tabla_235JB;
        }

        public Cliente ObtenerPorDNI_235JB(int dni_235JB)
        {
            Cliente cliente_235JB = _dal_235JB.ObtenerPorDNI_235JB(dni_235JB);
            if (cliente_235JB != null)
                cliente_235JB.Domicilio = _cripto_235JB.Desencriptar_235JB(cliente_235JB.Domicilio);
            return cliente_235JB;
        }

        
        private Cliente ConDomicilioEncriptado_235JB(Cliente cliente_235JB)
        {
            return new Cliente
            {
                DNI = cliente_235JB.DNI,
                Nombre = cliente_235JB.Nombre,
                Apellido = cliente_235JB.Apellido,
                Mail = cliente_235JB.Mail,
                NumTelefono = cliente_235JB.NumTelefono,
                Domicilio = _cripto_235JB.Encriptar_235JB(cliente_235JB.Domicilio),
                Activo = cliente_235JB.Activo,
                CodigoCliente = cliente_235JB.CodigoCliente,
                FechaAlta = cliente_235JB.FechaAlta
            };
        }

        
        public void Registrar_235JB(Cliente cliente_235JB)
        {
            if (cliente_235JB == null) throw new ArgumentNullException("cliente");
            if (_dal_235JB.ObtenerPorDNI_235JB(cliente_235JB.DNI) != null)
                throw new InvalidOperationException("Ya existe un cliente con ese DNI.");

            
            cliente_235JB.FechaAlta = DateTime.Today;
            Cliente aGuardar_235JB = ConDomicilioEncriptado_235JB(cliente_235JB);
            _dal_235JB.Insertar_235JB(aGuardar_235JB);
            cliente_235JB.CodigoCliente = aGuardar_235JB.CodigoCliente;
            _integridad_235JB.SincronizarFilasNuevas("Cliente");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de cliente " + cliente_235JB.DNI, "Maestros", "Baja");
        }

        
        public bool Modificar_235JB(Cliente cliente_235JB)
        {
            if (cliente_235JB == null) throw new ArgumentNullException("cliente");
            Cliente actual_235JB = ObtenerPorDNI_235JB(cliente_235JB.DNI);
            if (actual_235JB == null)
                throw new InvalidOperationException("No existe un cliente con ese DNI.");
            if (actual_235JB.Nombre == cliente_235JB.Nombre && actual_235JB.Apellido == cliente_235JB.Apellido && actual_235JB.Mail == cliente_235JB.Mail &&
                actual_235JB.NumTelefono == cliente_235JB.NumTelefono && actual_235JB.Domicilio == cliente_235JB.Domicilio && actual_235JB.Activo == cliente_235JB.Activo)
                return false;

            if (!cliente_235JB.Activo) VerificarSinPedidoEnCurso_235JB(cliente_235JB.DNI);

            _dal_235JB.Modificar_235JB(ConDomicilioEncriptado_235JB(cliente_235JB));
            _integridad_235JB.SincronizarFilaPorClave("Cliente", cliente_235JB.DNI);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Modificacion de cliente " + cliente_235JB.DNI, "Maestros", "Media");
            return true;
        }

        private void VerificarSinPedidoEnCurso_235JB(int dni_235JB)
        {
            if (_pedidoDAL_235JB.ObtenerPedidoEnCurso_235JB(dni_235JB) != null)
                throw new InvalidOperationException("msg_ClienteEnUso");
        }

        
        
        public Cliente ConsultarRegistro_235JB(int dni_235JB)
        {
            Cliente cliente_235JB = ObtenerPorDNI_235JB(dni_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Consulta de registro del DNI " + dni_235JB, "Reservas", "Baja");
            return cliente_235JB;
        }

        
        public string ValidarDatos_235JB(Cliente cliente_235JB)
        {
            const string patronNombre_235JB = @"^[A-Za-z\u00C0-\u00FF]+(\s[A-Za-z\u00C0-\u00FF]+)*$";

            if (cliente_235JB == null || cliente_235JB.DNI <= 0 || cliente_235JB.NumTelefono <= 0 ||
                string.IsNullOrWhiteSpace(cliente_235JB.Nombre) || string.IsNullOrWhiteSpace(cliente_235JB.Apellido) ||
                string.IsNullOrWhiteSpace(cliente_235JB.Domicilio) || string.IsNullOrWhiteSpace(cliente_235JB.Mail))
                return "msg_CamposObligatorios";
            if (!Regex.IsMatch(cliente_235JB.DNI.ToString(), @"^\d{8}$")) return "msg_DniInvalido";
            if (!Regex.IsMatch(cliente_235JB.Nombre.Trim(), patronNombre_235JB)) return "msg_NombreInvalido";
            if (!Regex.IsMatch(cliente_235JB.Apellido.Trim(), patronNombre_235JB)) return "msg_ApellidoInvalido";
            if (!Regex.IsMatch(cliente_235JB.Mail.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) return "msg_MailInvalido";
            if (!Regex.IsMatch(cliente_235JB.NumTelefono.ToString(), @"^\d{6,10}$")) return "msg_TelefonoInvalido";
            return null;
        }

        
        public bool IngresarDatos_235JB(Cliente cliente_235JB, out string claveError_235JB)
        {
            claveError_235JB = ValidarDatos_235JB(cliente_235JB);
            if (claveError_235JB != null) return false;

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Ingreso de datos del cliente " + cliente_235JB.DNI, "Reservas", "Baja");
            return true;
        }
        
        private static string CarpetaSerializadosRuta_235JB()
        {
            string documentos_235JB = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (string.IsNullOrWhiteSpace(documentos_235JB))
                documentos_235JB = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(documentos_235JB, "CAFECCINO", "Serializados");
        }

        private static string RutaSerializado_235JB(int dni_235JB, string formato_235JB)
        {
            return Path.Combine(CarpetaSerializadosRuta_235JB(), "Cliente_" + dni_235JB + "." + formato_235JB.ToLowerInvariant());
        }

        
        public string NombreArchivoSerializado_235JB(int dni_235JB, string formato_235JB)
        {
            return Path.GetFileName(RutaSerializado_235JB(dni_235JB, formato_235JB));
        }

        
        
        public string Serializar_235JB(int dni_235JB, string formato_235JB, string ruta_235JB, out string texto_235JB)
        {
            string formato_NORM_235JB = _serializador_235JB.NormalizarFormato_235JB(formato_235JB);
            Cliente cliente_235JB = ObtenerPorDNI_235JB(dni_235JB);
            if (cliente_235JB == null)
                throw new InvalidOperationException("No existe un cliente con ese DNI.");

            texto_235JB = _serializador_235JB.Serializar_235JB(cliente_235JB, formato_NORM_235JB);
            if (string.IsNullOrWhiteSpace(ruta_235JB))
                ruta_235JB = RutaSerializado_235JB(dni_235JB, formato_NORM_235JB);
            Directory.CreateDirectory(Path.GetDirectoryName(ruta_235JB));
            File.WriteAllText(ruta_235JB, texto_235JB, new System.Text.UTF8Encoding(false));

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Serializacion de cliente " + dni_235JB + " (" + formato_NORM_235JB + ")", "Maestros", "Baja");
            return ruta_235JB;
        }

        
        public string CarpetaSerializados_235JB()
        {
            string carpeta_235JB = CarpetaSerializadosRuta_235JB();
            Directory.CreateDirectory(carpeta_235JB);
            return carpeta_235JB;
        }

        
        public Cliente Deserializar_235JB(string ruta_235JB, string formato_235JB, out string texto_235JB)
        {
            string formato_NORM_235JB = _serializador_235JB.NormalizarFormato_235JB(formato_235JB);
            if (string.IsNullOrWhiteSpace(ruta_235JB) || !File.Exists(ruta_235JB))
                throw new FileNotFoundException("No existe el archivo seleccionado.", ruta_235JB);

            texto_235JB = File.ReadAllText(ruta_235JB);
            Cliente cliente_235JB = _serializador_235JB.Deserializar_235JB<Cliente>(texto_235JB, formato_NORM_235JB);

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Deserializacion de cliente " + cliente_235JB.DNI + " (" + formato_NORM_235JB + ")", "Maestros", "Baja");
            return cliente_235JB;
        }
        public void Desactivar_235JB(int dni_235JB)
        {
            VerificarSinPedidoEnCurso_235JB(dni_235JB);
            _dal_235JB.Desactivar_235JB(dni_235JB);
            _integridad_235JB.SincronizarFilaPorClave("Cliente", dni_235JB);
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Baja logica de cliente " + dni_235JB, "Maestros", "Alta");
        }
    }
}