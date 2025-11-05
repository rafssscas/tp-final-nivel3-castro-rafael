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

            // --- INICIO DE MODIFICACIÓN ---
            // 1. Enlazar GridView (escritorio)
            gvArticulos.DataSource = datos;
            gvArticulos.DataBind();

            // 2. Enlazar ListView (móvil)
            lvArticulos.DataSource = datos;
            lvArticulos.DataBind();
            // --- FIN DE MODIFICACIÓN ---

            lblTotal.Text = datos != null ? $"{datos.Count} resultado(s)" : "0 resultado(s)";
            MostrarMsg(null, false, hide: true);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            // Resetear paginación de GridView
            gvArticulos.PageIndex = 0;

            // --- INICIO DE MODIFICACIÓN ---
            // Resetear paginación de ListView
            var dataPager = lvArticulos.FindControl("DataPager1") as DataPager;
            if (dataPager != null)
            {
                dataPager.SetPageProperties(0, dataPager.PageSize, false);
            }
            // --- FIN DE MODIFICACIÓN ---

            BindGrid();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtBuscar.Text = string.Empty;
            ddlMarca.SelectedIndex = 0;
            ddlCategoria.SelectedIndex = 0;

            // Resetear paginación de GridView
            gvArticulos.PageIndex = 0;

            // --- INICIO DE MODIFICACIÓN ---
            // Resetear paginación de ListView
            var dataPager = lvArticulos.FindControl("DataPager1") as DataPager;
            if (dataPager != null)
            {
                dataPager.SetPageProperties(0, dataPager.PageSize, false);
            }
            // --- FIN DE MODIFICACIÓN ---

            BindGrid();
        }

        protected void gvArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvArticulos.PageIndex = e.NewPageIndex;

            // --- INICIO DE MODIFICACIÓN ---
            // Sincronizar el DataPager del ListView
            var dataPager = lvArticulos.FindControl("DataPager1") as DataPager;
            if (dataPager != null)
            {
                // e.NewPageIndex es el índice de página (0, 1, 2)
                // Debemos convertirlo a StartRowIndex (0, 10, 20)
                int startRowIndex = e.NewPageIndex * dataPager.PageSize;
                dataPager.SetPageProperties(startRowIndex, dataPager.PageSize, false);
            }
            // --- FIN DE MODIFICACIÓN ---

            BindGrid();
        }

        // --- INICIO DE NUEVO MÉTODO ---
        /// <summary>
        /// Maneja la paginación del control DataPager del ListView.
        /// </summary>
        protected void lvArticulos_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            // Establece las nuevas propiedades de página para el DataPager
            (lvArticulos.FindControl("DataPager1") as DataPager).SetPageProperties(e.StartRowIndex, e.MaximumRows, false);

            // Sincronizar el GridView (e.StartRowIndex es 0, 10, 20, etc.)
            gvArticulos.PageIndex = (e.StartRowIndex / gvArticulos.PageSize);

            // Volvemos a enlazar los datos con la nueva página
            BindGrid();
        }
        // --- FIN DE NUEVO MÉTODO ---


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

        // --- INICIO DE NUEVO MÉTODO ---
        /// <summary>
        /// Maneja los comandos de los botones dentro del ListView (ej. Eliminar).
        /// </summary>
        protected void lvArticulos_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                try
                {
                    int id = int.Parse(e.CommandArgument.ToString());
                    artNeg.eliminar(id);
                    MostrarMsg("Artículo eliminado correctamente.", error: false);
                    BindGrid(); // Recarga los datos en ambas vistas
                }
                catch (Exception)
                {
                    MostrarMsg("No se pudo eliminar el artículo.", error: true);
                }
            }
        }
        // --- FIN DE NUEVO MÉTODO ---

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
