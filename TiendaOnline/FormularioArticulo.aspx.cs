using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiendaOnline
{
    public partial class FormularioArticulo : System.Web.UI.Page
    {
        private readonly MarcasNegocio marcaNeg = new MarcasNegocio();
        private readonly CategoriasNegocio catNeg = new CategoriasNegocio();
        private readonly ArticulosNegocio artNeg = new ArticulosNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Sólo Admin
                if (!Seguridad.esAdmin(Session["user"]))
                {
                    Session["returnUrl"] = Request.RawUrl;
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                {
                    CargarCombos();
                    CargarSiEdicion();
                }
            }
            
            catch (Exception ex)
            {
                LogError(ex, "Page_Load");
                MostrarMsg("Ocurrió un problema al cargar la página.", true);
            }
        }

        private void CargarCombos()
        {
            try
            {
                // Marcas
                ddlMarca.DataSource = marcaNeg.listar();
                ddlMarca.DataTextField = "Descripcion";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();
                ddlMarca.Items.Insert(0, new ListItem("-- Seleccione --", ""));

                // Categorías
                ddlCategoria.DataSource = catNeg.listar();
                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();
                ddlCategoria.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            }
            catch (Exception ex)
            {
                LogError(ex, "CargarCombos");
                MostrarMsg("No se pudieron cargar Marcas/Categorías. Reintentá en unos segundos.", true);
                ddlMarca.Items.Clear();
                ddlMarca.Items.Insert(0, new ListItem("-- Seleccione --", ""));
                ddlCategoria.Items.Clear();
                ddlCategoria.Items.Insert(0, new ListItem("-- Seleccione --", ""));
            }
        }

        private void CargarSiEdicion()
        {
            try
            {
                lblTitulo.Text = "Nuevo artículo";

                int id;
                if (!int.TryParse(Request.QueryString["id"], out id)) return;

                var art = artNeg.obtenerPorId(id);
                if (art == null)
                {
                    MostrarMsg("No se encontró el artículo.", true);
                    return;
                }

                lblTitulo.Text = "Editar artículo";
                hfId.Value = art.Id.ToString();

                txtCodigo.Text = art.Codigo;
                txtNombre.Text = art.Nombre;
                txtDescripcion.Text = art.Descripcion;

                // Selección segura de combos
                var itmMarca = ddlMarca.Items.FindByValue(art.IdMarca.ToString());
                if (itmMarca != null) ddlMarca.SelectedValue = itmMarca.Value;

                var itmCat = ddlCategoria.Items.FindByValue(art.IdCategoria.ToString());
                if (itmCat != null) ddlCategoria.SelectedValue = itmCat.Value;

                txtImagenUrl.Text = art.ImagenUrl;
                imgPreview.ImageUrl = string.IsNullOrWhiteSpace(art.ImagenUrl)
                    ? "https://placehold.co/800x600?text=Sin+Imagen"
                    : art.ImagenUrl;

                txtPrecio.Text = art.Precio.HasValue
                    ? art.Precio.Value.ToString("N2", new CultureInfo("es-AR"))
                    : string.Empty;
            }
            catch (Exception ex)
            {
                LogError(ex, "CargarSiEdicion");
                MostrarMsg("No se pudo cargar el artículo para edición.", true);
            }
        }


        // =============== Subida a Cloudinary (botón dedicado) ===============
        protected void btnSubirCloudinary_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fuImagen.HasFile)
                {
                    MostrarMsg("Seleccioná un archivo para subir.", true);
                    return;
                }

                if (!ValidarArchivoImagen(fuImagen))
                    return;

                string url, err;
                bool ok = CloudinaryService.UploadImage(fuImagen.PostedFile, out url, out err);

                if (!ok)
                {
                    // Mostramos el motivo real
                    MostrarMsg("No se pudo subir la imagen: " + err, true);
                    return;
                }

                txtImagenUrl.Text = url;
                imgPreview.ImageUrl = url;
                MostrarMsg("Imagen subida correctamente a Cloudinary.", false);
            }
            catch (Exception ex)
            {
                LogError(ex, "btnSubirCloudinary_Click");
                MostrarMsg("Ocurrió un error al subir la imagen: " + ex.Message, true);
            }
        }

        // =============== Guardar artículo (alta/modificación) ===============
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MostrarMsg("El nombre es obligatorio.", true);
                    return;
                }

                if (string.IsNullOrEmpty(ddlMarca.SelectedValue) || string.IsNullOrEmpty(ddlCategoria.SelectedValue))
                {
                    MostrarMsg("Seleccioná Marca y Categoría.", true);
                    return;
                }

                // Parseo de precio
                decimal? precio = null;
                if (!string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    var textoPrecio = txtPrecio.Text.Trim();
                    decimal p;
                    if (!TryParseDecimal(textoPrecio, out p) || p < 0)
                    {
                        MostrarMsg("Precio inválido (usá números y separador decimal , o .).", true);
                        return;
                    }
                    precio = p;
                }

                // Subir imagen si no hay URL pero sí archivo
                if (string.IsNullOrWhiteSpace(txtImagenUrl.Text) && fuImagen.HasFile)
                {
                    if (!ValidarArchivoImagen(fuImagen)) return;

                    string url, err;
                    if (!CloudinaryService.UploadImage(fuImagen.PostedFile, out url, out err))
                    {
                        MostrarMsg("No se pudo subir la imagen a Cloudinary: " + err, true);
                        return;
                    }
                    txtImagenUrl.Text = url;
                }

                var art = new dominio.Articulos
                {
                    Codigo = string.IsNullOrWhiteSpace(txtCodigo.Text) ? null : txtCodigo.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(txtDescripcion.Text) ? null : txtDescripcion.Text.Trim(),
                    IdMarca = int.Parse(ddlMarca.SelectedValue),
                    IdCategoria = int.Parse(ddlCategoria.SelectedValue),
                    ImagenUrl = string.IsNullOrWhiteSpace(txtImagenUrl.Text) ? null : txtImagenUrl.Text.Trim(),
                    Precio = precio
                };

                int idExistente;
                if (int.TryParse(hfId.Value, out idExistente) && idExistente > 0)
                {
                    art.Id = idExistente;
                    artNeg.modificar(art);
                    MostrarMsg("Artículo actualizado correctamente.", false);
                }
                else
                {
                    int newId = artNeg.agregar(art);
                    MostrarMsg("Artículo creado correctamente.", false);
                    LimpiarFormulario();
                }

                // Actualizar preview (una sola vez)
                imgPreview.ImageUrl = string.IsNullOrWhiteSpace(art.ImagenUrl)
                    ? "https://placehold.co/800x600?text=Sin+Imagen"
                    : art.ImagenUrl;
            }
            catch (FormatException)
            {
                MostrarMsg("Formato inválido en algún campo. Revisá los datos ingresados.", true);
            }
            catch (Exception ex)
            {
                LogError(ex, "btnGuardar_Click");
                MostrarMsg("Ocurrió un error al guardar el artículo.", true);
            }
        }


        // =============== Helpers ===============
        private bool ValidarArchivoImagen(FileUpload fu)
        {
            var ext = System.IO.Path.GetExtension(fu.FileName)?.ToLowerInvariant();
            var okExt = new[] { ".jpg", ".jpeg", ".png" };

            if (string.IsNullOrEmpty(ext) || Array.IndexOf(okExt, ext) < 0)
            {
                MostrarMsg("Solo se permiten imágenes JPG o PNG.", true);
                return false;
            }

            const int max = 2 * 1024 * 1024; // 2MB
            if (fu.PostedFile.ContentLength > max)
            {
                MostrarMsg("La imagen no puede superar los 2 MB.", true);
                return false;
            }
            return true;
        }


        private static bool TryParseDecimal(string input, out decimal value)
        {
            input = input.Replace(".", ",");
            return decimal.TryParse(input, NumberStyles.Number, new CultureInfo("es-AR"), out value);
        }

        private void MostrarMsg(string msg, bool error)
        {
            lblMsg.Visible = true;
            lblMsg.Text = msg;
            lblMsg.CssClass = error ? "alert alert-danger d-block" : "alert alert-success d-block";
        }

        private void LogError(Exception ex, string contexto)
        {
            Session["error"] = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {contexto}\n{ex}";
        }


        private void LimpiarFormulario()
        {
            // Textos
            txtCodigo.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtImagenUrl.Text = string.Empty;

            // Combos
            ddlMarca.ClearSelection();
            if (ddlMarca.Items.Count > 0) ddlMarca.SelectedIndex = 0;

            ddlCategoria.ClearSelection();
            if (ddlCategoria.Items.Count > 0) ddlCategoria.SelectedIndex = 0;

            // Imagen + estado (forzar refresh con cache-buster)
            string placeholder = "https://placehold.co/800x600?text=Sin+Imagen";
            string bust = placeholder + (placeholder.Contains("?") ? "&" : "?") + "t=" + DateTime.UtcNow.Ticks;

            // Server-side
            imgPreview.ImageUrl = bust;

            hfId.Value = string.Empty;
            lblTitulo.Text = "Nuevo artículo";

            // JS: limpiar fileupload y asegurar que el <img> cambie el src en el cliente
            string js = $@"
        (function(){{
            var fu = document.getElementById('{fuImagen.ClientID}');
            if (fu) fu.value = '';
            var img = document.getElementById('{imgPreview.ClientID}');
            if (img) img.src = '{bust.Replace("'", "\\'")}';
        }})();";

            ScriptManager.RegisterStartupScript(this, GetType(), "resetFileUploadAndImg", js, true);

            // Foco en el primer campo requerido
            txtNombre.Focus();
        }



    }
}
