<%@ Page Title="Marcas y Categorias" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="MarcasCategorias.aspx.cs" Inherits="TiendaOnline.MarcasCategorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-card{ max-width:1100px; }
        .table thead th{ white-space:nowrap; }
        .msg{ display:block; margin-bottom:1rem; }
        
        .form-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }
        
        .gv-actions .btn {
            padding: .25rem .5rem;
            font-size: .875rem;
        }

        /* =================================
           VISIBILIDAD RESPONSIVE (Como en Articulos.aspx)
           ================================= */
        @media (min-width: 992px) {
            .d-mobile-only {
                display: none;
            }
        }
        @media (max-width: 991.98px) {
            .d-desktop-only {
                display: none;
            }
        }

        /* =================================
           ESTILOS VISTA MÓVIL (TARJETAS) (Como en Articulos.aspx)
           ================================= */
        .admin-card {
            background-color: var(--sm-card-background);
            border: 1px solid var(--sm-border-color);
            border-radius: var(--radius-md);
            box-shadow: var(--shadow-sm);
            margin-bottom: 1rem;
            padding: 1rem;
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

        .admin-card-body {
            padding: 0.5rem 0;
        }

        .admin-card-footer {
            display: flex;
            gap: 0.5rem;
            border-top: 1px solid var(--sm-border-color);
            padding-top: 1rem;
            margin-top: 1rem;
        }
            .admin-card-footer .btn {
                flex-grow: 1; 
                text-align: center;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card form-card mx-auto shadow-sm">
        <div class="card-body">
            <h2 class="h4 mb-3 form-title">Administrar Marcas y Categorías</h2>

            <!-- ========== MARCAS ========== -->
            <div class="mb-4">
                <h3 class="h5 mb-2 form-title">Marcas</h3>
                <asp:Label runat="server" ID="lblMsgMarcas" Visible="false" CssClass="msg alert"></asp:Label>

                <div class="row g-2 align-items-end mb-2">
                    <div class="col-md-6">
                        <label class="form-label">Nueva marca</label>
                        <asp:TextBox runat="server" ID="txtNuevaMarca" CssClass="form-control" MaxLength="50" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button runat="server" ID="btnAgregarMarca" CssClass="btn btn-otani w-100"
                            Text="Agregar" OnClick="btnAgregarMarca_Click" />
                    </div>
                </div>

                <asp:UpdatePanel runat="server" ID="upMarcas" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- VISTA ESCRITORIO (TABLA) -->
                        <div class="d-desktop-only gridview">
                            <asp:GridView runat="server" ID="gvMarcas" CssClass="table table-sm table-striped table-hover"
                                AutoGenerateColumns="False" DataKeyNames="Id"
                                OnRowEditing="gvMarcas_RowEditing"
                                OnRowCancelingEdit="gvMarcas_RowCancelingEdit"
                                OnRowUpdating="gvMarcas_RowUpdating"
                                OnRowDeleting="gvMarcas_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="true" Visible="false" />
                                    <asp:TemplateField HeaderText="Descripción">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblMarcaDesc" Text='<%# Eval("Descripcion") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtMarcaDesc" CssClass="form-control form-control-sm"
                                                Text='<%# Bind("Descripcion") %>' MaxLength="50" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="gv-actions text-end" HeaderStyle-CssClass="text-end" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <div class="d-flex gap-2 justify-content-end">
                                                <asp:LinkButton runat="server" ID="btnEditar" CssClass="btn btn-sm btn-edit"
                                                    Text="Editar" CommandName="Edit" />
                                               <asp:LinkButton runat="server" ID="btnEliminar" CssClass="btn btn-sm btn-delete btn-eliminar-marca"
    Text="Eliminar" CommandName="Eliminar"
    CausesValidation="false"
    data-id='<%# Eval("Id") %>'
    data-nombre='<%# Eval("Descripcion") %>'
    data-uid='<%# ((System.Web.UI.Control)Container).FindControl("btnEliminar").UniqueID %>'

    OnClientClick="return false;" />

                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="d-flex gap-2 justify-content-end">
                                                <asp:LinkButton runat="server" ID="btnGuardar" CssClass="btn btn-sm btn-otani"
                                                    Text="Guardar" CommandName="Update" />
                                                <asp:LinkButton runat="server" ID="btnCancelar" CssClass="btn btn-sm btn-secondary"
                                                    Text="Cancelar" CommandName="Cancel" />
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        
                        <!-- VISTA MÓVIL (TARJETAS) -->
                        <div class="d-mobile-only">
                            <asp:ListView ID="lvMarcas" runat="server"
                                DataKeyNames="Id"
                                OnItemEditing="lvMarcas_ItemEditing"
                                OnItemCanceling="lvMarcas_ItemCanceling"
                                OnItemUpdating="lvMarcas_ItemUpdating"
                                OnItemCommand="lvMarcas_ItemCommand">
                                <LayoutTemplate>
                                    <div id="itemPlaceholder" runat="server"></div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <div class="admin-card">
                                        <div class="admin-card-header">
                                            <h5><%# Eval("Descripcion") %></h5>
                                        </div>
                                        <div class="admin-card-footer">
                                            <asp:LinkButton runat="server" ID="btnEditar" CssClass="btn btn-sm btn-edit"
                                                Text="Editar" CommandName="Edit" />
                                            <asp:LinkButton runat="server" ID="btnEliminar" CssClass="btn btn-sm btn-delete btn-eliminar-marca-mobile"
    Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
    CausesValidation="false"
    data-id='<%# Eval("Id") %>'
    data-nombre='<%# Eval("Descripcion") %>'
    data-uid='<%# ((System.Web.UI.Control)Container).FindControl("btnEliminar").UniqueID %>'

    OnClientClick="return false;" />

                                        </div>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="admin-card">
                                        <div class="admin-card-body">
                                            <label class="form-label">Descripción</label>
                                            <asp:TextBox runat="server" ID="txtMarcaDesc" CssClass="form-control"
                                                Text='<%# Bind("Descripcion") %>' MaxLength="50" />
                                        </div>
                                        <div class="admin-card-footer">
                                            <asp:LinkButton runat="server" ID="btnGuardar" CssClass="btn btn-sm btn-otani"
                                                Text="Guardar" CommandName="Update" />
                                            <asp:LinkButton runat="server" ID="btnCancelar" CssClass="btn btn-sm btn-secondary"
                                                Text="Cancelar" CommandName="Cancel" />
                                        </div>
                                    </div>
                                </EditItemTemplate>
                            </asp:ListView>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAgregarMarca" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>

            <hr />

            <!-- ========== CATEGORÍAS ========== -->
            <div class="mt-4">
                <h3 class="h5 mb-2 form-title">Categorías</h3>
                <asp:Label runat="server" ID="lblMsgCategorias" Visible="false" CssClass="msg alert"></asp:Label>

                <div class="row g-2 align-items-end mb-2">
                    <div class="col-md-6">
                        <label class="form-label">Nueva categoría</label>
                        <asp:TextBox runat="server" ID="txtNuevaCategoria" CssClass="form-control" MaxLength="50" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button runat="server" ID="btnAgregarCategoria" CssClass="btn btn-otani w-100"
                            Text="Agregar" OnClick="btnAgregarCategoria_Click" />
                    </div>
                </div>

                <asp:UpdatePanel runat="server" ID="upCategorias" UpdateMode="Conditional">
                    <ContentTemplate>
                        <!-- VISTA ESCRITORIO (TABLA) -->
                        <div class="d-desktop-only gridview">
                            <asp:GridView runat="server" ID="gvCategorias" CssClass="table table-sm table-striped table-hover"
                                AutoGenerateColumns="False" DataKeyNames="Id"
                                OnRowEditing="gvCategorias_RowEditing"
                                OnRowCancelingEdit="gvCategorias_RowCancelingEdit"
                                OnRowUpdating="gvCategorias_RowUpdating"
                                OnRowDeleting="gvCategorias_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="true" Visible="false" />
                                    <asp:TemplateField HeaderText="Descripción">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCatDesc" Text='<%# Eval("Descripcion") %>' />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox runat="server" ID="txtCatDesc" CssClass="form-control form-control-sm"
                                                Text='<%# Bind("Descripcion") %>' MaxLength="50" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="gv-actions text-end" HeaderStyle-CssClass="text-end" ItemStyle-Width="180px">
                                        <ItemTemplate>
                                            <div class="d-flex gap-2 justify-content-end">
                                                <asp:LinkButton runat="server" ID="btnEditarCat" CssClass="btn btn-sm btn-edit"
                                                    Text="Editar" CommandName="Edit" />
                                                <asp:LinkButton runat="server" ID="btnEliminarCat" CssClass="btn btn-sm btn-delete btn-eliminar-categoria"
    Text="Eliminar" CommandName="Eliminar"
    CausesValidation="false"
    data-id='<%# Eval("Id") %>'
    data-nombre='<%# Eval("Descripcion") %>'
    data-uid='<%# ((System.Web.UI.Control)Container).FindControl("btnEliminarCat").UniqueID %>'

    OnClientClick="return false;" />

                                            </div>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <div class="d-flex gap-2 justify-content-end">
                                                <asp:LinkButton runat="server" ID="btnGuardarCat" CssClass="btn btn-sm btn-otani"
                                                    Text="Guardar" CommandName="Update" />
                                                <asp:LinkButton runat="server" ID="btnCancelarCat" CssClass="btn btn-sm btn-secondary"
                                                    Text="Cancelar" CommandName="Cancel" />
                                            </div>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <!-- VISTA MÓVIL (TARJETAS) -->
                        <div class="d-mobile-only">
                            <asp:ListView ID="lvCategorias" runat="server"
                                DataKeyNames="Id"
                                OnItemEditing="lvCategorias_ItemEditing"
                                OnItemCanceling="lvCategorias_ItemCanceling"
                                OnItemUpdating="lvCategorias_ItemUpdating"
                                OnItemCommand="lvCategorias_ItemCommand">
                                <LayoutTemplate>
                                    <div id="itemPlaceholder" runat="server"></div>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <div class="admin-card">
                                        <div class="admin-card-header">
                                            <h5><%# Eval("Descripcion") %></h5>
                                        </div>
                                        <div class="admin-card-footer">
                                            <asp:LinkButton runat="server" ID="btnEditar" CssClass="btn btn-sm btn-edit"
                                                Text="Editar" CommandName="Edit" />
                                            <asp:LinkButton runat="server" ID="btnEliminar" CssClass="btn btn-sm btn-delete btn-eliminar-categoria-mobile"
    Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
    CausesValidation="false"
    data-id='<%# Eval("Id") %>'
    data-nombre='<%# Eval("Descripcion") %>'
   data-uid='<%# ((System.Web.UI.Control)Container).FindControl("btnEliminar").UniqueID %>'

    OnClientClick="return false;" />

                                        </div>
                                    </div>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <div class="admin-card">
                                        <div class="admin-card-body">
                                            <label class="form-label">Descripción</label>
                                            <asp:TextBox runat="server" ID="txtCatDesc" CssClass="form-control"
                                                Text='<%# Bind("Descripcion") %>' MaxLength="50" />
                                        </div>
                                        <div class="admin-card-footer">
                                            <asp:LinkButton runat="server" ID="btnGuardar" CssClass="btn btn-sm btn-otani"
                                                Text="Guardar" CommandName="Update" />
                                            <asp:LinkButton runat="server" ID="btnCancelar" CssClass="btn btn-sm btn-secondary"
                                                Text="Cancelar" CommandName="Cancel" />
                                        </div>
                                    </div>
                                </EditItemTemplate>
                            </asp:ListView>
                        </div>
                    </ContentTemplate>
                     <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnAgregarCategoria" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    
    <!-- 
      =================================
      SCRIPT PARA SWEETALERT (Eliminación)
      =================================
    -->
    <script type="text/javascript">
        function pageLoad(sender, args) {

            // 1. CONFIRMACIÓN PARA MARCAS (Escritorio + Móvil)
            $('.btn-eliminar-marca, .btn-eliminar-marca-mobile').off('click.swal').on('click.swal', function (e) {
                e.preventDefault();
                var button = $(this);
                var nombre = button.data('nombre');
                // UniqueID primero; si no está, intenta con "name" (algunos controles lo traen); último recurso: id
                var target = button.data('uid') || button.attr('name') || button.attr('id');
                var postBackScript = "__doPostBack('" + target + "', '')";

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: "Se eliminará la marca '" + nombre + "'. ¡Esto puede fallar si tiene artículos asociados!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#8B5E83',
                    cancelButtonColor: '#C69F77',
                    confirmButtonText: 'Sí, eliminar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => { if (result.isConfirmed) { eval(postBackScript); } });
            });

            // 2. CONFIRMACIÓN PARA CATEGORÍAS (Escritorio + Móvil)
            $('.btn-eliminar-categoria, .btn-eliminar-categoria-mobile').off('click.swal').on('click.swal', function (e) {
                e.preventDefault();
                var button = $(this);
                var nombre = button.data('nombre');
                var target = button.data('uid') || button.attr('name') || button.attr('id');
                var postBackScript = "__doPostBack('" + target + "', '')";

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: "Se eliminará la categoría '" + nombre + "'. ¡Esto puede fallar si tiene artículos asociados!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#8B5E83',
                    cancelButtonColor: '#C69F77',
                    confirmButtonText: 'Sí, eliminar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => { if (result.isConfirmed) { eval(postBackScript); } });
            });
        }

        $(document).ready(function () {
            pageLoad(null, null);
        });

        if (typeof (Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageLoad);
        }
    </script>
</asp:Content>

