using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DAL;
using SERVICIOS;

namespace BLL
{
    public class BLLUsuario
    {
        BLLPermiso _permisoBLL = new BLLPermiso();
        Encriptacion encriptacion = new Encriptacion();
        DALUsuario DALUsuario = new DALUsuario();
        private readonly BLLIntegridad _integridadBLL = new BLLIntegridad();

        public Usuario Login(Usuario user)
        {
            Usuario usuarioEncontrado = DALUsuario.ObtenerParaLogin(user.NombreUsuario);

            if (usuarioEncontrado == null)
                return null;

            if (!usuarioEncontrado.Activo)
                throw new Exception("El usuario está desactivado. Contacte al administrador.");

            if (usuarioEncontrado.Bloqueado)
                throw new Exception("El usuario está bloqueado por intentos fallidos. Contacte al administrador para que lo desbloquee.");

            string hashIngresado = encriptacion.GenerarHash(user.Contraseña);

            if (usuarioEncontrado.Contraseña != hashIngresado)
            {
                bool quedoBloqueado = DALUsuario.RegistrarIntentoFallido(usuarioEncontrado.DNI);

                if (quedoBloqueado)
                    throw new Exception("Contraseña incorrecta. El usuario fue bloqueado tras 3 intentos fallidos.");

                return null;
            }

            DALUsuario.ResetearIntentosFallidos(usuarioEncontrado.DNI);

            if (usuarioEncontrado.Perfil != null)
            {
                _permisoBLL.ArmarArbolComponente(usuarioEncontrado.Perfil);
            }

            return usuarioEncontrado;
        }
        public void RegistrarUsuario(Usuario usuario, int idPerfil)
        {
            usuario.Contraseña = encriptacion.GenerarHash(usuario.Contraseña);
            DALUsuario.RegistrarUsuario(usuario, idPerfil);

            _integridadBLL.SincronizarFilasNuevas("Usuario_Permiso");
        }
        public DataTable ListarUsuarios()
        {
            return DALUsuario.ListarUsuarios();
        }
        public void ModificarUsuario(Usuario usuario)
        {
            usuario.Contraseña = encriptacion.GenerarHash(usuario.Contraseña);
            DALUsuario.ModificarUsuario(usuario);
        }
        public void EliminarUsuario(int dni)
        {
            DALUsuario.EliminarUsuario(dni);

            _integridadBLL.SincronizarFilasNuevas("Usuario_Permiso");
        }
        private readonly DALUsuario _usuarioDAL = new DALUsuario();

        public void SincronizarDigitos()
        {
            _integridadBLL.SincronizarTabla("Usuario");
        }

        public bool ValidarIntegridad()
        {
            return !_integridadBLL.ObtenerTablasAlteradas().ContainsKey("Usuario");
        }

        public Usuario ObtenerPorDNI(int dni)
        {
            return _usuarioDAL.ObtenerPorDNI(dni);
        }

        public void ActualizarActivo(int dni, bool activo)
        {
            _usuarioDAL.ActualizarActivo(dni, activo);
            SincronizarDigitos();
        }

        public void Desbloquear(int dni)
        {
            _usuarioDAL.Desbloquear(dni);
            SincronizarDigitos();
        }

        public void CambiarContraseña(string nombreUsuario, string nuevaContraseña)
        {
            Usuario usuario = _usuarioDAL.ObtenerPorUsername(nombreUsuario);
            if (usuario == null)
                throw new Exception("No existe un usuario con ese nombre de usuario.");

            string hash = encriptacion.GenerarHash(nuevaContraseña);
            _usuarioDAL.ActualizarContraseña(nombreUsuario, hash);

            SincronizarDigitos();
        }
    }
}

