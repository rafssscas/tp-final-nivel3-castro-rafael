<%@ Page Title="Marcas y Categorias" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="MarcasCategorias.aspx.cs" Inherits="TiendaOnline.MarcasCategorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-card{ max-width:1100px; }
        .table thead th{ white-space:nowrap; }
        .msg{ display:block; margin-bottom:1rem; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card form-card mx-auto shadow-sm">
        <div class="card-body">
            <h2 class="h4 mb-3">Administrar Marcas y Categorías</h2>

            <!-- ========== MARCAS ========== -->
            <div class="mb-4">
                <h3 class="h5 mb-2">Marcas</h3>
                <asp:Label runat="server" ID="lblMsgMarcas" Visible="false" CssClass="msg alert"></asp:Label>

                <div class="row g-2 align-items-end mb-2">
                    <div class="col-md-6">
                        <label class="form-label">Nueva marca</label>
                        <asp:TextBox runat="server" ID="txtNuevaMarca" CssClass="form-control" MaxLength="50" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button runat="server" ID="btnAgregarMarca" CssClass="btn btn-primary w-100"
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

                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true"
                    EditText="Editar" UpdateText="Guardar" CancelText="Cancelar" DeleteText="Eliminar" />
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>




            </div>

            <hr />

            <!-- ========== CATEGORÍAS ========== -->
            <div class="mt-4">
                <h3 class="h5 mb-2">Categorías</h3>
                <asp:Label runat="server" ID="lblMsgCategorias" Visible="false" CssClass="msg alert"></asp:Label>

                <div class="row g-2 align-items-end mb-2">
                    <div class="col-md-6">
                        <label class="form-label">Nueva categoría</label>
                        <asp:TextBox runat="server" ID="txtNuevaCategoria" CssClass="form-control" MaxLength="50" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button runat="server" ID="btnAgregarCategoria" CssClass="btn btn-primary w-100"
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

                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true"
                    EditText="Editar" UpdateText="Guardar" CancelText="Cancelar" DeleteText="Eliminar" />
            </Columns>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>



            </div>
        </div>
    </div>
</asp:Content>
