<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="TiendaOnline.Default" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Por ahora no necesitamos JS/CSS extra acá, todo viene del master -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="mb-3">Catálogo</h1>

    <!-- Filtros -->
    <!-- ENVOLVEMOS EN PANEL CON DefaultButton Y CLASE filters-scope -->
    <asp:Panel ID="pnlFiltros" runat="server" CssClass="filters-scope" DefaultButton="btnFiltrar">
        <div class="card mb-3 shadow-sm">
            <div class="card-body">
                <div class="row g-2 align-items-end">
                    <div class="col-md-4">
                        <label class="form-label">Buscar</label>
                        <asp:TextBox runat="server" ID="txtBuscar" CssClass="form-control"
                                     placeholder="Nombre, código o descripción..." />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Marca</label>
                        <asp:DropDownList runat="server" ID="ddlMarca" CssClass="form-select"
                                          AutoPostBack="true" OnSelectedIndexChanged="btnFiltrar_Click" />
                    </div>
                    <div class="col-md-3">
                        <label class="form-label">Categoría</label>
                        <asp:DropDownList runat="server" ID="ddlCategoria" CssClass="form-select"
                                          AutoPostBack="true" OnSelectedIndexChanged="btnFiltrar_Click" />
                    </div>
                    <div class="col-md-2 d-flex gap-2">
                        <!-- Usamos btn-otani / btn-secondary para la paleta nueva -->
                        <asp:Button runat="server" ID="btnFiltrar" CssClass="btn btn-otani w-100"
                                    Text="Filtrar" OnClick="btnFiltrar_Click" />
                        <asp:Button runat="server" ID="btnLimpiar" CssClass="btn btn-secondary w-100"
                                    Text="Limpiar" OnClick="btnLimpiar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Toolbar de exportación/envío: abren modal (no postean) -->
    <div class="mb-3 toolbar">
        <asp:Button runat="server" ID="btnEnviarExcel" CssClass="btn btn-success"
            Text="ENVIAR EXCEL"
            OnClientClick="openEmailModal('excel'); return false;" />
        <asp:Button runat="server" ID="btnEnviarPdf" CssClass="btn btn-danger"
            Text="ENVIAR PDF"
            OnClientClick="openEmailModal('htmlpdf'); return false;" />
    </div>

    <!-- Mensajes -->
    <asp:Label runat="server" ID="lblMsg" EnableViewState="false"></asp:Label>

    <!-- Grilla de tarjetas -->
    <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-4">
        <asp:Repeater runat="server" ID="repArticulos">
            <ItemTemplate>
                <div class="col">
                    <div class="card h-100 shadow-sm product-catalog-card">
                        <img class="card-img-top"
                             src='<%# Eval("ImagenUrl") %>'
                             onerror="this.src='https://placehold.co/600x400?text=Sin+Imagen';"
                             alt='<%# Eval("Nombre") %>' />
                        <div class="card-body d-flex flex-column">
                            <h5 class="card-title mb-1"><%# Eval("Nombre") %></h5>
                            <div class="mb-2">
                                <span class="badge badge-otani me-1"><%# Eval("Marca.Descripcion") %></span>
                                <span class="badge badge-otani alt"><%# Eval("Categoria.Descripcion") %></span>
                            </div>
                            <p class="card-text text-muted truncate-2"><%# Eval("Descripcion") %></p>
                            <div class="mt-auto d-flex justify-content-between align-items-center">
                                <span class="price">
                                    <%# Container.DataItem != null && Eval("Precio") != null
                                            ? "$ " + string.Format("{0:N2}", Eval("Precio"))
                                            : "Consultar" %>
                                </span>
                                <a class="btn btn-sm btn-otani"
                                   href='Detalle.aspx?id=<%# Eval("Id") %>'>Ver detalle</a>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- ====== Modal para ingresar email de destino ====== -->
    <asp:HiddenField ID="hfTipoEnvio" runat="server" />

    <div class="modal fade" id="modalEmail" tabindex="-1"
         aria-labelledby="modalEmailLabel" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="modalEmailLabel">Enviar por correo</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal"
                    aria-label="Cerrar"></button>
          </div>
          <div class="modal-body">
            <div class="mb-3">
              <label for="<%= txtEmailDestino.ClientID %>" class="form-label">Email destinatario</label>
              <asp:TextBox ID="txtEmailDestino" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmailDestino"
                  CssClass="text-danger" ErrorMessage="El email es obligatorio."
                  Display="Dynamic" ValidationGroup="vgEnviar" />
              <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmailDestino"
                  CssClass="text-danger" ValidationGroup="vgEnviar"
                  ErrorMessage="Formato de email no válido."
                  ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                  Display="Dynamic" />
            </div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
            <asp:Button ID="btnConfirmEnviar" runat="server"
                CssClass="btn btn-success"
                Text="Enviar"
                ValidationGroup="vgEnviar"
                OnClick="btnConfirmEnviar_Click" />
          </div>
        </div>
      </div>
    </div>

    <!-- ====== JS: MODAL EMAIL (sin jQuery) ====== -->
    <script type="text/javascript">
      function openEmailModal(tipo) {
        document.getElementById('<%= hfTipoEnvio.ClientID %>').value = tipo;
        var el = document.getElementById('modalEmail');
        if (!el || !window.bootstrap || !bootstrap.Modal) return;
        var modal = bootstrap.Modal.getOrCreateInstance(el);
        modal.show();
        setTimeout(function () {
            var txt = document.getElementById('<%= txtEmailDestino.ClientID %>');
            if (txt) txt.focus();
        }, 250);
      }
    </script>

    <!-- ====== JS: FILTRO AUTOMÁTICO (mismo patrón que Home) ====== -->
    <script type="text/javascript">
        // Misma firma y nombres de variables que el sistema de gestión Otani
        var txtBuscarID = '<%= txtBuscar.ClientID %>';
        var btnFiltrarID = '<%= btnFiltrar.ClientID %>';
        var searchTimeout;

        function triggerFilterPostback() {
            __doPostBack(btnFiltrarID, '');
        }

        // Esta función la llama el Master cuando jQuery está listo
        function pageLoad(sender, args) {
            if (!window.jQuery) return;

            // 1) Filtro por texto con debounce
            $('#' + txtBuscarID)
                .off('keyup.filter')
                .on('keyup.filter', function (e) {
                    clearTimeout(searchTimeout);
                    var query = $(this).val();

                    if (e.keyCode === 13) {
                        // Enter -> filtrar directo
                        triggerFilterPostback();
                        return;
                    }

                    if (query.length === 0 || query.length >= 3) {
                        searchTimeout = setTimeout(function () {
                            triggerFilterPostback();
                        }, 600);
                    }
                });

            // 2) Filtro al perder foco
            $('#' + txtBuscarID)
                .off('blur.filter')
                .on('blur.filter', function () {
                    triggerFilterPostback();
                });
        }
    </script>
</asp:Content>
