//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestionInventarioElectroMobile.Model
{
    using System;
    
    public partial class ListarPiezas_Result
    {
        public string ID_Pieza { get; set; }
        public string ID_Proveedor { get; set; }
        public Nullable<byte> ID_Estado { get; set; }
        public byte ID_TipoPiezas { get; set; }
        public string SRCImagen { get; set; }
        public string SRCImagenCir { get; set; }
        public string SRCImagenCorr { get; set; }
        public string estante { get; set; }
        public Nullable<byte> tension { get; set; }
        public decimal precio { get; set; }
        public int cantidad { get; set; }
        public string circuito { get; set; }
        public Nullable<byte> dientes { get; set; }
        public Nullable<byte> recuento_terminales { get; set; }
        public string voltaje { get; set; }
        public string Fabricante { get; set; }
    }
}
