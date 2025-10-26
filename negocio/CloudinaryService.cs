using System;
using System.Configuration;
using System.Net;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace negocio
{
    public static class CloudinaryService
    {
        private static readonly Cloudinary _cloud;

        static CloudinaryService()
        {
            // TLS 1.2 por si el SO no lo toma por defecto
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var cloudinaryUrl = ConfigurationManager.AppSettings["CloudinaryUrl"];
            if (string.IsNullOrWhiteSpace(cloudinaryUrl))
                throw new InvalidOperationException("Falta appSettings['CloudinaryUrl'] en Web.config.");

            // ✅ Correcto: construir Cloudinary directamente desde la URL (no Account)
            _cloud = new Cloudinary(cloudinaryUrl);
            _cloud.Api.Secure = true;
            _cloud.Api.Timeout = 60000; // opcional
        }

        /// <summary>
        /// Sube una imagen a Cloudinary y devuelve true/false, la url segura y el mensaje de error (si falla).
        /// </summary>
        public static bool UploadImage(HttpPostedFile file, out string secureUrl, out string error)
        {
            secureUrl = null;
            error = null;

            try
            {
                if (file == null || file.ContentLength == 0)
                {
                    error = "No se recibió archivo.";
                    return false;
                }

                if (file.InputStream.CanSeek) file.InputStream.Position = 0;

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.InputStream),
                    PublicId = "productos/" + Guid.NewGuid().ToString("N") // carpeta lógica
                    // Folder = "productos" // alternativa
                };

                var result = _cloud.Upload(uploadParams);

                if (result == null)
                {
                    error = "Respuesta nula del servicio de Cloudinary.";
                    return false;
                }

                // Cloudinary reporta errores aquí
                if (result.Error != null)
                {
                    error = "Cloudinary error: " + result.Error.Message;
                    return false;
                }

                // Por las dudas, chequeamos status
                if ((int)result.StatusCode >= 400)
                {
                    error = $"Cloudinary status {(int)result.StatusCode} {result.StatusCode}";
                    return false;
                }

                secureUrl = result.SecureUrl?.AbsoluteUri;
                if (string.IsNullOrEmpty(secureUrl))
                {
                    error = "No se obtuvo URL segura.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = "Excepción: " + ex.Message;
                return false;
            }
        }
    }
}
