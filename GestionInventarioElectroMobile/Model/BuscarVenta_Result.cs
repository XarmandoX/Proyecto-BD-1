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
    
    public partial class BuscarVenta_Result
    {
        public int ID_Venta { get; set; }
        public string ID_Cliente { get; set; }
        public Nullable<System.DateTime> Fecha_Inicial { get; set; }
        public System.DateTime FechaEntrega { get; set; }
        public string Estado { get; set; }
        public decimal Total { get; set; }
    }
}
