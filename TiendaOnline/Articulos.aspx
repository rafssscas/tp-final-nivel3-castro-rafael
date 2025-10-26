<%@ Page Title="Artículos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="Articulos.aspx.cs" Inherits="TiendaOnline.Articulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table thead th { white-space: nowrap; }
        .w-actions { width: 160px; }
        .truncate-1 {
            display:-webkit-box; -webkit-line-clamp:1; -webkit-box-orient:vertical; overflow:hidden;
            max-width: 420px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="h4 mb-0">Administrar artículos</h2>
        <a href="FormularioArticulo.aspx" class="btn btn-primary">Nuevo artículo</a>
    </div>

    <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert d-block"></asp:Label>

    <!-- Filtros -->
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

    <!-- Grilla -->
    <asp:GridView ID="gvArticulos" runat="server" CssClass="table table-striped table-hover align-middle"
        AutoGenerateColumns="False" DataKeyNames="Id" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvArticulos_PageIndexChanging" OnRowCommand="gvArticulos_RowCommand">
        <Columns>
   
    <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-VerticalAlign="Middle" Visible="false" />

    <asp:BoundField DataField="Codigo" HeaderText="Código" ItemStyle-VerticalAlign="Middle" />
    <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-VerticalAlign="Middle" />

    <asp:TemplateField HeaderText="Descripción">
        <ItemTemplate>
            <span class="text-muted truncate-1"><%# Eval("Descripcion") %></span>
        </ItemTemplate>
    </asp:TemplateField>

   
    <asp:TemplateField HeaderText="Marca" Visible="false">
        <ItemTemplate>
            <%# Eval("Marca.Descripcion") %>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Categoría" Visible="false">
        <ItemTemplate>
            <%# Eval("Categoria.Descripcion") %>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C2}" ItemStyle-VerticalAlign="Middle" />

    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="w-actions">
        <ItemTemplate>
            <a class="btn btn-sm btn-outline-primary me-1" href='FormularioArticulo.aspx?id=<%# Eval("Id") %>'>Editar</a>
            <asp:LinkButton runat="server" ID="btnEliminar" CssClass="btn btn-sm btn-outline-danger"
                Text="Eliminar" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                OnClientClick="return confirm('¿Seguro que querés eliminar este artículo? Esto también quitará sus favoritos.');" />
        </ItemTemplate>
    </asp:TemplateField>
</Columns>

        <PagerStyle CssClass="pagination-outer" />
    </asp:GridView>

    <div class="d-flex justify-content-between">
        <asp:Label ID="lblTotal" runat="server" CssClass="text-muted"></asp:Label>
    </div>

</asp:Content>
