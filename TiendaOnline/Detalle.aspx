<%@ Page Title="Detalle de artículo" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Detalle.aspx.cs" Inherits="TiendaOnline.Detalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .img-detalle{ width:100%; max-height:420px; object-fit:cover; border-radius:.75rem; }
        .badge-soft{ background:#eef2ff; color:#4f46e5; }
        .price-lg{ font-size:1.5rem; font-weight:700; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="pnlNotFound" Visible="false" CssClass="alert alert-warning">
        No se encontró el artículo solicitado. Volvé al <a href="Default.aspx">catálogo</a>.
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlDetalle" Visible="false">
        <div class="row g-4">
            <div class="col-md-6">
                <asp:Image ID="imgArticulo" runat="server" CssClass="img-detalle"
                    onerror="this.src='https://placehold.co/800x600?text=Sin+Imagen';" />
            </div>
            <div class="col-md-6">
                <h2 class="mb-1"><asp:Label ID="lblNombre" runat="server" /></h2>
                <div class="mb-2">
                    <span class="badge badge-soft me-1"><asp:Label ID="lblMarca" runat="server" /></span>
                    <span class="badge bg-light text-dark"><asp:Label ID="lblCategoria" runat="server" /></span>
                </div>
                <div class="price-lg mb-3"><asp:Label ID="lblPrecio" runat="server" /></div>
                <p class="text-muted"><asp:Label ID="lblDescripcion" runat="server" /></p>

                <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="text-success d-block mb-2"></asp:Label>

                <asp:Button runat="server" ID="btnFavorito" CssClass="btn btn-primary me-2" Text="Agregar a favoritos" OnClick="btnFavorito_Click" />
                <a class="btn btn-outline-secondary" href="Default.aspx">Volver</a>

                <asp:HiddenField ID="hfIdArticulo" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
