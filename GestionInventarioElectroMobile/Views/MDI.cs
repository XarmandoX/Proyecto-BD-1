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
    public partial class frmMain : Form
    {
        frmNuevaVenta FrmNuevaVenta = new frmNuevaVenta();
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            initializeMenu();
        }

        private void initializeMenu()
        {
            pnlSubMenuVentas.Visible = false;
            pnlSubMenuCompras.Visible = false;
            pnlSubMenuReparaciones.Visible = false;
            pnlSubMenuDevolucion.Visible = false;
            pnlSubMenuInventario.Visible = false;
            pnlSubMenuCliente.Visible = false;
            pnlSubMenuProveedores.Visible = false;
        }

        private void hideSubMenu()
        {
            if (pnlSubMenuVentas.Visible == true)
                pnlSubMenuVentas.Visible = false;
            if (pnlSubMenuCompras.Visible == true)
                pnlSubMenuCompras.Visible = false;
            if (pnlSubMenuReparaciones.Visible == true)
                pnlSubMenuReparaciones.Visible = false;
            if (pnlSubMenuDevolucion.Visible == true)
                pnlSubMenuDevolucion.Visible = false;
            if (pnlSubMenuInventario.Visible == true)
                pnlSubMenuInventario.Visible = false;
        }

        private void showSubMenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hideSubMenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnVentas_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuVentas);
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuCompras);
        }

        private void btnReparacion_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuReparaciones);
        }

        private void btnNuevaDevolucion_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuDevolucion);
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuInventario);
        }

        #region Ventas
        private void btnNuevaVenta_Click(object sender, EventArgs e)
        {
            openChildForm(new frmNuevaVenta());
            hideSubMenu();
        }

        private void btnVerVenta_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVerVentas());
            hideSubMenu();
        }
        #endregion

        #region Compras
        private void btnNCompra_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnVerCompra_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }
        #endregion

        #region reparaciones
        private void btnNReparacion_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnVerReparacion_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        #endregion

        #region devoluciones
        private void btnNDevolucionVenta_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnVerDevVenta_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnNuevaDevolucionCompra_Click(object sender, EventArgs e)
        {
            openChildForm(new frmNuevaDevCompra());
            hideSubMenu();
        }

        private void btnVisualizarDevolucionCompra_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        #endregion

        #region piezas
        private void btnAlternadores_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnArmaduras_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnBendix_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnModelos_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnMotoresDeArranque_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnPortadiodos_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnReguladores_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnSoleinoides_Click(object sender, EventArgs e)
        {
            hideSubMenu();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            openChildForm(new frmGestionInventario());
            hideSubMenu();
        }
        #endregion


        private Form frm = null;

        private void openChildForm(Form childForm)
        {
            if (frm != null)
                frm.Close();
            frm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlMain.Controls.Add(childForm);
            pnlMain.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuCliente);
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            showSubMenu(pnlSubMenuProveedores);
        }

        private void btnVisualizarCliente_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVerCliente());
            hideSubMenu();
        }

        private void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            openChildForm(new frmNuevoCliente());
            hideSubMenu();
        }

        private void btnNuevoProveedor_Click(object sender, EventArgs e)
        {
            openChildForm(new frmNuevoProveedor());
            hideSubMenu();
        }

        private void btnVerProv_Click(object sender, EventArgs e)
        {
            openChildForm(new frmVerProveedores());
            hideSubMenu();
        }

        private void btnPedidos_Click(object sender, EventArgs e)
        {
            openChildForm(new frmPedido());
            hideSubMenu();
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }
    }
}
