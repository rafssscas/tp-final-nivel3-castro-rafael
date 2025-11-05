<%@ Page Title="Home" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TiendaOnline.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* =================================
           ESTILOS TARJETA DE PRODUCTO (Consistente con Default.aspx)
           ================================= */
        .product-catalog-card {
            transition: all .25s ease;
        }

        .product-catalog-card .card-img-top {
            height: 220px; /* Altura unificada */
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
            min-height: 4.5em; /* Espacio para descripción (aprox 3 líneas) */
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
                    src="https://res.cloudinary.com/ds8ezx92r/image/upload/v1761890426/productos/3750f49337c4443ea5d3b095e80cd8a6.jpg"
                    alt="Banner 1"
                    onerror="this.src='https://placehold.co/1600x500/F8F4F0/3D2E3C?text=Sagrada+Madre';" />
                <span class="overlay"></span>
                <div class="caption">
                    <h2 class="h3 mb-1"></h2>
                    <p class="mb-0"></p>
                </div>
            </div>

            <div class="carousel-item">
                <img class="bg-cover"
                    src="https://res.cloudinary.com/ds8ezx92r/image/upload/v1761890488/productos/6ffdd5e0a9cc42448b7da25a2d84e080.jpg"
                    alt="Banner 2"
                    onerror="this.src='https://placehold.co/1600x500/F8F4F0/3D2E3C?text=Productos+Destacados';" />
                <span class="overlay"></span>
                <div class="caption">
                    <h2 class="h3 mb-1"></h2>
                    <p class="mb-0"></p>
                </div>
            </div>

            <div class="carousel-item">
                <img class="bg-cover"
                    src="https://res.cloudinary.com/ds8ezx92r/image/upload/v1761890532/productos/1004cb8d08c74a8db03b8b172958e79a.jpg"
                    alt="Banner 3"
                    onerror="this.src='https://placehold.co/1600x500/F8F4F0/3D2E3C?text=Cat%C3%A1logo+Completo';" />
                <span class="overlay"></span>
                <div class="caption">
                    <h2 class="h3 mb-1"></h2>
                    <p class="mb-0"></p>
                </div>
            </div>
        </div>

        <button class="carousel-control-prev" type="button" data-bs-target="#hero" data-bs-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Anterior</span>
        </button>
        <button class="carousel-control-next" type="button" data-bs-target="#hero" data-bs-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="visually-hidden">Siguiente</span>
        </button>
    </div>

    <!-- ====== TOP 4 MÁS VENDIDOS ====== -->
    <section class="mb-4">
        <div class="d-flex align-items-center justify-content-between mb-2">
            <h3 class="h5 mb-0" style="color: var(--sm-primary-accent); font-weight: 600;">Productos destacados</h3>
            <!-- Botón actualizado -->
            <a runat="server" href="~/Default.aspx" class="btn btn-sm btn-secondary">Ver catálogo</a>
        </div>

        <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-4 g-3">
            <asp:Repeater ID="repTopVendidos" runat="server">
                <ItemTemplate>
                    <div class="col">
                        <!-- Estilos de tarjeta unificados -->
                        <div class="card h-100 shadow-sm product-catalog-card">
                            <img class="card-img-top"
                                src='<%# Eval("ImagenUrl") %>'
                                onerror="this.src='https://placehold.co/600x400/F8F4F0/3D2E3C?text=Sagrada+Madre';"
                                alt='<%# Eval("Nombre") %>' />
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
                                <div class="mb-2">
                                    <!-- Badges actualizados -->
                                    <span class="badge badge-otani me-1"><%# Eval("Marca.Descripcion") %></span>
                                    <span class="badge badge-otani alt"><%# Eval("Categoria.Descripcion") %></span>
                                </div>
                                <p class="card-text text-muted truncate-2"><%# Eval("Descripcion") %></p>
                                <div class="mt-auto d-flex justify-content-between align-items-center">
                                    <!-- Precio actualizado -->
                                    <span class="price">
                                        <%# Container.DataItem != null && Eval("Precio") != null ? "$ " + string.Format("{0:N2}", Eval("Precio")) : "Consultar" %>
                                    </span>
                                    <!-- Botón actualizado -->
                                    <a class="btn btn-sm btn-otani" href='Detalle.aspx?id=<%# Eval("Id") %>'>Ver</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </section>

    <!-- ====== BUSCADOR / FILTROS (con UpdatePanel) ====== -->
<asp:UpdatePanel ID="upFiltros" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
    <!-- ⚠️ NUEVO: panel con DefaultButton -->
    <asp:Panel ID="pnlFiltros" runat="server" DefaultButton="btnFiltrar">
      <div class="card mb-3 shadow-sm">
        <div class="card-body">
          <div class="row g-3 align-items-end">
            <div class="col-lg-4 col-md-12">
              <label class="form-label">Buscar</label>
              <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control" placeholder="Nombre, código o descripción..." />
            </div>
            <div class="col-lg-3 col-md-6">
              <label class="form-label">Marca</label>
              <asp:DropDownList runat="server" ID="ddlMarca" CssClass="form-select"
                  AutoPostBack="true" OnSelectedIndexChanged="btnFiltrar_Click" />
            </div>
            <div class="col-lg-3 col-md-6">
              <label class="form-label">Categoría</label>
              <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select"
                  AutoPostBack="true" OnSelectedIndexChanged="btnFiltrar_Click" />
            </div>
            <div class="col-lg-2 col-md-12 d-flex gap-2">
              <asp:Button runat="server" ID="btnFiltrar" CssClass="btn btn-otani w-100" Text="Filtrar" OnClick="btnFiltrar_Click" />
              <asp:Button runat="server" ID="btnLimpiar" CssClass="btn btn-secondary w-100" Text="Limpiar" OnClick="btnLimpiar_Click" />
            </div>
          </div>
        </div>
      </div>
    </asp:Panel>
  </ContentTemplate>
</asp:UpdatePanel>


    <!-- ====== PRODUCTOS (con UpdatePanel) ====== -->
    <asp:UpdatePanel ID="upContenido" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ListView ID="lvProductos" runat="server" OnPagePropertiesChanging="lvProductos_PagePropertiesChanging">
                <LayoutTemplate>
                    <div class="lv-row">
                        <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>
                    </div>
                    <div class="d-flex justify-content-center mt-3">
                        <asp:DataPager ID="dp" runat="server" PagedControlID="lvProductos" PageSize="12" CssClass="pager">
                            <Fields>
                                
                                <asp:NumericPagerField ButtonType="Link"
                                    NextPageText="»" PreviousPageText="«"
                                    CurrentPageLabelCssClass="btn btn-otani disabled me-1"
                                    NumericButtonCssClass="btn btn-secondary me-1 page-link" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                </LayoutTemplate>

                <GroupTemplate>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder" />
                </GroupTemplate>

                <ItemTemplate>
                    <div class="lv-col">
                        <!-- Estilos de tarjeta unificados -->
                        <div class="card h-100 shadow-sm product-catalog-card">
                            <img class="card-img-top"
                                src='<%# Eval("ImagenUrl") %>'
                                onerror="this.src='https://placehold.co/600x400/F8F4F0/3D2E3C?text=Sagrada+Madre';"
                                alt='<%# Eval("Nombre") %>' />
                            <div class="card-body d-flex flex-column">
                                <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
                                <div class="mb-2">
                                    <!-- Badges actualizados -->
                                    <span class="badge badge-otani me-1"><%# Eval("Marca.Descripcion") %></span>
                                    <span class="badge badge-otani alt"><%# Eval("Categoria.Descripcion") %></span>
                                </div>
                                <p class="card-text text-muted truncate-2"><%# Eval("Descripcion") %></p>
                                <div class="mt-auto d-flex justify-content-between align-items-center">
                                    <!-- Precio actualizado -->
                                    <span class="price">
                                        <%# Container.DataItem != null && Eval("Precio") != null ? "$ " + string.Format("{0:N2}", Eval("Precio")) : "Consultar" %>
                                    </span>
                                    <!-- Botón actualizado -->
                                    <a class="btn btn-sm btn-otani" href='Detalle.aspx?id=<%# Eval("Id") %>'>Ver detalle</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>

                <EmptyDataTemplate>
                    <div class="alert alert-info">No se encontraron productos con los filtros aplicados.</div>
                </EmptyDataTemplate>
            </asp:ListView>
        </ContentTemplate>
        
        
    </asp:UpdatePanel>

    <!-- 
      =================================
      SCRIPT PARA FILTRO AUTOMÁTICO (Igual que en Default.aspx)
      =================================
    -->
    <script type="text/javascript">
        var txtBuscarID = '<%= txtBuscar.ClientID %>';
        var btnFiltrarID = '<%= btnFiltrar.ClientID %>';
        var searchTimeout;

        function triggerFilterPostback() {
            // No usamos .click() porque podría no estar inicializado por jQuery
            // __doPostBack es la forma segura de ASP.NET
            __doPostBack(btnFiltrarID, '');
        }

        function pageLoad(sender, args) {
            // 1. FILTRO AUTOMÁTICO POR TEXTO (keyup con debounce)
            // Usamos .off() para evitar doble bindeo en postbacks
            $('#' + txtBuscarID).off('keyup.filter').on('keyup.filter', function (e) {
                clearTimeout(searchTimeout);
                var query = $(this).val();
                
                if (e.keyCode === 13) {
                    triggerFilterPostback();
                    return;
                }

                if (query.length === 0 || query.length >= 3) {
                    searchTimeout = setTimeout(function () {
                        triggerFilterPostback();
                    }, 600); // 600ms de espera
                }
            });

            // 2. FILTRO AUTOMÁTICO AL SALIR (blur)
            $('#' + txtBuscarID).off('blur.filter').on('blur.filter', function () {
                triggerFilterPostback();
            });
        }

        // Carga inicial
        $(document).ready(function() {
            pageLoad(null, null);
        });

        // Cargas de UpdatePanel
        if (typeof (Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageLoad);
        }
    </script>
</asp:Content>
