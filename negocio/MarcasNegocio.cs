using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class MarcasNegocio
    {
        public List<Marcas> listar()
        {
            var lista = new List<Marcas>();
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Marcas_Listar");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(new Marcas
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

        public Marcas obtenerPorId(int id)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Marcas_ObtenerPorId");
                datos.setearParametro("@Id", id);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    return new Marcas
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

        public int agregar(Marcas m)
        {
            if (m == null || string.IsNullOrWhiteSpace(m.Descripcion))
                throw new ArgumentException("Descripción requerida.");

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Marcas_Insertar");
                datos.setearParametro("@Descripcion", m.Descripcion);
                return datos.ejecutarAccionScalar(); // SCOPE_IDENTITY()
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public void modificar(Marcas m)
        {
            if (m == null || m.Id <= 0 || string.IsNullOrWhiteSpace(m.Descripcion))
                throw new ArgumentException("Datos de marca inválidos.");

            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Marcas_Actualizar");
                datos.setearParametro("@Id", m.Id);
                datos.setearParametro("@Descripcion", m.Descripcion);
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
                datos.setearProcedimiento("dbo.sp_Marcas_Eliminar");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();   // el SP bloquea si hay artículos asociados
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }
    }
}