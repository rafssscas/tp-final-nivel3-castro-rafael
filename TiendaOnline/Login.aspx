<%@ Page Title="Login" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TiendaOnline.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card-login{ max-width:420px; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="card form-card mx-auto shadow-sm mt-4">
    <div class="card-body">
      <h2 class="h4 mb-3">Iniciar sesión</h2>
      <asp:ValidationSummary runat="server" CssClass="text-danger mb-3" />

       <div class="mb-3">
        <label class="form-label">Usuario</label>
        <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
            ErrorMessage="Usuario requerido" CssClass="text-danger" />
      </div>

      
      <div class="mb-3">
        <label class="form-label">Contraseña</label>
        <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="Contraseña requerida" CssClass="text-danger" />
      </div>
      <asp:Button runat="server" ID="btnLogin" Text="Ingresar" CssClass="btn btn-primary" OnClick="btnLogin_Click" />
      <a href="Registro.aspx" class="ms-2">Crear cuenta</a>
    </div>
  </div>
</asp:Content>
