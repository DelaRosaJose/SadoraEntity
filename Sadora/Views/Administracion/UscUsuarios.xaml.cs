using Sadora.Clases;
using Sadora.Inventario;
using Sadora.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para UscUsuarios.xaml
    /// </summary>
    public partial class UscUsuarios : UserControl
    {

        readonly ViewModels.BaseViewModel<TsysUsuario> ViewModel = new ViewModels.BaseViewModel<TsysUsuario>() { Ventana = new TsysUsuario() { UsuarioID = ClassVariables.UsuarioID } };
        Expression<Func<TsysUsuario, bool>> predicate;

        readonly ViewModels.BaseViewModel<ObservableCollection<TsysAcceso>> ViewModelDataGrid = new ViewModels.BaseViewModel<ObservableCollection<TsysAcceso>>() { Ventana = new ObservableCollection<TsysAcceso>() {} };

        public UscUsuarios()
        {
            InitializeComponent();

            Name = nameof(UscUsuarios);
            DataContext = ViewModel;
            Grilla.DataContext = ViewModelDataGrid;
        }

        //public ObservableCollection<TsysAcceso> Accesos { get; set; }

        bool Inicializador = false;
        bool Imprime, Modifica, Agrega;
        readonly bool PuedeUsarBotonAnular = false;
        private int? _FistID, _LastID, last;
        //DataTable TableGrid;



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

        #region SafeThisCode
        private void BtnSeleccionMasiva_Click(object sender, RoutedEventArgs e)
        {

            //    new FrmValidarAccion("Puede Visualizar masivamente?").ShowDialog();
            //    ClassControl.GridCheckEdit(GridMain, "Visualiza", ClassVariables.ValidarAccion);

            //    new FrmValidarAccion("Puede Imprimir masivamente?").ShowDialog();
            //    ClassControl.GridCheckEdit(GridMain, "Imprime", ClassVariables.ValidarAccion);

            //    new FrmValidarAccion("Puede Agregar masivamente?").ShowDialog();
            //    ClassControl.GridCheckEdit(GridMain, "Agrega", ClassVariables.ValidarAccion);

            //    new FrmValidarAccion("Puede Modificar masivamente?").ShowDialog();
            //    ClassControl.GridCheckEdit(GridMain, "Modifica", ClassVariables.ValidarAccion);

            //    new FrmValidarAccion("Puede Anular masivamente?").ShowDialog();
            //    ClassControl.GridCheckEdit(GridMain, "Anula", ClassVariables.ValidarAccion);

        }


        #endregion



        //void setDatos(int Flag, string Usuario) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        //{
        //    if (Usuario == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
        //    {
        //        if (txtUsuarioID.Text == "")
        //            UsuarioID = 0;
        //        else
        //        {
        //            try
        //            {
        //                UsuarioID = Convert.ToInt32(txtUsuarioID.Text);
        //            }
        //            catch (Exception exception)
        //            {
        //                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del Usuario
        //            }
        //        }
        //    }
        //    else //Si pasamos un Usuario, lo convertimos actualizamos la variable Usuario principal
        //        UsuarioID = Convert.ToInt32(Usuario);

        //    List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
        //    {
        //        new SqlParameter("Flag",Flag),
        //        new SqlParameter("@UsuarioID",UsuarioID),
        //        new SqlParameter("@Nombre",txtNombre.Text),
        //        new SqlParameter("@EmpleadoID",txtEmpleadoID.Text),
        //        new SqlParameter("@GrupoID",txtGrupoID.Text),
        //        new SqlParameter("@Contraseña", PastWordRandom == txtPassword.Password ?  LastPastword : Clases.ClassControl.GetSHA256(txtPassword.Password)),
        //        new SqlParameter("@Activo",cActivar.IsChecked)
        //    };

        //    tabla = Clases.ClassData.runDataTable("sp_sysUsuarios", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

        //    if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
        //    {
        //        Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
        //        frm.ShowDialog();
        //        ClassVariables.GetSetError = null;
        //    }

        //    if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
        //    {
        //        PastWordRandom = tabla.Rows[0]["Contraseña"].ToString().Substring(0, new Random().Next(6, 15));
        //        txtUsuarioID.Text = tabla.Rows[0]["UsuarioID"].ToString();
        //        txtNombre.Text = tabla.Rows[0]["Nombre"].ToString();
        //        txtEmpleadoID.Text = tabla.Rows[0]["EmpleadoID"].ToString();
        //        txtGrupoID.Text = tabla.Rows[0]["GrupoID"].ToString();
        //        txtPassword.Password = PastWordRandom;
        //        cActivar.IsChecked = Convert.ToBoolean(tabla.Rows[0]["Activo"].ToString());
        //        LastPastword = tabla.Rows[0]["Contraseña"].ToString();

        //        if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
        //        {
        //            try
        //            {
        //                LastUsuarioID = Convert.ToInt32(txtUsuarioID.Text); //intentamos convertir el id del Usuario
        //            }
        //            catch (Exception exception)
        //            {
        //                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error ald intentar convertirlo lo enviamos
        //            }
        //        }
        //        ClassControl.setValidador("select * from TrhnEmpleados where EmpleadoID =", txtEmpleadoID, tbxEmpleadoID); //ejecutamos el metodo validador con el campo seleccionado para que lo busque y muestre una vez se guarde el registro
        //        ClassControl.setValidador("select Nombre from TsysGruposUsuarios where GrupoID =", txtGrupoID, tbxGrupoID);
        //    }
        //    listSqlParameter.Clear(); //Limpiamos la lista de parametros.

        //    if (Estado == "Modo Consulta")
        //        setDatosGrid(0);
        //    else if (Estado == "Modo Agregar" || Estado == "Modo Editar")
        //    {
        //        GridMain.View.MoveFirstRow();
        //        for (int i = 0; i < GridMain.VisibleRowCount; i++)
        //        {
        //            setDatosGrid(1);
        //            GridMain.View.MoveNextRow();
        //        }
        //    }
        //}

        //void setDatosGrid(int Flag) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        //{

        //    String Nombre = "";
        //    String Titulo = "";
        //    String Modulo = "";
        //    Boolean Visualiza = false;
        //    Boolean Imprime = false;
        //    Boolean Agrega = false;
        //    Boolean Modifica = false;
        //    Boolean Anula = false;

        //    if (Estado == "Modo Agregar" || Estado == "Modo Editar")
        //    {
        //        Nombre = GridMain.GetFocusedRowCellValue("Nombre").ToString();
        //        Titulo = GridMain.GetFocusedRowCellValue("Titulo").ToString();
        //        Modulo = GridMain.GetFocusedRowCellValue("Modulo").ToString();
        //        Visualiza = Convert.ToBoolean(GridMain.GetFocusedRowCellValue("Visualiza"));
        //        Imprime = Convert.ToBoolean(GridMain.GetFocusedRowCellValue("Imprime"));
        //        Agrega = Convert.ToBoolean(GridMain.GetFocusedRowCellValue("Agrega"));
        //        Modifica = Convert.ToBoolean(GridMain.GetFocusedRowCellValue("Modifica"));
        //        Anula = Convert.ToBoolean(GridMain.GetFocusedRowCellValue("Anula"));
        //    }

        //    List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
        //    {
        //        new SqlParameter("Flag",Flag),
        //        new SqlParameter("@UsuarioID", txtUsuarioID.Text),
        //        new SqlParameter("@Nombre", Nombre),
        //        new SqlParameter("@Modulo", Modulo),
        //        new SqlParameter("@Titulo", Titulo),
        //        new SqlParameter("@Visualiza", Visualiza),
        //        new SqlParameter("@Imprime", Imprime),
        //        new SqlParameter("@Agrega", Agrega),
        //        new SqlParameter("@Modifica", Modifica),
        //        new SqlParameter("@Anula", Anula)
        //    };

        //    DataTable TablaGrid = Clases.ClassData.runDataTable("sp_sysAccesos", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

        //    if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
        //    {
        //        Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
        //        frm.ShowDialog();
        //        ClassVariables.GetSetError = null;
        //    }

        //    if (TablaGrid.Rows.Count > 0) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
        //        GridMain.ItemsSource = TablaGrid;

        //    List<String> listaColumnas = new List<String>() //Estos son los controles que seran controlados, readonly, enable.
        //    {
        //        "Visualiza","Agrega","Modifica","Imprime","Anula"
        //    };

        //    if (Estado == "Modo Agregar" && Estado == "Modo Editar")
        //        Clases.ClassControl.SetGridReadOnly(GridMain, listaColumnas, false);
        //    else
        //        ClassControl.SetGridReadOnly(GridMain);


        //    listSqlParameter.Clear(); listaColumnas.Clear(); //Limpiamos la lista de parametros.
        //}

        //void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        //{
        //    List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
        //    {
        //        txtUsuarioID,txtNombre,txtEmpleadoID,txtGrupoID,txtPassword,cActivar
        //    };

        //    List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
        //    {
        //        txtEmpleadoID,tbxEmpleadoID,txtPassword,txtGrupoID,tbxGrupoID,cActivar
        //    };

        //    List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
        //    {
        //        txtUsuarioID,txtNombre,txtPassword
        //    };

        //    if (Modo == null) //si no trae ningun modo entra el validador
        //    {
        //        if (Estado == "Modo Busqueda")
        //            Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, listaControles);
        //        else if (Estado == "Modo Agregar")
        //            Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, true, null);
        //        else
        //            Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, null);
        //    }
        //    else if (Modo == "Validador") //si el parametro modo es igual a validador ingresa.
        //        Lista = Clases.ClassControl.ValidadorControles(listaControlesValidar); //Este metodo se encarga de validar que cada unos de los controles que se les indica en la lista no se dejen vacios.

        //    listaControl.Clear(); //limpiamos ambas listas
        //    listaControles.Clear();
        //    listaControlesValidar.Clear();
        //}

        //void SetEnabledButton(String status) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        //{

        //    Estado = status;
        //    lIconEstado.ToolTip = Estado;

        //    if (Estado != "Modo Agregar" && Estado != "Modo Editar") //Si el sistema se encuentra en modo consulta o busqueda entra el validador
        //    {
        //        BtnPrimerRegistro.IsEnabled = true;
        //        BtnAnteriorRegistro.IsEnabled = true;
        //        BtnProximoRegistro.IsEnabled = true;
        //        BtnUltimoRegistro.IsEnabled = true;
        //        BtnBuscar.IsEnabled = true;
        //        BtnImprimir.IsEnabled = true;
        //        BtnAgregar.IsEnabled = true;
        //        BtnEditar.IsEnabled = true;

        //        BtnCancelar.IsEnabled = false;
        //        BtnGuardar.IsEnabled = false;
        //        BtnOpciones.IsEnabled = false;
        //        if (Estado == "Modo Consulta") //Si el estado es modo consulta enviamos a ejecutar otro metodo parametizado de forma especial
        //        {
        //            SetControls(true, null, false);
        //            IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
        //        }
        //        else //Si el estado es modo busqueda enviamos a ejecutar el mismo metodo parametizado de forma especial y cambiamos el estado de los botones
        //        {
        //            BtnProximoRegistro.IsEnabled = false;
        //            BtnAnteriorRegistro.IsEnabled = false;
        //            BtnImprimir.IsEnabled = false;
        //            BtnEditar.IsEnabled = false;
        //            SetControls(false, null, false);
        //            IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Search;
        //        }
        //    }
        //    else  //Si el sistema se encuentra en modo Agregar o Editar entra el validador
        //    {
        //        BtnPrimerRegistro.IsEnabled = false;
        //        BtnAnteriorRegistro.IsEnabled = false;
        //        BtnProximoRegistro.IsEnabled = false;
        //        BtnUltimoRegistro.IsEnabled = false;

        //        BtnBuscar.IsEnabled = false;
        //        BtnImprimir.IsEnabled = false;

        //        BtnAgregar.IsEnabled = false;
        //        BtnEditar.IsEnabled = false;

        //        BtnCancelar.IsEnabled = true;
        //        BtnGuardar.IsEnabled = true;
        //        BtnOpciones.IsEnabled = true;
        //        if (Estado == "Modo Agregar") //Si el estado es modo Agregar enviamos a ejecutar otro metodo parametizado de forma especial
        //        {
        //            SetControls(false, null, false);
        //            IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.AddThick;
        //            new FrmValidarAccion("Desea crear un usuario igual a este?").ShowDialog();
        //            if (ClassVariables.ValidarAccion)
        //            {
        //                Estado = "Modo Consulta";
        //                txtUsuarioID.Text = last;
        //                setDatosGrid(0);
        //                txtUsuarioID.Text = (LastUsuarioID + 1).ToString();
        //            }
        //            else
        //            {
        //                txtUsuarioID.Text = (LastUsuarioID + 1).ToString();
        //                setDatosGrid(0);
        //            }

        //        }
        //        else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
        //        {
        //            SetControls(true, null, true);
        //            IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
        //        }
        //        txtUsuarioID.IsReadOnly = true;
        //        List<String> listaColumnas = new List<String>() //Estos son los controles que seran controlados, readonly, enable.
        //        {
        //            "Visualiza","Agrega","Modifica","Imprime","Anula"
        //        };
        //        Clases.ClassControl.SetGridReadOnly(GridMain, listaColumnas, false);
        //    }
        //    if (Imprime == false)
        //        BtnImprimir.IsEnabled = Imprime;
        //    if (Agrega == false)
        //        BtnAgregar.IsEnabled = Agrega;
        //    if (Modifica == false)
        //        BtnEditar.IsEnabled = Modifica;
        //}

        async Task FillGrid()
        {
            using SadoraEntity db = new SadoraEntity();
            var result = db.TsysAccesos.ToListAsync();

            ObservableCollection<TsysAcceso> ac = new ObservableCollection<TsysAcceso>(await result);
            ViewModelDataGrid.Ventana = ac;
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
                    ClassControl.PresentadorSnackBar(SnackbarThree, "Debe completar los campos vacios");

                if (Imprime == false)
                    ControlesGenerales.BtnImprimir.IsEnabled = Imprime;
                if (Agrega == false)
                    ControlesGenerales.BtnAgregar.IsEnabled = Agrega;
                if (Modifica == false)
                    ControlesGenerales.BtnEditar.IsEnabled = Modifica;
                if (PuedeUsarBotonAnular == false)
                    ControlesGenerales.BtnAnular.IsEnabled = PuedeUsarBotonAnular;

                ViewModel.EstadoVentana = ControlesGenerales.EstadoVentana;
                await FillGrid();
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }

    }
}
