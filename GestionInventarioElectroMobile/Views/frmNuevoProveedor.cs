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
    public partial class frmNuevoProveedor : Form
    {
        public frmNuevoProveedor()
        {
            InitializeComponent();
        }

        private bool isNumericValue(TextBox txt)
        {
            return txt.Text.All(char.IsDigit);
        }

        private void clear()
        {
            txtCodProv.Clear();
            txtTel.Clear();
            txtCor.Clear();
            txtDir.Clear();
            txtEmp.Clear();
            cmbTipoProv.SelectedIndex = 0;
            txtNom.Enabled = false;
            txtTel.Enabled = false;
            btnAgregarProveedor.Enabled = false;
        }
        private void frmNuevoProveedor_Load(object sender, EventArgs e)
        {

        }

        private void btnAgregarProveedor_Click(object sender, EventArgs e)
        {
        }



        public string validatingEmptyValues(TextBox text)
        {
            if (text.Text == "")
            {
                return null;
            }
            return text.Text;
        }

        private void btnAgregarProveedor_Click_1(object sender, EventArgs e)
        {
            if (txtCodProv.Text == "" || cmbTipoProv.SelectedIndex < 0)
            {
                MessageBox.Show("Inserte los valores requeridos");
                return;
            }

            if (isNumericValue(txtCor) || isNumericValue(txtDir) || isNumericValue(txtEmp))
            {
                MessageBox.Show("Inserte solo valores literales en los campos que los requieren", "Alerta");
                return;
            }

            if (!isNumericValue(txtTel))
            {
                MessageBox.Show("No se pueden insertar letras en el campo de telefono");
                return;
            }

            txtCor.Text = validatingEmptyValues(txtCor);
            txtDir.Text = validatingEmptyValues(txtDir);
            txtEmp.Text = validatingEmptyValues(txtEmp);
            txtTel.Text = validatingEmptyValues(txtTel);
            txtNom.Text = validatingEmptyValues(txtCor);
            txtAp.Text = validatingEmptyValues(txtAp);
            int valor;
            if (cmbTipoProv.SelectedIndex == 0)
            {
                valor = 2;
            }
            else
            {
                valor = 1;
            }

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.InsertarProveedor(txtCodProv.Text, txtNom.Text, txtAp.Text, txtEmp.Text, txtCor.Text, txtTel.Text, txtTel.Text, (byte?)valor);
                model.SaveChanges();
                MessageBox.Show("Proveedor insertado con exito");
            }

        }
    }
}
