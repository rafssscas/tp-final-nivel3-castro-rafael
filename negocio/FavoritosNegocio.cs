using dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class FavoritosNegocio
    {
        // Devuelve una lista con los Ids de los artículos favoritos de un usuario
        public List<Favoritos> listarPorUser(int idUser)
        {
            var lista = new List<Favoritos>();
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Favoritos_ListarPorUser");
                datos.setearParametro("@IdUser", idUser);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    var art = new Articulos
                    {
                        Id = (int)datos.Lector["Id"],
                        Codigo = datos.Lector["Codigo"] as string,
                        Nombre = datos.Lector["Nombre"] as string,
                        Descripcion = datos.Lector["Descripcion"] as string,
                        ImagenUrl = datos.Lector["ImagenUrl"] as string,
                        Precio = datos.Lector["Precio"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(datos.Lector["Precio"]),
                        IdMarca = (int)datos.Lector["IdMarca"],
                        IdCategoria = (int)datos.Lector["IdCategoria"]
                    };

                    lista.Add(new Favoritos
                    {
                        Id = (int)datos.Lector["IdFavorito"],
                        IdUser = idUser,
                        IdArticulo = art.Id
                        
                    });
                }
                return lista;
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public void agregar(int idUser, int idArticulo)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Favoritos_Agregar");
                datos.setearParametro("@IdUser", idUser);
                datos.setearParametro("@IdArticulo", idArticulo);
                
                datos.ejecutarAccion();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }

        public void eliminar(int idFavorito)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.setearProcedimiento("dbo.sp_Favoritos_Eliminar");
                datos.setearParametro("@Id", idFavorito);
                datos.ejecutarAccion();
            }
            catch { throw; }
            finally { datos.cerrarConexion(); }
        }
    }
}
