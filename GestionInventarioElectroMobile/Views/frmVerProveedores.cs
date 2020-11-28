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
    public partial class frmVerProveedores : Form
    {
        public frmVerProveedores()
        {
            InitializeComponent();
        }

        private void frmVerProveedores_Load(object sender, EventArgs e)
        {
            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                dgvProveedores.DataSource = model.InformacionProveedores.ToList();
            }
            btnEditar.Enabled = false;
            btnEliminar.Enabled = false;
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            if (dgvProveedores.Rows.Count > 0)
            {
                /*  P.ID_Proveedor,
          Nombre,
          Apellido,
          Empresa,
          Correo,
          Telefono,
          Direccion,
          Tipo*/
                if ((txtNombre.Text != "" || txtApellido.Text != "") && cmbTipoProv.SelectedIndex == 0)
                {
                    MessageBox.Show("Los atributos de nombre y apellido solo estan disponibles si el proveedor es de tipo natural", "informacion");
                    return;
                }

                DataGridViewRow row = dgvProveedores.Rows[indexRow];
                txtCodProd.Text = row.Cells[0].Value.ToString();
                txtNombre.Text = row.Cells[1].Value.ToString();
                txtApellido.Text = row.Cells[2].Value.ToString();
                txtEmpresa.Text = row.Cells[3].Value.ToString();
                txtCor.Text = row.Cells[4].Value.ToString();
                txtTel.Text = row.Cells[5].Value.ToString();
                txtDir.Text = row.Cells[6].Value.ToString();
                if (row.Cells[7].Value.ToString().Equals("Natural"))
                {
                    cmbTipoProv.SelectedIndex = 1;
                }
                cmbTipoProv.SelectedIndex = 0;

                if (isNumericValue(txtNombre) || isNumericValue(txtApellido) || isNumericValue(txtEmpresa) || isNumericValue(txtCor) || isNumericValue(txtDir))
                {
                    MessageBox.Show("Inserte valores literales donde se le piden", "informacion");
                    return;
                }

                if (!isNumericValue(txtTel))
                {
                    MessageBox.Show("Inserte un numero de telefono adecuado", "informacion");
                    return;
                }

                txtNombre.Text = validatingEmptyValues(txtNombre);
                txtApellido.Text = validatingEmptyValues(txtApellido);
                txtEmpresa.Text = validatingEmptyValues(txtEmpresa);
                txtCor.Text = validatingEmptyValues(txtCor);
                txtDir.Text = validatingEmptyValues(txtDir);

                using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
                {
                    model.EditarProveedor(txtCodProd.Text, txtNombre.Text, txtApellido.Text, txtEmpresa.Text, txtCor.Text, txtTel.Text, txtDir.Text);
                    model.SaveChanges();
                    MessageBox.Show("El proveedor ha sido editado con exito");
                }
            }

        }


        private bool isNumericValue(TextBox txt)
        {
            return txt.Text.All(char.IsDigit);
        }

        public string validatingEmptyValues(TextBox text)
        {
            if (text.Text == "")
            {
                return null;
            }
            return text.Text;
        }
        private void dgvProveedores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtCodProd.Text == "")
            {
                MessageBox.Show("Seleccione la celda del proveedor a eliminar", "informacion");
                return;
            }

            string codProd = txtCodProd.Text;
            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.EliminarProveedor(codProd);
                model.SaveChanges();
                MessageBox.Show("El cliente ha sido eliminado", "informacion");
            }
        }
    }
}
