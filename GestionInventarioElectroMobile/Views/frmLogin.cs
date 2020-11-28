using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GestionInventarioElectroMobile.Model;
using GestionInventarioElectroMobile.Views;

namespace GestionInventarioElectroMobile
{
    public partial class frmLogin : Form
    {

        frmMain frm = new frmMain();
        int contador = 3;
        ELECTROMOBILEModel bD;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (txtPass.Text.Equals("") || txtUser.Text.Equals(""))
            {
                MessageBox.Show("No pueden haber campos vacios", "Alerta");
                Cursor.Current = Cursors.Default;
                return;
            }

            try
            {
                bD = new ELECTROMOBILEModel("Server=CHINO-PC;Database=ELECTROMOBILEDB;UID=" + txtUser.Text + ";PWD=" + txtPass.Text);
                bD.Database.Connection.Open();

            }
            catch (Exception ex)
            {

                Console.WriteLine("Error de registro"); ;
            }


            if (bD.Database.Connection.State == ConnectionState.Open)
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
                txtPass.Text = "";
                txtUser.Text = "";
            }
            else
            {
                Cursor.Current = Cursors.Default;
                --contador;
                MessageBox.Show("Usuario o contraseña incorrecta, tiene: " + contador + " intentos restantes");
                if (contador == 0)
                {
                    contador = 3;
                    btnRegistrar.Enabled = false;
                    //pictureBox2.Enabled = false;
                    Thread.Sleep(3000);
                    btnRegistrar.Enabled = true;
                    //pictureBox2.Enabled = true;
                }
            }

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtPass.AcceptsTab = true;
        }
    }
}
