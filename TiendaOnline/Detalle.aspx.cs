using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                return;
            }

            var art = artNeg.obtenerPorId(id);
            if (art == null)
            {
                pnlNotFound.Visible = true;
                return;
            }

            // Mostrar datos
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
                // Requiere sesión
                var user = Session["user"] as Users;
                if (!Seguridad.sesionActiva(user))
                {
                    Session["returnUrl"] = Request.RawUrl;
                    Response.Redirect("Login.aspx", false);
                    return;
                }

                int idArticulo = int.Parse(hfIdArticulo.Value);
                favNeg.agregar(user.Id, idArticulo);

                lblMsg.Text = "Artículo agregado a tus favoritos.";
                lblMsg.Visible = true;
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception)
            {
                lblMsg.CssClass = "text-danger d-block mb-2";
                lblMsg.Text = "No se pudo agregar a favoritos.";
                lblMsg.Visible = true;
            }
        }
    }
}