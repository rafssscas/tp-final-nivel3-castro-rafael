using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using dominio; // Asegurate de tener AttachmentDto en el namespace dominio

namespace negocio
{
    public class EmailService
    {
        private MailMessage email;
        private readonly SmtpClient server;

        public EmailService()
        {
            string mode = ConfigurationManager.AppSettings["EmailMode"] ?? "Relay";
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "587");

            server = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true
            };

            // En Relay NO seteamos credenciales (auth por IP)
            if (!string.Equals(mode, "Relay", StringComparison.OrdinalIgnoreCase))
            {
                server.Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["EmailUser"],
                    ConfigurationManager.AppSettings["EmailPass"]
                );
            }
        }

        /// <summary>
        /// Arma un correo simple sin adjuntos.
        /// </summary>
        public void ArmarCorreo(string emailDestino, string asunto, string cuerpoHtml)
        {
            email = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["FromAddress"]),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            email.To.Add(emailDestino);
        }

        /// <summary>
        /// Arma un correo con lista de adjuntos.
        /// </summary>
        public void ArmarCorreoConAdjuntos(string emailDestino, string asunto, string cuerpoHtml, List<AttachmentDto> adjuntos)
        {
            email = new MailMessage
            {
                From = new MailAddress(ConfigurationManager.AppSettings["FromAddress"]),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            email.To.Add(emailDestino);

            // Cargar adjuntos en memoria
            foreach (var a in adjuntos ?? new List<AttachmentDto>())
            {
                if (a.Content != null && a.Content.Length > 0)
                {
                    var stream = new MemoryStream(a.Content);
                    var attachment = new Attachment(stream, a.FileName, a.ContentType);
                    email.Attachments.Add(attachment);
                }
            }
        }

        /// <summary>
        /// Envía el correo previamente armado (sincrónico).
        /// </summary>
        public void EnviarEmail()
        {
            try
            {
                server.Send(email);
            }
            catch (SmtpException smtpEx)
            {
                throw new Exception($"Error SMTP al enviar correo: {smtpEx.Message}", smtpEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error general al enviar correo: {ex.Message}", ex);
            }
            finally
            {
                // Liberar recursos de los adjuntos
                email?.Dispose();
            }
        }
    }
}
