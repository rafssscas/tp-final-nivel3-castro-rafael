<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="TiendaOnline.Registro" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <style>.form-card{max-width:480px}</style>


</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="card form-card mx-auto shadow-sm mt-4">
    <div class="card-body">
      <h2 class="h4 mb-3">Crear cuenta</h2>

      <asp:ValidationSummary runat="server" CssClass="text-danger mb-3" />
      <div class="mb-3">
        <label class="form-label">Email</label>
        <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Email requerido" CssClass="text-danger" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="Formato inválido" CssClass="text-danger"
          ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
      </div>

      <div class="mb-3">
        <label class="form-label">Contraseña</label>
        <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" />
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="Contraseña requerida" CssClass="text-danger" />
        <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword" 
          ValidationExpression="^.{6,}$" ErrorMessage="Mínimo 6 caracteres" CssClass="text-danger" />
      </div>

      <asp:Button runat="server" ID="btnRegistrarse" Text="Registrarse" CssClass="btn btn-primary" OnClick="btnRegistrarse_Click" />
      <a href="Login.aspx" class="ms-2">¿Ya tenés cuenta?</a>
    </div>
  </div>


</asp:Content>
