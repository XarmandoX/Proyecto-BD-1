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
    public partial class frmNuevoCliente : Form
    {
        public frmNuevoCliente()
        {
            InitializeComponent();
        }

        private void btnAgregarCliente_Click(object sender, EventArgs e)
        {
            if (txtRuc.Text == "" || txtPNomC.Text == "" || txtPApC.Text == "" || txtEmpr.Text == "")
            {
                MessageBox.Show("Es necesario llenar las casillas con valores esenciales (Ruc, primer nombre representante, primer apellido representante, empresa)");
                return;
            }


            if (!(isNumericValue(txtRuc) || isNumericValue(txtPNomC) || isNumericValue(txtSNomC) || isNumericValue(txtPApC) ||
                isNumericValue(txtSApC) || isNumericValue(txtCor)))

            {
                MessageBox.Show("No inserte valores solo numericos en los campos que requieren de valores literales", "alerta");
            }

            string sNom, sAp, Corr, Tel, TipoEmp, TipoSer, Dir;

            sNom = validatingEmptyValues(txtSNomC);
            sAp = validatingEmptyValues(txtSApC);
            Corr = validatingEmptyValues(txtCor);
            Tel = validatingEmptyValues(txtTel);
            TipoEmp = validatingEmptyValues(txtTipoEmp);
            TipoSer = validatingEmptyValues(txtTipoSer);
            Dir = validatingEmptyValues(txtDir);

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                model.InsertarCliente(txtRuc.Text, txtPNomC.Text, txtSNomC.Text, txtPApC.Text, txtSApC.Text, txtCor.Text,
                    txtTel.Text, txtTipoEmp.Text, txtTipoSer.Text, txtDir.Text, txtEmpr.Text);
                model.SaveChanges();
                clear();
            }
            MessageBox.Show("Cliente registrado con exito", "informacion");
        }


        private bool isNumericValue(TextBox txt)
        {
            return txt.Text.All(char.IsDigit);
        }

        private void clear()
        {
            txtRuc.Clear();
            txtPNomC.Clear();
            txtSNomC.Clear();
            txtPApC.Clear();
            txtSApC.Clear();
            txtCor.Clear();
            txtDir.Clear();
            txtTipoEmp.Clear();
            txtTipoSer.Clear();
            txtTel.Clear();
        }

       
	    public string validatingEmptyValues(TextBox text)
        {
            if (text.Text == "")
            {
                return null;
            }
            return text.Text;
        }

        private void frmNuevoCliente_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
