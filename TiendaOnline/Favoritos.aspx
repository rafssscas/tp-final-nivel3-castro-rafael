<%@ Page Title="Mis Favoritos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="TiendaOnline.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Sin estilos inline: todo lo toma de Content/site.css -->
    <style>
        .product-catalog-card .card-img-top { height: 220px; }
        @media (max-width: 767.98px){ .product-catalog-card .card-img-top{ height:250px; } }
        @media (max-width: 575.98px){ .product-catalog-card .card-img-top{ height:200px; } }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-3">Mis Favoritos</h2>

    <!-- Panel cuando no hay favoritos -->
    <asp:Panel runat="server" ID="pnlVacio" CssClass="empty mb-4" Visible="false">
        <p class="mb-0">No tenés artículos en favoritos. Volvé al <a href="Default.aspx" class="link-cambiar">catálogo</a> y agregá algunos.</p>
    </asp:Panel>

    <!-- Repeater de favoritos (usa un ViewModel plano para evitar nulls) -->
    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
        <asp:Repeater runat="server" ID="repFavoritos" OnItemCommand="repFavoritos_ItemCommand">
            <ItemTemplate>
                <div class="col">
                    <div class="card h-100 shadow-sm product-card product-catalog-card">
                        <img class="card-img-top img-contain-180"
                             src='<%# Eval("ImagenUrl") %>'
                             onerror="this.src='https://placehold.co/600x400?text=Sin+Imagen';"
                             alt='<%# Eval("Nombre") %>' />
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
                            <p class="card-text text-muted truncate-2"><%# Eval("Descripcion") %></p>
                            <div class="mt-auto d-flex justify-content-between align-items-center">
                                <span class="price"><%# Eval("PrecioTexto") %></span>
                                <div class="d-flex gap-2">
                                    <a class="btn btn-sm btn-otani" href='Detalle.aspx?id=<%# Eval("IdArticulo") %>'>Ver detalle</a>
                                    <asp:Button runat="server" ID="btnQuitar" CssClass="btn btn-sm btn-delete"
                                        Text="Quitar" CommandName="Quitar" CommandArgument='<%# Eval("IdFavorito") %>' />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
