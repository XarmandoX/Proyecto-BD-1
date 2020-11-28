using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GestionInventarioElectroMobile.Model;

namespace GestionInventarioElectroMobile.Views
{
    public partial class frmVerCliente : Form
    {
        public frmVerCliente()
        {
            InitializeComponent();
        }

        private void frmVerVentas_Load(object sender, EventArgs e)
        {
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                dgvVerClientes.DataSource = model.ListarCliente().ToList();
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;

                var searchValue = model.Cliente.Select(c => c.PNombre_rep);
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                auto.AddRange(searchValue.ToArray());
                this.txtSearchClientes.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.txtSearchClientes.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtSearchClientes.AutoCompleteCustomSource = auto;
            }
            btnEditar.Enabled = false;
            txtRuc.Enabled = false;
        }


        private bool isNumericValue(TextBox txt)
        {
            return txt.Text.All(char.IsDigit);
        }

        void Clear()
        {
            txtRuc.Clear();
            txtPNomC.Clear();
            txtSNomC.Clear();
            txtPApC.Clear();
            txtSApC.Clear();
            txtCorreo.Clear();
            txtDir.Clear();
            txtEmp.Clear();
            txtTipoSer.Clear();
            txtTipoEmp.Clear();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (txtRuc.Text == "" || txtPNomC.Text == "" || txtPApC.Text == "" || txtEmp.Text == "")
            {
                MessageBox.Show("Inserte la informacion pedida", "Atencion");
                return;
            }

            if (!(txtTelC.Text.StartsWith("2") || txtTelC.Text.StartsWith("5") || txtTelC.Text.StartsWith("7") || txtTelC.Text.StartsWith("8")) || txtTelC.Text.Trim().Length > 8 || !isNumericValue(txtTelC))
            {
                MessageBox.Show("Inserte un numero de telefono adecuado");
                return;
            }


            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.EditarCliente(txtRuc.Text, txtPNomC.Text, txtSNomC.Text, txtPApC.Text, txtSApC.Text, txtCorreo.Text, txtTelC.Text, txtTipoEmp.Text, txtTipoSer.Text, txtDir.Text, txtEmp.Text);
                model.SaveChanges();

                dgvVerClientes.DataSource = model.ListarCliente().ToList();
            }

            MessageBox.Show("Cliente editado con exito!!");
            Clear();
        }

        private void btnBuscar_Click_1(object sender, EventArgs e)
        {
            //Yesin tqm
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {

            if (txtRuc.Text == "")
            {
                MessageBox.Show("Seleccione la celda del cliente a eliminar", "informacion");
            }

            var codCliente = txtRuc.Text;
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.EliminarCliente(codCliente);
                model.SaveChanges();
            }
            MessageBox.Show("Cliente borrado con exito");
        }

        private void dgvVerClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvVerClientes.Rows.Count > 0)
            {
                DataGridViewRow row = dgvVerClientes.Rows[indexRow];

                txtRuc.Text = row.Cells[0].Value.ToString();
                txtPNomC.Text = row.Cells[1].Value.ToString();
                txtSNomC.Text = row.Cells[2].Value.ToString();
                txtPApC.Text = row.Cells[3].Value.ToString();
                txtSNomC.Text = row.Cells[4].Value.ToString();
                txtCorreo.Text = row.Cells[5].Value.ToString();
                txtTelC.Text = row.Cells[6].Value.ToString();
                txtEmp.Text = row.Cells[7].Value.ToString();
                txtTipoEmp.Text = row.Cells[8].Value.ToString();
                txtTipoSer.Text = row.Cells[9].Value.ToString();
                txtDir.Text = row.Cells[10].Value.ToString();

                btnEditar.Enabled = true;
                btnEliminar.Enabled = true;

            }
            else
                return;

        }
    }
}
