<%@ Page Title="Artículo" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="FormularioArticulo.aspx.cs" Inherits="TiendaOnline.FormularioArticulo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .img-preview{
            width:100%;
            height:auto;
            max-height:320px;
            object-fit:contain;
            background:#f8f9fa;
            border-radius:.75rem;
        }
        .form-card{ max-width:980px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card form-card mx-auto shadow-sm">
        <div class="card-body">
            <h2 class="h4 mb-3"><asp:Label runat="server" ID="lblTitulo" /></h2>

            <asp:Label runat="server" ID="lblMsg" Visible="false" CssClass="alert d-block"></asp:Label>

            <asp:ValidationSummary runat="server" ID="valSum" CssClass="text-danger mb-3" />

            <div class="row g-4">
                <div class="col-md-6">
                    <div class="mb-3">
                        <label class="form-label">Código</label>
                        <asp:TextBox runat="server" ID="txtCodigo" CssClass="form-control" MaxLength="50" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Nombre <span class="text-danger">*</span></label>
                        <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" MaxLength="50" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                            CssClass="text-danger" ErrorMessage="El nombre es obligatorio." />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Descripción</label>
                        <asp:TextBox runat="server" ID="txtDescripcion" CssClass="form-control" TextMode="MultiLine" Rows="4" MaxLength="150" />
                    </div>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label">Marca <span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlMarca" CssClass="form-select" />
                            <asp:RequiredFieldValidator runat="server" InitialValue=""
                                ControlToValidate="ddlMarca" CssClass="text-danger" ErrorMessage="Seleccioná una marca." />
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">Categoría <span class="text-danger">*</span></label>
                            <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select" />
                            <asp:RequiredFieldValidator runat="server" InitialValue=""
                                ControlToValidate="ddlCategoria" CssClass="text-danger" ErrorMessage="Seleccioná una categoría." />
                        </div>
                    </div>
                    <div class="mb-3 mt-3">
                        <label class="form-label">Precio</label>
                        <asp:TextBox runat="server" ID="txtPrecio" CssClass="form-control" placeholder="Ej: 19999,99" />
                        <small class="text-muted">Usá coma o punto como separador decimal. Dejalo vacío si no querés mostrar precio.</small>
                    </div>
                </div>

                <div class="col-md-6">
                    <!-- URL directa (opcional) -->
                    <label class="form-label">Imagen (URL)</label>
                    <asp:TextBox runat="server" ID="txtImagenUrl" CssClass="form-control mb-2" AutoPostBack="true" OnTextChanged="txtImagenUrl_TextChanged" />

                    <!-- Subir archivo a Cloudinary -->
                    <div class="mb-2">
                        <label class="form-label d-block">O subir archivo (JPG/PNG, máx. 2MB)</label>
                        <asp:FileUpload ID="fuImagen" runat="server" CssClass="form-control" />
                        <div class="mt-2 d-flex gap-2">
                            <asp:Button runat="server" ID="btnSubirCloudinary" CssClass="btn btn-outline-secondary"
                                Text="Subir a Cloudinary" OnClick="btnSubirCloudinary_Click" />
                            <small class="text-muted align-self-center">Al subir, guardamos la URL en el campo de arriba y se previsualiza.</small>
                        </div>
                    </div>

                    <asp:Image runat="server" ID="imgPreview" CssClass="img-preview"
                        ImageUrl="https://placehold.co/800x600?text=Sin+Imagen" 
                        Attributes='onerror="this.onerror=null;this.src=`https://placehold.co/800x600?text=Sin+Imagen`;"'/>
                </div>
            </div>

            <asp:HiddenField runat="server" ID="hfId" />

            <div class="d-flex gap-2 mt-4">
                <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-primary" Text="Guardar" OnClick="btnGuardar_Click" />
                <a href="Articulos.aspx" class="btn btn-outline-secondary">Cancelar</a>
            </div>
        </div>
    </div>
</asp:Content>
