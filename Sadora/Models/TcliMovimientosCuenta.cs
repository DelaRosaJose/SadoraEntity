//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sadora.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TcliMovimientosCuenta
    {
        public int RowID { get; set; }
        public Nullable<int> MovimientoID { get; set; }
        public string TipoMovimiento { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> ClienteID { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<double> Debito { get; set; }
        public Nullable<double> Credito { get; set; }
    }
}