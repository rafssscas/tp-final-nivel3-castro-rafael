using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
        public class AccesoDatos
        {
            private SqlConnection conexion;
            private SqlCommand comando;
            private SqlDataReader lector;
            public SqlDataReader Lector => lector;

            public AccesoDatos()
            {
                var cs = ConfigurationManager.ConnectionStrings["cadenaConexion"]?.ConnectionString;
                if (string.IsNullOrWhiteSpace(cs))
                    throw new InvalidOperationException("Falta 'cadenaConexion' en <connectionStrings> de Web.config.");

                conexion = new SqlConnection(cs);
                comando = new SqlCommand();
            }

            public void setearConsulta(string consulta)
            {
                comando.Parameters.Clear();
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = consulta;
            }

            public void setearProcedimiento(string sp)
            {
                comando.Parameters.Clear();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = sp;
            }

            public void ejecutarLectura()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    lector = comando.ExecuteReader();
                }
                catch
                {
                    throw;
                }
            }

            public void ejecutarAccion()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }

            public int ejecutarAccionScalar()
            {
                comando.Connection = conexion;
                try
                {
                    conexion.Open();
                    return Convert.ToInt32(comando.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
            }

            public void setearParametro(string nombre, object valor)
            {
                comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            }

            public void cerrarConexion()
            {
                if (lector != null && !lector.IsClosed) lector.Close();
                if (conexion.State == System.Data.ConnectionState.Open) conexion.Close();
            }
        }
}
