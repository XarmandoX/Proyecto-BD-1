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
    public partial class frmNuevaDevCompra : Form
    {
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        DataTable dt = new DataTable();
        int indexRow;


        public frmNuevaDevCompra()
        {
            InitializeComponent();
        }

        private void frmNuevaDevCompra_Load(object sender, EventArgs e)
        {
                
                //cargar datos de compra para que elija
                using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
                {
                    var searchValue = model.Compra.Select(p => p.ID_Compra);
                    AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    auto.AddRange(searchValue.ToArray());
                    this.txtID_Compra.AutoCompleteMode = AutoCompleteMode.Suggest;
                    this.txtID_Compra.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtID_Compra.AutoCompleteCustomSource = auto;
                }

                //una vez elija, que se rellenen con las piezas de esa compra unicamente
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
                btnDevolucion.Enabled = false;

                dt.Columns.Add("ID_DevolucionCompra", typeof(int));
                dt.Columns.Add("Pieza", typeof(string));
                dt.Columns.Add("cantidad", typeof(int));
                dt.Columns.Add("precioCompra", typeof(decimal));
                dt.Columns.Add("subtotal", typeof(decimal));
                dgvDetDevCompra.DataSource = dt;

                lblDate.Text = DateTime.Now.ToString("dd/M/yy");
           
            
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
            var id_dev = int.Parse(txtID_Dev.Text);
            var id_compra = txtID_Compra.Text;
            var pieza = bD.Pieza.
                Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();

            var cantidad = txtCantidad.Text;

            var pieza_ya_existente = bD.DetalleDevCompra.Where(p => p.ID_Pieza == pieza).Select(p => p.ID_Pieza).FirstOrDefault();

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

            lblTotal.Text = (float.Parse(lblTotal.Text) + subtotal).ToString();
            if (dgvDetDevCompra.Rows.Count == 1)  
            {
                txtID_Compra.Enabled = false;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                btnDevolucion.Enabled = true;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtCantidad.Text == "" || txtID_Compra.Text == ""  || txtDesc.Text == "" || txtPieza.Text == "")
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

            var pieza = bD.Pieza.Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();
            int cantidad = int.Parse(txtCantidad.Text);
            var cant_detallada = bD.Detalle_compra.Where(p => p.ID_Pieza == pieza && p.ID_Compra == txtID_Compra.Text).Select(p => p.cantidad).FirstOrDefault();
            var precio = bD.Pieza.Where(p => p.ID_Pieza == pieza).
                Select(p => p.precio).
                FirstOrDefault();
            var subtotal = cantidad * float.Parse(precio.ToString());

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

            DataGridViewRow row = dgvDetDevCompra.Rows[indexRow];

            int cant_anterior = int.Parse((string)row.Cells[3].Value);
            int cantidad_actual = int.Parse(txtCantidad.Text);
            if (cantidad_actual <= 0)
            {
                MessageBox.Show("la cantidad no puede ser menor o igual que 0", "Atencion");
                return;
            }
            if(cant_anterior > cantidad_actual)
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
            newRow.Cells[0].Value = int.Parse(txtID_Dev.Text);
            newRow.Cells[1].Value = txtPieza.Text;
            newRow.Cells[2].Value = int.Parse(txtCantidad.Text);
            newRow.Cells[3].Value = float.Parse(precio.ToString());
            newRow.Cells[4].Value = float.Parse(subtotal.ToString());
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
            else
            {
                MessageBox.Show("Selecciona la hilera a eliminar");
            }
            if (dgvDetDevCompra.Rows.Count == 0)
            {
                txtID_Compra.Enabled = true;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnAgregar.Enabled = true;
                btnDevolucion.Enabled = true;
            }
        }

        private void btnDevolucion_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDesc.Text) )
            {
                MessageBox.Show(this, "Debes llenar todos los campos!", "Capos Faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string idCompra = dgvDetDevCompra.Rows[0].Cells[0].FormattedValue.ToString();
            string pieza = "";
            
            bD.InsertarDevolucionCompra(idCompra, txtDesc.Text);
            bD.SaveChangesAsync();


            foreach (DataGridViewRow r in dgvDetDevCompra.Rows)
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

        private void dgvDetalleDevCompra_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvDetDevCompra.Rows.Count >= 0)
            {
                txtID_Compra.Enabled = false;
                DataGridViewRow row = dgvDetDevCompra.Rows[indexRow];

                txtPieza.Text = row.Cells[1].Value.ToString();
                txtCantidad.Text = row.Cells[2].Value.ToString();
                txtID_Compra.Text = row.Cells[0].Value.ToString();

                btnAgregar.Enabled = false;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(this, "¿Seguro que desea Cancelar la Accion?", "Cancelar Accion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                txtPieza.Text = "";
                txtCantidad.Text = "";
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
    }
}
