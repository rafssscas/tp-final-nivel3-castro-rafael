<%@ Page Title="Mis Favoritos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="TiendaOnline.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card-img-top{ height:160px; object-fit:cover; }
        .empty{ background:#fff; border:1px dashed #cbd5e1; border-radius:.75rem; padding:2rem; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-3">Mis Favoritos</h2>

    <asp:Panel runat="server" ID="pnlVacio" CssClass="empty mb-4" Visible="false">
        <p class="mb-0">No tenés artículos en favoritos. Volvé al <a href="Default.aspx">catálogo</a> y agregá algunos.</p>
    </asp:Panel>

 
    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
    <asp:Repeater runat="server" ID="repFavoritos" OnItemCommand="repFavoritos_ItemCommand">
        <ItemTemplate>
            <div class="col">
                <div class="card h-100 shadow-sm">
                    <img class="card-img-top"
                         src='<%# Eval("Articulo.ImagenUrl") %>'
                         onerror="this.src='https://placehold.co/600x400?text=Sin+Imagen';"
                         alt='<%# Eval("Articulo.Nombre") %>' />
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title mb-1"><%# Eval("Articulo.Nombre") %></h5>
                        <p class="card-text text-muted"><%# Eval("Articulo.Descripcion") %></p>
                        <div class="mt-auto d-flex justify-content-between align-items-center">
                            <span class="fw-bold">
                                <%# Eval("Articulo.Precio") == null ? "Consultar" : "$ " + string.Format("{0:N2}", Eval("Articulo.Precio")) %>
                            </span>
                            <div class="d-flex gap-2">
                                <a class="btn btn-sm btn-outline-primary" href='Detalle.aspx?id=<%# Eval("Articulo.Id") %>'>Ver detalle</a>
                                <asp:Button runat="server" ID="btnQuitar" CssClass="btn btn-sm btn-outline-danger"
                                    Text="Quitar" CommandName="Quitar" CommandArgument='<%# Eval("Id") %>' />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>


</asp:Content>
