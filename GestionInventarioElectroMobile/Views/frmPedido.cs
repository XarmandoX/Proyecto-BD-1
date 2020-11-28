using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GestionInventarioElectroMobile.Model;

namespace GestionInventarioElectroMobile.Views
{
    public partial class frmPedido : Form
    {
        public frmPedido()
        {
            InitializeComponent();
        }

        private void frmPedido_Load(object sender, EventArgs e)
        {
            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                dgvPedidos.DataSource = model.InformacionDePedidos.ToList();
                cmbEstado.SelectedIndex = 0;
                btnEditar.Enabled = false;
                btnEliminar.Enabled = false;
            }
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int indexRow = e.RowIndex;
            if (indexRow < 0)
            {
                return;
            }

            /*v.ID_Venta AS ID_Venta,
		v.ID_Cliente AS ID_Cliente,
		v.Fecha_Inicial AS Fecha_inicial,
		v.FechaEntrega AS Fecha_venta,
		P.ID_Pieza AS Codigo_producto,
		tp.Nombre AS Tipo_pieza,
		e.Name AS estado,
		p.precio AS precio_venta,
		dv.cantidad AS cantidad,
		dbo.CalcularSubtotal(p.precio, dv.cantidad) AS subtotal*/

            DataGridViewRow row = dgvPedidos.Rows[indexRow];
            txtIdVenta.Text = row.Cells[0].Value.ToString();
            txtIdPi.Text = row.Cells[4].Value.ToString();
            txtIdCli.Text = row.Cells[1].Value.ToString();
            txtCan.Text = row.Cells[8].Value.ToString();
            dtIni.Value = DateTime.Parse(row.Cells[2].Value.ToString());
            dtFin.Value = DateTime.Parse(row.Cells[3].Value.ToString());
            if (row.Cells[6].Value.ToString().ToLower().Equals("entregado"))
            {
                cmbEstado.SelectedIndex = 0;
            }
            else
                cmbEstado.SelectedIndex = 1;


        }

        private void btnEncargo_Click(object sender, EventArgs e)
        {
            if (txtCan.Text == "" || txtIdCli.Text == "" ||
                txtIdPi.Text == "" || panel.Text == "")
            {
                MessageBox.Show("Insertar la informacion requerida", "Alerta");
                return;
            }

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var idPieza = model.Pieza.
                                Where(p => p.ID_Pieza == txtIdPi.Text).
                                Select(p => p.ID_Pieza).
                                FirstOrDefault();
                if (idPieza != txtIdPi.Text)
                {
                    MessageBox.Show("El producto no esta registrado", "Alerta");
                    return;
                }

                int Can = model.Pieza.
                    Where(p => p.ID_Pieza == txtIdPi.Text.ToString()).
                    Select(p => p.cantidad).
                    FirstOrDefault();

                if (Can < int.Parse(txtCan.Text))
                {
                    MessageBox.Show("No puede vender una cantidad menor de la que esta en inventario", "Alerta");
                    return;
                }

                string idCli = model.Cliente.
                                Where(c => c.RUC == txtCan.Text.ToString()).
                                Select(c => c.RUC).
                                FirstOrDefault();

                if (idCli != txtIdCli.Text)
                {
                    MessageBox.Show("Cliente no registrado", "informacion");
                    return;
                }

                string estado;
                if (cmbEstado.SelectedIndex == 0)
                {
                    estado = "Entregado";
                }
                estado = "Solicitado";

                model.InsertarVenta(idCli, dtFin.Value);
                model.AgregarDetalleVenta(model.Venta.Select(v => v.ID_Venta).Last(), idPieza, Can);
                model.SaveChanges();
                MessageBox.Show("Encargo insertado con exito", "informacion");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (txtCan.Text == "" || txtIdCli.Text == "" || txtIdPi.Text == "" || panel.Text == "")
            {
                MessageBox.Show("Seleccionar la columna del encargo a editar", "informacion");
                return;
            }


            if (txtCan.Text == "" || txtIdCli.Text == "" ||
                txtIdPi.Text == "" || panel.Text == "")
            {
                MessageBox.Show("Insertar la informacion requerida", "Alerta");
                return;
            }

            using (ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
                var idPieza = model.Pieza.
                                Where(p => p.ID_Pieza == txtIdPi.Text).
                                Select(p => p.ID_Pieza).
                                FirstOrDefault();
                if (idPieza != txtIdPi.Text)
                {
                    MessageBox.Show("El producto no esta registrado", "Alerta");
                    return;
                }

                int Can = model.Pieza.
                    Where(p => p.ID_Pieza == txtIdPi.Text.ToString()).
                    Select(p => p.cantidad).
                    FirstOrDefault();

                if (Can < int.Parse(txtCan.Text))
                {
                    MessageBox.Show("No puede vender una cantidad menor de la que esta en inventario", "Alerta");
                    return;
                }

                string idCli = model.Cliente.
                                Where(c => c.RUC == txtCan.Text.ToString()).
                                Select(c => c.RUC).
                                FirstOrDefault();

                if (idCli != txtIdCli.Text)
                {
                    MessageBox.Show("Cliente no registrado", "informacion");
                    return;
                }

                string estado;
                if (cmbEstado.SelectedIndex == 0)
                {
                    estado = "Entregado";
                }
                estado = "Solicitado";

                int idVen;

                idVen = model.Venta.Where(v => v.ID_Venta == int.Parse(txtIdVenta.Text)).Select(V => V.ID_Venta).FirstOrDefault();

                if (idVen != int.Parse(txtIdVenta.Text))
                {
                    MessageBox.Show("Venta no registrada", "Alerta");
                    return;
                }

                model.EditarVenta(idVen, idCli, dtFin.Value, estado);
                model.EditarDetalle_venta(idVen, idPieza, Can);
                model.SaveChanges();
                MessageBox.Show("Encargo editado con exito", "informacion");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtIdVenta.Text == "")
            {
                MessageBox.Show("Seleccionar la celda a editar", "informacion");
                return;
            }

            using(ELECTROMOBILEModel model = new ELECTROMOBILEModel())
            {
               // model.Venta.Remove((Venta)model.Venta.Where(v => v.ID_Venta == int.Parse(txtIdVenta.Text)).Select(v => v.ID_Venta).SingleOrDefault());

                model.SaveChanges();
                MessageBox.Show("Se elimino el pedido", "informacion");
            }         
        }
    }
}
