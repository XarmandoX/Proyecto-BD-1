using GestionInventarioElectroMobile.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionInventarioElectroMobile.Views
{
    public partial class frmEditarDevCompra : Form
    {

        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        DataTable dt = new DataTable();
        int indexRow;

        DataRow drDevCompra;

        public DataRow DrDevCompra

        {
            set
            {
                drDevCompra = value;
                txtID_Dev.Text = drDevCompra["ID_DevolucionCompras"].ToString();
                txtID_Compra.Text = drDevCompra["ID_Compra"].ToString();
                txtDesc.Text = drDevCompra["Descripcion"].ToString();
                lblDate.Text = drDevCompra["Fecha"].ToString();
                lblTotal.Text = drDevCompra["Total"].ToString();
            }

        }

        public frmEditarDevCompra()
        {
            InitializeComponent();
        }

        

        private void frmEditarDevCompra_Load(object sender, EventArgs e)
        {
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var searchValue = model.Detalle_compra.Where(p => p.ID_Compra == txtID_Compra.Text).Select(p => p.ID_Pieza);
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                auto.AddRange(searchValue.ToArray());
                this.txtPieza.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.txtPieza.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtPieza.AutoCompleteCustomSource = auto;
            }

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnLimpiar.Enabled = false;
            btnDevolucion.Enabled = false;

            dt.Columns.Add("ID_DevolucionCompra", typeof(int));
            dt.Columns.Add("Pieza", typeof(string));
            dt.Columns.Add("cantidad", typeof(int));
            dt.Columns.Add("precioCompra", typeof(decimal));
            dt.Columns.Add("subtotal", typeof(decimal));
            dgvDetDevCompra.DataSource = dt;

            lblDate.Text = DateTime.Now.ToString("dd/M/yy");

            txtID_Dev.Enabled = false;
            txtID_Compra.Enabled = false;

            CargarDetDevCompra();
            CalcularTotal();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || txtID_Compra.Text == "" || txtDesc.Text == "" || txtPieza.Text == "")
            {
                MessageBox.Show("Inserte la informacion pedida", "Atencion");
                return;
            }

            if (!txtCantidad.Text.All(char.IsDigit))
            {
                MessageBox.Show("Inserte valores numericos en cantidad");
                txtCantidad.Clear();
                return;
            }
            var id_compra = txtID_Compra.Text;
            var pieza = bD.Pieza.
                Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();
            var pieza_ya_existente = bD.DetalleDevCompra.Where(p => p.ID_Pieza == pieza).Select(p => p.ID_Pieza).FirstOrDefault();
            var cantidad = txtCantidad.Text;

            var precio = bD.Detalle_compra.Where(p => p.ID_Compra == id_compra && p.ID_Pieza == pieza).
                Select(p => p.precio).
                FirstOrDefault();

            var subtotal = int.Parse(cantidad) * float.Parse(precio.ToString());
            var DescDev = txtDesc.Text;

            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }

            if (pieza == pieza_ya_existente)
            {
                MessageBox.Show("Producto ya detallado en la devolucion, si desea edite la cantidad");
                return;
            }

            dt.Rows.Add(id_compra, pieza, cantidad, precio, subtotal, DescDev);

            CalcularTotal();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || txtID_Compra.Text == "" || txtDesc.Text == "" || txtPieza.Text == "")
            {
                MessageBox.Show("Inserte la informacion pedida", "Atencion");
                return;
            }
            var id_compra = txtID_Compra.Text;

            var pieza = bD.Pieza.Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();
            var pieza_ya_existente = bD.DetalleDevVenta.Where(p => p.ID_Pieza == pieza).Select(p => p.ID_Pieza).FirstOrDefault();
            var cantidad = txtCantidad.Text;

            var precio = bD.Detalle_compra.Where(p => p.ID_Compra == id_compra && p.ID_Pieza == pieza).
                Select(p => p.precio).
                FirstOrDefault();

            var subtotal = int.Parse(cantidad) * float.Parse(precio.ToString());

            DataGridViewRow row = dgvDetDevCompra.Rows[indexRow];
            int cant_anterior = int.Parse((string)row.Cells[3].Value);
            int cantidad_actual = int.Parse(txtCantidad.Text);
            

            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }

            if (pieza == pieza_ya_existente)
            {
                MessageBox.Show("Producto ya detallado en la devolucion, si desea edite la cantidad");
                return;
            }
            
            if (cant_anterior > cantidad_actual)
            {
                int temp = cant_anterior - cantidad_actual;
                lblTotal.Text = (decimal.Parse(lblTotal.Text) - (precio * temp)).ToString();
            }
            else
            {
                int temp = cantidad_actual - cant_anterior;
                lblTotal.Text = (decimal.Parse(lblTotal.Text) + (precio * temp)).ToString();
            }

            dt.Rows.Add(id_compra, pieza, cantidad, precio, subtotal);
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.Cells[0].Value = txtID_Dev.Text;
            newRow.Cells[1].Value = txtPieza.Text;
            newRow.Cells[2].Value = int.Parse(txtCantidad.Text);
            newRow.Cells[3].Value = decimal.Parse(precio.ToString());
            newRow.Cells[4].Value = decimal.Parse(subtotal.ToString());
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetDevCompra.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvDetDevCompra.CurrentCell.RowIndex;

                DataGridViewRow row = dgvDetDevCompra.Rows[rowIndex];
                int cant = int.Parse((string)row.Cells[3].Value);
                decimal precio = decimal.Parse((string)row.Cells[4].Value);
                lblTotal.Text = (decimal.Parse(lblTotal.Text) - (cant * precio)).ToString();
                dgvDetDevCompra.Rows.RemoveAt(rowIndex);
                txtPieza.Text = "";
                txtCantidad.Text = "";
                return;
            }
            
            MessageBox.Show("Selecciona la hilera a eliminar");
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, "¿Seguro que desea Cancelar la Accion?", "Cancelar Accion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                Dispose();
            }
            else
            {
                return;
            }
        }

        public void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow r in dgvDetDevCompra.Rows)
            {
                int cant;
                decimal precio;
                cant = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                precio = decimal.Parse(r.Cells["precioCompra"].FormattedValue.ToString());
                decimal v = cant * precio;
                total += v;

            }
            lblTotal.Text = total.ToString();
        }

        public void CargarDetDevCompra()
        {
            var det_devcompra = bD.DetalleDevCompra.Where(p => p.ID_DevolucionCompra == int.Parse(txtID_Dev.Text));
            var precio_piezas = bD.Detalle_compra.Where(p => p.ID_Compra == txtID_Compra.Text);

            

            foreach (var det in det_devcompra.ToList())
            {
                
                foreach (var precio in precio_piezas.ToList())
                {
                    if (precio.ID_Pieza == det.ID_Pieza)
                    {
                        dgvDetDevCompra.Rows.Add(txtID_Compra.Text, det.ID_Pieza, det.Cantidad, precio.precio, (det.Cantidad * precio.precio));
                    }
                }
                
            }
        }

        private void btnDevolucion_Click(object sender, EventArgs e)
        {
            int id_dev = int.Parse(txtID_Dev.Text);

            bD.EditarDevolucionDeCompra(id_dev, txtID_Dev.Text);
            bD.SaveChangesAsync();

            foreach (DataGridViewRow r in dgvDetDevCompra.Rows)
            {
                var pieza = r.Cells["Pieza"].FormattedValue.ToString();
                int cantidad = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                var cant_anterior = bD.DetalleDevCompra.Where(p => p.ID_DevolucionCompra == int.Parse(txtDesc.Text) && p.ID_Pieza == pieza).Select(p => p.Cantidad);
                if (int.Parse(cant_anterior.ToString()) != cantidad)
                {
                    bD.EditarDetalleDevCompra(int.Parse(txtID_Dev.Text), pieza, cantidad);
                    bD.SaveChangesAsync();
                }
                
            }
        }

        private void dgvDetDevCompra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvDetDevCompra.Rows.Count >= 0)
            {
                DataGridViewRow row = dgvDetDevCompra.Rows[indexRow];

                txtPieza.Text = row.Cells[1].Value.ToString();
                txtCantidad.Text = row.Cells[2].Value.ToString();
                btnAgregar.Enabled = false;
                btnEliminar.Enabled = true;
                btnEditar.Enabled = true;
                btnLimpiar.Enabled = true;
            }
            else
                return;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, "¿Seguro que desea Cancelar la Accion?", "Cancelar Accion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                txtPieza.Text = "";
                txtCantidad.Text = "";
                btnAgregar.Enabled = true;
                btnLimpiar.Enabled = false;
            }
            else
            {
                return;
            }
        }
    }
}
