using BE;
using DAL;
using SERVICIOS;
using System;
using System.Data;
using System.IO;

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

        // El domicilio se guarda encriptado (AES): aca se desencripta para mostrarlo.
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

        // Copia del cliente con el domicilio encriptado, para guardar (el objeto original queda en texto plano).
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
                Activo = cliente_235JB.Activo
            };
        }

        
        public void Registrar_235JB(Cliente cliente_235JB)
        {
            if (cliente_235JB == null) throw new ArgumentNullException("cliente");
            if (_dal_235JB.ObtenerPorDNI_235JB(cliente_235JB.DNI) != null)
                throw new InvalidOperationException("Ya existe un cliente con ese DNI.");

            _dal_235JB.Insertar_235JB(ConDomicilioEncriptado_235JB(cliente_235JB));
            _integridad_235JB.SincronizarFilasNuevas("Cliente");
            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Alta de cliente " + cliente_235JB.DNI, "Maestros", "Baja");
        }

        // Devuelve false si los datos son iguales a los guardados (no se modifica nada).
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
                throw new EntidadEnUsoException("El cliente tiene un pedido en curso.");
        }

        
        // ------------------------- Serializacion (XML / JSON) -------------------------
        // Los archivos se guardan en la subcarpeta "Serializados" de esta carpeta (se crea si no existe).
        private const string CarpetaProyecto_235JB = @"C:\Users\joaquim\Desktop\Proyecto-Ingenieria-de-Software\Proyecto-Ingenieria-de-Software";

        private static string RutaSerializado_235JB(int dni_235JB, string formato_235JB)
        {
            string carpeta_235JB = Path.Combine(CarpetaProyecto_235JB, "Serializados");
            return Path.Combine(carpeta_235JB, "Cliente_" + dni_235JB + "." + formato_235JB.ToLowerInvariant());
        }

        // Serializa el cliente al formato indicado, lo guarda en un archivo y devuelve la ruta. El texto queda en texto_235JB.
        public string Serializar_235JB(int dni_235JB, string formato_235JB, out string texto_235JB)
        {
            string formato_NORM_235JB = _serializador_235JB.NormalizarFormato_235JB(formato_235JB);
            Cliente cliente_235JB = ObtenerPorDNI_235JB(dni_235JB);
            if (cliente_235JB == null)
                throw new InvalidOperationException("No existe un cliente con ese DNI.");

            texto_235JB = _serializador_235JB.Serializar_235JB(cliente_235JB, formato_NORM_235JB);
            string ruta_235JB = RutaSerializado_235JB(dni_235JB, formato_NORM_235JB);
            Directory.CreateDirectory(Path.GetDirectoryName(ruta_235JB));
            File.WriteAllText(ruta_235JB, texto_235JB, new System.Text.UTF8Encoding(false));

            _evento_235JB.RegistrarEvento(UsuarioActual_235JB(), "Serializacion de cliente " + dni_235JB + " (" + formato_NORM_235JB + ")", "Maestros", "Baja");
            return ruta_235JB;
        }

        // Carpeta donde se guardan los archivos serializados (para que el dialogo de busqueda arranque ahi).
        public string CarpetaSerializados_235JB()
        {
            return Path.Combine(CarpetaProyecto_235JB, "Serializados");
        }

        // Lee el archivo elegido por el usuario y lo vuelve a armar. No modifica la base de datos.
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