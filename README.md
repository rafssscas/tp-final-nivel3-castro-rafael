<!DOCTYPE html>
<html lang="es">
<head>
<meta charset="utf-8">


</head>
<body>
<div class="header">
  <div class="brand-badge">RAFAEL CASTRO</div>
  <div>
    <div><strong>Proyecto:</strong> Catálogo Web – Tienda Otani</div>
    <div class="small">Última actualización: 05-11-2025</div>
  </div>
</div>

<h1>Documento Técnico y Funcional</h1>

<h2>0. Índice</h2>
<ol class="toc">
  <li>Resumen ejecutivo</li>
  <li>Alcance y consignas</li>
  <li>Arquitectura en capas</li>
  <li>Modelo de datos</li>
  <li>Vistas y flujo de navegación</li>
  <li>Casos de uso y validaciones</li>
  <li>Seguridad y login (por <em>Usuario</em>)</li>
  <li>Checklist de cumplimiento</li>
  <li>Limpieza de código & convenciones</li>
  <li>Pruebas y criterios de aceptación</li>
</ol>

<h2>1. Resumen ejecutivo</h2>
<p>Portal web de catálogo para un comercio real "Tienda Otani", con visualización de productos (tarjetas, listados, detalle), filtros por texto, marca y categoría; y un área protegida para administración (ABM). Login por <strong>usuario</strong> (no email). Funcionalidad opcional implementada: favoritos por usuario.</p>

<div class="kpi-grid">
  <div class="kpi">
    <h4>Front</h4>
    <p>ASP.NET Web Forms (C#), Bootstrap, componentes GridView/ListView/Repeater.</p>
  </div>
  <div class="kpi">
    <h4>Back</h4>
    <p>.NET C# – Capas: <strong>dominio</strong>, <strong>negocio</strong>, <strong>Web</strong>.</p>
  </div>
  <div class="kpi">
    <h4>DB</h4>
    <p>SQL Server – <code>CATALOGO_WEB_DB</code> (script provisto en la consigna del TP).</p>
  </div>
</div>

<h2>2. Alcance y consignas</h2>
<ul>
  <li>Home con catálogo y filtros; detalle de producto.</li>
  <li>Login para acceder a administración (solo <span class="badge">admin</span>).</li>
  <li>ABM de artículos (listar/buscar/agregar/modificar/eliminar) + marcas y categorías.</li>
  <li>Persistencia en base de datos provista.</li>
  <li>Implementacion opcional: registro de usuario y <strong>favoritos</strong>.</li>
</ul>

<h2>3. Arquitectura en capas</h2>
<table class="table">
<thead><tr><th>Capa</th><th>Responsabilidad</th><th>Ejemplos</th></tr></thead>
<tbody>
<tr><td>Dominio</td><td>Entidades simples (POCOs)</td><td><code>Articulos</code>, <code>Marcas</code>, <code>Categorias</code>, <code>Users</code>, <code>Favoritos</code></td></tr>
<tr><td>Negocio</td><td>Lógica, validaciones, acceso a datos (SPs)</td><td><code>ArticulosNegocio</code>, <code>MarcasNegocio</code>, <code>CategoriasNegocio</code>, <code>UsersNegocio</code>, <code>FavoritosNegocio</code>, <code>AccesoDatos</code></td></tr>
<tr><td>Web</td><td>Presentación, code-behind, manejo de eventos</td><td><code>Default.aspx</code>, <code>Home.aspx</code>, <code>Detalle.aspx</code>, <code>Articulos.aspx</code>, <code>FormularioArticulo.aspx</code>, <code>Login.aspx</code>, <code>MarcasCategorias.aspx</code>, <code>Favoritos.aspx</code></td></tr>
</tbody>
</table>

<h2>4. Modelo de datos</h2>
<p>Se respeta el script de la consigna: tablas <code>MARCAS</code>, <code>CATEGORIAS</code>, <code>ARTICULOS</code>, <code>USERS</code>, <code>FAVORITOS</code> y sus claves. Procedimientos almacenados creados para <em>listar/buscar/obtener</em> y <em>ABM</em>.</p>
<div class="callout">
  <strong>Nota:</strong> El login utiliza <code>USERS.nombre</code> y <code>USERS.pass</code>.
</div>

<h2>5. Vistas y flujo</h2>
<ul>
  <li><strong>Home</strong>: carrusel + destacados + filtros + paginación.</li>
  <li><strong>Default</strong>: catálogo completo con filtros, envío por email (XLSX/PDF).</li>
  <li><strong>Detalle</strong>: ficha de producto + agregar a favoritos (logueado).</li>
  <li><strong>Login</strong>: usuario/contraseña (no email), redirección a <em>returnUrl</em>.</li>
  <li><strong>Articulos</strong>: administración con GridView (desktop) + ListView (móvil).</li>
  <li><strong>FormularioArticulo</strong>: alta/edición + subida Cloudinary con validación.</li>
  <li><strong>MarcasCategorias</strong>: ABM integral para tablas de soporte.</li>
  <li><strong>Favoritos</strong>: listado/quitar favoritos por usuario.</li>
</ul>

<h2>6. Casos de uso y validaciones</h2>
<ul>
  <li>Búsqueda con debounce y filtro por marca/categoría (UpdatePanel, UX fluida).</li>
  <li>Validaciones de campos requeridos en formularios y formatos (precio, imagenes).</li>
  <li>Manejo de errores y <em>feedback</em> visual (alertas estilo de marca).</li>
  <li>Control de sesión/rol con <code>Seguridad.sesionActiva</code> y <code>Seguridad.esAdmin</code>.</li>
</ul>

<h2>7. Seguridad y Login por Usuario</h2>
<p>Se migró el login para utilizar <strong>nombre de usuario</strong> en vez de email.</p>
<div class="caption">SQL sugerido:</div>
<pre>CREATE OR ALTER PROCEDURE dbo.sp_Users_Login
  @Nombre VARCHAR(50),
  @Pass   VARCHAR(20)
AS
BEGIN
  SET NOCOUNT ON;
  SELECT TOP 1 Id, nombre, apellido, email, urlImagenPerfil, admin
  FROM dbo.USERS
  WHERE nombre = @Nombre AND pass = @Pass;
END
</pre>
<div class="caption">Uso en C#:</div>
<pre>// UsersNegocio.Login(user) completa: Id, Nombre, Apellido, Email, UrlImagenPerfil, Admin
// Login.aspx.cs arma Users { Nombre, Pass } y redirige usando Session["returnUrl"] si existía.</pre>

<h2>8. Checklist de cumplimiento</h2>
<table class="table">
<thead><tr><th>Requisito</th><th>Estado</th><th>Notas</th></tr></thead>
<tbody>
<tr><td>Home con catálogo y filtros</td><td>✔</td><td>Home.aspx / Default.aspx con UpdatePanel y debounce</td></tr>
<tr><td>Detalle de producto</td><td>✔</td><td>Detalle.aspx</td></tr>
<tr><td>Login por usuario + rol admin</td><td>✔</td><td>Login.aspx + Seguridad.esAdmin</td></tr>
<tr><td>Listado/Búsqueda/ABM artículos</td><td>✔</td><td>Articulos.aspx + FormularioArticulo.aspx</td></tr>
<tr><td>Persistencia en DB oficial</td><td>✔</td><td>Stored procedures y AccesoDatos</td></tr>
<tr><td>Favoritos (opcional)</td><td>✔</td><td>Favoritos.aspx + FavoritosNegocio</td></tr>
</tbody>
</table>

<h2>9. Limpieza de código & convenciones</h2>
<ul>
  <li>Eliminar <em>usings</em> no utilizados y comentarios obsoletos.</li>
  <li>Unificar tipos totalmente calificados en code-behind (<code>dominio.Articulos</code>).</li>
  <li>Centralizar literales de rutas y textos repetidos.</li>
  <li>Reforzar guardas de admin en páginas de gestión (<code>MarcasCategorias.aspx.cs</code>).</li>
</ul>

<h2>10. Pruebas y criterios de aceptación</h2>
<ul>
  <li>Login con usuario válido/invalidación y redirección con <em>returnUrl</em>.</li>
  <li>ABM artículos: crear/editar/eliminar y refresco de grillas.</li>
  <li>Filtros en Home/Default: texto, marca, categoría, paginación.</li>
  <li>Detalle: visualización completa, favoritos agregando/quitar.</li>
  <li>Exportaciones (XLSX/PDF) y validación de tamaño de adjuntos.</li>
</ul>

<div class="footer">
  © 2025 Tienda Otani 
</div>
</body>
</html>
