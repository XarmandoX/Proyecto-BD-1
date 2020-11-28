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
    public partial class frmNuevaCompra : Form
    {
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        DataTable dt = new DataTable();
        int indexRow;

        DataRow drCompras;

        public DataRow DrCompras

        {
            set
            {
                drCompras = value;
                txtID_Compra.Text = drCompras["ID_Compra"].ToString();
                var nombreprov = bD.Proveedor.Where(p => p.Nombre == drCompras["Id_Proveedor"].ToString()).Select(p  => p.Nombre).FirstOrDefault();
                txtProveedor.Text = nombreprov;
                lblFecha.Text = drCompras["Fecha"].ToString();
                lblFecha.Text = drCompras["Total_Compra"].ToString();
            }

        }
        public frmNuevaCompra()
        {
            InitializeComponent();
        }

        private void frmNuevaCompra_Load(object sender, EventArgs e)
        {
            if (drCompras != null)
            {
                btnEditar.Enabled = true;
                btnEliminar.Enabled = false;
                btnCompra.Enabled = false;
                btnLimpiar.Enabled = false;

                txtID_Compra.Enabled = false;
                txtProveedor.Enabled = false;

                dt.Columns.Add("ID_Compra", typeof(string));
                dt.Columns.Add("ID_Pieza", typeof(string));
                dt.Columns.Add("precio", typeof(decimal));
                dt.Columns.Add("cantidad", typeof(int));
                dt.Columns.Add("Subtotal", typeof(decimal));
                dgvDetCompras.DataSource = dt;
                lblAccion.Text = "Editar Compra";
                CargarDetDevCompra();
                CalcularTotal();
            }
            else
            {
                using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
                {
                    var searchProv = model.Proveedor.Select(p => p.Nombre);
                    AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    auto.AddRange(searchProv.ToArray());
                    this.txtProveedor.AutoCompleteMode = AutoCompleteMode.Suggest;
                    this.txtProveedor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtProveedor.AutoCompleteCustomSource = auto;
                }
                lblFecha.Text = DateTime.Now.ToString("dd/M/yy");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtID_Compra.Text) || string.IsNullOrWhiteSpace(txtPieza.Text) || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
            {
                MessageBox.Show(this, "Debes llenar todos los campos!", "Capos Faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }


            foreach (DataGridViewRow item in dgvDetCompras.Rows)
            {
                if (item.Cells["ID_Pieza"].ToString() == txtPieza.Text)
                {
                    MessageBox.Show("Pieza ya esta detallado, si desea puede editar");
                    return;
                }
            }

            int cantidad = int.Parse(txtCantidad.Text);
            decimal precioCompra = decimal.Parse(txtPrecioCompra.Text);
            decimal precioVenta = bD.Pieza.Where(p => p.ID_Pieza == txtPieza.ToString()).Select(p => p.precio).FirstOrDefault();
            if (precioCompra <= 0)
            {
                MessageBox.Show(this, "El precio no puede ser menor o igual a 0", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (precioCompra > precioVenta)
            {
                MessageBox.Show(this, "El precio de compra debe ser menor al precio de venta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cantidad <= 0)
            {
                MessageBox.Show("la cantidad no puede ser menor o igual a 0");
                return;
            }
            var subtotal = cantidad * precioCompra;
            dt.Rows.Add(id_compra, txtPieza.Text, precioCompra, cantidad, subtotal);
            if (dgvDetCompras.Rows.Count == 1)
            {
                txtID_Compra.Enabled = false;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                btnCompra.Enabled = true;
            }
            CalcularTotal();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPieza.Text) || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecioCompra.Text))
            {
                MessageBox.Show(this, "Debes llenar todos los campos!", "Capos Faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!txtCantidad.Text.All(char.IsDigit))
            {
                MessageBox.Show("Inserte valores numericos en cantidad");
                txtCantidad.Clear();
                return;
            }
            var id_compra = txtID_Compra.Text;
            var pieza = bD.Pieza.Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();
            var cantidad = int.Parse(txtCantidad.Text);
            if(cantidad <= 0)
            {
                MessageBox.Show("la cantidad no puede ser menor o igual a 0");
                return;
            }
            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }
            decimal precioCompra = decimal.Parse(txtPrecioCompra.Text);
            decimal precioVenta = bD.Pieza.Where(p => p.ID_Pieza == txtPieza.ToString()).Select(p => p.precio).FirstOrDefault();
            if (precioCompra <= 0)
            {
                MessageBox.Show(this, "El precio no puede ser menor o igual a 0", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (precioCompra > precioVenta)
            {
                MessageBox.Show(this, "El precio de compra debe ser menor al precio de venta", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var subtotal = cantidad * precioCompra;

            DataGridViewRow row = dgvDetCompras.Rows[indexRow];

            dt.Rows.Add(id_compra, pieza, precioCompra, cantidad, subtotal);
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.Cells[0].Value = id_compra;
            newRow.Cells[1].Value = pieza;
            newRow.Cells[2].Value = cantidad;
            newRow.Cells[3].Value = precioCompra;
            newRow.Cells[4].Value = subtotal;

            CalcularTotal();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetCompras.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvDetCompras.CurrentCell.RowIndex;

                DataGridViewRow row = dgvDetCompras.Rows[rowIndex];
                int cant = int.Parse((string)row.Cells[3].Value);
                decimal precio = decimal.Parse((string)row.Cells[4].Value);
                CalcularTotal();
                dgvDetCompras.Rows.RemoveAt(rowIndex);
                txtPieza.Text = "";
                txtCantidad.Text = "";
                txtPrecioCompra.Text = "";
                return;
            }
            else
            {
                MessageBox.Show("Selecciona la hilera a eliminar");
            }
            CalcularTotal();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Realmente desea Limpiar los datos?", "Mensaje del Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                txtPieza.Text = "";
                txtCantidad.Text = "";
                txtPrecioCompra.Text = "";
                btnAgregar.Enabled = true;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnLimpiar.Enabled = false;
            }
            else
            {
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

       

        private void dgvDetCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvDetCompras.Rows.Count >= 0)
            {
                DataGridViewRow row = dgvDetCompras.Rows[indexRow];

                txtPieza.Text = row.Cells[1].Value.ToString();
                txtCantidad.Text = row.Cells[2].Value.ToString();
                txtPrecioCompra.Text = row.Cells[3].Value.ToString();


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

        public void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow r in dgvDetCompras.Rows)
            {
                int cant;
                decimal precio;
                cant = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                precio = decimal.Parse(r.Cells["precio"].FormattedValue.ToString());
                decimal subtotal = cant * precio;
                total += subtotal;

            }
            lblTotal.Text = total.ToString();
        }

        public void CargarDetDevCompra()
        {
            var det_compras = bD.Detalle_compra.Where(p => p.ID_Compra == txtID_Compra.Text);

            foreach (var det in det_compras.ToList())
            {
                dgvDetCompras.Rows.Add(txtID_Compra.Text, det.ID_Pieza, det.precio, det.cantidad, det.subtotal);
            }
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            if (drCompras != null)
            {
                foreach (DataGridViewRow r in dgvDetCompras.Rows)
                {
                    var pieza = r.Cells["ID_Pieza"].FormattedValue.ToString();
                    int cantidad = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                    var cant_anterior = bD.Detalle_compra.Where(p => p.ID_Compra == txtID_Compra.Text && p.ID_Pieza == pieza).Select(p => p.cantidad);

                    decimal precio = decimal.Parse(r.Cells["precio"].FormattedValue.ToString());
                    var precio_anterior = bD.Detalle_compra.Where(p => p.ID_Compra == txtID_Compra.Text && p.ID_Pieza == pieza).Select(p => p.precio);
                    if (int.Parse(cant_anterior.ToString()) != cantidad || precio != decimal.Parse(precio_anterior.ToString()))
                    {
                        bD.EditarDetalleCompra(txtID_Compra.Text, pieza, precio, cantidad);
                        bD.SaveChangesAsync();
                    }
                }
            }
            else
            {
                if (dgvDetCompras.Rows.Count == 0)
                {
                    MessageBox.Show(this, "Porfavor Inserte al menos 1 detalle en la reparacion", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string idCompra = dgvDetCompras.Rows[0].Cells[0].FormattedValue.ToString();
                var idprov = bD.Proveedor.Where(p => p.Nombre == txtProveedor.Text).Select(p => p.ID_Proveedor);
                string pieza = "";
                bD.CrearCompra(idCompra, idprov.ToString(), null);

                foreach (DataGridViewRow r in dgvDetCompras.Rows)
                {
                    pieza = r.Cells["ID_Pieza"].FormattedValue.ToString();
                    var cantidad = int.Parse(r.Cells["cantidad"].FormattedValue.ToString());
                    decimal precio = decimal.Parse(r.Cells["precio"].FormattedValue.ToString());

                    bD.AgregarDetalleCompra(idCompra, pieza, precio, cantidad);
                    bD.SaveChangesAsync();
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
