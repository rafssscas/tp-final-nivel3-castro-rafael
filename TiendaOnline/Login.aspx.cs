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
            //if (Seguridad.sesionActiva(Session["user"]))
              //  Response.Redirect("Default.aspx", false);
        }

        /*protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var email = txtEmail.Text?.Trim();
                var pass = txtPassword.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
                {
                    ShowError("Debés completar ambos campos.");
                    return;
                }

                var usersNeg = new UsersNegocio();
                var u = new Users { Email = email, Pass = pass };

                if (usersNeg.Login(u))
                {
                    Session["user"] = u;
                    Session["admin"] = u.Admin;

                    // Si guardaste a dónde quería ir (desde Master), redirigí ahí
                    var returnUrl = Session["returnUrl"] as string;
                    Session["returnUrl"] = null;
                    Response.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "Default.aspx" : returnUrl, false);
                }
                else
                {
                    ShowError("Usuario o contraseña inválidos.");
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignorar redirect
            }
            catch (Exception)
            {
                ShowError("Ocurrió un error al iniciar sesión.");
                // Si querés debug: Session["error"] = ex.ToString(); Response.Redirect("Error.aspx");
            }
        }*/


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
                    Response.Redirect("Default.aspx", false);
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
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }

     
    }
}