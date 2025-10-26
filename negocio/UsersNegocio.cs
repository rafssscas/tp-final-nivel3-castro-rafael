using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{

    public class UsersNegocio
    {
        // LOGIN: completa el objeto recibido si ok
        public bool Login(Users user)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Users_Login");
                datos.setearParametro("@Nombre", user.Nombre);
                datos.setearParametro("@Pass", user.Pass);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    user.Id = (int)datos.Lector["Id"];
                    user.Email = datos.Lector["email"] as string;
                    user.Apellido = datos.Lector["apellido"] as string;
                    user.UrlImagenPerfil = datos.Lector["urlImagenPerfil"] as string;
                    user.Admin = (bool)datos.Lector["admin"];
                    return true;
                }
                return false;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // REGISTRAR: devuelve el Id nuevo (SCOPE_IDENTITY)
        public int Registrar(Users nuevo)
        {
            if (nuevo == null) throw new ArgumentNullException(nameof(nuevo));
            if (string.IsNullOrWhiteSpace(nuevo.Nombre)) throw new ArgumentException("Usuario (nombre) requerido.");
            if (string.IsNullOrWhiteSpace(nuevo.Pass)) throw new ArgumentException("Contraseña requerida.");

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Users_Registrar");
                datos.setearParametro("@Email", (object)nuevo.Email ?? DBNull.Value);
                datos.setearParametro("@Pass", nuevo.Pass); // (TP: texto plano)
                datos.setearParametro("@Nombre", (object)nuevo.Nombre ?? DBNull.Value);
                datos.setearParametro("@Apellido", (object)nuevo.Apellido ?? DBNull.Value);
                datos.setearParametro("@UrlImagenPerfil", (object)nuevo.UrlImagenPerfil ?? DBNull.Value);
                datos.setearParametro("@Admin", nuevo.Admin);

                return datos.ejecutarAccionScalar();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // OBTENER POR ID
        public Users ObtenerPorId(int id)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Users_ObtenerPorId");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    return new Users
                    {
                        Id = (int)datos.Lector["Id"],
                        Email = datos.Lector["email"] as string,
                        Nombre = datos.Lector["nombre"] as string,
                        Apellido = datos.Lector["apellido"] as string,
                        UrlImagenPerfil = datos.Lector["urlImagenPerfil"] as string,
                        Admin = (bool)datos.Lector["admin"]
                    };
                }
                return null;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // ACTUALIZAR PERFIL (nombre, apellido, imagen)
        public void ActualizarPerfil(Users user)
        {
            if (user == null || user.Id <= 0) throw new ArgumentException("Usuario inválido.");

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Users_ActualizarPerfil");
                datos.setearParametro("@Id", user.Id);
                datos.setearParametro("@Nombre", (object)user.Nombre ?? DBNull.Value);
                datos.setearParametro("@Apellido", (object)user.Apellido ?? DBNull.Value);
                datos.setearParametro("@UrlImagenPerfil", (object)user.UrlImagenPerfil ?? DBNull.Value);
                datos.ejecutarAccion();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public int InsertarNuevo(Users nuevo)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Users_InsertarNuevo"); // ver script más abajo
                datos.setearParametro("@Email", nuevo.Email);
                datos.setearParametro("@Pass", nuevo.Pass);
                return datos.ejecutarAccionScalar(); // debe devolver el ID
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public bool EmailYaRegistrado(string email)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT 1 FROM USERS WHERE email = @e");
                datos.setearParametro("@e", email);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public bool NombreYaRegistrado(string nombre)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("SELECT 1 FROM dbo.USERS WHERE nombre = @n");
                datos.setearParametro("@n", nombre);
                datos.ejecutarLectura();
                return datos.Lector.Read();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

    }
}
