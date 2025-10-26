using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class CategoriasNegocio
    {
        public List<Categorias> listar()
        {
            var lista = new List<Categorias>();
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Categorias_Listar");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(new Categorias
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = datos.Lector["Descripcion"] as string
                    });
                }
                return lista;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public Categorias obtenerPorId(int id)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Categorias_ObtenerPorId");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return new Categorias
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = datos.Lector["Descripcion"] as string
                    };
                }
                return null;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public int agregar( Categorias c)
        {
            if (c == null || string.IsNullOrWhiteSpace(c.Descripcion))
                throw new ArgumentException("Descripción requerida.");

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Categorias_Insertar");
                datos.setearParametro("@Descripcion", c.Descripcion);
                return datos.ejecutarAccionScalar();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public void modificar(Categorias c)
        {
            if (c == null || c.Id <= 0 || string.IsNullOrWhiteSpace(c.Descripcion))
                throw new ArgumentException("Datos de categoría inválidos.");

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Categorias_Actualizar");
                datos.setearParametro("@Id", c.Id);
                datos.setearParametro("@Descripcion", c.Descripcion);
                datos.ejecutarAccion();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public void eliminar(int id)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Categorias_Eliminar");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();   // el SP bloquea si hay artículos asociados
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }
    }
}
