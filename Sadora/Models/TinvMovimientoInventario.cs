//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sadora.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TinvMovimientoInventario
    {
        public int RowID { get; set; }
        public Nullable<int> MovimientoID { get; set; }
        public string TipoMovimiento { get; set; }
        public Nullable<System.DateTime> FechaMovimiento { get; set; }
        public string Estado { get; set; }
        public Nullable<int> UsuarioID { get; set; }
    }
}
