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
    public partial class frmNuevaReparacion : Form
    {
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        DataTable dt = new DataTable();
        int indexRow;

        DataRow drReparacion;

        public DataRow DrReparacion

        {
            set
            {
                drReparacion = value;
                txtID_Reparacion.Text = drReparacion["ID_Reparacion"].ToString();
                txtCliente.Text = drReparacion["ID_Cliente"].ToString();
                dtpFechaEntrega.Text = drReparacion["FechaFinal"].ToString();
                lblFecha.Text = drReparacion["FechaIni"].ToString();
                lblTotal.Text = drReparacion["Total"].ToString();
            }

        }

        public frmNuevaReparacion()
        {
            InitializeComponent();
        }

        private void frmNuevaReparacion_Load(object sender, EventArgs e)
        {
            if (drReparacion != null)
            {
                btnEditar.Enabled = true;
                btnEliminar.Enabled = false;
                btnReparacion.Enabled = false;

                txtID_Reparacion.Enabled = false;
                txtCliente.Enabled = false;
                radTieneCliente.Enabled = false;

                dt.Columns.Add("ID_Reparacion", typeof(int));
                dt.Columns.Add("Pieza", typeof(string));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("Precio", typeof(decimal));
                dt.Columns.Add("Subtotal", typeof(float));
                dgvDetReparacion.DataSource = dt;
                lblAccion.Text = "Editar Reparacion";
                CargarDetReparaciones();
                CalcularTotal();
            }
            else
            {
                using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
                {
                    var searchValue = model.Pieza.Select(p => p.ID_Pieza);
                    AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    auto.AddRange(searchValue.ToArray());
                    this.txtPieza.AutoCompleteMode = AutoCompleteMode.Suggest;
                    this.txtPieza.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtPieza.AutoCompleteCustomSource = auto;
                }

                using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
                {
                    var searchValue = model.Cliente.Select(p => p.PNombre_rep);
                    AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                    auto.AddRange(searchValue.ToArray());
                    this.txtPieza.AutoCompleteMode = AutoCompleteMode.Suggest;
                    this.txtPieza.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.txtPieza.AutoCompleteCustomSource = auto;
                }
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnReparacion.Enabled = false;
                dt.Columns.Add("ID_Reparacion", typeof(int));
                dt.Columns.Add("Pieza", typeof(string));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("Precio", typeof(decimal));
                dt.Columns.Add("Subtotal", typeof(float));
                var ultima_devVenta = bD.Reparacion.Max(v => v.ID_Reparacion).ToString();
                txtID_Reparacion.Text = ultima_devVenta + 1;
                dgvDetReparacion.DataSource = dt;
                lblFecha.Text = DateTime.Now.ToString("dd/M/yy");
            }
        }

        private void btn_Agregar_Click(object sender, EventArgs e)
        {
            if (txtCant.Text == "" || txtPieza.Text == "")
            {
                MessageBox.Show("Inserte la informacion pedida", "Atencion");
                return;
            }
            if (!txtCant.Text.All(char.IsDigit))
            {
                MessageBox.Show("Inserte valores numericos en cantidad");
                txtCant.Clear();
                return;
            }
            var id_reparacion = txtID_Reparacion.Text;
            var pieza = bD.Pieza.
                Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();
            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }
            int cantidad = int.Parse(txtCant.Text);
            decimal precio = decimal.Parse(txtPrecio.Text);
            if (precio <= 0)
            {
                MessageBox.Show(this, "El precio no puede ser menor o igual a 0", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var subtotal = cantidad * precio;
            dt.Rows.Add(id_reparacion, pieza, cantidad, precio, subtotal);
            if (dgvDetReparacion.Rows.Count == 1)
            {
                txtCliente.Enabled = false;
                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;
                btnReparacion.Enabled = true;
            }
            CalcularTotal();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPieza.Text) || string.IsNullOrWhiteSpace(txtCant.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
            {
                MessageBox.Show(this, "Debes llenar todos los campos!", "Capos Faltantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var id_reparacion = txtID_Reparacion.Text;

            var pieza = bD.Pieza.Where(p => p.ID_Pieza.Contains(txtPieza.Text)).
                Select(p => p.ID_Pieza.ToString()).
                FirstOrDefault();

            if (pieza != txtPieza.Text)
            {
                MessageBox.Show("Producto no registrado");
                return;
            }
            var cantidad = txtCant.Text;
            var precio = decimal.Parse(txtPrecio.Text);
            if(precio <= 0)
            {
                MessageBox.Show(this, "El precio no puede ser menor o igual a 0", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var subtotal = int.Parse(cantidad) * float.Parse(precio.ToString());

            DataGridViewRow row = dgvDetReparacion.Rows[indexRow];

            int cant_anterior = int.Parse((string)row.Cells[3].Value);
            int cantidad_actual = int.Parse(txtCant.Text);

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

            dt.Rows.Add(id_reparacion, pieza, cantidad, precio, subtotal);
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.Cells[0].Value = txtID_Reparacion.Text;
            newRow.Cells[1].Value = txtPieza.Text;
            newRow.Cells[2].Value = int.Parse(txtCant.Text);
            newRow.Cells[3].Value = float.Parse(precio.ToString());
            newRow.Cells[4].Value = float.Parse(subtotal.ToString());

            CalcularTotal();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDetReparacion.CurrentCell.RowIndex >= 0)
            {
                int rowIndex = dgvDetReparacion.CurrentCell.RowIndex;

                DataGridViewRow row = dgvDetReparacion.Rows[rowIndex];
                int cant = int.Parse((string)row.Cells[3].Value);
                decimal precio = decimal.Parse((string)row.Cells[4].Value);
                CalcularTotal();
                dgvDetReparacion.Rows.RemoveAt(rowIndex);
                txtPieza.Text = "";
                txtCant.Text = "";
                return;
            }
            else
            {
                MessageBox.Show("Selecciona la hilera a eliminar");
            }


            CalcularTotal();
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

        private void btnReparacion_Click(object sender, EventArgs e)
        {
            if (drReparacion != null)
            {
                int id_rep = int.Parse(txtID_Reparacion.Text);
                if (radTieneCliente.Checked)
                {
                    bD.EditarReparacion(id_rep, txtCliente.Text, dtpFechaEntrega.Value);
                    bD.SaveChangesAsync();
                }
                else
                {
                    bD.EditarReparacion(id_rep, null, dtpFechaEntrega.Value);
                    bD.SaveChangesAsync();
                }
                foreach (DataGridViewRow r in dgvDetReparacion.Rows)
                {
                    var pieza = r.Cells["Pieza"].FormattedValue.ToString();
                    int cantidad = int.Parse(r.Cells["Cantidad"].FormattedValue.ToString());
                    var cant_anterior = bD.DetalleDevCompra.Where(p => p.ID_DevolucionCompra == int.Parse(txtCant.Text) && p.ID_Pieza == pieza).Select(p => p.Cantidad);
                    decimal precio = decimal.Parse(r.Cells["Precio"].FormattedValue.ToString());
                    if (int.Parse(cant_anterior.ToString()) != cantidad)
                    {
                        bD.EditarDetalleReparacion(id_rep, pieza, precio, cantidad);
                        bD.SaveChangesAsync();
                    }
                }
            }
            else
            {
                if (dgvDetReparacion.Rows.Count == 0)
                {
                    MessageBox.Show(this, "Porfavor Inserte al menos 1 detalle en la reparacion", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string idCompra = dgvDetReparacion.Rows[0].Cells[0].FormattedValue.ToString();
                string pieza = "";
                if (radTieneCliente.Checked)
                {
                    bD.InsertarReparacion(txtCliente.Text, dtpFechaEntrega.Value);
                    bD.SaveChangesAsync();
                }
                else
                {
                    bD.InsertarReparacion(null, dtpFechaEntrega.Value);
                    bD.SaveChangesAsync();
                }

                foreach (DataGridViewRow r in dgvDetReparacion.Rows)
                {
                    pieza = r.Cells["Pieza"].FormattedValue.ToString();
                    var cantidad = int.Parse(r.Cells["Cantidad"].FormattedValue.ToString());
                    decimal precio = decimal.Parse(r.Cells["Precio"].FormattedValue.ToString());

                    bD.InsertarDetalleReparacion(int.Parse(txtID_Reparacion.Text), pieza, precio, cantidad);
                    bD.SaveChangesAsync();
                }
            }
        }

        private void btnNuevoProd_Click(object sender, EventArgs e)
        {
            //llamar frm de nueva pieza
        }

        

        private void radTieneCliente_CheckedChanged(object sender, EventArgs e)
        {
            if (radTieneCliente.Checked == true)
            {
                lblCliente.Enabled = true;
                txtCliente.Enabled = true;
            }
            else
            {
                lblCliente.Enabled = false;
                txtCliente.Enabled = false;
            }
        }

        private void dgvDetReparacion_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvDetReparacion.Rows.Count >= 0)
            {
                DataGridViewRow row = dgvDetReparacion.Rows[indexRow];

                txtPieza.Text = row.Cells[1].Value.ToString();
                txtCant.Text = row.Cells[2].Value.ToString();
                txtPrecio.Text = row.Cells[3].Value.ToString();


                btn_Agregar.Enabled = false;
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
            DialogResult result = MessageBox.Show(this, "Realmente desea Limpiar los datos?", "Mensaje del Sistema", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                txtPieza.Text = "";
                txtCant.Text = "";
                txtPrecio.Text = "";
                btn_Agregar.Enabled = true;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
                btnLimpiar.Enabled = false;
            }
            else
            {
                return;
            }
        }


        public void CalcularTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow r in dgvDetReparacion.Rows)
            {
                int cant;
                decimal precio;
                cant = int.Parse(r.Cells["Cantidad"].FormattedValue.ToString());
                precio = decimal.Parse(r.Cells["Precio"].FormattedValue.ToString());
                decimal subtotal = cant * precio;
                total += subtotal;

            }
            lblTotal.Text = total.ToString();
        }

        public void CargarDetReparaciones()
        {
            var det_reparacion = bD.DetalleReparacion.Where(p => p.ID_Reparacion == int.Parse(txtID_Reparacion.Text));

            foreach (var det in det_reparacion.ToList())
            {
                dgvDetReparacion.Rows.Add(int.Parse(txtID_Reparacion.Text), det.ID_Pieza, det.Cantidad, det.Precio, (det.Cantidad * det.Precio));
            }
        }
    }
}
