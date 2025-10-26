using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class ExportService
    {
        public byte[] ExportArticulosToCsv(IEnumerable<Articulos> items)
        {
            // Cabecera
            var sb = new StringBuilder();
            sb.AppendLine("Codigo;Descripcion;Stock;Precio");
            foreach (var a in items)
            {
                // Ajustá nombres de propiedades si en tu modelo son, p.ej., CodigoBarra / Nombre / PrecioFinal
                var codigo = GetSafe(a, "Codigo", "CodigoBarra");
                var desc = GetSafe(a, "Descripcion", "Nombre");
                var stock = GetSafe(a, "Stock");
                var precio = GetSafe(a, "Precio", "PrecioFinal", "PrecioVenta");
                sb.AppendLine($"{codigo};{San(desc)};{stock};{precio}");
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public byte[] ExportArticulosToExcelHtml(IEnumerable<Articulos> items)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<table><tr><th>Código</th><th>Descripción</th><th>Stock</th><th>Precio</th></tr>");
            foreach (var a in items)
            {
                var codigo = GetSafe(a, "Codigo", "CodigoBarra");
                var desc = GetSafe(a, "Descripcion", "Nombre");
                var stock = GetSafe(a, "Stock");
                var precio = GetSafe(a, "Precio", "PrecioFinal", "PrecioVenta");
                sb.AppendLine($"<tr><td>{codigo}</td><td>{San(desc)}</td><td>{stock}</td><td>{precio}</td></tr>");
            }
            sb.AppendLine("</table>");
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        // Stub educativo: por ahora mandamos HTML como base; luego lo cambiamos por RDLC/iTextSharp
        public byte[] ExportArticulosToPdf(IEnumerable<Articulos> items)
            => ExportArticulosToExcelHtml(items);

        private string San(string s) => (s ?? "").Replace(";", ",");
        private string GetSafe(Articulos a, params string[] props)
        {
            foreach (var p in props)
            {
                var pr = a.GetType().GetProperty(p);
                if (pr != null) return (pr.GetValue(a)?.ToString()) ?? "";
            }
            return "";
        }
    }
}
