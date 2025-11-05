using ClosedXML.Excel;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet.Core;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using PageSize = iTextSharp.text.PageSize;
using Paragraph = iTextSharp.text.Paragraph;
using Rectangle = iTextSharp.text.Rectangle;

namespace negocio
{
    public static class ReportService
    {
        // =====================
        //   EXCEL (.xlsx)
        // =====================
        public static byte[] CatalogoArticulosXlsx(IEnumerable<dominio.Articulos> items, string titulo = "Catálogo de Artículos")
        {
            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Catálogo");

                // Encabezado
                ws.Cell("A1").Value = titulo;
                ws.Cell("A2").Value = $"Fecha: {DateTime.Now:dd/MM/yyyy}";
                ws.Range("A1:C1").Merge().Style
                    .Font.SetBold().Font.SetFontSize(14);
                ws.Range("A2:C2").Merge().Style
                    .Font.SetItalic().Font.SetFontColor(XLColor.Gray);

                // Tabla
                ws.Cell("A4").Value = "Código";
                ws.Cell("B4").Value = "Descripción";
                ws.Cell("C4").Value = "Precio";

                var header = ws.Range("A4:C4");
                header.Style.Fill.SetBackgroundColor(XLColor.FromHtml("#F1F5F9"));
                header.Style.Font.SetBold();
                header.Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);

                int row = 5;
                foreach (var a in items ?? Enumerable.Empty<dominio.Articulos>())
                {
                    // Detecta campo de código disponible (CodigoBarra o Codigo o Id)
                    string codigo = a.GetType().GetProperty("CodigoBarra") != null
                                    ? Convert.ToString(a.GetType().GetProperty("CodigoBarra").GetValue(a))
                                    : a.GetType().GetProperty("Codigo") != null
                                        ? Convert.ToString(a.GetType().GetProperty("Codigo").GetValue(a))
                                        : a.Id.ToString();

                    ws.Cell(row, 1).Value = codigo;
                    ws.Cell(row, 2).Value = a.Nombre ?? a.Descripcion ?? "(Sin descripción)";
                    ws.Cell(row, 3).Value = a.Precio.HasValue ? (double)a.Precio.Value : 0d;
                    ws.Cell(row, 3).Style.NumberFormat.Format = "$ #,##0.00";
                    row++;
                }

                // Estética
                ws.Columns("A:C").AdjustToContents();
                ws.SheetView.FreezeRows(4);

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        // =====================
        //        PDF
        // =====================
        public static byte[] CatalogoArticulosPdf(IEnumerable<dominio.Articulos> items,string titulo = "Catálogo de Artículos", string logoUrlOpcional = null)
        {
            using (var ms = new MemoryStream())
            {
                // Tamaño A4 y márgenes cómodos
                var doc = new Document(PageSize.A4, 36, 36, 90, 36);
                var writer = PdfWriter.GetInstance(doc, ms);

                // Encabezado (PageEvent)
                writer.PageEvent = new PdfHeaderEvent(titulo, logoUrlOpcional);

                doc.Open();

                // Fuente para tabla
                var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);
                var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

                // Título dentro del documento (además del header)
                var p = new Paragraph(titulo, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14));
                p.Alignment = Element.ALIGN_CENTER;
                p.SpacingAfter = 10f;
                doc.Add(p);

                // Tabla: Código / Descripción / Precio
                var table = new PdfPTable(3) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 22f, 58f, 20f });

                // Header row
                AddHeaderCell(table, "Código", fontHeader);
                AddHeaderCell(table, "Descripción", fontHeader);
                AddHeaderCell(table, "Precio", fontHeader);

                // Rows
                foreach (var a in items ?? Enumerable.Empty<dominio.Articulos>())
                {
                    string codigo = a.GetType().GetProperty("CodigoBarra") != null
                                    ? Convert.ToString(a.GetType().GetProperty("CodigoBarra").GetValue(a))
                                    : a.GetType().GetProperty("Codigo") != null
                                        ? Convert.ToString(a.GetType().GetProperty("Codigo").GetValue(a))
                                        : a.Id.ToString();

                    AddCell(table, codigo, fontCell);
                    AddCell(table, a.Nombre ?? a.Descripcion ?? "(Sin descripción)", fontCell);
                    AddCell(table, a.Precio.HasValue ? $"$ {a.Precio.Value:N2}" : "Consultar", fontCell, Element.ALIGN_RIGHT);
                }

                doc.Add(table);
                doc.Close();
                return ms.ToArray();
            }
        }

        private static void AddHeaderCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font))
            {
                BackgroundColor = new BaseColor(241, 245, 249), // slate-100
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 6f
            };
            table.AddCell(cell);
        }

        private static void AddCell(PdfPTable table, string text, Font font, int align = Element.ALIGN_LEFT)
        {
            var cell = new PdfPCell(new Phrase(text ?? "", font))
            {
                HorizontalAlignment = align,
                Padding = 5f
            };
            table.AddCell(cell);
        }

        // ======= Header con logo + título + fecha =======
        private class PdfHeaderEvent : PdfPageEventHelper
        {
            private readonly string _titulo;
            private readonly string _logoUrl;

            public PdfHeaderEvent(string titulo, string logoUrl)
            {
                _titulo = titulo;
                _logoUrl = logoUrl;
            }

            public override void OnEndPage(PdfWriter writer, Document document)
            {
                var cb = writer.DirectContent;
                var page = document.PageSize;

                // Área de header
                var headerTable = new PdfPTable(2) { TotalWidth = page.Width - document.LeftMargin - document.RightMargin };
                headerTable.SetWidths(new float[] { 20f, 80f });

                // Logo (si hay URL válida)
                PdfPCell logoCell;
                try
                {
                    if (!string.IsNullOrWhiteSpace(_logoUrl))
                    {
                        var img = Image.GetInstance(new Uri(_logoUrl));
                        img.ScaleAbsoluteHeight(48);
                        img.ScaleAbsoluteWidth(48);
                        logoCell = new PdfPCell(img, fit: false);
                    }
                    else
                    {
                        // Placeholder si no hay logo
                        logoCell = new PdfPCell(new Phrase("LOGO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.GRAY)));
                    }
                }
                catch
                {
                    logoCell = new PdfPCell(new Phrase("LOGO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.GRAY)));
                }
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                logoCell.PaddingRight = 10f;

                // Título + fecha
                var title = new Paragraph(_titulo, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12));
                var fecha = new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}", FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.GRAY));
                var titleCell = new PdfPCell();
                titleCell.AddElement(title);
                titleCell.AddElement(fecha);
                titleCell.Border = Rectangle.NO_BORDER;
                titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                headerTable.AddCell(logoCell);
                headerTable.AddCell(titleCell);

                headerTable.WriteSelectedRows(0, -1, document.LeftMargin, page.Top - 20, cb);
            }
        }
    }
}
