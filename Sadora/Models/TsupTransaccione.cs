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
    
    public partial class TsupTransaccione
    {
        public int RowID { get; set; }
        public Nullable<int> TransaccionID { get; set; }
        public string TipoTransaccion { get; set; }
        public Nullable<int> ProveedorID { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<double> MontoExcento { get; set; }
        public Nullable<double> MontoGravado { get; set; }
        public Nullable<double> ITBIS { get; set; }
        public string Estado { get; set; }
        public Nullable<int> UsuarioID { get; set; }
    }
}
