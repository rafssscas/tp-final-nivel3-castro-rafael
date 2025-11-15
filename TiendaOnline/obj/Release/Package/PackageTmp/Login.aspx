<%@ Page Title="Login" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TiendaOnline.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .card-login{ 
            max-width:420px; 
        }
        
        /* Estilo para el título del formulario */
        .form-title {
            color: var(--sm-primary-accent);
            font-weight: 700;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <!-- Usamos 'card-login' para el ancho y 'card' para los estilos de la paleta -->
     <div class="card card-login mx-auto shadow-sm mt-4">
        <div class="card-body">
            <!-- Título actualizado -->
            <h2 class="h4 mb-3 form-title">Iniciar sesión</h2>
          
            <asp:ValidationSummary runat="server" CssClass="text-danger mb-3" />

            <div class="mb-3">
                <label class="form-label">Usuario</label>
                <asp:TextBox runat="server" ID="txtNombre" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                    ErrorMessage="Usuario requerido" CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
                <label class="form-label">Contraseña</label>
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" 
                    ErrorMessage="Contraseña requerida" CssClass="text-danger" Display="Dynamic" />
            </div>
            
            <!-- Botón de Ingreso actualizado (full width) -->
            <asp:Button runat="server" ID="btnLogin" Text="Ingresar" CssClass="btn btn-otani w-100" OnClick="btnLogin_Click" />
            
            <!-- Link de Registro actualizado a botón secundario (full width) -->
            <a href="Registro.aspx" class="btn btn-secondary w-100 mt-2">Crear cuenta</a>
        </div>
    </div>
</asp:Content>
