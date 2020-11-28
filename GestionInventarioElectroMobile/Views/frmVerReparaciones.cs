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
    public partial class frmVerReparaciones : Form
    {
        int index;
        BindingSource bsReparaciones;
        ELECTROMOBILEModel bD = new ELECTROMOBILEModel();
        frmNuevaReparacion frmNuevaReparacion;

        public frmVerReparaciones()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmNuevaReparacion = new frmNuevaReparacion();
            frmNuevaReparacion.ShowDialog();
            CargarReparaciones();
        }

        private void BTN_Editar_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection rowCollection = dgvReparaciones.SelectedRows;
            int id = int.Parse(dgvReparaciones.Rows[index].Cells["Id_cliente"].FormattedValue.ToString());

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

                frmNuevaReparacion = new frmNuevaReparacion();
                frmNuevaReparacion.DrReparacion = drow;
                frmNuevaReparacion.ShowDialog();
                CargarReparaciones();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bsReparaciones.Filter = string.Format(" convert(ID_Reparacion, 'System.String') like '*{0}*' or convert(ID_Cliente, 'System.String') like '*{0}*' or convert(FechaIni, 'System.String') like '*{0}*' or convert(FechaFinal, 'System.String') like '*{0}*' or convert(Total, 'System.String') like '*{0}*'", txtBuscar.Text);
            }
            catch (InvalidExpressionException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void dgvReparaciones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvReparaciones.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dgvReparaciones.CurrentRow.Selected = true;
                    index = e.RowIndex;
                }
            }
            catch
            {
                MessageBox.Show(this, "ERROR, debe seleccionar una fila de la tabla para poder editar", "Mensaje de ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void CargarReparaciones()
        {
            var devcompra = bD.DevolucionCompra;
            bsReparaciones.DataSource = devcompra.ToList();
            dgvReparaciones.DataSource = bsReparaciones;
            dgvReparaciones.AutoGenerateColumns = true;
        }
    }
}
