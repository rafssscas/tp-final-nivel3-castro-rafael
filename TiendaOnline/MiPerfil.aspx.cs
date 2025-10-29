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
            else
                // Asegurarse de que el placeholder se muestre si no hay imagen
                imgFoto.ImageUrl = "https://placehold.co/200x200/F8F4F0/3D2E3C?text=Avatar";
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
                neg.ActualizarPerfil(u);      // sp_Users_ActualizarPerfil (Nombre, Apellido, UrlImagenPerfil)

                UsuarioActual = u;            // refrescamos sesión

                // --- INICIO DE MODIFICACIÓN ---
                // Mensaje de éxito con clases de alerta
                lblMsgPerfil.CssClass = "alert alert-success d-block";
                lblMsgPerfil.Text = "Perfil actualizado correctamente.";
                lblMsgPerfil.Visible = true;
                // --- FIN DE MODIFICACIÓN ---

                ActualizarAvatarEnMaster(u.UrlImagenPerfil);
            }
            catch (Exception ex)
            {
                // --- INICIO DE MODIFICACIÓN ---
                // Mensaje de error con clases de alerta
                lblMsgPerfil.CssClass = "alert alert-danger d-block";
                lblMsgPerfil.Text = "No se pudo actualizar el perfil.";
                lblMsgPerfil.Visible = true;
                // --- FIN DE MODIFICACIÓN ---
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
                    // --- INICIO DE MODIFICACIÓN ---
                    lblMsgAvatar.CssClass = "alert alert-danger d-block mt-2";
                    lblMsgAvatar.Text = "Seleccioná un archivo.";
                    lblMsgAvatar.Visible = true;
                    // --- FIN DE MODIFICACIÓN ---
                    return;
                }

                var ext = Path.GetExtension(fuAvatar.FileName).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    // --- INICIO DE MODIFICACIÓN ---
                    lblMsgAvatar.CssClass = "alert alert-danger d-block mt-2";
                    lblMsgAvatar.Text = "Formato inválido. Usa .jpg, .jpeg o .png";
                    lblMsgAvatar.Visible = true;
                    // --- FIN DE MODIFICACIÓN ---
                    return;
                }

                if (fuAvatar.PostedFile.ContentLength > (2 * 1024 * 1024)) // 2MB
                {
                    // --- INICIO DE MODIFICACIÓN ---
                    lblMsgAvatar.CssClass = "alert alert-danger d-block mt-2";
                    lblMsgAvatar.Text = "El archivo supera 2MB.";
                    lblMsgAvatar.Visible = true;
                    // --- FIN DE MODIFICACIÓN ---
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
                neg.ActualizarPerfil(u);      // actualiza UrlImagenPerfil

                // Refrescar sesión y UI
                UsuarioActual = u;
                imgFoto.ImageUrl = url;
                ActualizarAvatarEnMaster(url);

                // --- INICIO DE MODIFICACIÓN ---
                lblMsgAvatar.CssClass = "alert alert-success d-block mt-2";
                lblMsgAvatar.Text = "Avatar actualizado.";
                lblMsgAvatar.Visible = true;
                // --- FIN DE MODIFICACIÓN ---
            }
            catch (Exception ex)
            {
                // --- INICIO DE MODIFICACIÓN ---
                lblMsgAvatar.CssClass = "alert alert-danger d-block mt-2";
                lblMsgAvatar.Text = "No se pudo actualizar el avatar.";
                lblMsgAvatar.Visible = true;
                // --- FIN DE MODIFICACIÓN ---
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
