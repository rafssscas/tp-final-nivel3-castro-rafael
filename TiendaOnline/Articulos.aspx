<%@ Page Title="Artículos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="Articulos.aspx.cs" Inherits="TiendaOnline.Articulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
       
        /* Oculta la vista de tarjetas (ListView) en escritorio (lg y más grande) */
        @media (min-width: 992px) {
            .d-mobile-only {
                display: none;
            }
        }

        /* Oculta la vista de tabla (GridView) en móvil (hasta lg) */
        @media (max-width: 991.98px) {
            .d-desktop-only {
                display: none;
            }
        }

        /* =================================
           ESTILOS VISTA MÓVIL (TARJETAS)
           ================================= */

        /* Estilos para la tarjeta de artículo en móvil */
        .admin-card {
            background-color: var(--sm-card-background);
            border: 1px solid var(--sm-border-color);
            border-radius: var(--radius-md);
            box-shadow: var(--shadow-sm);
            margin-bottom: 1rem;
            padding: 1rem;
            display: flex;
            flex-direction: column;
            gap: 0.75rem;
        }

        .admin-card-header {
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
        }
        .admin-card-header h5 {
                font-weight: 700;
                color: var(--sm-primary-accent);
                margin: 0;
        }
        .admin-card-header .price {
                font-weight: 700;
                color: var(--sm-text-dark);
                font-size: 1.1rem;
                white-space:nowrap;
        }
        
        .admin-card-body .code {
            font-weight: 600;
            color: var(--sm-secondary-accent); /* Ocre */
            font-size: 0.9rem;
            display: block;
            margin-bottom: 0.25rem;
        }

        .admin-card-body .description {
            color: var(--gray-600);
            font-size: 0.95rem;
            /* Aplicamos el truncado de 2 líneas */
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
            min-height: 2.8em; /* Aprox 2 líneas */
        }

        .admin-card-footer {
            display: flex;
            gap: 0.5rem;
            border-top: 1px solid var(--sm-border-color);
            padding-top: 0.75rem;
            margin-top: 0.25rem;
        }
        /* Hacemos que los botones ocupen el espacio disponible */
        .admin-card-footer .btn {
             flex-grow: 1; 
             text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="h4 mb-0">Administrar artículos</h2>
        <!-- Botón actualizado a la nueva paleta -->
        <a href="FormularioArticulo.aspx" class="btn btn-otani">Nuevo artículo</a>
    </div>

    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert d-block"></asp:Label>

   
    <!-- =================================
     UPDATE PANEL PARA FILTROS
     ================================= -->
<asp:UpdatePanel ID="upFiltros" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <!-- 🔑 NUEVO: Panel con DefaultButton para capturar ENTER -->
        <asp:Panel ID="pnlFiltros" runat="server" CssClass="filters-scope" DefaultButton="btnFiltrar">
            <div class="card mb-3 shadow-sm">
                <div class="card-body">
                    <div class="row g-3 align-items-end">
                        <div class="col-lg-4 col-md-12">
                            <label class="form-label">Buscar</label>
                            <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control"
                                         placeholder="Nombre, código o descripción..." />
                        </div>
                        <div class="col-lg-3 col-md-6">
                            <label class="form-label">Marca</label>
                            <!-- AutoPostBack para filtrar al cambiar -->
                            <asp:DropDownList runat="server" ID="ddlMarca" CssClass="form-select"
                                              AutoPostBack="true" OnSelectedIndexChanged="btnFiltrar_Click" />
                        </div>
                        <div class="col-lg-3 col-md-6">
                            <label class="form-label">Categoría</label>
                            <!-- AutoPostBack para filtrar al cambiar -->
                            <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select"
                                              AutoPostBack="true" OnSelectedIndexChanged="btnFiltrar_Click" />
                        </div>
                        <div class="col-lg-2 col-md-12 d-flex gap-2">
                            <asp:Button runat="server" ID="btnFiltrar" CssClass="btn btn-otani w-100"
                                        Text="Filtrar" OnClick="btnFiltrar_Click" />
                            <asp:Button runat="server" ID="btnLimpiar" CssClass="btn btn-secondary w-100"
                                        Text="Limpiar" OnClick="btnLimpiar_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

    <!-- 
      =================================
      UPDATE PANEL PARA CONTENIDO (Tabla y Tarjetas)
      =================================
    -->
    <asp:UpdatePanel ID="upContenido" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <!-- VISTA DE TABLA (SOLO ESCRITORIO) -->
            <div class="d-desktop-only gridview">
                <asp:GridView ID="gvArticulos" runat="server" CssClass="table table-hover align-middle"
                    AutoGenerateColumns="False" DataKeyNames="Id" AllowPaging="true" PageSize="10"
                    OnPageIndexChanging="gvArticulos_PageIndexChanging" OnRowCommand="gvArticulos_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" />
                        <asp:BoundField DataField="Codigo" HeaderText="Código" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate>
                                <span class="text-muted"><%# Eval("Descripcion") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C2}" />
                        
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end" HeaderStyle-CssClass="text-end">
                            <ItemTemplate>
                                <div class="d-flex gap-2 justify-content-end">
                                    <!-- Botón 'Editar' actualizado -->
                                    <a class="btn btn-sm btn-edit" href='FormularioArticulo.aspx?id=<%# Eval("Id") %>'>Editar</a>
                                    
                                 
                                    <asp:LinkButton runat="server" ID="btnEliminar" CssClass="btn btn-sm btn-delete btn-eliminar-desktop"
                                        Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                                        data-id='<%# Eval("Id") %>' 
                                        data-nombre='<%# Eval("Nombre") %>'
                                        OnClientClick="return false;" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pager" />
                </asp:GridView>
            </div>


            <!-- VISTA DE TARJETAS (SOLO MÓVIL) -->
            <div class="d-mobile-only">
                <asp:ListView ID="lvArticulos" runat="server" 
                    DataKeyNames="Id" 
                    OnPagePropertiesChanging="lvArticulos_PagePropertiesChanging" 
                    OnItemCommand="lvArticulos_ItemCommand">
                    <LayoutTemplate>
                        <div id="itemPlaceholder" runat="server"></div>
                        <!-- Paginador para el ListView -->
                        <asp:DataPager ID="DataPager1" runat="server" PagedControlID="lvArticulos" PageSize="10" QueryStringField="page">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Link" ShowFirstPageButton="True" ShowNextPageButton="False" ButtonCssClass="page-link" />
                                <asp:NumericPagerField ButtonType="Link" ButtonCount="5" CurrentPageLabelCssClass="page-link active" NumericButtonCssClass="page-link" />
                                <asp:NextPreviousPagerField ButtonType="Link" ShowLastPageButton="True" ShowPreviousPageButton="False" ButtonCssClass="page-link" />
                            </Fields>
                        </asp:DataPager>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <!-- Plantilla de la tarjeta para móvil -->
                        <div class="admin-card">
                            <div class="admin-card-header">
                                <h5><%# Eval("Nombre") %></h5>
                                <span class="price"><%# Eval("Precio", "{0:C2}") %></span>
                            </div>
                            <div class="admin-card-body">
                                <span class="code"><%# Eval("Codigo") %></span>
                                <p class="description"><%# Eval("Descripcion") %></p>
                            </div>
                            <div class="admin-card-footer">
                                <!-- Botón 'Editar' -->
                                <a class="btn btn-sm btn-edit" href='FormularioArticulo.aspx?id=<%# Eval("Id") %>'>Editar</a>
                                
                                <!-- Botón 'Eliminar' con SweetAlert -->
                                <asp:LinkButton runat="server" ID="btnEliminarCard" CssClass="btn btn-sm btn-delete btn-eliminar-mobile"
                                    Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                                    data-id='<%# Eval("Id") %>' 
                                    data-nombre='<%# Eval("Nombre") %>'
                                    OnClientClick="return false;" />
                            </div>
                        </div>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <div class="empty">
                            No se encontraron artículos con los filtros seleccionados.
                        </div>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>

            <div class="d-flex justify-content-between mt-2">
                <asp:Label ID="lblTotal" runat="server" CssClass="text-muted"></asp:Label>
            </div>

        </ContentTemplate>
        <Triggers>
         
            <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnLimpiar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>


    <!-- 
      =================================
      SCRIPT PARA FILTROS Y SWEETALERT
      =================================
    -->
    <script type="text/javascript">
        // IDs de ASP.NET
        var txtBuscarID = '<%= txtBuscar.ClientID %>';
        var btnFiltrarID = '<%= btnFiltrar.ClientID %>';
        
        // Variable para el debounce del filtro de texto
        var searchTimeout;

        // Función para disparar el postback del botón Filtrar
        function triggerFilterPostback() {
            // Usamos __doPostBack para simular el click del botón dentro del UpdatePanel
            __doPostBack(btnFiltrarID, '');
        }

        // Esta función se asegura de que los scripts se vuelvan a vincular
        // después de CADA postback de UpdatePanel (muy importante)
        function pageLoad(sender, args) {

            // 1. FILTRO AUTOMÁTICO POR TEXTO (keyup con debounce)
            $('#' + txtBuscarID).on('keyup', function (e) {
                // Limpiamos el timeout anterior
                clearTimeout(searchTimeout);
                
                var query = $(this).val();
                
                // Si la tecla es 'Enter' (13), filtra de inmediato
                if (e.keyCode === 13) {
                    triggerFilterPostback();
                    return;
                }

                // Si la longitud es 0 o 3+, programamos un filtro
                if (query.length === 0 || query.length >= 3) {
                    searchTimeout = setTimeout(function () {
                        triggerFilterPostback();
                    }, 600); // 600ms de espera antes de buscar
                }
            });

            // 2. FILTRO AUTOMÁTICO AL SALIR DEL CAMPO (blur)
            $('#' + txtBuscarID).on('blur', function () {
                // Al salir, disparamos el filtro
                // (esto captura el "Tab" y el "click fuera")
                triggerFilterPostback();
            });


            // 3. CONFIRMACIÓN DE ELIMINAR CON SWEETALERT
            // Vinculamos el evento a las clases '.btn-eliminar-desktop' y '.btn-eliminar-mobile'
            $('.btn-eliminar-desktop, .btn-eliminar-mobile').on('click', function (e) {
                e.preventDefault(); // Detenemos el postback inmediato

                var button = $(this);
                var id = button.data('id');
                var nombre = button.data('nombre');
                
                // Usamos el 'id' del botón de LinkButton para el __doPostBack
                var postBackScript = "__doPostBack('" + button.attr('id') + "', '')";

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: "Se eliminará el artículo '" + nombre + "'. ¡Esta acción no se puede revertir!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#8B5E83', // Color primario (malva)
                    cancelButtonColor: '#C69F77',  // Color secundario (ocre)
                    confirmButtonText: 'Sí, eliminar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        // Si confirma, ejecutamos el postback para eliminar
                        eval(postBackScript);
                    }
                });
            });
        }

        // Vinculamos la función 'pageLoad' para que se ejecute en la carga inicial
        // y después de cada postback asíncrono.
        $(document).ready(function() {
            pageLoad(null, null); // Carga inicial
        });

        if (typeof (Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageLoad); // Cargas de UpdatePanel
        }

    </script>

</asp:Content>