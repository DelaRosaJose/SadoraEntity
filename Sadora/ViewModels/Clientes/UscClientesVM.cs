using Sadora.Clases;
using System;

namespace Sadora.ViewModels.Clientes
{
    class UscClientesVM : ObservableObject
    {
        #region Declaracion de variables
        private int? _cliente, _claseID, _usuarioID, _diascredito; 
        private int _clasecomprobanteID; 
        private string _rnc, _nombre, _representante, _direccion, _correoelectronico, _telefono, _celular;
        private bool? _activo;
        #endregion
        public Nullable<int> ClienteID { get { return _cliente; } set { _cliente = value; OnPropertyChanged(); } }
        public string RNC { get { return _rnc; } set { _rnc = value; OnPropertyChanged(); } }
        public string Nombre { get { return _nombre; } set { _nombre = value; OnPropertyChanged(); } }
        public string Representante { get { return _representante; } set { _representante = value; OnPropertyChanged(); } }
        public Nullable<int> ClaseID { get { return _claseID; } set { _claseID = value; OnPropertyChanged(); } }
        public string Direccion { get { return _direccion; } set { _direccion = value; OnPropertyChanged(); } }
        public string CorreoElectronico { get { return _correoelectronico; } set { _correoelectronico = value; OnPropertyChanged(); } }
        public string Telefono { get { return _telefono; } set { _telefono = value; OnPropertyChanged(); } }
        public string Celular { get { return _celular; } set { _celular = value; OnPropertyChanged(); } }
        public Nullable<bool> Activo { get { return _activo; } set { _activo = value; OnPropertyChanged(); } }
        public Nullable<int> UsuarioID { get { return _usuarioID; } set { _usuarioID = value; OnPropertyChanged(); } }
        public int ClaseComprobanteID { get { return _clasecomprobanteID; } set { _clasecomprobanteID = value; OnPropertyChanged(); } }
        public Nullable<int> DiasCredito { get { return _diascredito; } set { _diascredito = value; OnPropertyChanged(); } }
    }
}
