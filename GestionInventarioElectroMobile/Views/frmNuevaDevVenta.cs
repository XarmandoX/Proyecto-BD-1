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
    public partial class frmNuevaDevVenta : Form
    {
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        DataTable dt = new DataTable();
        int indexRow;

        Boolean tiene_cliente;

        public Boolean Tiene_Cliente
        {
            set
            {
                tiene_cliente = value;
            }
        }
        public frmNuevaDevVenta()
        {
            InitializeComponent();
        }

        private void frmNuevaDevVenta_Load(object sender, EventArgs e)
        {
            //cargar datos de compra para que elija
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var searchValue = model.Compra.Select(p => p.ID_Compra);
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                auto.AddRange(searchValue.ToArray());
                this.txtID_Venta.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.txtID_Venta.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtID_Venta.AutoCompleteCustomSource = auto;
            }

            //una vez elija, que se rellenen con las piezas de esa compra unicamente
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
            var ultima_devVenta = bD.DevolucionVenta.Max(v => v.ID_DevVenta).ToString();
            txtID_Dev.Text = ultima_devVenta + 1;
            
            dgvDetVenta.DataSource = dt;
            btnDevolucion.Enabled = false;

            lblDate.Text = DateTime.Now.ToString("dd/M/yy");

            if (tiene_cliente)
            {
                string id_cliente = bD.Venta.Where(p => p.ID_Venta == int.Parse(txtID_Venta.Text)).Select(p => p.ID_Cliente).FirstOrDefault();
                var nombre_cliente = bD.Cliente.Where(p => p.RUC == id_cliente).Select(p => p.PNombre_rep).FirstOrDefault();
                txtCliente.Text = nombre_cliente;
            }
            else
            {
                lblCliente.Visible = false;
                txtCliente.Visible = true;
            }
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

            var id_venta = int.Parse(txtID_Venta.Text);
            var id_dev = int.Parse(txtDesc.Text);
            var pieza = bD.Pieza.
                Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();

            var cantidad = int.Parse(txtCantidad.Text);
            var cant_detallada = bD.Detalle_venta.Where(p => p.ID_Pieza == pieza && p.ID_Venta == int.Parse(txtID_Venta.Text)).Select(p => p.cantidad).FirstOrDefault();
            var pieza_ya_existente = bD.DetalleDevVenta.Where(p => p.ID_Pieza == pieza).Select(p => p.ID_Pieza).FirstOrDefault();
            var precio = bD.Pieza.
                Select(p => p.precio).
                FirstOrDefault();

            var subtotal = cantidad * float.Parse(precio.ToString());

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
           
            if (cantidad > cant_detallada)
            {
                MessageBox.Show("Cantidad de la pieza excede la que se encuentra en la venta ");
                return;
            }

            dt.Rows.Add(id_dev, pieza, cantidad, precio, subtotal);
            
            if (dgvDetVenta.Rows.Count == 1)
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
                MessageBox.Show("Inserte la informacion pedida, recuerde seleccionar el detalle que desea modificar", "Atencion");
                return;
            }
            var id_dev = int.Parse(txtID_Dev.Text);
            var id_venta = int.Parse(txtID_Venta.Text);

            var pieza = bD.Pieza.Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();
            var cantidad = int.Parse(txtCantidad.Text);
            var cant_detallada = bD.Detalle_venta.Where(p => p.ID_Pieza == pieza && p.ID_Venta == int.Parse(txtID_Venta.Text)).Select(p => p.cantidad).FirstOrDefault();
            var precio = bD.Pieza.Where(p => p.ID_Pieza == pieza).
                Select(p => p.precio).
                FirstOrDefault();
            var subtotal = cantidad * float.Parse(precio.ToString());

            DataGridViewRow row = dgvDetVenta.Rows[indexRow];
            int cant_anterior = int.Parse((string)row.Cells[3].Value);
            int cantidad_actual = int.Parse(txtCantidad.Text);
            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }
            
            if (cantidad > cant_detallada)
            {
                MessageBox.Show("Cantidad de la pieza excede la que se encuentra en la venta ");
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

            dt.Rows.Add(id_venta, pieza, cantidad, precio, subtotal);
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.Cells[0].Value = int.Parse(txtID_Dev.Text);
            newRow.Cells[1].Value = txtPieza.Text;
            newRow.Cells[2].Value = int.Parse(txtCantidad.Text);
            newRow.Cells[3].Value = decimal.Parse(precio.ToString());
            newRow.Cells[4].Value = decimal.Parse(subtotal.ToString());
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetVenta.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvDetVenta.CurrentCell.RowIndex;

                DataGridViewRow row = dgvDetVenta.Rows[rowIndex];
                int cant = int.Parse((string)row.Cells[3].Value);
                decimal precio = decimal.Parse((string)row.Cells[4].Value);
                dgvDetVenta.Rows.RemoveAt(rowIndex);
                txtPieza.Text = "";
                txtCantidad.Text = "";
                CalcularTotal();
                return;
            }
            else
            {
                MessageBox.Show("Selecciona la hilera a eliminar");
            }
            if (dgvDetVenta.Rows.Count <= 0)
            {
                txtID_Venta.Enabled = true;
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
            if (string.IsNullOrWhiteSpace(txtDesc.Text) || string.IsNullOrWhiteSpace(txtID_Venta.Text))
            {
                MessageBox.Show(this, "Debes llenar todos los campos!", "Capos Faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string idCompra = dgvDetVenta.Rows[0].Cells[0].FormattedValue.ToString();
            string pieza = "";

            bD.InsertarDevolucionCompra(idCompra, txtDesc.Text);
            bD.SaveChangesAsync();


            foreach (DataGridViewRow r in dgvDetVenta.Rows)
            {
                pieza = r.Cells["Pieza"].FormattedValue.ToString();
                var cantidad = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                var id_devcompra = bD.DevolucionCompra.Where(p => p.ID_Compra.Contains(idCompra)).
                    Select(p => p.ID_DevolucionCompras).
                    FirstOrDefault();
                bD.InsertarDetalleDevCompra(id_devcompra, pieza, cantidad);
                bD.SaveChangesAsync();
            }
        }

        private void dgvDetVenta_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvDetVenta.Rows.Count >= 0)
            {
                txtID_Venta.Enabled = false;
                DataGridViewRow row = dgvDetVenta.Rows[indexRow];

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
            if (dgvDetVenta.Rows.Count <= 0)
            {
                txtID_Venta.Enabled = true;
            }
        }

        public void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow r in dgvDetVenta.Rows)
            {
                int cant;
                decimal precio;
                cant = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                precio = decimal.Parse(r.Cells["precioVenta"].FormattedValue.ToString());
                decimal subtotal = cant * precio;
                total += subtotal;

            }
            lblTotal.Text = total.ToString();
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
                        dgvDetVenta.Rows.Add(txtID_Venta.Text, det.ID_Pieza, det.CantPieza, precios.precio, (det.CantPieza * precios.precio));
                    }
                }
            }
        }
    }
}
