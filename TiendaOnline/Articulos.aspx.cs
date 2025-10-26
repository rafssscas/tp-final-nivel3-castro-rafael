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
    public partial class Articulos : System.Web.UI.Page
    {
        private readonly ArticulosNegocio artNeg = new ArticulosNegocio();
        private readonly MarcasNegocio marcaNeg = new MarcasNegocio();
        private readonly CategoriasNegocio catNeg = new CategoriasNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Sólo admin (el Master ya protege /Admin, pero dejamos doble check)
            if (!Seguridad.esAdmin(Session["user"]))
            {
                Session["returnUrl"] = Request.RawUrl;
                Response.Redirect("~/Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarCombos();
                BindGrid();
            }
        }

        private void CargarCombos()
        {
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

        private void BindGrid()
        {
            string texto = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim();
            int? idMarca = string.IsNullOrEmpty(ddlMarca.SelectedValue) ? (int?)null : int.Parse(ddlMarca.SelectedValue);
            int? idCat = string.IsNullOrEmpty(ddlCategoria.SelectedValue) ? (int?)null : int.Parse(ddlCategoria.SelectedValue);

            List<dominio.Articulos> datos = (texto != null || idMarca.HasValue || idCat.HasValue)
                ? artNeg.buscar(texto, idMarca, idCat)
                : artNeg.listar();

            gvArticulos.DataSource = datos;
            gvArticulos.DataBind();

            lblTotal.Text = datos != null ? $"{datos.Count} resultado(s)" : "0 resultado(s)";
            MostrarMsg(null, false, hide: true);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            gvArticulos.PageIndex = 0;
            BindGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            ddlMarca.SelectedIndex = 0;
            ddlCategoria.SelectedIndex = 0;
            gvArticulos.PageIndex = 0;
            BindGrid();
        }

        protected void gvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvArticulos.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void gvArticulos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    artNeg.eliminar(id);
                    MostrarMsg("Artículo eliminado correctamente.", error: false);
                    BindGrid();
                }
                catch (Exception)
                {
                    MostrarMsg("No se pudo eliminar el artículo.", error: true);
                }
            }
        }

        private void MostrarMsg(string msg, bool error, bool hide = false)
        {
            if (hide)
            {
                lblMsg.Visible = false;
                return;
            }

            lblMsg.Visible = true;
            lblMsg.Text = msg;
            lblMsg.CssClass = error ? "alert alert-danger d-block" : "alert alert-success d-block";
        }
    }
}