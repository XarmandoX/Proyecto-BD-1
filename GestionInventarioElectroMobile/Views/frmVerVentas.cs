using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionInventarioElectroMobile.Model;

namespace GestionInventarioElectroMobile.Views
{
    public partial class frmVerVentas : Form
    {
        int indexRow;
        public frmVerVentas()
        {
            InitializeComponent();
        }

        private void frmVerVentas_Load(object sender, EventArgs e)
        {
            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                dgvVerVentas.DataSource = model.InformacionDeVentas.ToList();
            }

        }

        private void dgvVerVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvVerVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            DataGridViewRow row = dgvVerVentas.Rows[indexRow];
            txtIdPieza.Text = row.Cells[2].Value.ToString();
            txtIdVenta.Text = row.Cells[0].Value.ToString();
            txtCant.Text = row.Cells[6].Value.ToString();
            dtFechaEntrega.Value = DateTime.Parse(row.Cells[1].Value.ToString());
            txtEstado.Text = row.Cells[4].Value.ToString();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.EditarDetalle_venta(int.Parse(txtCant.Text), txtIdPieza.Text, int.Parse(txtCant.Text));
                model.EditarVenta(int.Parse(txtIdVenta.Text), null, DateTime.Parse(dtFechaEntrega.Value.ToShortDateString()), txtEstado.Text);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (indexRow < 0)
            {
                using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
                {
                    string codProd = model.Pieza.Where(p => p.ID_Pieza == txtIdPieza.Text.ToString()).Select(p => p.ID_Pieza).FirstOrDefault().ToString();
                    if (codProd != txtIdPieza.Text)
                    {
                        MessageBox.Show("El producto no esta registrado", "Alerta");
                        return;
                    }

                    model.Venta.Remove((Venta)model.Venta.
                        Where(V => V.ID_Venta == int.Parse(txtIdVenta.Text)).
                        FirstOrDefault());
                    model.SaveChanges();
                    model.EliminarDetalle_venta(int.Parse(txtIdVenta.Text), txtIdPieza.Text.ToString());
                    MessageBox.Show("La venta ha sido eliminada", "informacion");
                }
            }
            else
                MessageBox.Show("Seleccione la columna a eliminar", "Alerta");
        }
    }
}
