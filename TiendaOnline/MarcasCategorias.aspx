<%@ Page Title="Marcas y Categorias" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="MarcasCategorias.aspx.cs" Inherits="TiendaOnline.MarcasCategorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-card{ max-width:1100px; }
        .table thead th{ white-space:nowrap; }
        .msg{ display:block; margin-bottom:1rem; }
        
        /* Título con paleta */
        .form-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }
        
        /* Clases para los botones del GridView (sm) */
        .gv-actions .btn {
            padding: .25rem .5rem;
            font-size: .875rem;
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
                        <!-- Botón 'Agregar' actualizado -->
                        <asp:Button runat="server" ID="btnAgregarMarca" CssClass="btn btn-otani w-100"
                            Text="Agregar" OnClick="btnAgregarMarca_Click" />
                    </div>
                </div>

                <asp:UpdatePanel runat="server" ID="upMarcas" UpdateMode="Conditional">
                    <ContentTemplate>
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
                                                Text="Eliminar" CommandName="Delete" 
                                                data-id='<%# Eval("Id") %>' 
                                                data-nombre='<%# Eval("Descripcion") %>'
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
                        <!-- Botón 'Agregar' actualizado -->
                        <asp:Button runat="server" ID="btnAgregarCategoria" CssClass="btn btn-otani w-100"
                            Text="Agregar" OnClick="btnAgregarCategoria_Click" />
                    </div>
                </div>

                <asp:UpdatePanel runat="server" ID="upCategorias" UpdateMode="Conditional">
                    <ContentTemplate>
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
                                                Text="Eliminar" CommandName="Delete" 
                                                data-id='<%# Eval("Id") %>' 
                                                data-nombre='<%# Eval("Descripcion") %>'
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
        // Esta función se asegura de que los scripts se vuelvan a vincular
        // después de CADA postback de UpdatePanel
        function pageLoad(sender, args) {
            
            // 1. CONFIRMACIÓN PARA MARCAS
            $('.btn-eliminar-marca').on('click', function (e) {
                e.preventDefault(); 
                var button = $(this);
                var nombre = button.data('nombre');
                // Buscamos el ID del LinkButton (que es único) para el script de postback
                var postBackScript = "__doPostBack('" + button.attr('id') + "', '')";

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: "Se eliminará la marca '" + nombre + "'. ¡Esto puede fallar si tiene artículos asociados!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#8B5E83', // Malva
                    cancelButtonColor: '#C69F77',  // Ocre
                    confirmButtonText: 'Sí, eliminar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        eval(postBackScript);
                    }
                });
            });

            // 2. CONFIRMACIÓN PARA CATEGORÍAS
            $('.btn-eliminar-categoria').on('click', function (e) {
                e.preventDefault(); 
                var button = $(this);
                var nombre = button.data('nombre');
                var postBackScript = "__doPostBack('" + button.attr('id') + "', '')";

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: "Se eliminará la categoría '" + nombre + "'. ¡Esto puede fallar si tiene artículos asociados!",
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#8B5E83', // Malva
                    cancelButtonColor: '#C69F77',  // Ocre
                    confirmButtonText: 'Sí, eliminar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        eval(postBackScript);
                    }
                });
            });
        }

        // Vinculamos la función 'pageLoad' para la carga inicial
        $(document).ready(function() {
            pageLoad(null, null); 
        });

        // Y para las recargas de UpdatePanel
        if (typeof (Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageLoad);
        }
    </script>
</asp:Content>

