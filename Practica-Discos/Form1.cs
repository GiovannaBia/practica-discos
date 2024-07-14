using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Practica_Discos
{
    public partial class Form1 : Form
    {

        private List<Discos> listaDiscos; 

        public Form1()
        {
            InitializeComponent();
        }

        private void cargarImagen(string imagen)
        {
            try
            {
                pbxDiscos.Load(imagen);
            }
            catch (Exception)
            {

                pbxDiscos.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRKZ1SMO3FUQBP7gzSU3d1Rr1SqSIQzqKdqVA&usqp=CAU");
            }
            
        }
       

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Cantidad canciones");
            cboCampo.Items.Add("Titulo");
            cboCampo.Items.Add("Estilo");
        }

        private void dgvDiscos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDiscos.CurrentRow != null)
            {
                Discos seleccionado= (Discos) dgvDiscos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagenTapa);
            }
        }

        private void cargar()
        {
            DiscosNegocio negocio = new DiscosNegocio();
            listaDiscos = negocio.Listar();
            dgvDiscos.DataSource = listaDiscos;
            ocultarColumnas();
            cargarImagen(listaDiscos[0].UrlImagenTapa);
        }

        private void ocultarColumnas()
        {
            dgvDiscos.Columns["UrlImagenTapa"].Visible = false;
            dgvDiscos.Columns["Id"].Visible = false;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAltaDisco alta = new frmAltaDisco();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Discos seleccionado;
            seleccionado = (Discos)dgvDiscos.CurrentRow.DataBoundItem;
            
            frmAltaDisco modificar = new frmAltaDisco(seleccionado);
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            Eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            Eliminar(true);
        }
        
        private void Eliminar(bool logico = false)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            Discos seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("Está seguro que desea eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (respuesta == DialogResult.Yes)
                {
                    seleccionado = (Discos)dgvDiscos.CurrentRow.DataBoundItem;

                    if (logico)
                    {
                        negocio.EliminarLogico(seleccionado.Id);
                    }
                    else
                    {
                        negocio.Eliminar(seleccionado.Id);
                    }
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            DiscosNegocio negocio = new DiscosNegocio();
            try
            {
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvDiscos.DataSource = negocio.Filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            List<Discos> listaFiltrada;
            string filtro = txtFiltro.Text;

            if (filtro.Length >= 2)
            {
                listaFiltrada = listaDiscos.FindAll(x => x.Titulo.ToUpper().Contains(filtro.ToUpper()) || x.Estilo.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.TipoEdicion.Descripcion.ToUpper().Contains(filtro.ToUpper()));
            }
            else
            {
                listaFiltrada = listaDiscos;
            }

            dgvDiscos.DataSource = null;
            dgvDiscos.DataSource = listaFiltrada;
            ocultarColumnas();
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cboCampo.SelectedItem.ToString();
            if (opcion == "Cantidad canciones")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            } 
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Empieza con ");
                cboCriterio.Items.Add("Termina con ");
                cboCriterio.Items.Add("Contiene ");
            }
        }
    }
}
