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
    public partial class Favoritos : System.Web.UI.Page
    {
        private readonly FavoritosNegocio favNeg = new FavoritosNegocio();
        private readonly ArticulosNegocio artNeg = new ArticulosNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Seguridad.sesionActiva(Session["user"]))
            {
                Session["returnUrl"] = Request.RawUrl;
                Response.Redirect("Login.aspx", false);
                return;
            }

            if (!IsPostBack) BindFavoritos();
        }

        private void BindFavoritos(Users user = null)
        {
            try
            {
                if (user == null)
                    user = Session["user"] as Users;

                if (!Seguridad.sesionActiva(user))
                    return;

                var favs = favNeg.listarPorUser(user.Id) ?? new List<dominio.Favoritos>();
                if (favs.Count == 0)
                {
                    pnlVacio.Visible = true;
                    repFavoritos.DataSource = null;
                    repFavoritos.DataBind();
                    return;
                }

                var idsArticulos = favs.Select(f => f.IdArticulo).Distinct().ToList();
                var articulos = artNeg.listarByIds(idsArticulos) ?? new List<dominio.Articulos>();
                var dict = articulos.ToDictionary(a => a.Id, a => a);

                var vm = favs.Select(f =>
                {
                    dict.TryGetValue(f.IdArticulo, out var art);
                    return new FavVM
                    {
                        IdFavorito = f.Id,
                        IdArticulo = f.IdArticulo,
                        Nombre = art != null ? art.Nombre : "(Sin nombre)",
                        Descripcion = art != null ? art.Descripcion ?? string.Empty : string.Empty,
                        ImagenUrl = art != null && !string.IsNullOrWhiteSpace(art.ImagenUrl)
                                    ? art.ImagenUrl
                                    : "https://placehold.co/600x400?text=Sin+Imagen",
                        PrecioTexto = (art != null && art.Precio.HasValue)
                                      ? "$ " + art.Precio.Value.ToString("N2")
                                      : "Consultar"
                    };
                }).ToList();

                pnlVacio.Visible = vm.Count == 0;
                repFavoritos.DataSource = vm;
                repFavoritos.DataBind();
            }
            catch
            {
                pnlVacio.Visible = true;
                repFavoritos.DataSource = null;
                repFavoritos.DataBind();
            }
        }


        protected void repFavoritos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Quitar" && int.TryParse(e.CommandArgument.ToString(), out int idFav))
            {
                try
                {
                    favNeg.eliminar(idFav);
                }
                catch
                {
                    // Podés mostrar un mensaje si querés
                }
                BindFavoritos();
            }
        }

        // ViewModel de la vista
        private class FavVM
        {
            public int IdFavorito { get; set; }
            public int IdArticulo { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public string ImagenUrl { get; set; }
            public string PrecioTexto { get; set; }
        }
    }
}

