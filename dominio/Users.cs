using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Users
    {

        // Tabla: USERS (columnas en minúscula en DB)
        public int Id { get; set; }                 // USERS.Id
        public string Email { get; set; }           // USERS.email
        public string Pass { get; set; }            // USERS.pass (para TP: texto plano)
        public string Nombre { get; set; }          // USERS.nombre
        public string Apellido { get; set; }        // USERS.apellido
        public string UrlImagenPerfil { get; set; } // USERS.urlImagenPerfil
        public bool Admin { get; set; }             // USERS.admin

        public override string ToString() => $"{Nombre} {Apellido} <{Email}>";
    }
}
