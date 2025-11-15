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
    public partial class MarcasCategorias : System.Web.UI.Page
    {
        private readonly MarcasNegocio marcasNeg = new MarcasNegocio();
        private readonly CategoriasNegocio categoriasNeg = new CategoriasNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Seguridad.esAdmin(Session["user"])) 
                { 
                    Response.Redirect("~/Login.aspx", false); return; 
                }

                if (!IsPostBack)
                {
                    BindMarcas();
                    BindCategorias();
                }
            }
            catch (System.Threading.ThreadAbortException) { /* esperado en Redirect */ }
            catch (Exception ex)
            {
                LogError(ex, "Page_Load");
                ShowMsg(lblMsgMarcas, "Ocurrió un problema al cargar la página.", true);
                ShowMsg(lblMsgCategorias, "Ocurrió un problema al cargar la página.", true);
            }
        }

        /* ===================== M A R C A S ===================== */

        private void BindMarcas()
        {
            try
            {
                var datos = marcasNeg.listar() ?? new List<dominio.Marcas>();
                // --- INICIO DE MODIFICACIÓN ---
                // Enlazar ambas vistas
                gvMarcas.DataSource = datos;
                gvMarcas.DataBind();
                lvMarcas.DataSource = datos;
                lvMarcas.DataBind();
                // --- FIN DE MODIFICACIÓN ---
            }
            catch (Exception ex)
            {
                LogError(ex, "BindMarcas");
                // --- INICIO DE MODIFICACIÓN ---
                // Limpiar ambas vistas
                gvMarcas.DataSource = new List<dominio.Marcas>();
                gvMarcas.DataBind();
                lvMarcas.DataSource = new List<dominio.Marcas>();
                lvMarcas.DataBind();
                // --- FIN DE MODIFICACIÓN ---
                ShowMsg(lblMsgMarcas, "No se pudieron cargar las marcas.", true);
            }
        }

        protected void btnAgregarMarca_Click(object sender, EventArgs e)
        {
            try
            {
                var desc = (txtNuevaMarca.Text ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(desc))
                {
                    ShowMsg(lblMsgMarcas, "La descripción es obligatoria.", true);
                    return;
                }

                marcasNeg.agregar(new dominio.Marcas { Descripcion = desc });

                txtNuevaMarca.Text = string.Empty;
                ShowMsg(lblMsgMarcas, "Marca agregada correctamente.", false);
                BindMarcas(); // BindMarcas() ahora actualiza ambas vistas
                upMarcas.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "btnAgregarMarca_Click");
                ShowMsg(lblMsgMarcas, GetFriendlySqlError(ex, "No se pudo agregar la marca."), true);
            }
        }

        protected void gvMarcas_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // --- INICIO DE MODIFICACIÓN ---
                // Sincronizar EditIndex en ambas vistas
                gvMarcas.EditIndex = e.NewEditIndex;
                lvMarcas.EditIndex = e.NewEditIndex;
                // --- FIN DE MODIFICACIÓN ---
                BindMarcas();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvMarcas_RowEditing");
                ShowMsg(lblMsgMarcas, "No se pudo iniciar la edición.", true);
            }
        }

        protected void gvMarcas_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                // --- INICIO DE MODIFICACIÓN ---
                // Sincronizar EditIndex en ambas vistas
                gvMarcas.EditIndex = -1;
                lvMarcas.EditIndex = -1;
                // --- FIN DE MODIFICACIÓN ---
                BindMarcas();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvMarcas_RowCancelingEdit");
            }
        }

        protected void gvMarcas_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvMarcas.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvMarcas.Rows[e.RowIndex];
                var txtDesc = row.FindControl("txtMarcaDesc") as TextBox;
                string desc = (txtDesc?.Text ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(desc))
                {
                    ShowMsg(lblMsgMarcas, "La descripción es obligatoria.", true);
                    return;
                }

                var entidad = new dominio.Marcas { Id = id, Descripcion = desc };
                marcasNeg.modificar(entidad);

                // --- INICIO DE MODIFICACIÓN ---
                // Sincronizar EditIndex en ambas vistas
                gvMarcas.EditIndex = -1;
                lvMarcas.EditIndex = -1;
                // --- FIN DE MODIFICACIÓN ---

                ShowMsg(lblMsgMarcas, "Marca actualizada correctamente.", false);
                BindMarcas();
                upMarcas.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvMarcas_RowUpdating");
                ShowMsg(lblMsgMarcas, GetFriendlySqlError(ex, "No se pudo actualizar la marca."), true);
            }
        }

        protected void gvMarcas_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvMarcas.DataKeys[e.RowIndex].Value);
                marcasNeg.eliminar(id);

                ShowMsg(lblMsgMarcas, "Marca eliminada correctamente.", false);
                BindMarcas(); // BindMarcas() ahora actualiza ambas vistas
                upMarcas.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvMarcas_RowDeleting");
                ShowMsg(lblMsgMarcas, GetFriendlySqlError(ex, "No se pudo eliminar la marca."), true);
            }
        }

        // --- INICIO DE NUEVOS MÉTODOS (PARA LISTVIEW DE MARCAS) ---
        protected void lvMarcas_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            // Sincronizar EditIndex en ambas vistas
            gvMarcas.EditIndex = e.NewEditIndex;
            lvMarcas.EditIndex = e.NewEditIndex;
            BindMarcas();
        }

        protected void lvMarcas_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            // Sincronizar EditIndex en ambas vistas
            gvMarcas.EditIndex = -1;
            lvMarcas.EditIndex = -1;
            BindMarcas();
        }

        protected void lvMarcas_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(lvMarcas.DataKeys[e.ItemIndex].Value);
                // --- INICIO DE CORRECCIÓN ---
                var txtDesc = lvMarcas.Items[e.ItemIndex].FindControl("txtMarcaDesc") as TextBox;
                // --- FIN DE CORRECCIÓN ---
                string desc = (txtDesc?.Text ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(desc))
                {
                    ShowMsg(lblMsgMarcas, "La descripción es obligatoria.", true);
                    return;
                }

                var entidad = new dominio.Marcas { Id = id, Descripcion = desc };
                marcasNeg.modificar(entidad);

                // Sincronizar EditIndex en ambas vistas
                gvMarcas.EditIndex = -1;
                lvMarcas.EditIndex = -1;

                ShowMsg(lblMsgMarcas, "Marca actualizada correctamente.", false);
                BindMarcas();
                upMarcas.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "lvMarcas_ItemUpdating");
                ShowMsg(lblMsgMarcas, GetFriendlySqlError(ex, "No se pudo actualizar la marca."), true);
            }
        }

        protected void lvMarcas_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            
            if (e.CommandName == "Eliminar")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    marcasNeg.eliminar(id);

                    ShowMsg(lblMsgMarcas, "Marca eliminada correctamente.", false);
                    BindMarcas(); // BindMarcas() ahora actualiza ambas vistas
                    upMarcas.Update();
                }
                catch (Exception ex)
                {
                    LogError(ex, "lvMarcas_ItemCommand_Delete");
                    ShowMsg(lblMsgMarcas, GetFriendlySqlError(ex, "No se pudo eliminar la marca."), true);
                }
            }
        }
        // --- FIN DE NUEVOS MÉTODOS ---


        /* ================== C A T E G O R Í A S ================= */

        private void BindCategorias()
        {
            try
            {
                var datos = categoriasNeg.listar() ?? new List<dominio.Categorias>();
                // --- INICIO DE MODIFICACIÓN ---
                // Enlazar ambas vistas
                gvCategorias.DataSource = datos;
                gvCategorias.DataBind();
                lvCategorias.DataSource = datos;
                lvCategorias.DataBind();
                // --- FIN DE MODIFICACIÓN ---
            }
            catch (Exception ex)
            {
                LogError(ex, "BindCategorias");
                // --- INICIO DE MODIFICACIÓN ---
                // Limpiar ambas vistas
                gvCategorias.DataSource = new List<dominio.Categorias>();
                gvCategorias.DataBind();
                lvCategorias.DataSource = new List<dominio.Categorias>();
                lvCategorias.DataBind();
                // --- FIN DE MODIFICACIÓN ---
                ShowMsg(lblMsgCategorias, "No se pudieron cargar las categorías.", true);
            }
        }

        protected void btnAgregarCategoria_Click(object sender, EventArgs e)
        {
            try
            {
                var desc = (txtNuevaCategoria.Text ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(desc))
                {
                    ShowMsg(lblMsgCategorias, "La descripción es obligatoria.", true);
                    return;
                }

                categoriasNeg.agregar(new dominio.Categorias { Descripcion = desc });

                txtNuevaCategoria.Text = string.Empty;
                ShowMsg(lblMsgCategorias, "Categoría agregada correctamente.", false);
                BindCategorias(); // BindCategorias() ahora actualiza ambas vistas
                upCategorias.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "btnAgregarCategoria_Click");
                ShowMsg(lblMsgCategorias, GetFriendlySqlError(ex, "No se pudo agregar la categoría."), true);
            }
        }

        protected void gvCategorias_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                // --- INICIO DE MODIFICACIÓN ---
                // Sincronizar EditIndex en ambas vistas
                gvCategorias.EditIndex = e.NewEditIndex;
                lvCategorias.EditIndex = e.NewEditIndex;
                // --- FIN DE MODIFICACIÓN ---
                BindCategorias();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvCategorias_RowEditing");
                ShowMsg(lblMsgCategorias, "No se pudo iniciar la edición.", true);
            }
        }

        protected void gvCategorias_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            try
            {
                // --- INICIO DE MODIFICACIÓN ---
                // Sincronizar EditIndex en ambas vistas
                gvCategorias.EditIndex = -1;
                lvCategorias.EditIndex = -1;
                // --- FIN DE MODIFICACIÓN ---
                BindCategorias();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvCategorias_RowCancelingEdit");
            }
        }

        protected void gvCategorias_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCategorias.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvCategorias.Rows[e.RowIndex];
                var txtDesc = row.FindControl("txtCatDesc") as TextBox;
                string desc = (txtDesc?.Text ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(desc))
                {
                    ShowMsg(lblMsgCategorias, "La descripción es obligatoria.", true);
                    return;
                }

                var entidad = new dominio.Categorias { Id = id, Descripcion = desc };
                categoriasNeg.modificar(entidad);

                // --- INICIO DE MODIFICACIÓN ---
                // Sincronizar EditIndex en ambas vistas
                gvCategorias.EditIndex = -1;
                lvCategorias.EditIndex = -1;
                // --- FIN DE MODIFICACIÓN ---

                ShowMsg(lblMsgCategorias, "Categoría actualizada correctamente.", false);
                BindCategorias();
                upCategorias.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvCategorias_RowUpdating");
                ShowMsg(lblMsgCategorias, GetFriendlySqlError(ex, "No se pudo actualizar la categoría."), true);
            }
        }

        protected void gvCategorias_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(gvCategorias.DataKeys[e.RowIndex].Value);
                categoriasNeg.eliminar(id);
                ShowMsg(lblMsgCategorias, "Categoría eliminada correctamente.", false);
                BindCategorias(); // BindCategorias() ahora actualiza ambas vistas
                upCategorias.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "gvCategorias_RowDeleting");
                ShowMsg(lblMsgCategorias, GetFriendlySqlError(ex, "No se pudo eliminar la categoría."), true);
            }
        }

        // --- INICIO DE NUEVOS MÉTODOS (PARA LISTVIEW DE CATEGORÍAS) ---
        protected void lvCategorias_ItemEditing(object sender, ListViewEditEventArgs e)
        {
            // Sincronizar EditIndex en ambas vistas
            gvCategorias.EditIndex = e.NewEditIndex;
            lvCategorias.EditIndex = e.NewEditIndex;
            BindCategorias();
        }

        protected void lvCategorias_ItemCanceling(object sender, ListViewCancelEventArgs e)
        {
            // Sincronizar EditIndex en ambas vistas
            gvCategorias.EditIndex = -1;
            lvCategorias.EditIndex = -1;
            BindCategorias();
        }

        protected void lvCategorias_ItemUpdating(object sender, ListViewUpdateEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(lvCategorias.DataKeys[e.ItemIndex].Value);
                // --- INICIO DE CORRECCIÓN ---
                var txtDesc = lvCategorias.Items[e.ItemIndex].FindControl("txtCatDesc") as TextBox;
                // --- FIN DE CORRECCIÓN ---
                string desc = (txtDesc?.Text ?? string.Empty).Trim();

                if (string.IsNullOrEmpty(desc))
                {
                    ShowMsg(lblMsgCategorias, "La descripción es obligatoria.", true);
                    return;
                }

                var entidad = new dominio.Categorias { Id = id, Descripcion = desc };
                categoriasNeg.modificar(entidad);

                // Sincronizar EditIndex en ambas vistas
                gvCategorias.EditIndex = -1;
                lvCategorias.EditIndex = -1;

                ShowMsg(lblMsgCategorias, "Categoría actualizada correctamente.", false);
                BindCategorias();
                upCategorias.Update();
            }
            catch (Exception ex)
            {
                LogError(ex, "lvCategorias_ItemUpdating");
                ShowMsg(lblMsgCategorias, GetFriendlySqlError(ex, "No se pudo actualizar la categoría."), true);
            }
        }

        protected void lvCategorias_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                try
                {
                    int id = Convert.ToInt32(e.CommandArgument);
                    categoriasNeg.eliminar(id);
                    ShowMsg(lblMsgCategorias, "Categoría eliminada correctamente.", false);
                    BindCategorias(); // BindCategorias() ahora actualiza ambas vistas
                    upCategorias.Update();
                }
                catch (Exception ex)
                {
                    LogError(ex, "lvCategorias_ItemCommand_Delete");
                    ShowMsg(lblMsgCategorias, GetFriendlySqlError(ex, "No se pudo eliminar la categoría."), true);
                }
            }
        }
        // --- FIN DE NUEVOS MÉTODOS ---

        /* ===================== U T I L E S ===================== */

        private void ShowMsg(Label lbl, string msg, bool error)
        {
            if (lbl == null) return;
            lbl.Visible = true;
            lbl.Text = msg;
            lbl.CssClass = error ? "msg alert alert-danger" : "msg alert alert-success";
        }

        private void LogError(Exception ex, string contexto)
        {
            Session["error"] = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {contexto}\n{ex}";
        }

        private string GetFriendlySqlError(Exception ex, string fallback)
        {
            var msg = ex?.Message ?? string.Empty;

            if (msg.IndexOf("hay artículos asociados", StringComparison.OrdinalIgnoreCase) >= 0)
                return "No se puede eliminar: hay artículos asociados.";

            if (msg.IndexOf("ya existe", StringComparison.OrdinalIgnoreCase) >= 0 ||
                msg.IndexOf("duplicate", StringComparison.OrdinalIgnoreCase) >= 0 ||
                msg.IndexOf("duplic", StringComparison.OrdinalIgnoreCase) >= 0 ||
                msg.IndexOf("clave única", StringComparison.OrdinalIgnoreCase) >= 0)
                return "La descripción ya existe.";

            return fallback;
        }
    }
}

