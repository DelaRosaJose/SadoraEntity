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
    
    public partial class TconComprobante
    {
        public int RowID { get; set; }
        public int ComprobanteID { get; set; }
        public string Nombre { get; set; }
        public string Auxiliar { get; set; }
        public string Nomenclatura { get; set; }
        public Nullable<int> Desde { get; set; }
        public Nullable<int> Hasta { get; set; }
        public string NextNCF { get; set; }
        public Nullable<int> Disponibles { get; set; }
        public Nullable<int> UsuarioID { get; set; }
        public Nullable<bool> SinComprobantes { get; set; }
    }
}
