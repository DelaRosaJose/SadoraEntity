using Sadora.Clases;
using Sadora.Models;
using System;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.Clientes
{
    public partial class UscClientes : UserControl
    {
        readonly ViewModels.BaseViewModel<TcliCliente> ViewModel;
        Expression<Func<TcliCliente, bool>> predicate;
         
        public UscClientes()
        {
            InitializeComponent();
            Name = nameof(UscClientes);
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

        private async void UscBotones_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ButtonName = ((Button)e.OriginalSource).Name;
                string Registro = ViewModel.Ventana != null ? ViewModel.Ventana.ID.ToString() : null;
                int intValue = int.TryParse(Registro, out intValue) ? intValue : 0;

                if (ButtonName == "BtnAnteriorRegistro")
                    predicate = (x) => x.ID < (intValue) && x.ID > ((intValue) - 5);
                else if(ButtonName == "BtnProximoRegistro")
                    predicate = (x) => x.ID > (intValue) && x.ID < ((intValue) + 5);
                else if (ButtonName != "BtnCancelar" && ViewModel.Ventana != null)
                {
                    last = ViewModel.Ventana.ID;
                    ViewModel.Ventana.ID = ButtonName == "BtnAgregar" ? ViewModel.Ventana.ID + 1 : default;
                }
                else if (ButtonName == "BtnCancelar" && ViewModel.Ventana != null)
                    ViewModel.Ventana.ID = last.Value;

                ViewModel.Ventana = await BaseModel.Procesar(BotonPulsado: ButtonName, viewModel: ViewModel, IdRegistro: intValue.ToString(),
                    getProp: x => x.ID, getExpresion: predicate, view: MainView.Children, lastRegistro: last);

                _FistID = ButtonName == "BtnPrimerRegistro" ? ViewModel.Ventana.ID : _FistID;
                _LastID = ButtonName == "BtnUltimoRegistro" ? ViewModel.Ventana.ID : _LastID;

                ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado:
                    ButtonName == "BtnGuardar" ? "BtnUltimoRegistro" :
                    ButtonName == "BtnAnteriorRegistro" ? ViewModel.Ventana.ID > _FistID ? ButtonName : "BtnPrimerRegistro" :
                    ButtonName == "BtnProximoRegistro" ? ViewModel.Ventana.ID < _LastID ? ButtonName : "BtnUltimoRegistro" :
                    ButtonName == "BtnCancelar" ? "BtnUltimoRegistro" :
                    ButtonName,
                    ButtonName == "BtnBuscar" ? ViewModel.EstadoVentana : null);

                if (ButtonName == "BtnCancelar")
                    MessageBox.Show($"{last}");

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

        private void txtRNC_KeyUp(object sender, KeyEventArgs e)
        {
            //    if (Estado != "Modo Consulta")
            //    {
            //        if (e.Key == Key.Enter)
            //        {
            //            reader = ClassControl.getDatosCedula(Cli.Cliente.RNC);
            //            if (reader != null)
            //            {
            //                if (reader.HasRows)
            //                {
            //                    if (reader.Read())
            //                    {
            //                        Cli.Cliente.Nombre = reader["NombreCompleto"].ToString();
            //                        //txtDireccion.Text = reader["Direccion"].ToString();
            //                        //txtTelefono.Text = reader["Telefono"].ToString();
            //                        reader.NextResult();

            //                    }
            //                    reader.Close();
            //                    reader.Dispose();
            //                }
            //                else
            //                {
            //                    reader.Close();
            //                    reader.Dispose();
            //                }
            //                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
            //            }
            //            else if (ClassVariables.ExistClient)
            //            {
            //                if (SnackbarThree.MessageQueue is { } messageQueue)
            //                {
            //                    var message = "Ya existe un cliente con este RNC";
            //                    Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            //                }
            //            }

            //        }
            //    }
        }


    }
}
