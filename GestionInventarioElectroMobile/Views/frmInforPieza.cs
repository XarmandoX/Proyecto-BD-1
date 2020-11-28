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
    public partial class frmInForPieza : Form
    {
        string rutaPrincipal;
        public frmInForPieza()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmInForPieza_Load(object sender, EventArgs e)
        {
            
            cmbEstado.SelectedIndex = 0;
            cmbTipoPieza.SelectedIndex = 0;

            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var searchProv = model.Proveedor.Select(p => p.Empresa);
                AutoCompleteStringCollection auto = new AutoCompleteStringCollection();
                auto.AddRange(searchProv.ToArray());
                this.txtNomProv.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.txtNomProv.AutoCompleteSource = AutoCompleteSource.CustomSource;
                this.txtNomProv.AutoCompleteCustomSource = auto;
            }
        }

        private void btnAgregarImgPrin_Click(object sender, EventArgs e)
        {
            pbImgPrin.Image = Image.FromFile(GetImagePath());
        }

        private string GetImagePath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = "C:\\";
            fileDialog.Filter = "Image Files (*.jpg)|*.jpg|All Files(*.*)|*.*";
            fileDialog.FilterIndex = 1;

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (fileDialog.CheckFileExists)
                {
                    string paths = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
                    string imgName = System.IO.Path.GetFileName(fileDialog.FileName);
                    System.IO.File.Copy(fileDialog.FileName, paths + "\\Resources\\" + imgName);
                    MessageBox.Show("La imagen se ha guardado exitosamente", "informacion");
                    return "\\Resources\\" + imgName;
                }
                return "";
            }

            return "";
        }

        private void btnAgregarImgCor_Click(object sender, EventArgs e)
        {
            pbImgCor.Image = Image.FromFile(GetImagePath());
        }

        private void btnAgregarImgCir_Click(object sender, EventArgs e)
        {
            labelr.Image = Image.FromFile(GetImagePath());
        }

        private void btnGuardarPiezas_Click(object sender, EventArgs e)
        {
            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var codPro = model.Pieza.
                    Where(p => p.ID_Pieza == txtIdPi.Text).
                    Select(p => p.ID_Pieza).
                    FirstOrDefault();

                if (codPro == txtIdPi.Text)
                {
                    MessageBox.Show("El producto ya esta resgistrado", "Alerta");
                    return;
                }

                var idProv = model.Proveedor.
                            Where(p => p.Empresa == txtNomProv.Text).
                            Select(p => p.ID_Proveedor).
                            FirstOrDefault();

                if (idProv == null)
                {
                    MessageBox.Show("Proveedor no registrado", "Alerta");
                    return;
                }

                int estado;
                if (cmbEstado.SelectedIndex == 0)
                {
                    estado = 3;
                }
                else if (cmbEstado.SelectedIndex == 1)
                {
                    estado = 2;
                }
                else
                {
                    estado = 1;
                }

                int idTipoPieza;

               

                int tension = int.Parse(txtTension.Text);

                if (txtPrecio.Text == "")
                {
                    MessageBox.Show("Inserte el precio de venta", "informacion");
                    return;
                }
                decimal precio = decimal.Parse(txtPrecio.Text);

                string circuito = txtCir.Text;

                
                string estante = txtEstante.Text;

                int dientes = int.Parse(txtDir.Text);

                int recuentoTerminales = int.Parse(txtTerminales.Text);

                string volt = txtVolatje.Text;



                string fab = txtFabricante.Text;

                for (int i = 0; i < 8; i++)
                {
                    if (cmbTipoPieza.SelectedIndex == i)
                    {
                        
                        model.InsertarPieza(codPro,
                                            idProv,
                                            (byte?)estado,
                                            (byte?)(i + 1),
                                            pbImgPrin.ImageLocation,
                                            pbImgCir.ImageLocation,
                                            pbImgCor.ImageLocation,
                                            estante,
                                            (byte?)tension,
                                            precio,
                                            circuito,
                                            (byte?)dientes,
                                            (byte?)recuentoTerminales,
                                            volt,
                                            fab);

                    }
                    break;
                }

                MessageBox.Show("Pieza insertada exitosamente");
                model.SaveChanges();
                this.Close();
            }
        }

    }
}
