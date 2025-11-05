using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;              // <-- necesario para Take/ToList
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiendaOnline
{
    public partial class Home : System.Web.UI.Page
    {
        
        public List<dominio.Articulos> ListaTopVendidos { get; set; }
        public List<dominio.Articulos> ListaFiltrada { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargarCombos();
                    CargarTopVendidos();
                    BindProductos();
                }
            }
            
            catch (Exception ex)
            {
                LogError(ex, "Page_Load");
            }
        }

        private void CargarCombos()
        {
            try
            {
                var marcaNeg = new MarcasNegocio();
                var catNeg = new CategoriasNegocio();

                ddlMarca.DataSource = marcaNeg.listar();
                ddlMarca.DataTextField = "Descripcion";
                ddlMarca.DataValueField = "Id";
                ddlMarca.DataBind();
                ddlMarca.Items.Insert(0, new ListItem("-- Todas --", ""));

                ddlCategoria.DataSource = catNeg.listar();
                ddlCategoria.DataTextField = "Descripcion";
                ddlCategoria.DataValueField = "Id";
                ddlCategoria.DataBind();
                ddlCategoria.Items.Insert(0, new ListItem("-- Todas --", ""));
            }
            catch (Exception ex)
            {
                LogError(ex, "CargarCombos");
                ddlMarca.Items.Clear();
                ddlMarca.Items.Insert(0, new ListItem("-- Todas --", ""));
                ddlCategoria.Items.Clear();
                ddlCategoria.Items.Insert(0, new ListItem("-- Todas --", ""));
            }
        }

        private void CargarTopVendidos()
        {
            try
            {
                var artNeg = new ArticulosNegocio();

               
                // Tomamos 4 elementos del listar(), metodo para implementar la funcion mas vendidos mas adelante.
                var all = artNeg.listar() ?? new List<dominio.Articulos>();
                ListaTopVendidos = all.Take(4).ToList();  // <-- evita GetRange y asegura List<dominio.Articulos>

                repTopVendidos.DataSource = ListaTopVendidos;
                repTopVendidos.DataBind();
            }
            catch (Exception ex)
            {
                LogError(ex, "CargarTopVendidos");
                ListaTopVendidos = new List<dominio.Articulos>();
                repTopVendidos.DataSource = ListaTopVendidos;
                repTopVendidos.DataBind();
            }
        }

        private void BindProductos()
        {
            try
            {
                var artNeg = new ArticulosNegocio();

                string texto = string.IsNullOrWhiteSpace(txtBuscar.Text) ? null : txtBuscar.Text.Trim();

                int? idMarca = null;
                if (!string.IsNullOrEmpty(ddlMarca.SelectedValue) && int.TryParse(ddlMarca.SelectedValue, out int idM))
                    idMarca = idM;

                int? idCat = null;
                if (!string.IsNullOrEmpty(ddlCategoria.SelectedValue) && int.TryParse(ddlCategoria.SelectedValue, out int idC))
                    idCat = idC;

                // Puede devolver IEnumerable<dominio.Articulos> o List<dominio.Articulos>; normalizamos con ToList()
                var datos = (texto != null || idMarca.HasValue || idCat.HasValue)
                                ? artNeg.buscar(texto, idMarca, idCat)
                                : artNeg.listar();

                ListaFiltrada = datos?.ToList() ?? new List<dominio.Articulos>();  
                lvProductos.DataSource = ListaFiltrada;
                lvProductos.DataBind();
            }
            catch (Exception ex)
            {
                LogError(ex, "BindProductos");
                ListaFiltrada = new List<dominio.Articulos>();
                lvProductos.DataSource = ListaFiltrada;
                lvProductos.DataBind();
            }
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                var dp = lvProductos.FindControl("dp") as DataPager;
                if (dp != null) dp.SetPageProperties(0, dp.PageSize, false);
                BindProductos();
            }
            catch (Exception ex)
            {
                LogError(ex, "btnFiltrar_Click");
            }
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscar.Text = string.Empty;
                ddlMarca.SelectedIndex = 0;
                ddlCategoria.SelectedIndex = 0;

                var dp = lvProductos.FindControl("dp") as DataPager;
                if (dp != null) dp.SetPageProperties(0, dp.PageSize, false);

                BindProductos();
            }
            catch (Exception ex)
            {
                LogError(ex, "btnLimpiar_Click");
            }
        }

        protected void lvProductos_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            var dp = lvProductos.FindControl("dp") as DataPager;
            if (dp != null)
            {
                dp.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
                BindProductos();
            }
        }

        private void LogError(Exception ex, string contexto)
        {
            Session["error"] = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {contexto}\n{ex}";
        }
    }
}
