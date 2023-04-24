using Sadora.Clases;
using Sadora.Models;
using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using Model = Sadora.Models.TsupProveedore; //Agregamos este alias para no tener que repetir el mismo tipo en varias partes.

namespace Sadora.Proveedores
{
    /// <summary>
    /// Lógica de interacción para UscProveedores.xaml
    /// </summary>
    public partial class UscProveedores : UserControl
    {
        readonly ViewModels.BaseViewModel<Model> ViewModel = new ViewModels.BaseViewModel<Model>() { Ventana= new Model() {UsuarioID = ClassVariables.UsuarioID } };
        Expression<Func<Model, bool>> predicate;

        public UscProveedores()
        {
            InitializeComponent();
            Name = nameof(UscProveedores);
            DataContext = ViewModel;
        }

        bool Imprime, Modifica, Agrega;
        readonly bool PuedeUsarBotonAnular = false;
        public bool Inicializador = false;

        int? _FistID, _LastID, last;

        private void UserControl_Initialized(object sender, EventArgs e) => Inicializador = true;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Inicializador == true)
            {
                Inicializador = false;
                Imprime = ClassVariables.Imprime;
                Agrega = ClassVariables.Agrega;
                Modifica = ClassVariables.Modifica;

                ViewModel.EstadoVentana = "Modo Consulta";
                this.ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }
         
        private async void UscBotones_Click(object sender, RoutedEventArgs e)
        {
            int? LastRegister = default;
            try
            {
                string ButtonName = ((Button)e.OriginalSource).Name;
                string Registro = ViewModel.Ventana != null ? ViewModel.Ventana.ID.ToString() : null;
                int intValue = int.TryParse(Registro, out intValue) ? intValue : 0;

                if (ButtonName == "BtnAnteriorRegistro")
                    predicate = (x) => x.ID < (intValue) && x.ID > ((intValue) - 5);
                else if (ButtonName == "BtnProximoRegistro")
                    predicate = (x) => x.ID > (intValue) && x.ID < ((intValue) + 5);
                else if (ButtonName != "BtnCancelar" && ViewModel.Ventana != null)
                {
                    LastRegister = ViewModel.Ventana.ID;
                    ViewModel.Ventana.ID = ButtonName == "BtnAgregar" ? ViewModel.Ventana.ID + 1 : ViewModel.Ventana.ID;
                }
                else if (ButtonName == "BtnCancelar" && ViewModel.Ventana != null)
                    ViewModel.Ventana.ID = last.Value;

                var Process = await BaseModel.Procesar(BotonPulsado: ButtonName, viewModel: ViewModel, IdRegistro: intValue.ToString(),
                    getProp: x => x.ID, getExpresion: predicate, view: MainView.Children, lastRegistro: last);

                ViewModel.Ventana = Process.Item1;

                _FistID = ButtonName == "BtnPrimerRegistro" && ViewModel.Ventana != null ? ViewModel.Ventana.ID : _FistID;
                _LastID = ButtonName == "BtnUltimoRegistro" && ViewModel.Ventana != null ? ViewModel.Ventana.ID : _LastID;

                if (Process.Item2)
                {
                    ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado:
                        ButtonName == "BtnGuardar" ? "BtnUltimoRegistro" :
                        ButtonName == "BtnAnteriorRegistro" && ViewModel.Ventana != null ? ViewModel.Ventana.ID > _FistID ? ButtonName : "BtnPrimerRegistro" :
                        ButtonName == "BtnProximoRegistro" && ViewModel.Ventana != null ? ViewModel.Ventana.ID < _LastID ? ButtonName : "BtnUltimoRegistro" :
                        ButtonName == "BtnCancelar" ? "BtnUltimoRegistro" :
                        ButtonName,
                        ButtonName == "BtnBuscar" ? ViewModel.EstadoVentana : null);

                    last = LastRegister;
                }
                else
                    ClassControl.PresentadorSnackBar(SnackbarThree, Process.Item3);

                if (Imprime == false)
                    ControlesGenerales.BtnImprimir.IsEnabled = Imprime;
                if (Agrega == false)
                    ControlesGenerales.BtnAgregar.IsEnabled = Agrega;
                if (Modifica == false)
                    ControlesGenerales.BtnEditar.IsEnabled = Modifica;
                if (PuedeUsarBotonAnular == false)
                    ControlesGenerales.BtnAnular.IsEnabled = PuedeUsarBotonAnular;

                ViewModel.EstadoVentana = ControlesGenerales.EstadoVentana;
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }

        private void UscRNC_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ClassControl.IsValidCedulaORNC(ViewModel.Ventana.RNC, ViewModel.EstadoVentana))
            {
                DGII_RNC Cedula = ClassControl.BuscarPorRNCoCedula(ViewModel.Ventana.RNC);
                ViewModel.Ventana.Nombre = Cedula.RazonSocial != default ? Cedula.RazonSocial : ViewModel.Ventana.Nombre;
                ViewModel.Ventana.Representante = Cedula.NombreComercial != default ? Cedula.NombreComercial : ViewModel.Ventana.Representante;
                ViewModel.Ventana = ViewModel.Ventana;
            }
        }
    }
}
