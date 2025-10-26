using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Favoritos
    {
        // Tabla: FAVORITOS (ON DELETE CASCADE hacia USERS y ARTICULOS)
        public int Id { get; set; }         // FAVORITOS.Id
        public int IdUser { get; set; }     // FAVORITOS.IdUser -> USERS.Id
        public int IdArticulo { get; set; } // FAVORITOS.IdArticulo -> ARTICULOS.Id

        // Navegación para el binding en la vista
        public Articulos Articulo { get; set; }
    }
}
