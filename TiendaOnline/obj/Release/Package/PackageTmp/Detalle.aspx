<%@ Page Title="Detalle de artículo" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="TiendaOnline.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* * Imagen de detalle: 
         * - Respeta la proporción original (height: auto)
         * - Se adapta al 100% del ancho del contenedor.
         * - Tiene un fondo blanco y borde sutil, por si la imagen tiene fondo transparente.
        */
        .img-detalle {
            width: 100%;
            height: auto;
            border-radius: var(--radius-md);
            border: 1px solid var(--sm-border-color);
            background-color: #fff;
        }

        /* Título y Precio con el color de acento principal */
        .detalle-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }

        .price-lg {
            font-size: 1.5rem;
            font-weight: 700;
            color: var(--sm-primary-accent);
        }
        
        /* Botones a ancho completo en móviles (breakpoint 'sm' de Bootstrap) */
        @media (max-width: 575.98px) {
            .btn-mobile-full {
                width: 100%;
                margin-bottom: 0.5rem;
            }
            /* Quitar el margen derecho del primer botón en móvil */
            .btn-mobile-full.me-lg-2 {
                margin-right: 0 !important;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="pnlNotFound" Visible="false" CssClass="alert alert-warning">
        No se encontró el artículo solicitado. Volvé al <a href="Default.aspx" class="link-cambiar">catálogo</a>.
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetalle" Visible="false">
        <div class="row g-4">
            <div class="col-md-6">
                <!-- Placeholder actualizado con los colores de la nueva paleta -->
                <asp:Image ID="imgArticulo" runat="server" CssClass="img-detalle"
                    onerror="this.src='https://placehold.co/800x600/F8F4F0/3D2E3C?text=Sagrada+Madre';" />
            </div>
            <div class="col-md-6">
                <!-- Título con la nueva clase de estilo -->
                <h2 class="mb-1 detalle-title"><asp:Label ID="lblNombre" runat="server" /></h2>
                
                <!-- Badges actualizados a la nueva paleta -->
                <div class="mb-2">
                    <span class="badge badge-otani me-1"><asp:Label ID="lblMarca" runat="server" /></span>
                    <span class="badge badge-otani alt"><asp:Label ID="lblCategoria" runat="server" /></span>
                </div>
                
                <!-- Precio con la nueva clase de estilo -->
                <div class="price-lg mb-3"><asp:Label ID="lblPrecio" runat="server" /></div>
                
                <p class="text-muted"><asp:Label ID="lblDescripcion" runat="server" /></p>

                <!-- Mensaje de feedback (éxito/error) -->
                <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert alert-success d-block mb-2"></asp:Label>

                <!-- 
                  Botones actualizados:
                  - Nuevos colores de la paleta.
                  - Clases 'btn-mobile-full' para mejorar la UX en móvil.
                  - 'me-lg-2' para que el margen solo aplique en escritorio.
                -->
                <asp:Button runat="server" ID="btnFavorito" CssClass="btn btn-otani me-lg-2 btn-mobile-full" Text="Agregar a favoritos" OnClick="btnFavorito_Click" />
                <a class="btn btn-secondary btn-mobile-full" href="Default.aspx">Volver</a>

                <asp:HiddenField ID="hfIdArticulo" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
