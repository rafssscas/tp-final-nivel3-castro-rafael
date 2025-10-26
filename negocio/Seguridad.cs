using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public static class Seguridad
    {
        // La sesión está activa si hay un Usuario y su Id != 0
        public static bool sesionActiva(object user)
        {
            var u = user as Users;
            return u != null && u.Id != 0;
        }

        // Devuelve true si el usuario logueado tiene flag Admin = true
        public static bool esAdmin(object user)
        {
            var u = user as Users;
            return u != null && u.Admin;
        }
    }
}
