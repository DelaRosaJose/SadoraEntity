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
    
    public partial class TcliCliente
    {
        public int ID { get; set; }
        public string RNC { get; set; }
        public string Nombre { get; set; }
        public string Representante { get; set; }
        public Nullable<int> ClaseID { get; set; }
        public string Direccion { get; set; }
        public string CorreoElectronico { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> UsuarioID { get; set; }
        public int ClaseComprobanteID { get; set; }
        public Nullable<int> DiasCredito { get; set; }
    }
}
