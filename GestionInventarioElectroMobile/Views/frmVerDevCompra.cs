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
    public partial class frmVerDevCompra : Form
    {
        int index;
        BindingSource bsDevCompras;
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        frmNuevaDevCompra frmNuevaDevCompra;
        frmEditarDevCompra frmEditarDevCompra;

        public frmVerDevCompra()
        {
            InitializeComponent();
            bsDevCompras = new BindingSource();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmNuevaDevCompra = new frmNuevaDevCompra();
            frmNuevaDevCompra.ShowDialog();
            CargarDevCompras();
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rowCollection = dgvDevCompras.SelectedRows;
            int id = int.Parse(dgvDevCompras.Rows[index].Cells["Id_cliente"].FormattedValue.ToString());

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

                frmEditarDevCompra = new frmEditarDevCompra();
                frmEditarDevCompra.DrDevCompra = drow;
                frmNuevaDevCompra.ShowDialog();
                CargarDevCompras();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bsDevCompras.Filter = string.Format(" convert(ID_DevolucionCompras, 'System.String') like '*{0}*' or convert(ID_Compra, 'System.String') like '*{0}*' or convert(Descripcion, 'System.String') like '*{0}*' or convert(Fecha, 'System.String') like '*{0}*' or convert(Total, 'System.String') like '*{0}*'", txtBuscar.Text);
            }
            catch (InvalidExpressionException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dgvDevCompras_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvDevCompras.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvDevCompras.CurrentRow.Selected = true;
                    index = e.RowIndex;
                }
            }
            catch
            {
                MessageBox.Show(this, "ERROR, debe seleccionar una fila de la tabla para poder editar", "Mensaje de ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CargarDevCompras()
        {
            var devcompra = bD.DevolucionCompra;
            bsDevCompras.DataSource = devcompra.ToList();
            dgvDevCompras.DataSource = bsDevCompras;
            dgvDevCompras.AutoGenerateColumns = true;
        }

        
    }
}
