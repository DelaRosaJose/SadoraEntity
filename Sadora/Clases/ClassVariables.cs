using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Sadora.Clases
{


    public class ClassVariables : INotifyPropertyChanged
    {

        public static ClassVariables ClasesVariables = new ClassVariables();

        public static string GetSetError;

        public static bool ValidarAccion;
        public static int UsuarioID;
        public static string UsuarioNombre;
        private static bool existclient = false;

        public static bool Imprime;
        public static bool Agrega;
        public static bool Modifica;
        public static bool Anula;

        public string Nombre { get; set; }
        public string Formulario { get; set; }
        public string Modulo { get; set; }
        public string Titulo { get; set; }

        #region FormaPagoProperty
        public string IdFormaPago { get; set; }
        public string FormaPago { get; set; }
        public double CantidadFormaPago { get; set; }
        #endregion

        public static bool ExistClient
        {
            get { return existclient; }
            set { existclient = value; }
        }

        public static bool IsFullFormaPago;

        private string cliente;
        private string rnc;
        private string ncf;
        private string Clasencf;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string ClienteDinamic { get { return cliente; } set { cliente = value; OnPropertyChanged(); } }
        public string RNCDinamic { get { return rnc; } set { rnc = value; OnPropertyChanged(); } }
        public string NCFDinamic { get { return ncf; } set { ncf = value; OnPropertyChanged(); } }
        public string ClaseNCFDinamic { get { return Clasencf; } set { Clasencf = value; OnPropertyChanged(); } }

        public static List<Clases.ClassVariables> ListFormasPagos = new List<Clases.ClassVariables>();


        #region UserName
        private string username;
        public string UserName { get { return username; } set { username = value; OnPropertyChanged(); } }

        #endregion
        public string UserID { get; set; }
        public int CountIntent { get; set; }

        #region PropVenArticulos

        public string NombreServicio { get; set; }
        public double PrecioServicio { get; set; }
        public bool AltaServicio { get; set; }

        #endregion

        #region Datos Empresa
        private string nombreempresa;
        public string NombreEmpresa { get { return !string.IsNullOrWhiteSpace(nombreempresa) ? nombreempresa : "Sadora Inc."; } set { nombreempresa = value; OnPropertyChanged(); } }

        
        private string RNCempresa;
        public string RNCEmpresa { get { return !string.IsNullOrWhiteSpace(RNCempresa) ? RNCempresa : "Sadora Inc."; } set { RNCempresa = value; OnPropertyChanged(); } }

        private string direccionempresa;
        public string DireccionEmpresa { get { return !string.IsNullOrWhiteSpace(direccionempresa) ? direccionempresa : "Sadora Inc."; } set { direccionempresa = value; OnPropertyChanged(); } }

        private string telefonoempresa;
        public string TelefonoEmpresa { get { return !string.IsNullOrWhiteSpace(telefonoempresa) ? telefonoempresa : "Sadora Inc."; } set { telefonoempresa = value; OnPropertyChanged(); } }



        //public static string NombreEmpresa;

        public static byte[] LogoEmpresa;

        #endregion
    }
}
