<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="TiendaOnline.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-card{ max-width:980px; }
        .avatar-xl{ 
            width:120px; 
            height:120px; 
            border-radius:50%; 
            object-fit:cover; 
            border: 3px solid var(--sm-border-color); /* Borde con paleta */
        }
        .help{ color:#64748b; font-size:.9rem; }
        
        /* Título con paleta */
        .form-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="vs" runat="server" CssClass="alert alert-danger" HeaderText="Revisá los campos:" />

    <!-- 
      =================================
      UPDATE PANEL PARA DATOS BÁSICOS
      =================================
    -->
    <asp:UpdatePanel ID="upPerfil" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="card form-card mx-auto shadow-sm mb-4">
                <div class="card-body">
                    <h2 class="h5 mb-3 form-title">Mi Perfil</h2>

                    <div class="row g-4 align-items-center">
                        <div class="col-auto text-center">
                            <!-- Placeholder actualizado -->
                            <asp:Image ID="imgFoto" runat="server" CssClass="avatar-xl" 
                                ImageUrl="https://placehold.co/200x200/F8F4F0/3D2E3C?text=Avatar" />
                        </div>
                        <div class="col">
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label class="form-label">Nombre</label>
                                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="50" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre" ErrorMessage="Nombre requerido" Display="Dynamic" CssClass="text-danger" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Apellido</label>
                                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" MaxLength="50" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido" ErrorMessage="Apellido requerido" Display="Dynamic" CssClass="text-danger" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Email</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
                                    <span class="help">El email no se edita desde aquí.</span>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Rol</label>
                                    <asp:TextBox ID="txtRol" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                            </div>

                            <div class="mt-3 d-flex gap-2 align-items-center">
                                <!-- Botón 'Guardar' actualizado -->
                                <asp:Button ID="btnGuardarPerfil" runat="server" CssClass="btn btn-otani" Text="Guardar cambios" OnClick="btnGuardarPerfil_Click" />
                                <asp:Label ID="lblMsgPerfil" runat="server" CssClass="ms-2" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
       
    </asp:UpdatePanel>

    <!-- 
      =================================
      PANEL PARA AVATAR (POSTBACK COMPLETO)
      =================================
    -->
    <div class="card form-card mx-auto shadow-sm">
        <div class="card-body">
            <h3 class="h6 mb-3 form-title">Actualizar foto de perfil</h3>
            <div class="row g-2 align-items-end">
                <div class="col-md-8">
                    <asp:FileUpload ID="fuAvatar" runat="server" CssClass="form-control" />
                    <div class="help mt-1">Formatos: .jpg, .jpeg, .png — máx 2MB.</div>
                </div>
                <div class="col-md-4">
                    <!-- Botón 'Subir' actualizado -->
                    <asp:Button ID="btnSubirAvatar" runat="server" CssClass="btn btn-secondary w-100" Text="Subir" OnClick="btnSubirAvatar_Click" />
                </div>
            </div>
            <asp:Label ID="lblMsgAvatar" runat="server" CssClass="d-block mt-2" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
