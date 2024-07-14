using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;


namespace Negocio
{
    public class TipoEdicionNegocio
    {
        public List<TIpoEdicion> listar()
        {
            List<TIpoEdicion> lista = new List<TIpoEdicion>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("select Id, Descripcion from TIPOSEDICION");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TIpoEdicion aux = new TIpoEdicion();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

            finally
            {
                datos.CerrarConexion();
            }
        }


    }
    
}
