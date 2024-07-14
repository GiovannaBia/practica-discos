using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using Negocio;

namespace Negocio
{
   public class DiscosNegocio
    {
        //método publico para que pueda usarse desde otros lados, me va a leer registros DB y agruparlos en una lista, esta
        // lista queda como una especie de lista virtual, luego para mostrarla la debo poner en este caso en la 
        //dgv!!
        public List<Discos> Listar()
        
            {
                List<Discos> lista = new List<Discos>();

                SqlConnection conexion = new SqlConnection(); //declarar objeto de tipo sqlconnection
                SqlCommand comando = new SqlCommand();  //objeto que me permita realizar acciones o comandos
                SqlDataReader lector; //aca se alojara el set de datos que obtendre de la conexion <3

                try
                {
                    conexion.ConnectionString = "server=DESKTOP-5VE62F0\\SQLEXPRESS; database=DISCOS_DB; integrated security=true";
                    comando.CommandType = System.Data.CommandType.Text;
                    comando.CommandText = "select D.Titulo, D.FechaLanzamiento, D.CantidadCanciones, D.UrlImagenTapa, E.Descripcion as Estilo, N.Descripcion as TipoEdicion, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION N where E.Id=D.IdEstilo and N.Id=D.IdTipoEdicion AND D.Activo = 1" ;
                    comando.Connection = conexion; //ejecuto ese comando declarado en esa conexion declarada <3

                    conexion.Open();
                    lector = comando.ExecuteReader();

                    while (lector.Read())
                    {
                        Discos aux = new Discos();
                        aux.Id = (int)lector["Id"];
                        // lee la columna "Id" y si no es nulo, lo guarda como un int en la property Id de la instancia aux
                        aux.Titulo = lector["Titulo"] != DBNull.Value ? (string)lector["Titulo"] : null;
                        aux.FechaLanzamiento = lector["FechaLanzamiento"] != DBNull.Value ? (DateTime?)lector["FechaLanzamiento"] : null;
                        aux.CantidadCanciones = (int)lector["CantidadCanciones"];
                        
                        aux.UrlImagenTapa = lector["UrlImagenTapa"] != DBNull.Value ? (string)lector["UrlImagenTapa"] : null;
                        aux.Estilo = new Estilo();
                    aux.Estilo.Id = (int)lector["IdEstilo"];
                        aux.Estilo.Descripcion = lector["Estilo"] != DBNull.Value ? (string)lector["Estilo"] : null;
                        aux.TipoEdicion = new TIpoEdicion();
                    aux.TipoEdicion.Id = (int)lector["IdTipoEdicion"];
                        aux.TipoEdicion.Descripcion = lector["TipoEdicion"] != DBNull.Value ? (string)lector["TipoEdicion"] : null;

                        

                        lista.Add(aux);
                    }
                         conexion.Close();
                         return lista;
            }
                catch (Exception ex)
                {

                    throw ex;
                }


            }

        //para cuando aprete en el boton aceptar del FrmAlta, siempre instanciando el acceso a datos, me agrega un 
        // objeto de clase Discos de nombre nuevo. Llamo del objeto datos el metodo setear consulta en dnde 
        // le paso como texto de comando una string ("consulta") con el insert! y seteo los parametros
        // con el @nombreenDBtalcual (coma) nuevo.artibuto
        public void Agregar (Discos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            

            try
            {
                datos.SetearConsulta("insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, IdEstilo, IdTipoEdicion) values (@Titulo, @FechaLanzamiento, @CantidadCanciones, @UrlImagenTapa, @IdEstilo, @IdTipoEdicion)");
                datos.setearParametro("@Titulo", nuevo.Titulo);
                datos.setearParametro("@FechaLanzamiento", nuevo.FechaLanzamiento);
                datos.setearParametro("@CantidadCanciones", nuevo.CantidadCanciones);
                datos.setearParametro("@IdEstilo", nuevo.Estilo.Id);  // Asegúrate de que Estilo.Id tenga un valor
                datos.setearParametro("@IdTipoEdicion", nuevo.TipoEdicion.Id);  // Asegúrate de que TipoEdicion.Id tenga un valor
                datos.setearParametro("@UrlImagenTapa", nuevo.UrlImagenTapa);

                datos.EjecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //basicamente con el comando "where id = id" lo que hace es agarrar el atributo id del seleccionado de la dgv
        // y le guarda los datos que ya estaban y los cambiados, si no me equivoco :O
        public void Modificar (Discos discos)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.SetearConsulta("UPDATE DISCOS SET Titulo = @titulo, FechaLanzamiento = @fechaLanzamiento, CantidadCanciones = @cantidadCanciones, UrlImagenTapa = @urlImagen, IdEstilo = @idEstilo, IdTipoEdicion = @idTipo WHERE Id = @id");
                datos.setearParametro("@titulo", discos.Titulo);
                datos.setearParametro("@fechaLanzamiento", discos.FechaLanzamiento);
                datos.setearParametro("@cantidadCanciones", discos.CantidadCanciones);
                datos.setearParametro("@urlImagen", discos.UrlImagenTapa);
                datos.setearParametro("@idEstilo", discos.Estilo.Id);
                datos.setearParametro("@idTipo", discos.TipoEdicion.Id);
                datos.setearParametro("@id", discos.Id);

                datos.EjecutarAccion();

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

        //elimina de la db y de todo un dato, es importante hacerle un cartelito de esta seguro!! eso lo hice en el 
        // btnEliminarFisico!
        public void Eliminar (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("DELETE FROM DISCOS WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //no elimina de la db, solo los inactiva, por eso el texto del comando no es delete sino update set activo = 0!
        public void EliminarLogico (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("UPDATE DISCOS SET Activo = 0 WHERE Id = @id");
                datos.setearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //filtra segun los parametros que le pasamos, y hace una lista que muestra en la DGV, ver bien los switchs!
        public List<Discos> Filtrar (string campo, string criterio, string filtro)
        {
            List<Discos> lista = new List<Discos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "select D.Titulo, D.FechaLanzamiento, D.CantidadCanciones, D.UrlImagenTapa, E.Descripcion as Estilo, N.Descripcion as TipoEdicion, D.IdEstilo, D.IdTipoEdicion, D.Id from DISCOS D, ESTILOS E, TIPOSEDICION N where E.Id=D.IdEstilo and N.Id=D.IdTipoEdicion AND D.Activo = 1 and ";
                switch (campo)
                {
                    case "Cantidad canciones":
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "CantidadCanciones > " + filtro;
                                break;
                            case "Menor a":
                                consulta += "CantidadCanciones < " + filtro;
                                break;
                            default:
                                consulta += "CantidadCanciones = " + filtro;
                                break;
                        }
                        break;
                    case "Titulo":
                        switch (criterio)
                        {
                            case "Empiece con ":
                                consulta += "D.Titulo like '" + filtro + "%'";
                                break;
                            case "Termine con":
                                consulta += "D.Titulo like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "D.Titulo like '%" + filtro + "%'";
                                break;
                        }
                        break;
                    default:
                        switch (criterio)
                        {
                            case "Empiece con ":
                                consulta += "E.Descripcion like '" + filtro + "%'";
                                break;
                            case "Termine con":
                                consulta += "E.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "E.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                        break;
                }

                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Discos aux = new Discos();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = datos.Lector["Titulo"] != DBNull.Value ? (string)datos.Lector["Titulo"] : null;
                    aux.FechaLanzamiento = datos.Lector["FechaLanzamiento"] != DBNull.Value ? (DateTime?)datos.Lector["FechaLanzamiento"] : null;
                    aux.CantidadCanciones = (int)datos.Lector["CantidadCanciones"];

                    aux.UrlImagenTapa = datos.Lector["UrlImagenTapa"] != DBNull.Value ? (string)datos.Lector["UrlImagenTapa"] : null;
                    aux.Estilo = new Estilo();
                    aux.Estilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.Estilo.Descripcion = datos.Lector["Estilo"] != DBNull.Value ? (string)datos.Lector["Estilo"] : null;
                    aux.TipoEdicion = new TIpoEdicion();
                    aux.TipoEdicion.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.TipoEdicion.Descripcion = datos.Lector["TipoEdicion"] != DBNull.Value ? (string)datos.Lector["TipoEdicion"] : null;



                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
