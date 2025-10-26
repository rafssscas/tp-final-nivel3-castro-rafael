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
    public partial class Registro : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new Users
                {
                    Nombre = txtNombre.Text.Trim(),
                    Pass = txtPassword.Text,
                    // Si tenés txtEmail en el form, tomalo; si no, dejalo null
                    Email = string.IsNullOrWhiteSpace(Request.Form["txtEmail"]) ? null : Request.Form["txtEmail"].Trim(),
                    Apellido = null,
                    UrlImagenPerfil = "https://picsum.photos/seed/newuser/200",
                    Admin = false
                };

                var negocio = new UsersNegocio();

                // Validar nombre único
                if (negocio.NombreYaRegistrado(user.Nombre))
                {
                    Page.Validators.Add(new CustomValidator
                    {
                        IsValid = false,
                        ErrorMessage = "El usuario (nombre) ya está registrado."
                    });
                    return;
                }

                // Si capturás email y querés validarlo también:
                if (!string.IsNullOrWhiteSpace(user.Email) && negocio.EmailYaRegistrado(user.Email))
                {
                    Page.Validators.Add(new CustomValidator
                    {
                        IsValid = false,
                        ErrorMessage = "El email ya está registrado."
                    });
                    return;
                }

                user.Id = negocio.Registrar(user);

                Session["user"] = user;
                Session["admin"] = user.Admin;

                Response.Redirect("Default.aspx", false);
            }
            catch (Exception ex)
            {
                Session["error"] = ex.ToString();
                Response.Redirect("Error.aspx", false);
            }
        }
    }

    
}