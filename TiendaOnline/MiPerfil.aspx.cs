using dominio;
using negocio;
using System;
using System.IO;
using System.Web.UI;

namespace TiendaOnline
{
    public partial class MiPerfil : System.Web.UI.Page
    {
        private Users UsuarioActual
        {
            get => Session["user"] as Users;
            set => Session["user"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Seguridad.sesionActiva(Session["user"]))
                {
                    Response.Redirect("~/Login.aspx", false);
                    return;
                }

                if (!IsPostBack)
                    CargarPerfil();
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex) { Session["error"] = ex.ToString(); }
        }

        private void CargarPerfil()
        {
            var u = UsuarioActual;
            if (u == null) return;

            txtNombre.Text = u.Nombre ?? "";
            txtApellido.Text = u.Apellido ?? "";
            txtEmail.Text = u.Email ?? "";
            txtRol.Text = u.Admin ? "Administrador" : "Usuario";

            if (!string.IsNullOrWhiteSpace(u.UrlImagenPerfil))
                imgFoto.ImageUrl = u.UrlImagenPerfil;
        }

        protected void btnGuardarPerfil_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                var u = UsuarioActual;
                if (u == null) return;

                u.Nombre = txtNombre.Text.Trim();
                u.Apellido = txtApellido.Text.Trim();
                // Email no se edita aquí para respetar tu SP
                // UrlImagenPerfil se actualiza desde btnSubirAvatar

                var neg = new UsersNegocio();
                neg.ActualizarPerfil(u);   // sp_Users_ActualizarPerfil (Nombre, Apellido, UrlImagenPerfil)

                UsuarioActual = u;         // refrescamos sesión
                lblMsgPerfil.CssClass = "text-success";
                lblMsgPerfil.Text = "Perfil actualizado correctamente.";
                ActualizarAvatarEnMaster(u.UrlImagenPerfil);
            }
            catch (Exception ex)
            {
                lblMsgPerfil.CssClass = "text-danger";
                lblMsgPerfil.Text = "No se pudo actualizar el perfil.";
                Session["error"] = ex.ToString();
            }
        }

        protected void btnSubirAvatar_Click(object sender, EventArgs e)
        {
            try
            {
                var u = UsuarioActual;
                if (u == null) return;

                if (!fuAvatar.HasFile)
                {
                    lblMsgAvatar.CssClass = "text-danger";
                    lblMsgAvatar.Text = "Seleccioná un archivo.";
                    return;
                }

                var ext = Path.GetExtension(fuAvatar.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    lblMsgAvatar.CssClass = "text-danger";
                    lblMsgAvatar.Text = "Formato inválido. Usa .jpg, .jpeg o .png";
                    return;
                }

                if (fuAvatar.PostedFile.ContentLength > (2 * 1024 * 1024))
                {
                    lblMsgAvatar.CssClass = "text-danger";
                    lblMsgAvatar.Text = "El archivo supera 2MB.";
                    return;
                }

                // Guardado local: ~/Uploads/Avatares/
                var folder = Server.MapPath("~/Uploads/Avatares/");
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                var fileName = $"usr_{u.Id}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
                fuAvatar.SaveAs(Path.Combine(folder, fileName));

                var url = ResolveUrl($"~/Uploads/Avatares/{fileName}");

                // Persistir en DB reutilizando tu método de perfil
                u.UrlImagenPerfil = url;
                var neg = new UsersNegocio();
                neg.ActualizarPerfil(u);   // actualiza UrlImagenPerfil

                // Refrescar sesión y UI
                UsuarioActual = u;
                imgFoto.ImageUrl = url;
                ActualizarAvatarEnMaster(url);

                lblMsgAvatar.CssClass = "text-success";
                lblMsgAvatar.Text = "Avatar actualizado.";
            }
            catch (Exception ex)
            {
                lblMsgAvatar.CssClass = "text-danger";
                lblMsgAvatar.Text = "No se pudo actualizar el avatar.";
                Session["error"] = ex.ToString();
            }
        }

        private void ActualizarAvatarEnMaster(string url)
        {
            try
            {
                var img = Master.FindControl("imgAvatar") as System.Web.UI.WebControls.Image;
                if (img != null && !string.IsNullOrWhiteSpace(url))
                    img.ImageUrl = url;
            }
            catch { /* noop */ }
        }
    }
}
