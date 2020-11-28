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
    public partial class frmEditarDevVenta : Form
    {
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        DataTable dt = new DataTable();
        int indexRow;

        DataRow drDevVenta;

        public DataRow DrDevVenta

        {
            set
            {
                drDevVenta = value;
                txtID_Dev.Text = drDevVenta["ID_DevVenta"].ToString();
                txtID_Venta.Text = drDevVenta["ID_Venta"].ToString();
                txtDesc.Text = drDevVenta["Descripcion"].ToString();
                lblDate.Text = drDevVenta["Fecha"].ToString();
                lblTotal.Text = drDevVenta["Total"].ToString();
            }

        }


        public frmEditarDevVenta()
        {
            InitializeComponent();
        }

        private void frmEditarDevVentacs_Load(object sender, EventArgs e)
        {
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var searchValue = model.Detalle_venta.Where(p => p.ID_Venta == int.Parse(txtID_Venta.Text)).Select(p => p.ID_Pieza);
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                auto.AddRange(searchValue.ToArray());
                this.txtPieza.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.txtPieza.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtPieza.AutoCompleteCustomSource = auto;
            }

            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
            btnDevolucion.Enabled = false;

            dt.Columns.Add("ID_DevVenta", typeof(int));
            dt.Columns.Add("Pieza", typeof(string));
            dt.Columns.Add("cantidad", typeof(int));
            dt.Columns.Add("precioVenta", typeof(decimal));
            dt.Columns.Add("subtotal", typeof(decimal));
            dgvDetDevVenta.DataSource = dt;

            CargarDetDevCompra();
            CalcularTotal();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || txtID_Venta.Text == "" || txtPieza.Text == "")
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


            var id_venta = txtID_Venta.Text;
            var pieza = bD.Pieza.
                Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();

            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }
            var pieza_ya_existente = bD.DetalleDevVenta.Where(p => p.ID_Pieza == pieza).Select(p => p.ID_Pieza).FirstOrDefault();
            if (pieza == pieza_ya_existente)
            {
                MessageBox.Show("Producto ya detallado en la devolucion, si desea edite la cantidad");
                return;
            }

            var cantidad = txtCantidad.Text;

            var precio = bD.Pieza.
               Select(p => p.precio).
               FirstOrDefault();

            var subtotal = int.Parse(cantidad) * float.Parse(precio.ToString());

            dt.Rows.Add(id_venta, pieza, cantidad, precio, subtotal);
            if (dgvDetDevVenta.Rows.Count == 1)
            {
                txtID_Venta.Enabled = false;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                btnDevolucion.Enabled = true;
            }
            CalcularTotal();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || txtID_Venta.Text == "" || txtPieza.Text == "")
            {
                MessageBox.Show("Inserte la informacion pedida", "Atencion");
                return;
            }

            var id_compra = txtID_Venta.Text;

            var pieza = bD.Pieza.Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();

            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }
            int cantidad = int.Parse(txtCantidad.Text);
            var cant_detallada = bD.Detalle_venta.Where(p => p.ID_Pieza == pieza && p.ID_Venta == int.Parse(txtID_Venta.Text)).Select(p => p.cantidad).FirstOrDefault();
            if (cantidad > cant_detallada)
            {
                MessageBox.Show("Cantidad de la pieza excede la que se encuentra en la venta ");
                return;
            }
            var precio = bD.Pieza.Where(p => p.ID_Pieza == pieza).
                Select(p => p.precio).
                FirstOrDefault();
            var subtotal = cantidad * float.Parse(precio.ToString());

            DataGridViewRow row = dgvDetDevVenta.Rows[indexRow];

            int cant_anterior = int.Parse((string)row.Cells[3].Value);
            int cantidad_actual = int.Parse(txtCantidad.Text);
            
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
            newRow.Cells[0].Value = txtID_Venta.Text;
            newRow.Cells[2].Value = txtPieza.Text;
            newRow.Cells[3].Value = int.Parse(txtCantidad.Text);
            newRow.Cells[4].Value = float.Parse(precio.ToString());
            newRow.Cells[5].Value = float.Parse(subtotal.ToString());

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetDevVenta.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvDetDevVenta.CurrentCell.RowIndex;

                DataGridViewRow row = dgvDetDevVenta.Rows[rowIndex];
                int cant = int.Parse((string)row.Cells[3].Value);
                decimal precio = decimal.Parse((string)row.Cells[4].Value);
                dgvDetDevVenta.Rows.RemoveAt(rowIndex);
                txtPieza.Text = "";
                txtCantidad.Text = "";
                CalcularTotal();
                return;
            }
            {
                MessageBox.Show("Selecciona la hilera a eliminar");
                return;
            }
            
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

        private void btnDevolucion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDesc.Text))
            {
                MessageBox.Show(this, "Debes llenar todos los campos!", "Capos Faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int id_dev = int.Parse(txtID_Dev.Text);

            bD.EditarDevolucionDeCompra(id_dev, txtID_Dev.Text);
            bD.SaveChangesAsync();



            foreach (DataGridViewRow r in dgvDetDevVenta.Rows)
            {
                var pieza = r.Cells["Pieza"].FormattedValue.ToString();
                int cantidad = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                var cant_anterior = bD.DetalleDevCompra.Where(p => p.ID_DevolucionCompra == int.Parse(txtDesc.Text) && p.ID_Pieza == pieza).Select(p => p.Cantidad);
                if (int.Parse(cant_anterior.ToString()) != cantidad)
                {
                    bD.EditarDetalleDevVenta(int.Parse(txtID_Dev.Text), pieza, cantidad);
                    bD.SaveChangesAsync();
                }
            }
        }

        public void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow r in dgvDetDevVenta.Rows)
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
            var det_devcompra = bD.DetalleDevVenta.Where(p => p.ID_DevVenta == int.Parse(txtID_Dev.Text));

            var piezas_dev = bD.Pieza.Select(i => new { i.ID_Pieza, i.precio });

            foreach (var det in det_devcompra.ToList())
            {
                foreach (var precios in piezas_dev)
                {
                    if (precios.ID_Pieza == det.ID_Pieza)
                    {
                        dgvDetDevVenta.Rows.Add(txtID_Venta.Text, det.ID_Pieza, det.CantPieza, precios.precio, (det.CantPieza * precios.precio));
                    }
                }
            }
        }

        private void dgvDetDevVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvDetDevVenta.Rows.Count >= 0)
            {
                txtID_Venta.Enabled = false;
                DataGridViewRow row = dgvDetDevVenta.Rows[indexRow];

                txtPieza.Text = row.Cells[1].Value.ToString();
                txtCantidad.Text = row.Cells[2].Value.ToString();

                btnAgregar.Enabled = false;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                btnLimpiar.Enabled = true;
            }
            else
            {
                return;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, "¿Seguro que desea Cancelar la Accion?", "Cancelar Accion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                txtPieza.Text = "";
                txtCantidad.Text = "";
                btnAgregar.Enabled = true;
                btnEliminar.Enabled = false;
                btnEditar.Enabled = false;
                btnLimpiar.Enabled = false;
            }
            else
            {
                return;
            }
        }
    }
}
