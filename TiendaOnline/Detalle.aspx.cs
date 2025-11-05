using dominio;
using negocio;
using System;

using System.Web.UI;

namespace TiendaOnline
{
    public partial class Detalle : System.Web.UI.Page
    {
        private readonly ArticulosNegocio artNeg = new ArticulosNegocio();
        private readonly FavoritosNegocio favNeg = new FavoritosNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarArticulo();
        }

        private void CargarArticulo()
        {
            if (!int.TryParse(Request.QueryString["id"], out int id))
            {
                pnlNotFound.Visible = true;
                pnlDetalle.Visible = false;
                return;
            }

            var art = artNeg.obtenerPorId(id);
            if (art == null)
            {
                pnlNotFound.Visible = true;
                pnlDetalle.Visible = false;
                return;
            }

            // Mostrar datos
            pnlNotFound.Visible = false;
            pnlDetalle.Visible = true;
            hfIdArticulo.Value = art.Id.ToString();

            imgArticulo.ImageUrl = string.IsNullOrWhiteSpace(art.ImagenUrl)
                ? "https://placehold.co/800x600?text=Sin+Imagen"
                : art.ImagenUrl;

            lblNombre.Text = art.Nombre;
            lblMarca.Text = art.Marca != null ? art.Marca.Descripcion : "—";
            lblCategoria.Text = art.Categoria != null ? art.Categoria.Descripcion : "—";
            lblDescripcion.Text = art.Descripcion ?? string.Empty;
            lblPrecio.Text = art.Precio.HasValue ? "$ " + art.Precio.Value.ToString("N2") : "Consultar";
        }

        protected void btnFavorito_Click(object sender, EventArgs e)
        {
            try
            {
                var user = Session["user"] as Users;
                if (!Seguridad.sesionActiva(user))
                {
                    Session["returnUrl"] = Request.RawUrl;
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                if (!int.TryParse(hfIdArticulo.Value, out int idArticulo))
                {
                    lblMsg.CssClass = "alert alert-danger d-block mb-2";
                    lblMsg.Text = "No se pudo identificar el artículo.";
                    lblMsg.Visible = true;
                    return;
                }

                // Opcional: si tu FavoritosNegocio tiene un método de verificación, podés evitar duplicados.
                // if (favNeg.Existe(user.Id, idArticulo)) { ... }

                favNeg.agregar(user.Id, idArticulo);

                lblMsg.CssClass = "alert alert-success d-block mb-2";
                lblMsg.Text = "Artículo agregado a tus favoritos.";
                lblMsg.Visible = true;
            }
            catch (Exception)
            {
                lblMsg.CssClass = "alert alert-danger d-block mb-2";
                lblMsg.Text = "No se pudo agregar a favoritos.";
                lblMsg.Visible = true;
            }
        }
    }
}