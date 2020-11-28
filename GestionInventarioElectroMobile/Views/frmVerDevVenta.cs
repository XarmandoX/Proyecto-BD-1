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
    public partial class frmVerDevVenta : Form
    {
        int index;
        BindingSource bsDevVentas;
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        frmNuevaDevVenta frmNuevaDevVenta;
        frmEditarDevVenta FrmEditarDevVenta;

        public frmVerDevVenta()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmNuevaDevVenta = new frmNuevaDevVenta();
            frmNuevaDevVenta.ShowDialog();
            CargarDevVentas();
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rowCollection = dgvDevVentas.SelectedRows;

            if (rowCollection.Count == 0)
            {
                MessageBox.Show(this, "ERROR, seleccione al menos 1 fila para poder editar", "Mensaje de ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (rowCollection.Count > 1)
            {
                MessageBox.Show(this, "ERROR, debe seleccionar una sola fila de la tabla para poder editar", "Mensaje de ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DataGridViewRow gridRow = rowCollection[0];
                DataRow drow = ((DataRowView)gridRow.DataBoundItem).Row;

                FrmEditarDevVenta = new frmEditarDevVenta();
                FrmEditarDevVenta.DrDevVenta = drow;
                FrmEditarDevVenta.ShowDialog();
                CargarDevVentas();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bsDevVentas.Filter = string.Format(" convert(ID_DevVenta, 'System.String') like '*{0}*' or convert(ID_Venta, 'System.String') like '*{0}*' or convert(Descripcion, 'System.String') like '*{0}*' or convert(Fecha, 'System.String') like '*{0}*' or convert(Total, 'System.String') like '*{0}*'", txtBuscar.Text);
            }
            catch (InvalidExpressionException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dgvDevVentas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvDevVentas.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvDevVentas.CurrentRow.Selected = true;
                    index = e.RowIndex;
                }
            }
            catch
            {
                MessageBox.Show(this, "ERROR, debe seleccionar una fila de la tabla para poder editar", "Mensaje de ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CargarDevVentas()
        {
            var devcompra = bD.DevolucionCompra;
            bsDevVentas.DataSource = devcompra.ToList();
            dgvDevVentas.DataSource = bsDevVentas;
            dgvDevVentas.AutoGenerateColumns = true;
        }
    }
}
