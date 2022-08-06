using Sadora.Clases;
using Sadora.Models;
using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.Clientes
{
    public partial class UscClientes : UserControl
    {
        readonly ViewModels.Clientes.ClientesViewModel ViewModel = new ViewModels.Clientes.ClientesViewModel();

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
            }
        }

        private async void UscBotones_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int intValue;
                //using (SadoraEntity db = new Models.SadoraEntity())
                //{
                    string ButtonName = ((Button)e.OriginalSource).Name;

                    TcliCliente Tsql = default;

                    var este = this;

                    //BaseModel.Procesar<TcliCliente>(ButtonName);
                    ViewModel.Cliente = await BaseModel.Procesar(BotonPulsado: ButtonName, Viewmodel: ViewModel.Cliente, getProp: x => x.ClienteID, IdRegistro: ViewModel.Cliente.ClienteID.ToString(), EstadoVentana: ViewModel.EstadoVentana);

                switch (ButtonName)
                {
                    case "BtnPrimerRegistro":
                        //Tsql = db.TcliClientes.OrderBy(x => x.ClienteID).FirstOrDefault();
                        //ViewModel.Cliente = Tsql != default ? Tsql : ViewModel.Cliente;
                        //_FistClienteID = ViewModel.Cliente.ClienteID;
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;

                    case "BtnAnteriorRegistro":
                        //intValue = int.TryParse(ViewModel.Cliente.ClienteID.ToString(), out intValue) ? intValue : 0;
                        //Tsql = db.TcliClientes.Where(x => x.ClienteID == (intValue - 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                        //ViewModel.Cliente = Tsql != default ? Tsql : ViewModel.Cliente;
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Cliente.ClienteID > _FistClienteID ? ButtonName : "BtnPrimerRegistro");
                        break;

                    case "BtnProximoRegistro":
                        //intValue = int.TryParse(ViewModel.Cliente.ClienteID.ToString(), out intValue) ? intValue : 0;
                        //Tsql = db.TcliClientes.Where(x => x.ClienteID == (intValue + 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                        //ViewModel.Cliente = Tsql != default ? Tsql : ViewModel.Cliente;
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Cliente.ClienteID < _LastClienteID ? ButtonName : "BtnUltimoRegistro");
                        break;

                    case "BtnUltimoRegistro":
                        //Tsql = db.TcliClientes.OrderByDescending(x => x.ClienteID).FirstOrDefault();
                        //ViewModel.Cliente = Tsql != default ? Tsql : ViewModel.Cliente;
                        //_LastClienteID = ViewModel.Cliente.ClienteID;
                        ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                        break;

                    case "BtnBuscar":
                        //if (ViewModel.EstadoVentana == "Modo Consulta")
                        //    last = ViewModel.Cliente.ClienteID;
                        //else if (ViewModel.EstadoVentana == "Modo Busqueda")
                        //{


                        //    ViewModel.Cliente.ClienteID = 0;
                        //    var Result = db.TcliClientes.Where(x => (x.ClienteID == ViewModel.Cliente.ClienteID || ViewModel.Cliente.ClienteID == 0)

                        //                                            //(ClienteID = @ClienteID or @ClienteID = 0) and(RNC = @RNC or @RNC = '') and(Nombre like '%' + @Nombre + '%' or @Nombre = '')

                        //                                            /* || (x.RNC.Contains(ViewModel.Cliente.RNC) //== ViewModel.Cliente.RNC
                        //                                             && x.Nombre.StartsWith(ViewModel.Cliente.Nombre)// == ViewModel.Cliente.Nombre
                        //                                             && x.Representante.Contains(ViewModel.Cliente.Representante)// == ViewModel.Cliente.Representante
                        //                                             && x.Direccion.Contains(ViewModel.Cliente.Direccion)// == ViewModel.Cliente.Direccion
                        //                                             && x.CorreoElectronico.Contains(ViewModel.Cliente.CorreoElectronico)// == ViewModel.Cliente.CorreoElectronico
                        //                                             && x.Telefono.Contains(ViewModel.Cliente.Telefono)// == ViewModel.Cliente.Telefono
                        //                                             && x.Celular.Contains(ViewModel.Cliente.Celular))
                        //                                            */

                        //                                            //&& x.Nombre.Contains(ViewModel.Cliente.Nombre)// == ViewModel.Cliente.Nombre
                        //                                            //&& x.Representante.Contains(ViewModel.Cliente.Representante)// == ViewModel.Cliente.Representante
                        //                                            //&& x.Direccion.Contains(ViewModel.Cliente.Direccion)// == ViewModel.Cliente.Direccion
                        //                                            //&& x.CorreoElectronico.Contains(ViewModel.Cliente.CorreoElectronico)// == ViewModel.Cliente.CorreoElectronico
                        //                                            //&& x.Telefono.Contains(ViewModel.Cliente.Telefono)// == ViewModel.Cliente.Telefono
                        //                                            //&& x.Celular.Contains(ViewModel.Cliente.Celular))// == ViewModel.Cliente.Celular
                        //                                            ).ToList();
                        //    if (Result.Count == 0)
                        //        ClassControl.PresentadorSnackBar(SnackbarThree, "No se encontraron datos");
                        //    else if (Result.Count == 1)
                        //        ViewModel.Cliente = Result[0];
                        //    else
                        //    {
                        //        Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, ClassControl.ListToDataTable(Result), null);
                        //        frm.ShowDialog();

                        //        if (frm.GridMuestra.SelectedItem != null)
                        //        {
                        //            DataRowView item = frm.GridMuestra.SelectedItem as DataRowView;
                        //            //txtClienteID.Text = item.Row.ItemArray[0].ToString();
                        //            //setDatos(0, txtClienteID.Text);
                        //            frm.Close();
                        //        }
                        //        else
                        //        {
                        //            //setDatos(0, last);
                        //        }
                        //    }

                        //    //var matches = from x in db.TcliClientes
                        //    //              where x.ClienteID.Contains(ViewModel.Cliente.ClienteID)
                        //    //              select x;


                        //    //var ResultSet = "";//db.TcliClientes.e
                        //    //Where(x=>x.ClienteID = ViewModel.Cliente.ClienteID).FirstOrDefault();
                        //}

                        //MessageBox.Show(ViewModel.EstadoVentana);

                        //ViewModel.Cliente = db.TcliClientes.Find(ViewModel.Cliente.ClienteID);//Where(x=>x.ClienteID = ViewModel.Cliente.ClienteID).FirstOrDefault();

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
                        //if (ViewModel.EstadoVentana == "Modo Agregar")
                        //    db.TcliClientes.Add(ViewModel.Cliente);
                        //else if (ViewModel.EstadoVentana == "Modo Editar")
                        //    db.Entry(ViewModel.Cliente).State = System.Data.Entity.EntityState.Modified;

                        //await db.SaveChangesAsync();

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
