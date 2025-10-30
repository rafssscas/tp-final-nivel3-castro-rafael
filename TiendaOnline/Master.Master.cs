using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TiendaOnline
{
    public partial class Master : System.Web.UI.MasterPage
    {
        // Páginas que solo puede ver un Admin
        private static readonly string[] AdminOnly = new[]
        {
            "Articulos.aspx",
            "FormularioArticulo.aspx",
            "MarcasCategorias.aspx",
            "Categorias.aspx"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            // Avatar por defecto
            imgAvatar.ImageUrl = "https://simg.nicepng.com/png/small/202-2022264_usuario-annimo-usuario-annimo-user-icon-png-transparent.png";

            var user = Session["user"] as Users;

            if (Seguridad.sesionActiva(user))
            {
                lblUser.Text = user.Email;
                if (!string.IsNullOrWhiteSpace(user.UrlImagenPerfil))
                    imgAvatar.ImageUrl = user.UrlImagenPerfil;
            }

            // PROTEGER páginas admin por nombre de archivo
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath); // ej: "Articulos.aspx"
            if (AdminOnly.Contains(currentPage, StringComparer.OrdinalIgnoreCase) && !Seguridad.esAdmin(user))
            {
                Session["returnUrl"] = Request.RawUrl;
                Response.Redirect(ResolveUrl("~/Login.aspx"), false);
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect(ResolveUrl("~/Login.aspx"), false);
        }
    }
}