<%@ Page Title="Home" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TiendaOnline.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        /* ====== Carrusel a ancho completo ====== */
        .hero-carousel .carousel-item{
            height: 340px;                  /* ajusta a gusto */
            background: #111;
            color:#fff;
            position: relative;
        }
        .hero-carousel .bg-cover{
            position:absolute; inset:0;
            width:100%; height:100%;
            object-fit:cover;
            opacity:.9;
        }
        .hero-carousel .overlay{
            position:absolute; inset:0;
            background:linear-gradient(180deg,rgba(0,0,0,.35),rgba(0,0,0,.55));
        }
        .hero-carousel .caption{
            position:absolute; inset-inline:0; bottom:18px;
            text-align:center; color:#fff;
            text-shadow:0 2px 10px rgba(0,0,0,.5);
        }

        /* ====== Cards y listas ====== */
        .badge-soft{ background:#eef2ff; color:#4f46e5; }
        .card-img-top{
            width:100%; height:180px; object-fit:contain;
            background:#f8f9fa; border-top-left-radius:.5rem; border-top-right-radius:.5rem;
        }
        .truncate-2{ display:-webkit-box; -webkit-line-clamp:2; -webkit-box-orient:vertical; overflow:hidden; }

        /* Grid 4 por fila en productos (ListView con GroupTemplate) */
        .lv-row{ display:flex; flex-wrap:wrap; margin:-.75rem; }
        .lv-col{ width:100%; padding:.75rem; }
        @media (min-width:576px){ .lv-col{ width:50%; } }
        @media (min-width:992px){ .lv-col{ width:25%; } } /* 4 por fila */

        /* DataPager */
        .pager .page-link{ min-width:42px; text-align:center; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- ====== CARRUSEL PRINCIPAL ====== -->
    <div id="hero" class="carousel slide hero-carousel mb-4 shadow-sm" data-bs-ride="carousel" data-bs-interval="3000">
        <div class="carousel-indicators">
            <button type="button" data-bs-target="#hero" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
            <button type="button" data-bs-target="#hero" data-bs-slide-to="1" aria-label="Slide 2"></button>
            <button type="button" data-bs-target="#hero" data-bs-slide-to="2" aria-label="Slide 3"></button>
        </div>
        <div class="carousel-inner">
            <div class="carousel-item active">
                <img class="bg-cover"
                    src="https://res.cloudinary.com/ds8ezx92r/image/upload/v1760165780/cld-sample-2.jpg"
                    alt="Banner 1"
                    onerror="this.src='https://placehold.co/1600x500?text=Banner+1';" />

                <span class="overlay"></span>
                <div class="caption">
                    <h2 class="h3 mb-1">Bienvenido a Tienda Online</h2>
                    <p class="mb-0">Ofertas y novedades todas las semanas</p>
                </div>
            </div>
            <div class="carousel-item">
                <img class="bg-cover" src="https://drive.google.com/uc?export=view&id=1YDL58CtgLifzc9zDbV2M5Y35VoZ4H_5K" alt="Banner 2" />
                <span class="overlay"></span>
                <div class="caption">
                    <h2 class="h3 mb-1">Los más vendidos</h2>
                    <p class="mb-0">Elegidos por nuestros clientes</p>
                </div>
            </div>
            <div class="carousel-item">
                <img class="bg-cover" src="https://drive.usercontent.google.com/download?id=1CG8LSqUv45HFaHEtMqQq0qFBJY8Htm7x&export=view" alt="Banner 3" />
                <span class="overlay"></span>
                <div class="caption">
                    <h2 class="h3 mb-1">Catálogo actualizado</h2>
                    <p class="mb-0">Buscá por marca y categoría</p>
                </div>
            </div>
        </div>
        <button class="carousel-control-prev" type="button" data-bs-target="#hero" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span><span class="visually-hidden">Anterior</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#hero" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span><span class="visually-hidden">Siguiente</span>
        </button>
    </div>

    <!-- ====== TOP 4 MÁS VENDIDOS ====== -->
    <section class="mb-4">
        <div class="d-flex align-items-center justify-content-between mb-2">
            <h3 class="h5 mb-0">Más vendidos</h3>
            <a runat="server" href="~/Default.aspx" class="btn btn-sm btn-outline-primary">Ver catálogo</a>
        </div>
        <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4 g-3">
            <asp:Repeater ID="repTopVendidos" runat="server">
                <ItemTemplate>
                    <div class="col">
                        <div class="card h-100 shadow-sm">
                            <img class="card-img-top" src='<%# Eval("ImagenUrl") %>' onerror="this.src='https://placehold.co/600x400?text=Sin+Imagen';" alt='<%# Eval("Nombre") %>' />
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
                                <div class="mb-2">
                                    <span class="badge badge-soft me-1"><%# Eval("Marca.Descripcion") %></span>
                                    <span class="badge bg-light text-dark"><%# Eval("Categoria.Descripcion") %></span>
                                </div>
                                <p class="card-text text-muted truncate-2"><%# Eval("Descripcion") %></p>
                                <div class="mt-auto d-flex justify-content-between align-items-center">
                                    <span class="fw-bold">
                                        <%# Container.DataItem != null && Eval("Precio") != null ? "$ " + string.Format("{0:N2}", Eval("Precio")) : "Consultar" %>
                                    </span>
                                    <a class="btn btn-sm btn-outline-primary" href='Detalle.aspx?id=<%# Eval("Id") %>'>Ver</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>

    <!-- ====== BUSCADOR / FILTROS ====== -->
    <div class="card mb-3 shadow-sm">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-md-4">
                    <label class="form-label">Buscar</label>
                    <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Nombre, código o descripción..." />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Marca</label>
                    <asp:DropDownList runat="server" ID="ddlMarca" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Categoría</label>
                    <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select"></asp:DropDownList>
                </div>
                <div class="col-md-2 d-flex gap-2">
                    <asp:Button runat="server" ID="btnFiltrar" CssClass="btn btn-primary w-100" Text="Filtrar" OnClick="btnFiltrar_Click" />
                    <asp:Button runat="server" ID="btnLimpiar" CssClass="btn btn-outline-secondary w-100" Text="Limpiar" OnClick="btnLimpiar_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- ====== PRODUCTOS (4 por fila, máx. 3 filas por página = 12 items) ====== -->
    <asp:ListView ID="lvProductos" runat="server" OnPagePropertiesChanging="lvProductos_PagePropertiesChanging">
        <LayoutTemplate>
            <div class="lv-row">
                <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>
            </div>
            <div class="d-flex justify-content-center mt-3">
                <asp:DataPager ID="dp" runat="server" PagedControlID="lvProductos" PageSize="12" CssClass="pager">
                    <Fields>
                        <asp:NumericPagerField ButtonType="Link" NextPageText="»" PreviousPageText="«" CurrentPageLabelCssClass="btn btn-primary disabled"
                                                NumericButtonCssClass="btn btn-outline-primary me-1 page-link" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>

        <GroupTemplate>
            <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
        </GroupTemplate>

        <ItemTemplate>
            <div class="lv-col">
                <div class="card h-100 shadow-sm">
                    <img class="card-img-top" src='<%# Eval("ImagenUrl") %>' onerror="this.src='https://placehold.co/600x400?text=Sin+Imagen';" alt='<%# Eval("Nombre") %>' />
                    <div class="card-body d-flex flex-column">
                        <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
                        <div class="mb-2">
                            <span class="badge badge-soft me-1"><%# Eval("Marca.Descripcion") %></span>
                            <span class="badge bg-light text-dark"><%# Eval("Categoria.Descripcion") %></span>
                        </div>
                        <p class="card-text text-muted truncate-2"><%# Eval("Descripcion") %></p>
                        <div class="mt-auto d-flex justify-content-between align-items-center">
                            <span class="fw-bold">
                                <%# Container.DataItem != null && Eval("Precio") != null ? "$ " + string.Format("{0:N2}", Eval("Precio")) : "Consultar" %>
                            </span>
                            <a class="btn btn-sm btn-outline-primary" href='Detalle.aspx?id=<%# Eval("Id") %>'>Ver detalle</a>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>

        <EmptyDataTemplate>
            <div class="alert alert-info">No se encontraron productos con los filtros aplicados.</div>
        </EmptyDataTemplate>
    </asp:ListView>
</asp:Content>
