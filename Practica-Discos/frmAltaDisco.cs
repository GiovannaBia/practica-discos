using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Practica_Discos
{
    public partial class frmAltaDisco : Form
    {
        private Discos disco = null;
        public frmAltaDisco()
        {
            InitializeComponent();
        }

        public frmAltaDisco(Discos disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificar disco";
        }

        // cierra la ventana de alta de discos
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //pregunta si esta nulo "disco" agrega uno nuevo, y antes de mandarlo a la db, setea este disco con 
        // lo que le pusimos en el formulario, luego si el id =! 0 entra en el metodo modificar, sino en el agrgar
        // 
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();

            try
            {   if (disco == null) //si esta distinto de null el constructor no era el que hay que poner un disco!
                    disco = new Discos();

                disco.Titulo = txtTitulo.Text;
                disco.FechaLanzamiento = dtpFecha.Value;
                disco.CantidadCanciones = int.Parse(txtCantidad.Text);
                if (cboEstilo.SelectedItem != null && cboTipoEdicion.SelectedItem != null)
                {
                    disco.Estilo = (Estilo)cboEstilo.SelectedItem;
                    disco.TipoEdicion = (TIpoEdicion)cboTipoEdicion.SelectedItem;
                }
                disco.UrlImagenTapa = txtUrlImagen.Text;

                if (disco.Id != 0)
                {
                    negocio.Modificar(disco);
                    MessageBox.Show("Modificado exitosamente!");
                }
                else
                {
                    negocio.Agregar(disco);
                    MessageBox.Show("Agregado exitosamente!");
                }


               
                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


       //aca se carga la dgv
        public void frmAltaDisco_Load(object sender, EventArgs e)
        {
            EstiloNegocio estilo = new EstiloNegocio();
            TipoEdicionNegocio tipoedicion = new TipoEdicionNegocio();

            try
            {
                //Configura el ComboBox "cboEstilo" para mostrar la propiedad "Descripcion" usando la propiedad "Id" como valor
                cboEstilo.DisplayMember = "Descripcion";
                cboEstilo.ValueMember = "Id";
                //Establece el origen de los datos de CboEstilo usando el metodo listar() de la clase instanciada estilo (EstiloNegogio)
                cboEstilo.DataSource = estilo.listar();
                cboTipoEdicion.DisplayMember = "Descripcion";
                cboTipoEdicion.ValueMember = "Id";
                cboTipoEdicion.DataSource = tipoedicion.listar();

                if (disco != null) 
                {
                  // Rellena los controles del formulario, los precarga digamos
                    txtTitulo.Text = disco.Titulo;
                    dtpFecha.Value = disco.FechaLanzamiento.Value;
                    txtCantidad.Text = disco.CantidadCanciones.ToString();
                    txtUrlImagen.Text = disco.UrlImagenTapa;
                    cargarImagen(disco.UrlImagenTapa);
                    //ahora para que se carguen los desplegables
                    cboEstilo.SelectedValue = disco.Estilo.Id;
                    cboTipoEdicion.SelectedValue = disco.TipoEdicion.Id;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        
        //llamo al metodo cargarImagen y el parametro se pasara con el texto que haya en el textbox de la url
        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }
        //es un metodo encapsulado (porque lo usare bastante) le paso por parametro el link, y lo carga en la picturebox
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDisco.Load(imagen);
            }
            catch (Exception)
            {

                pbxDisco.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRKZ1SMO3FUQBP7gzSU3d1Rr1SqSIQzqKdqVA&usqp=CAU");
            }

        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo = new OpenFileDialog();
            archivo.Filter = "jpg |* .jpg";

            if (archivo.ShowDialog()==DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;

                cargarImagen(archivo.FileName);

            }
        }
    }
}
