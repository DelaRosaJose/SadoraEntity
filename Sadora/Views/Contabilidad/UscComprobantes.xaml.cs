using DevExpress.XtraGauges.Core.Model;
using Sadora.Clases;
using Sadora.CustomElements;
using Sadora.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Model = Sadora.Models.TconComprobante; //Agregamos este alias para no tener que repetir el mismo tipo en varias partes.

namespace Sadora.Contabilidad
{
    /// <summary>
    /// Lógica de interacción para UscComprobantes.xaml
    /// </summary>
    public partial class UscComprobantes : UserControl
    {
        readonly ViewModels.BaseViewModel<Model> ViewModel = new ViewModels.BaseViewModel<Model>() { Ventana = new Model() { UsuarioID = ClassVariables.UsuarioID } };
        Expression<Func<Model, bool>> predicate;

        public UscComprobantes()
        {
            InitializeComponent();
            Name = nameof(UscComprobantes);
            DataContext = ViewModel;
        }

        bool Inicializador = false;
        bool Imprime, Modifica, Agrega;
        readonly bool PuedeUsarBotonAnular = false;
        private int? _FistID, _LastID, last;

        private void UserControl_Initialized(object sender, EventArgs e) => Inicializador = true;

        private void txtNomenclatura_KeyUp(object sender, KeyEventArgs e)
        {
            if (!new string[] { "Modo Editar", "Modo Agregar" }.Contains(ViewModel.EstadoVentana) || e.Key != Key.Enter)
                return;

            var _texto =
               (sender is UscTextboxGeneral) ? (sender as UscTextboxGeneral).Name :
               (sender is UscTextboxNumerico) ? (sender as UscTextboxNumerico).Name : null;

            PuedeGuardarSinComprobante(_texto);
            ValidarDesdeHastaComprobante(_texto);

        }

        bool PuedeGuardarSinComprobante(string field)
        {
            var ValidadorEnteros = new int?[] { null, 0 };
            switch (field)
            {
                case "txtNomenclatura" when ViewModel.Ventana.Nomenclatura.Length != 3:
                    ClassControl.PresentadorSnackBar(SnackbarThree, "Debe ingresar la nomenclatura");
                    return false;

                case "txtDesde" when ValidadorEnteros.Contains(ViewModel.Ventana.Desde):
                    ClassControl.PresentadorSnackBar(SnackbarThree, "Debe ingresar desde que comprobante");
                    return false;

                case "txtHasta" when ValidadorEnteros.Contains(ViewModel.Ventana.Hasta):
                    ClassControl.PresentadorSnackBar(SnackbarThree, "Debe ingresar hasta que comprobante");
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

        bool ValidarDesdeHastaComprobante(string field)
        {
            if (!field.Contains("txtHasta"))
                return false;

            Model model = new Model()
            {
                ID = ViewModel.Ventana.ID,
                Auxiliar = ViewModel.Ventana.Auxiliar,
                Desde = ViewModel.Ventana.Desde,
                Disponibles = ViewModel.Ventana.Disponibles,
                Hasta = ViewModel.Ventana.Hasta,
                NextNCF = ViewModel.Ventana.NextNCF,
                Nombre = ViewModel.Ventana.Nombre,
                Nomenclatura = ViewModel.Ventana.Nomenclatura,
                UsuarioID = ViewModel.Ventana.UsuarioID,
                SinComprobantes = ViewModel.Ventana.SinComprobantes

            };

            if (model.Desde > model.Hasta)
            {
                ClassControl.PresentadorSnackBar(SnackbarThree, "No esta permitido poner Hasta menor a Desde");
                model.Hasta = 0;
                return false;
            }

            model.NextNCF = $"{model.Nomenclatura}{model.Desde:D8}";
            model.Disponibles = model.Hasta - model.Desde;

            ViewModel.Ventana = model;

            return true;
        }

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

                //var result = ;
                if (new string[] { "Modo Editar", "Modo Agregar" }.Contains(ViewModel.EstadoVentana) && (!PuedeGuardarSinComprobante(null) || !ValidarDesdeHastaComprobante("txtHasta")))
                    return;

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

        private bool ValidateSaveSinComprobante()
        {
            //if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            //{
            //    SqlDataReader MetodoCaja = Clases.ClassData.runSqlDataReader("select * from TconComprobantes where SinComprobantes = 1 and comprobanteid <> " + txtComprobanteID.Text, null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

            //    if (MetodoCaja.HasRows && MetodoCaja.Read())
            //    {
            //        if (SnackbarThree.MessageQueue is { } messageQueue)
            //            Task.Factory.StartNew(() => messageQueue.Enqueue("Ya existe un comprobante con esta opcion habilitada"));
            //        SinComprobantes.IsChecked = false;
            //        MetodoCaja.Close();
            //        return false;
            //    }
            //    else
            //    {
            //        MetodoCaja.Close();
            //        return SinComprobantes.IsChecked == true ? true : false;
            //    }
            //}
            return false;

        }

        private void SinComprobantes_Checked(object sender, RoutedEventArgs e)
        {
            ValidateSaveSinComprobante();
        }
    }
}
