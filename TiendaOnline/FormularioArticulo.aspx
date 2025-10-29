<%@ Page Title="Artículo" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="FormularioArticulo.aspx.cs" Inherits="TiendaOnline.FormularioArticulo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* Estilo para el título del formulario */
        .form-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }

        /* Previsualización de imagen con la nueva paleta */
        .img-preview{
            width:100%;
            height:auto;
            max-height:320px;
            object-fit:contain;
            background: var(--sm-background-light); /* Fondo suave cálido */
            border: 1px dashed var(--sm-border-color); /* Borde sutil */
            border-radius:.75rem;
        }
        .form-card{ max-width:980px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <!-- Envolvemos todo el formulario en un UpdatePanel -->
    <asp:UpdatePanel ID="upForm" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="card form-card mx-auto shadow-sm">
                <div class="card-body">
                    <!-- Título con nuevo estilo -->
                    <h2 class="h4 mb-3 form-title"><asp:Label runat="server" ID="lblTitulo" /></h2>

                    <asp:Label runat="server" ID="lblMsg" Visible="false" CssClass="alert d-block"></asp:Label>

                    <!-- El ValidationSummary funciona bien dentro del UpdatePanel -->
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
                                    CssClass="text-danger" ErrorMessage="El nombre es obligatorio." Display="Dynamic" />
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
                                        ControlToValidate="ddlMarca" CssClass="text-danger" ErrorMessage="Seleccioná una marca." Display="Dynamic" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Categoría <span class="text-danger">*</span></label>
                                    <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select" />
                                    <asp:RequiredFieldValidator runat="server" InitialValue=""
                                        ControlToValidate="ddlCategoria" CssClass="text-danger" ErrorMessage="Seleccioná una categoría." Display="Dynamic" />
                                </div>
                            </div>
                            <div class="mb-3 mt-3">
                                <label class="form-label">Precio</label>
                                <asp:TextBox runat="server" ID="txtPrecio" CssClass="form-control" placeholder="Ej: 19999,99" />
                                <small class="text-muted">Usá coma o punto como separador decimal. Dejalo vacío si no querés mostrar precio.</small>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <!-- URL: AutoPostBack quitado. Se maneja con JS. -->
                            <label class="form-label">Imagen (URL)</label>
                            <asp:TextBox runat="server" ID="txtImagenUrl" CssClass="form-control mb-2" />

                            <!-- Subir archivo a Cloudinary -->
                            <div class="mb-2">
                                <label class="form-label d-block">O subir archivo (JPG/PNG, máx. 2MB)</label>
                                <asp:FileUpload ID="fuImagen" runat="server" CssClass="form-control" />
                                <div class="mt-2 d-flex gap-2">
                                    <!-- Botón 'Subir' actualizado -->
                                    <asp:Button runat="server" ID="btnSubirCloudinary" CssClass="btn btn-secondary btn-sm"
                                        Text="Subir y Previsualizar" OnClick="btnSubirCloudinary_Click" />
                                    <small class="text-muted align-self-center">Al subir, la URL aparece arriba.</small>
                                </div>
                            </div>

                            <!-- Preview e Imagen placeholder actualizados -->
                            <asp:Image runat="server" ID="imgPreview" CssClass="img-preview"
                                ImageUrl="https://placehold.co/800x600/F8F4F0/3D2E3C?text=Sagrada+Madre"
                                Attributes='onerror="this.onerror=null;this.src=`https://placehold.co/800x600/F8F4F0/3D2E3C?text=Sagrada+Madre`;"'/>
                        </div>
                    </div>

                    <asp:HiddenField runat="server" ID="hfId" />

                    <div class="d-flex gap-2 mt-4">
                        <!-- Botones 'Guardar' y 'Cancelar' actualizados -->
                        <asp:Button runat="server" ID="btnGuardar" CssClass="btn btn-otani" Text="Guardar" OnClick="btnGuardar_Click" />
                        <a href="Articulos.aspx" class="btn btn-secondary">Cancelar</a>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
          
            <asp:PostBackTrigger ControlID="btnSubirCloudinary" />
            <asp:PostBackTrigger ControlID="btnGuardar" />
        </Triggers>
    </asp:UpdatePanel>

    <!-- 
      =================================
      SCRIPT PARA PREVISUALIZACIÓN DE IMAGEN (CLIENT-SIDE)
      =================================
    -->
    <script type="text/javascript">
        // Esta función se vincula a la carga de la página Y a cada
        // recarga parcial del UpdatePanel
        function pageLoad(sender, args) {

            var txtImagenID = '<%= txtImagenUrl.ClientID %>';
            var imgPreviewID = '<%= imgPreview.ClientID %>';
            var placeholder = 'https://placehold.co/800x600/F8F4F0/3D2E3C?text=Sagrada+Madre';

            // 1. VINCULAR EVENTO 'BLUR' (salir del campo) AL TXT DE URL
            // Usamos .off() primero para evitar vincular el evento múltiples veces
            $('#' + txtImagenID).off('blur.preview').on('blur.preview', function () {
                var url = $(this).val();
                var imgPreview = $('#' + imgPreviewID);

                if (url.trim() === '') {
                    imgPreview.attr('src', placeholder);
                } else {
                    imgPreview.attr('src', url);
                }
            });

            // 2. VINCULAR 'ONERROR' (si la URL es inválida)
            // Esto ya está en el atributo 'onerror' del control <asp:Image>,
            // pero lo re-vinculamos por si acaso el UpdatePanel lo pierde.
            $('#' + imgPreviewID).off('error.preview').on('error.preview', function () {
                $(this).attr('src', placeholder);
            });
        }

        // Vinculamos para la carga inicial de la página
        $(document).ready(function () {
            pageLoad(null, null);
        });

        // Vinculamos para las recargas del UpdatePanel
        if (typeof (Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(pageLoad);
        }
    </script>
</asp:Content>
