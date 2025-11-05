using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Favoritos
    {
        
        public int Id { get; set; }         // FAVORITOS.Id
        public int IdUser { get; set; }     // FAVORITOS.IdUser -> USERS.Id
        public int IdArticulo { get; set; } // FAVORITOS.IdArticulo -> ARTICULOS.Id

       
        public Articulos Articulo { get; set; }
    }
}
