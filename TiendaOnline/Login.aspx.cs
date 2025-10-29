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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Opcional: si ya está logueado, redirigir
            // if (Seguridad.sesionActiva(Session["user"]))
            //     Response.Redirect("Default.aspx", false);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new Users
                {
                    Nombre = txtNombre.Text.Trim(),
                    Pass = txtPassword.Text
                };

                var negocio = new UsersNegocio();
                if (negocio.Login(user))
                {
                    Session["user"] = user;
                    Session["admin"] = user.Admin;

                    // --- INICIO DE MEJORA ---
                    // Redirigir a la URL de retorno si existe (ej. venía de "Favoritos")
                    var returnUrl = Session["returnUrl"] as string;
                    Session["returnUrl"] = null; // Limpiar la URL de retorno
                    Response.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "Default.aspx" : returnUrl, false);
                    // --- FIN DE MEJORA ---
                }
                else
                {
                    Page.Validators.Add(new CustomValidator
                    {
                        IsValid = false,
                        ErrorMessage = "Usuario o contraseña inválidos."
                    });
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // Ignorar excepción de Response.Redirect
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

    }
}
