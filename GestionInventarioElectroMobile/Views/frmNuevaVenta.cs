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
    public partial class frmNuevaVenta : Form
    {
        DataTable dt = new DataTable();
        public frmNuevaVenta()
        {
            InitializeComponent();
        }

        private void frmNuevaVenta_Load(object sender, EventArgs e)
        {
            dt.Columns.Add("Nombre cliente", typeof(string));
            dt.Columns.Add("Apellido cliente", typeof(string));
            dt.Columns.Add("Codigo de producto", typeof(string));
            dt.Columns.Add("Tipo de pieza", typeof(string));
            dt.Columns.Add("cantidad", typeof(int));
            dt.Columns.Add("precio", typeof(decimal));
            dt.Columns.Add("subtotal", typeof(decimal));

            dgvNVenta.DataSource = dt;
            btnRealizarVenta.Enabled = false;
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var searchValue = model.Pieza.Select(p => p.ID_Pieza);
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                auto.AddRange(searchValue.ToArray());
                this.txtPieza.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.txtPieza.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtPieza.AutoCompleteCustomSource = auto;
            }
        }

        private void dgvNVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }
            if (dgvNVenta.Rows.Count >= 0)
            {
                DataGridViewRow row = dgvNVenta.Rows[indexRow];

                txtNombre.Text = row.Cells[0].Value.ToString();
                txtApellido.Text = row.Cells[1].Value.ToString();
                txtPieza.Text = row.Cells[2].Value.ToString();
                txtCantidad.Text = row.Cells[3].Value.ToString();

            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtPieza.Text == "" || txtCantidad.Text == "")
            {
                MessageBox.Show("Inserte la informacion pedida", "Alerta");
                return;
            }

            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                int existenciaProd = model.Pieza.Where(p => p.ID_Pieza == txtPieza.Text).Select(p => p.cantidad).FirstOrDefault();
                if (int.Parse(txtCantidad.Text) > existenciaProd)
                {
                    MessageBox.Show("La cantidad que desea comprar es mayor a la que hay en existencia", "informacion");
                    return;
                }
            }

            if (isNumericValue(txtCantidad))
            {
                MessageBox.Show("Insertar valores numericos en el campo de cantidad", "Alerta");
                return;
            }

            string producto;
            decimal precio;
            string tipoPieza;

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {

                producto = model.Pieza.
                    Where(p => p.ID_Pieza == txtPieza.Text).
                    FirstOrDefault().ToString();

                precio = decimal.Parse(model.Pieza.
                    Where(p => p.ID_Pieza.ToString() == txtPieza.Text).
                    Select(p => p.precio).
                    FirstOrDefault().ToString());

                tipoPieza = model.Pieza.
                    Where(p => p.ID_Pieza == txtPieza.Text).
                    Select(p => p.Tipo_piezas).
                    FirstOrDefault().ToString();
            }

            if (producto != txtPieza.Text)
            {
                MessageBox.Show("Pieza no registrada");
                return;
            }
            decimal subTotalV = int.Parse(txtCantidad.Text) * precio;
            dt.Rows.Add(txtNombre.Text, txtApellido.Text, producto, tipoPieza, int.Parse(txtCantidad.Text), precio, subTotalV);
            btnRealizarVenta.Enabled = false;
        }

        private bool isNumericValue(TextBox txt)
        {
            return txt.Text.All(char.IsDigit);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || txtPieza.Text == "")
            {
                MessageBox.Show("No deje los campos vacios", "Alerta");
                return;
            }


            string producto;
            decimal precio;
            string tipoPieza;

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {

                producto = model.Pieza.
                    Where(p => p.ID_Pieza == txtPieza.Text).
                    FirstOrDefault().ToString();

                precio = decimal.Parse(model.Pieza.
                    Where(p => p.ID_Pieza.ToString() == txtPieza.Text).
                    Select(p => p.precio).
                    FirstOrDefault().ToString());

                tipoPieza = model.Pieza.
                    Where(p => p.ID_Pieza == txtPieza.Text).
                    Select(p => p.Tipo_piezas).
                    FirstOrDefault().ToString();
            }

            if (producto != txtPieza.Text)
            {
                MessageBox.Show("Pieza no registrada");
                return;
            }
            decimal subTotalV = int.Parse(txtCantidad.Text) * precio;

            DataGridViewRow newRow = new DataGridViewRow();
            newRow.Cells[0].Value = txtNombre.Text;
            newRow.Cells[1].Value = txtApellido.Text;
            newRow.Cells[2].Value = producto;
            newRow.Cells[3].Value = tipoPieza;
            newRow.Cells[4].Value = int.Parse(txtCantidad.Text);
            newRow.Cells[5].Value = precio;
            newRow.Cells[6].Value = subTotalV;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvNVenta.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvNVenta.CurrentCell.RowIndex;
                dgvNVenta.Rows.RemoveAt(rowIndex);
                return;
            }

            MessageBox.Show("Seleccionar la hilera a eliminar", "Alerta");
        }

        private void btnRealizarVenta_Click(object sender, EventArgs e)
        {
            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.InsertarVenta(null, null);

                var idVenta = int.Parse(model.Venta.Max(v => v.ID_Venta).ToString());

                for (int i = 0; i < dgvNVenta.Rows.Count; i++)
                {
                    string idPieza = dgvNVenta.Rows[i].Cells["Codigo de producto"].FormattedValue.ToString();

                    string codProd = model.Pieza.
                        Where(p => p.ID_Pieza.ToString().Equals(idPieza)).
                        Select(p => p.ID_Pieza).
                        FirstOrDefault();

                    int cantidad = int.Parse(dgvNVenta.Rows[i].Cells["cantidad"].FormattedValue.ToString());

                    model.AgregarDetalleVenta(idVenta, codProd, cantidad);
                }
                model.SaveChanges();
            }
            MessageBox.Show("Se ha realizado la venta con exito", "Informacion");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
