<%@ Page Title="Mis Favoritos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Favoritos.aspx.cs" Inherits="TiendaOnline.Favoritos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* =================================
           ESTILOS TARJETA DE PRODUCTO (Igual a Default.aspx)
           ================================= */
        .product-catalog-card {
            transition: all .25s ease;
        }

        .product-catalog-card .card-img-top {
            height: 220px; /* Misma altura que el catálogo */
            object-fit: cover;
            border-bottom: 1px solid var(--sm-border-color);
        }

        .product-catalog-card .price {
            font-size: 1.25rem;
            font-weight: 700;
            color: var(--sm-primary-accent);
        }
        
        .product-catalog-card .card-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }
        
        .product-catalog-card .card-text {
            min-height: 4.5em; /* Espacio para descripción */
        }

        @media (max-width: 767.98px) {
            .product-catalog-card .card-img-top {
                height: 250px;
            }
        }
        @media (max-width: 575.98px) {
            .product-catalog-card .card-img-top {
                height: 200px;
            }
        }

        /* El .empty ya está en el master css, pero lo dejamos por si acaso */
        .empty{ background:#fff; border:1px dashed var(--sm-border-color); border-radius:.75rem; padding:2rem; color: var(--gray-600); }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="mb-3">Mis Favoritos</h2>

    <!-- Panel para cuando no hay favoritos -->
    <asp:Panel runat="server" ID="pnlVacio" CssClass="empty mb-4" Visible="false">
        <p class="mb-0">No tenés artículos en favoritos. Volvé al <a href="Default.aspx" class="link-cambiar">catálogo</a> y agregá algunos.</p>
    </asp:Panel>

    <!-- Repeater con tarjetas de favoritos -->
    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
        <asp:Repeater runat="server" ID="repFavoritos" OnItemCommand="repFavoritos_ItemCommand">
            <ItemTemplate>
                <div class="col">
                    <!-- Clase 'product-catalog-card' añadida para estilos consistentes -->
                    <div class="card h-100 shadow-sm product-catalog-card">
                        <img class="card-img-top"
                            src='<%# Eval("Articulo.ImagenUrl") %>'
                            onerror="this.src='https://placehold.co/600x400/F8F4F0/3D2E3C?text=Sagrada+Madre';"
                            alt='<%# Eval("Articulo.Nombre") %>' />
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title mb-1"><%# Eval("Articulo.Nombre") %></h5>
                            <p class="card-text text-muted truncate-2"><%# Eval("Articulo.Descripcion") %></p>
                            <div class="mt-auto d-flex justify-content-between align-items.center">
                                <!-- Clase 'price' añadida para estilo de precio -->
                                <span class="price">
                                    <%# Eval("Articulo.Precio") == null ? "Consultar" : "$ " + string.Format("{0:N2}", Eval("Articulo.Precio")) %>
                                </span>
                                <div class="d-flex gap-2">
                                    <!-- Botón 'Ver detalle' actualizado -->
                                    <a class="btn btn-sm btn-otani" href='Detalle.aspx?id=<%# Eval("Articulo.Id") %>'>Ver detalle</a>
                                    <!-- Botón 'Quitar' actualizado -->
                                    <asp:Button runat="server" ID="btnQuitar" CssClass="btn btn-sm btn-delete"
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
