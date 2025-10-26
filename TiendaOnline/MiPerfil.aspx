<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="TiendaOnline.MiPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .form-card{ max-width:980px; }
        .avatar-xl{ width:120px; height:120px; border-radius:50%; object-fit:cover; }
        .help{ color:#64748b; font-size:.9rem; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ValidationSummary ID="vs" runat="server" CssClass="alert alert-danger" HeaderText="Revisá los campos:" />

    <!-- Datos básicos -->
    <div class="card form-card mx-auto shadow-sm mb-4">
        <div class="card-body">
            <h2 class="h5 mb-3">Mi Perfil</h2>

            <div class="row g-4 align-items-center">
                <div class="col-auto text-center">
                    <asp:Image ID="imgFoto" runat="server" CssClass="avatar-xl border" ImageUrl="https://placehold.co/200x200?text=Avatar" />
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

                    <div class="mt-3 d-flex gap-2">
                        <asp:Button ID="btnGuardarPerfil" runat="server" CssClass="btn btn-primary" Text="Guardar cambios" OnClick="btnGuardarPerfil_Click" />
                        <asp:Label ID="lblMsgPerfil" runat="server" CssClass="ms-2"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Avatar -->
    <div class="card form-card mx-auto shadow-sm">
        <div class="card-body">
            <h3 class="h6 mb-3">Actualizar foto de perfil</h3>
            <div class="row g-2 align-items-end">
                <div class="col-md-8">
                    <asp:FileUpload ID="fuAvatar" runat="server" CssClass="form-control" />
                    <div class="help mt-1">Formatos: .jpg, .jpeg, .png — máx 2MB.</div>
                </div>
                <div class="col-md-4">
                    <asp:Button ID="btnSubirAvatar" runat="server" CssClass="btn btn-outline-primary w-100" Text="Subir" OnClick="btnSubirAvatar_Click" />
                </div>
            </div>
            <asp:Label ID="lblMsgAvatar" runat="server" CssClass="d-block mt-2"></asp:Label>
        </div>
    </div>
</asp:Content>
