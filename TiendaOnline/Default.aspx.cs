using dominio;
using negocio;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiendaOnline
{
    public partial class Default : System.Web.UI.Page
    {
        // Lista EN EL NAMESPACE CORRECTO (dominio.Articulos)
        public List<dominio.Articulos> ListaArticulos { get; set; }

        // Servicios
        private readonly ArticulosNegocio _artNeg = new ArticulosNegocio();
        private readonly EmailService _email = new EmailService();

        // Límite práctico de adjuntos (~25MB SMTP Gmail/Workspace)
        private static readonly long MAX_ATTACHMENT_BYTES = 24 * 1024 * 1024;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarCombos();
                    BindArticulos();
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignorar redirecciones
            }
            catch (Exception ex)
            {
                LogError(ex, "Page_Load");
                SetMsg("Ocurrió un problema al cargar la página.", isOk: false);
            }
        }

        private void CargarCombos()
        {
            try
            {
                var marcaNeg = new MarcasNegocio();
                var catNeg = new CategoriasNegocio();

                // Marcas
                ddlMarca.DataSource = marcaNeg.listar();
                ddlMarca.DataTextField = "Descripcion";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();
                ddlMarca.Items.Insert(0, new ListItem("-- Todas --", ""));

                // Categorías
                ddlCategoria.DataSource = catNeg.listar();
                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();
                ddlCategoria.Items.Insert(0, new ListItem("-- Todas --", ""));
            }
            catch (Exception ex)
            {
                LogError(ex, "CargarCombos");

                // Fallback visual
                ddlMarca.Items.Clear();
                ddlMarca.Items.Insert(0, new ListItem("-- Todas --", ""));
                ddlCategoria.Items.Clear();
                ddlCategoria.Items.Insert(0, new ListItem("-- Todas --", ""));

                SetMsg("No se pudieron cargar los filtros de marca/categoría.", isOk: false);
            }
        }

        private void BindArticulos()
        {
            try
            {
                string texto = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim();

                int? idMarca = null;
                if (!string.IsNullOrEmpty(ddlMarca.SelectedValue) &&
                    int.TryParse(ddlMarca.SelectedValue, out int idM))
                    idMarca = idM;

                int? idCat = null;
                if (!string.IsNullOrEmpty(ddlCategoria.SelectedValue) &&
                    int.TryParse(ddlCategoria.SelectedValue, out int idC))
                    idCat = idC;

                // usar buscar(...) si hay criterios; si no, listar()
                List<dominio.Articulos> datos;
                if (texto != null || idMarca.HasValue || idCat.HasValue)
                    datos = _artNeg.buscar(texto, idMarca, idCat);
                else
                    datos = _artNeg.listar();

                // Evitamos '??' por el error CS0019 que tenías
                if (datos == null)
                    datos = new List<dominio.Articulos>();

                ListaArticulos = datos;

                // IMPORTANT: DataSource debe ser IEnumerable<dominio.Articulos>
                repArticulos.DataSource = ListaArticulos;
                repArticulos.DataBind();
            }
            catch (Exception ex)
            {
                LogError(ex, "BindArticulos");

                // Evita romper la página si falla DB/búsqueda
                ListaArticulos = new List<dominio.Articulos>();
                repArticulos.DataSource = ListaArticulos;
                repArticulos.DataBind();

                SetMsg("No se pudo cargar el catálogo.", isOk: false);
            }
        }

        // =========================
        //  ENVÍO POR MAIL (modal)
        // =========================

        protected bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new MailAddress(email.Trim());
                return addr.Address == email.Trim();
            }
            catch { return false; }
        }

        protected void EnsureAttachmentSize(params byte[][] payloads)
        {
            long total = payloads.Where(p => p != null).Sum(p => (long)p.Length);
            if (total > MAX_ATTACHMENT_BYTES)
                throw new InvalidOperationException(
                    $"El adjunto supera el límite permitido (~{total / (1024 * 1024)} MB). Reducí el tamaño o exportá por partes."
                );
        }

        private void EnviarExcelA(string to)
        {
            var lista = ObtenerListaParaExport();
            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("No hay artículos para exportar.");

            // Generar XLSX
            byte[] bytesXlsx = ReportService.CatalogoArticulosXlsx(lista, "Catálogo de Artículos — Tienda Otani");
            EnsureAttachmentSize(bytesXlsx);

            string fecha = DateTime.Now.ToString("yyyyMMdd");
            var adj = new AttachmentDto
            {
                FileName = $"catalogo_otani_{fecha}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                Content = bytesXlsx
            };

            _email.ArmarCorreoConAdjuntos(
                emailDestino: to,
                asunto: $"Catálogo de Artículos (Excel) - {DateTime.Now:yyyy-MM-dd}",
                cuerpoHtml: "<p>Adjuntamos el catálogo completo en formato Excel (.xlsx) con Código / Descripción / Precio.</p>",
                adjuntos: new List<AttachmentDto> { adj }
            );

            _email.EnviarEmail();
        }


        private void EnviarPdfA(string to)
        {
            var lista = ObtenerListaParaExport();
            if (lista == null || lista.Count == 0)
                throw new InvalidOperationException("No hay artículos para exportar.");

            // URL del logo (podés reemplazar por tu Azure Blob si querés)
            string logoUrl = "https://otanistorageimg.blob.core.windows.net/imagenes/logoOtani2.png";

            // Generar PDF
            byte[] bytesPdf = ReportService.CatalogoArticulosPdf(lista, "Catálogo de Artículos — Tienda Otani", logoUrl);
            EnsureAttachmentSize(bytesPdf);

            string fecha = DateTime.Now.ToString("yyyyMMdd");
            var adj = new AttachmentDto
            {
                FileName = $"catalogo_otani_{fecha}.pdf",
                ContentType = "application/pdf",
                Content = bytesPdf
            };

            _email.ArmarCorreoConAdjuntos(
                emailDestino: to,
                asunto: $"Catálogo de Artículos (PDF) - {DateTime.Now:yyyy-MM-dd}",
                cuerpoHtml: "<p>Adjuntamos el catálogo en PDF con encabezado institucional y espacio de logo.</p>",
                adjuntos: new List<AttachmentDto> { adj }
            );

            _email.EnviarEmail();
        }

        private List<dominio.Articulos> ObtenerListaParaExport()
        {
            // Si guardás ListaArticulos en la página, podrías usarla para evitar re-consulta:
            // if (ListaArticulos != null && ListaArticulos.Count > 0) return ListaArticulos;
            return _artNeg.listarParaExport(); // => debe retornar List<dominio.Articulos>
        }

        // ========= Handlers UI =========

        protected async void btnConfirmEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                // Valida controles del grupo vgEnviar
                Page.Validate("vgEnviar");
                if (!Page.IsValid)
                {
                    // Mantener el modal abierto si hay error de validación
                    ScriptManager.RegisterStartupScript(this, GetType(), "reopenModal",
                        "var m=new bootstrap.Modal(document.getElementById('modalEmail')); m.show();", true);
                    return;
                }

                string to = txtEmailDestino.Text?.Trim();
                if (!IsValidEmail(to))
                {
                    SetMsg("El email ingresado no es válido.", isOk: false);

                    // Reabrir modal para que el usuario corrija
                    ScriptManager.RegisterStartupScript(this, GetType(), "reopenModal",
                        "var m=new bootstrap.Modal(document.getElementById('modalEmail')); m.show();", true);
                    return;
                }

                string tipo = hfTipoEnvio.Value;

                await Task.Run(() =>
                {
                    if (tipo == "excel") EnviarExcelA(to);
                    else if (tipo == "htmlpdf") EnviarPdfA(to);
                    else throw new InvalidOperationException("Tipo de envío no reconocido.");
                });

                SetMsg(tipo == "excel"
                    ? "📧 Enviado: catálogo en Excel (.xlsx)."
                    : "📧 Enviado: catálogo en PDF.", isOk: true);
            }
            catch (Exception ex)
            {
                LogError(ex, "btnConfirmEnviar_Click");
                SetMsg("No se pudo enviar el correo.", isOk: false);
            }
        }


        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                BindArticulos();
            }
            catch (Exception ex)
            {
                LogError(ex, "btnFiltrar_Click");
                SetMsg("Ocurrió un error al filtrar.", isOk: false);
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscar.Text = string.Empty;
                if (ddlMarca.Items.Count > 0) ddlMarca.ClearSelection();
                if (ddlCategoria.Items.Count > 0) ddlCategoria.ClearSelection();

                BindArticulos();
            }
            catch (Exception ex)
            {
                LogError(ex, "btnLimpiar_Click");
                SetMsg("Ocurrió un error al limpiar filtros.", isOk: false);
            }
        }

        // =========================
        // Helpers de UI y Log
        // =========================
        protected void SetMsg(string text, bool isOk)
        {
            lblMsg.CssClass = isOk
                ? "alert alert-success"
                : "alert alert-danger";
            lblMsg.Text = text;
        }

        protected void LogError(Exception ex, string where)
        {
            System.Diagnostics.Trace.TraceError($"[{where}] {ex}");
        }
    }
}
