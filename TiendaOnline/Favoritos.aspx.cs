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

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sólo usuarios logueados
            if (!Seguridad.sesionActiva(Session["user"]))
            {
                Session["returnUrl"] = Request.RawUrl;
                Response.Redirect("Login.aspx", false);
                return;
            }

            if (!IsPostBack)
                BindFavoritos();
        }

        private void BindFavoritos()
        {
            var user = (Users)Session["user"];
            var lista = favNeg.listarPorUser(user.Id);

            pnlVacio.Visible = lista == null || lista.Count == 0;
            repFavoritos.DataSource = lista;
            repFavoritos.DataBind();
        }

        protected void repFavoritos_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Quitar")
            {
                int idFav = int.Parse(e.CommandArgument.ToString());
                favNeg.eliminar(idFav);
                BindFavoritos();
            }
        }
    }
}