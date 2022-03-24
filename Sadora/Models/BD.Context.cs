﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SadoraEntities : DbContext
    {
        public SadoraEntities()
            : base("name=SadoraEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TcliClaseCliente> TcliClaseClientes { get; set; }
        public virtual DbSet<TcliCliente> TcliClientes { get; set; }
        public virtual DbSet<TconComprobante> TconComprobantes { get; set; }
        public virtual DbSet<TinvArticulo> TinvArticulos { get; set; }
        public virtual DbSet<TinvClaseArticulo> TinvClaseArticulos { get; set; }
        public virtual DbSet<TinvDepartamento> TinvDepartamentoes { get; set; }
        public virtual DbSet<TinvMarca> TinvMarcas { get; set; }
        public virtual DbSet<TinvMovimientoInventario> TinvMovimientoInventarios { get; set; }
        public virtual DbSet<TinvMovimientoInventarioDetalle> TinvMovimientoInventarioDetalles { get; set; }
        public virtual DbSet<TinvServicioArticulo> TinvServicioArticulos { get; set; }
        public virtual DbSet<TrhnEmpleado> TrhnEmpleados { get; set; }
        public virtual DbSet<TsupClaseProveedore> TsupClaseProveedores { get; set; }
        public virtual DbSet<TsupMovimientosCuenta> TsupMovimientosCuentas { get; set; }
        public virtual DbSet<TsupMovimientosCuentasDetalle> TsupMovimientosCuentasDetalles { get; set; }
        public virtual DbSet<TsupProveedore> TsupProveedores { get; set; }
        public virtual DbSet<TsupTransaccione> TsupTransacciones { get; set; }
        public virtual DbSet<TsysAcceso> TsysAccesos { get; set; }
        public virtual DbSet<TsysEmpresa> TsysEmpresas { get; set; }
        public virtual DbSet<TsysFormulario> TsysFormularios { get; set; }
        public virtual DbSet<TsysGruposAcceso> TsysGruposAccesos { get; set; }
        public virtual DbSet<TsysGruposUsuario> TsysGruposUsuarios { get; set; }
        public virtual DbSet<TsysUsuario> TsysUsuarios { get; set; }
        public virtual DbSet<TvenCaja> TvenCajas { get; set; }
        public virtual DbSet<TvenCajasDetalle> TvenCajasDetalles { get; set; }
        public virtual DbSet<TvenDesglosePago> TvenDesglosePagoes { get; set; }
        public virtual DbSet<TvenFactura> TvenFacturas { get; set; }
        public virtual DbSet<TvenFacturasDetalle> TvenFacturasDetalles { get; set; }
        public virtual DbSet<TvenMetodoPago> TvenMetodoPagos { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<TcliMovimientosCuenta> TcliMovimientosCuentas { get; set; }
        public virtual DbSet<TcliMovimientosCuentasDetalle> TcliMovimientosCuentasDetalles { get; set; }
        public virtual DbSet<TcliTransaccione> TcliTransacciones { get; set; }
    }
}
