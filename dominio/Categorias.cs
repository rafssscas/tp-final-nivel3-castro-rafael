using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Categorias
    {
        // Tablas: MARCAS / CATEGORIAS
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public override string ToString() => Descripcion;
    }
}
