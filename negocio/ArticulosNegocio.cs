using dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace negocio
{
    public class ArticulosNegocio
    {
        // LISTAR (sin filtros)
        public List<Articulos> listar()
        {
            var lista = new List<Articulos>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("dbo.sp_Articulos_Listar");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                    lista.Add(MapArticulo(datos.Lector));

                return lista;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // BUSCAR (filtros opcionales)
        public List<Articulos> buscar(string texto, int? idMarca, int? idCategoria)
        {
            var lista = new List<Articulos>();
            var datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("dbo.sp_Articulos_Buscar");
                datos.setearParametro("@Texto", (object)texto ?? DBNull.Value);
                datos.setearParametro("@IdMarca", (object)idMarca ?? DBNull.Value);
                datos.setearParametro("@IdCategoria", (object)idCategoria ?? DBNull.Value);

                datos.ejecutarLectura();
                while (datos.Lector.Read())
                    lista.Add(MapArticulo(datos.Lector));

                return lista;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // OBTENER POR ID
        public Articulos obtenerPorId(int id)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Articulos_ObtenerPorId");
                datos.setearParametro("@Id", id);

                datos.ejecutarLectura();
                if (datos.Lector.Read())
                    return MapArticulo(datos.Lector);

                return null;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // AGREGAR → devuelve Id nuevo
        public int agregar(Articulos a)
        {
            Validar(a, isUpdate: false);

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Articulos_Insertar");
                datos.setearParametro("@Codigo", (object)a.Codigo ?? DBNull.Value);
                datos.setearParametro("@Nombre", a.Nombre);
                datos.setearParametro("@Descripcion", (object)a.Descripcion ?? DBNull.Value);
                datos.setearParametro("@IdMarca", a.IdMarca);
                datos.setearParametro("@IdCategoria", a.IdCategoria);
                datos.setearParametro("@ImagenUrl", (object)a.ImagenUrl ?? DBNull.Value);
                datos.setearParametro("@Precio", (object)a.Precio ?? DBNull.Value);

                return datos.ejecutarAccionScalar(); // SCOPE_IDENTITY()
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // MODIFICAR
        public void modificar(Articulos a)
        {
            Validar(a, isUpdate: true);

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Articulos_Actualizar");
                datos.setearParametro("@Id", a.Id);
                datos.setearParametro("@Codigo", (object)a.Codigo ?? DBNull.Value);
                datos.setearParametro("@Nombre", a.Nombre);
                datos.setearParametro("@Descripcion", (object)a.Descripcion ?? DBNull.Value);
                datos.setearParametro("@IdMarca", a.IdMarca);
                datos.setearParametro("@IdCategoria", a.IdCategoria);
                datos.setearParametro("@ImagenUrl", (object)a.ImagenUrl ?? DBNull.Value);
                datos.setearParametro("@Precio", (object)a.Precio ?? DBNull.Value);

                datos.ejecutarAccion();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // ELIMINAR
        public void eliminar(int id)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Articulos_Eliminar");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        // =========================
        // NUEVO: listar para exportación (semántica)
        // =========================
        public List<Articulos> listarParaExport()
        {
            // Podrías llamar a un SP específico si querés (ej: sp_Articulos_ListarParaExport)
            return listar();
        }
  

        // =========================
        // Helpers
        // =========================
        private static Articulos MapArticulo(SqlDataReader dr)
        {
            var art = new Articulos
            {
                Id = (int)dr["Id"],
                Codigo = dr["Codigo"] as string,
                Nombre = dr["Nombre"] as string,
                Descripcion = dr["Descripcion"] as string,
                IdMarca = (int)dr["IdMarca"],
                IdCategoria = (int)dr["IdCategoria"],
                ImagenUrl = dr["ImagenUrl"] as string,
                Precio = dr["Precio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dr["Precio"]),
                // Aliases devueltos por los SPs
                Marca = new Marcas
                {
                    Id = (int)dr["IdMarca"],
                    Descripcion = dr["Marca"] as string
                },
                Categoria = new Categorias
                {
                    Id = (int)dr["IdCategoria"],
                    Descripcion = dr["Categoria"] as string
                }
            };

            

            return art;
        }

        private static void Validar(Articulos a, bool isUpdate)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            if (isUpdate && a.Id <= 0) throw new ArgumentException("Id inválido para actualización.");
            if (string.IsNullOrWhiteSpace(a.Nombre)) throw new ArgumentException("El Nombre es obligatorio.");
            if (a.IdMarca <= 0) throw new ArgumentException("La Marca es obligatoria.");
            if (a.IdCategoria <= 0) throw new ArgumentException("La Categoría es obligatoria.");
            if (a.Precio.HasValue && a.Precio.Value < 0) throw new ArgumentException("El Precio no puede ser negativo.");
        }

        private static bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        public List<Articulos> listarByIds(IEnumerable<int> ids)
        {
            
            var todos = listar() ?? new List<Articulos>();
            var set = new HashSet<int>(ids);
            return todos.Where(a => set.Contains(a.Id)).ToList();
        }
    }
}
