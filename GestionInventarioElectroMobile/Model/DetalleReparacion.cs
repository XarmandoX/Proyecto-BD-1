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
    using System.Collections.Generic;
    
    public partial class DetalleReparacion
    {
        public int ID_Reparacion { get; set; }
        public string ID_Pieza { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    
        public virtual Pieza Pieza { get; set; }
        public virtual Reparacion Reparacion { get; set; }
    }
}
