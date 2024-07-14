using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        public SqlDataReader Lector 
        { 
            get { return lector; }
        }

        //constructor, cada vez que quiera establecer una conexion, instancio un objeto de este
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=DESKTOP-5VE62F0\\SQLEXPRESS; database=DISCOS_DB; integrated security=true");
            comando = new SqlCommand();
        }

        //una funcion qe me permita setear la consulta que va en el comando (en su commandText)
        public void SetearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        //esta funcion ejecuta la lectura y la guarda
        public void EjecutarLectura () 
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void EjecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //este metodo es para cuando me refiero a datos de sql, por ej si quiero agregar un objeto con sus datos, 
        // el string nombbre pondo @algo que va a ser el nombre de la columna en la DB, y el valor será lo que
        // le asigne y lo mande a la DB, es object para aceptar cualquier tipo de dato,  ej algo.nombre
        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void CerrarConexion()
        {
            if (lector != null)
            {
                lector.Close();

                conexion.Close();
            }
        }
    }

   
}
