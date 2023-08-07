using Sadora.Clases;
using Sadora.CustomElements;
using Sadora.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model = Sadora.Models.TcliTransaccione; //Agregamos este alias para no tener que repetir el mismo tipo en varias partes.

namespace Sadora.Clientes
{
    /// <summary>
    /// Lógica de interacción para UscTransacciones.xaml
    /// </summary>
    public partial class UscTransacciones : UserControl
    {
        readonly ViewModels.BaseViewModel<Model> ViewModel = new ViewModels.BaseViewModel<Model>() { Ventana = new Model() { UsuarioID = ClassVariables.UsuarioID } };
        Expression<Func<Model, bool>> predicate;

        public UscTransacciones()
        {
            InitializeComponent();
            Name = nameof(UscTransacciones);
            DataContext = ViewModel;
        }

        bool Inicializador = false;
        bool Imprime, Modifica, Agrega;
        readonly bool PuedeUsarBotonAnular = false;
        private int? _FistID, _LastID, last;

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

                ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                _FistID = 1;
            }
        }


        private void txtNomenclatura_KeyUp(object sender, KeyEventArgs e)
        {
            if (!new string[] { "Modo Editar", "Modo Agregar" }.Contains(ViewModel.EstadoVentana) || e.Key != Key.Enter)
                return;

            var _texto =
               (sender is UscTextboxGeneral) ? (sender as UscTextboxGeneral).Name :
               (sender is UscTextboxNumerico) ? (sender as UscTextboxNumerico).Name : null;

            EventMontoGravadoYExento(_texto);

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

                if (new string[] { "Modo Editar", "Modo Agregar" }.Contains(ViewModel.EstadoVentana) && (ViewModel.Ventana.MontoExcento.Value != 0 && ViewModel.Ventana.MontoGravado.Value != 0))
                {
                    ClassControl.PresentadorSnackBar(SnackbarThree, "No puede guardar con valor en Monto Gravado y Monto Exento"); ;
                    return;
                }

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

        bool EventMontoGravadoYExento(string field)
        {
            var model = new ViewModels.BaseViewModel<Model>().Ventana = ViewModel.Ventana;
            switch (field)
            {
                case "txtMontoExento" when ViewModel.Ventana.MontoGravado.Value > 0:
                    model.MontoGravado = model.ITBIS = 0;
                    ViewModel.Ventana = model;
                    return false;

                case "txtMontoGravado":
                    model.MontoExcento = 0;
                    model.ITBIS = ViewModel.Ventana.MontoGravado.Value * 0.18;
                    ViewModel.Ventana = model;
                    return false;

                case null:
                    bool ResultadoGeneral = true;
                    foreach (var item in new string[] { "txtNomenclatura", "txtDesde", "txtHasta" })
                    {
                        var CanSave = PuedeGuardarSinComprobante(item);
                        ResultadoGeneral = ResultadoGeneral && CanSave;
                    }
                    return ResultadoGeneral;


                default:
                    return true;
            }
        }


    }
}
