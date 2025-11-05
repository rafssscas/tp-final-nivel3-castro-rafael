using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace dominio
{
    public class Articulos
    {
        // Tabla: ARTICULOS
        public int Id { get; set; }                 // ARTICULOS.Id
        public string Codigo { get; set; }          // ARTICULOS.Codigo 
        public string Nombre { get; set; }          // ARTICULOS.Nombre
        public string Descripcion { get; set; }     // ARTICULOS.Descripcion

        
        public int IdMarca { get; set; }            // ARTICULOS.IdMarca -> MARCAS.Id
        public int IdCategoria { get; set; }        // ARTICULOS.IdCategoria -> CATEGORIAS.Id

        public string ImagenUrl { get; set; }       // ARTICULOS.ImagenUrl
        public decimal? Precio { get; set; }        // ARTICULOS.Precio (money -> decimal?)

       
        public Marcas Marca { get; set; }
        public Categorias Categoria { get; set; }

        public override string ToString() => $"{Nombre} ({Codigo})";
    }
}
