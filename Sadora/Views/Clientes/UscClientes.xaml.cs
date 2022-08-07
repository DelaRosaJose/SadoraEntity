using Sadora.Clases;
using Sadora.Models;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.Clientes
{
    public partial class UscClientes : UserControl
    {
        //readonly ViewModels.Clientes.ClientesViewModel ViewModel = new ViewModels.Clientes.ClientesViewModel();
        readonly ViewModels.BaseViewModel<TcliCliente> ViewModel = new ViewModels.BaseViewModel<TcliCliente>();

        public UscClientes()
        {
            InitializeComponent();
            Name = nameof(UscClientes);
            DataContext = ViewModel;
        }

        bool Imprime, Modifica, Agrega;
        readonly bool PuedeUsarBotonAnular = false;

        bool Inicializador = false;
        //string _estado;

        //public string Estado
        //{
        //    get { return _estado; }
        //    set { _estado = value; }

        //    //get { return _estado; }
        //    //set
        //    //{
        //    //    if (_estado == value)
        //    //        return;
        //    //    _estado = value;
        //    //    OnPropertyChanged(nameof(Estado));
        //    //}

        //}


        #region Variables Comentadas

        //DataTable tabla;
        //SqlDataReader reader;
        //string Estado, Lista, last;
        //int ClienteID, LastClienteID;

        #endregion

        //string last;
        private int? _FistClienteID, _LastClienteID, last;


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
                _FistClienteID = 1;
            }
        }

        private async void UscBotones_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ButtonName = ((Button)e.OriginalSource).Name;


                string Registro = ViewModel.Ventana != null ? ViewModel.Ventana.ClienteID.ToString() : null;

                int intValue = int.TryParse(Registro, out intValue) ? intValue : 0;

                ViewModel.Ventana = await BaseModel.Procesar<TcliCliente, ViewModels.BaseViewModel<TcliCliente>>(BotonPulsado: ButtonName, viewModel: ViewModel, IdRegistro: Registro, getProp: x => x.ClienteID, getExpresion: x => x.ClienteID < (intValue - 1) && x.ClienteID > ((intValue - 1) - 5)/*x.ClienteID == (intValue - 1)*/);

                _FistClienteID = ButtonName == "BtnPrimerRegistro" ? ViewModel.Ventana.ClienteID : _FistClienteID;
                _LastClienteID = ButtonName == "BtnUltimoRegistro" ? ViewModel.Ventana.ClienteID : _FistClienteID;



                switch (ButtonName)
                {
                    case "BtnPrimerRegistro":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);

                        break;

                    case "BtnAnteriorRegistro":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Ventana.ClienteID > _FistClienteID ? ButtonName : "BtnPrimerRegistro");
                        break;

                    case "BtnProximoRegistro":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Ventana.ClienteID < _LastClienteID ? ButtonName : "BtnUltimoRegistro");
                        break;

                    case "BtnUltimoRegistro":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;

                    case "BtnBuscar":

                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName, EstadoVentanaPadre: ViewModel.EstadoVentana);
                        break;

                    case "BtnAgregar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        ClassControl.LimpiadorGeneral(MainView.Children);
                        break;

                    case "BtnEditar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;

                    case "BtnCancelar":
                        ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                        break;

                    case "BtnGuardar":
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: "BtnUltimoRegistro");
                        break;
                }
                //}



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
            catch (System.Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }


        /*
        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {

            if (Estado == "Modo Consulta")
            {
                last = txtClienteID.Text;
                SetEnabledButton("Modo Busqueda");
            }
            else if (Estado == "Modo Busqueda")
            {
                List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
                {
                    txtRepresentante,txtClaseID,tbxClaseID,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular,cActivar
                };
                Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

                setDatos(0, null);

                List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
                {
                    "Cliente ID","RNC","Nombre","Representante","Direccion","Activo"
                };

                SetEnabledButton("Modo Consulta");

                if (tabla.Rows.Count > 1)
                {
                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
                    frm.ShowDialog();

                    if (frm.GridMuestra.SelectedItem != null)
                    {
                        DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                        txtClienteID.Text = item.Row.ItemArray[0].ToString();
                        setDatos(0, txtClienteID.Text);
                        frm.Close();
                    }
                    else
                    {
                        setDatos(0, last);
                    }
                }
                else if (tabla.Rows.Count < 1)
                {
                    //BtnProximoRegistro.IsEnabled = false;
                    //BtnAnteriorRegistro.IsEnabled = false;

                    if (SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        var message = "No se encontraron datos";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                }

            }
        }
        */
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
