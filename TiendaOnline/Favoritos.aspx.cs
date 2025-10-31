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

        private void BindFavoritos()
        {
            var user = (Users)Session["user"];
            var favs = favNeg.listarPorUser(user.Id) ?? new List<dominio.Favoritos>(); // <- ajustá tipo si tu DTO es distinto

            if (favs.Count == 0)
            {
                pnlVacio.Visible = true;
                repFavoritos.DataSource = null;
                repFavoritos.DataBind();
                return;
            }

            // 1) Traigo TODOS los artículos que necesito en un solo viaje
            var idsArticulos = favs.Select(f => f.IdArticulo).Distinct().ToList();
            var articulos = artNeg.listarByIds(idsArticulos) ?? new List<dominio.Articulos>(); // usa tu método real
            var dict = articulos.ToDictionary(a => a.Id, a => a);

            // 2) Proyección a un ViewModel plano para el Repeater (evita null refs y Evals anidados)
            var vm = favs
                .Select(f =>
                {
                    dict.TryGetValue(f.IdArticulo, out var art);
                    return new FavVM
                    {
                        IdFavorito = f.Id,
                        IdArticulo = f.IdArticulo,
                        Nombre = art?.Nombre ?? "(Sin nombre)",
                        Descripcion = art?.Descripcion ?? "",
                        ImagenUrl = string.IsNullOrWhiteSpace(art?.ImagenUrl)
                                    ? "https://placehold.co/600x400?text=Sin+Imagen"
                                    : art.ImagenUrl,
                        PrecioTexto = art?.Precio.HasValue == true ? "$ " + art.Precio.Value.ToString("N2") : "Consultar"
                    };
                })
                .ToList();

            pnlVacio.Visible = vm.Count == 0;
            repFavoritos.DataSource = vm;
            repFavoritos.DataBind();
        }

        protected void repFavoritos_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                if (int.TryParse(e.CommandArgument.ToString(), out int idFav))
                {
                    favNeg.eliminar(idFav);
                    BindFavoritos();
                }
            }
        }

        // ---- ViewModel para la vista ----
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