using Sadora.Clases;
using Sadora.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.Inventario
{
    /// <summary>
    /// Lógica de interacción para UscArticulos.xaml
    /// </summary>
    public partial class UscArticulos : UserControl
    {
        readonly ViewModels.BaseViewModel<TinvArticulo> ViewModel = new ViewModels.BaseViewModel<TinvArticulo>() { Ventana = new TinvArticulo() { UsuarioID = ClassVariables.UsuarioID } };
        Expression<Func<TinvArticulo, bool>> predicate;
        public UscArticulos()
        {
            InitializeComponent();
            Name = nameof(UscArticulos);

            DataContext = ViewModel;
        }
        bool Inicializador = false;
        bool Imprime, Modifica, Agrega;
        readonly bool PuedeUsarBotonAnular = false;
        private int? _FistID, _LastID, last;
        DataTable TableGrid;

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
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }



        //void setDatosGrid(int Flag, string ArticuloID = null) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        //{
        //    string NombreServicio = "";
        //    double Precio = 0;
        //    bool Alta = false;

        //    if (ViewModel.EstadoVentana == "Modo Agregar" || ViewModel.EstadoVentana == "Modo Editar")
        //    {
        //        NombreServicio = GridMain.GetFocusedRowCellValue("NombreServicio").ToString();
        //        Precio = (double)GridMain.GetFocusedRowCellValue("Precio");
        //        Alta = (bool)GridMain.GetFocusedRowCellValue("Alta");
        //    }

        //    List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el NCF en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
        //    {
        //        new SqlParameter("Flag",Flag),
        //        new SqlParameter("@ArticuloID", ArticuloID == null ? ViewModel.Ventana.ID.ToString() : ArticuloID),
        //        new SqlParameter("@NombreServicio", NombreServicio),
        //        new SqlParameter("@Precio", Precio),
        //        new SqlParameter("@Alta", Alta),
        //        new SqlParameter("@UsuarioID", ClassVariables.UsuarioID)
        //    };

        //    TableGrid = Clases.ClassData.runDataTable("sp_invServicioArticulos", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

        //    if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
        //    {
        //        Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
        //        frm.ShowDialog();
        //        ClassVariables.GetSetError = null;
        //    }

        //    if (TableGrid.Rows.Count > 0) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
        //        AgregarModoGrid(GridMain, TablaGrid, TableGrid);


        //    //else
        //    //{
        //    //    GridMain.ItemsSource = null;
        //    //}

        //    List<String> listaColumnas = new List<String>() //Estos son los controles que seran controlados, readonly, enable.
        //    {
        //        //"Visualiza","Agrega","Modifica","Imprime","Anula"
        //    };

        //    if (ViewModel.EstadoVentana == "Modo Agregar" && ViewModel.EstadoVentana == "Modo Editar")
        //    {
        //        //Clases.ClassControl.SetGridReadOnly(GridMain, listaColumnas, false);
        //    }
        //    else
        //    {
        //        ClassControl.SetGridReadOnly(GridMain);
        //    }

        //    listSqlParameter.Clear(); listaColumnas.Clear(); //Limpiamos la lista de parametros.
        //}


        //private void BtnConfiguracionServicios_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ViewModel.EstadoVentana != "Modo Consulta" && ViewModel.EstadoVentana != "Modo Busqueda")
        //    {
        //        MiniDialogo.IsOpen = true;
        //        AgregarModoGrid(GridMain, TablaGrid);
        //    }
        //    else if (ViewModel.EstadoVentana == "Modo Consulta")
        //    {
        //        MiniDialogo.IsOpen = true;
        //        TablaGrid.AllowEditing = false;
        //    }
        //}

        //private void ButtonCerrar_Click(object sender, RoutedEventArgs e)
        //{
        //    GridMain.ItemsSource = null;
        //    MiniDialogo.IsOpen = false;
        //    TablaGrid.AllowEditing = true;
        //}

        //private void AgregarModoGrid(DevExpress.Xpf.Grid.GridControl Grid, DevExpress.Xpf.Grid.TableView tableView, DataTable table = null, bool IsServiciosMasivos = false)
        //{
        //    if (table == null)
        //    {
        //        DataTable reader;

        //        if (!IsServiciosMasivos)
        //            reader = Clases.ClassData.runDataTable("SELECT [NombreServicio], [Precio], [Alta] FROM [SadoraEntity].[dbo].[TinvServicioArticulos] where ArticuloID = " + ViewModel.Ventana.ID, null, "CommandText");
        //        else
        //            reader = Clases.ClassData.runDataTable("declare @Alta bit = 0; select ArticuloID, Tarjeta, Nombre, Descripcion, precio, @Alta as Alta from TinvArticulos where ArticuloID <> " + ViewModel.Ventana.ID /*+ " and HaveServices <> 1"*/, null, "CommandText");

        //        Grid.ItemsSource = reader;
        //        tableView.AutoWidth = true;
        //        if (!IsServiciosMasivos)
        //            tableView.AddNewRow();
        //    }
        //    else
        //        Grid.ItemsSource = table;

        //    if (!IsServiciosMasivos)
        //        foreach (DevExpress.Xpf.Grid.GridColumn Col in Grid.Columns)
        //        {
        //            switch (Col.HeaderCaption)
        //            {
        //                case "Precio":
        //                    Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(50, DevExpress.Xpf.Grid.GridColumnUnitType.Pixel);
        //                    break;

        //                case "Alta":
        //                    Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(20, DevExpress.Xpf.Grid.GridColumnUnitType.Pixel);
        //                    break;
        //            }
        //        }
            
        //}

        //private void TablaGrid_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (ViewModel.EstadoVentana != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter && TablaGrid.FocusedColumn.HeaderCaption.ToString() == "Alta")
        //        {
        //            if (!ValCampoIncompleto())
        //                TablaGrid.AddNewRow();
        //        }
        //        else if (e.Key == Key.F5 && TablaGrid.SelectedRows.Count == 1)
        //        {
        //            new Administracion.FrmValidarAccion("Esta seguro que desea eliminar esta fila?").ShowDialog(); ;
        //            if (ClassVariables.ValidarAccion == true)
        //                TablaGrid.DeleteRow(TablaGrid.FocusedRowHandle);
        //        }
        //    }
        //}

        //bool ValCampoIncompleto()
        //{
        //    string Result = "";

        //    List<int> rowHandles = new List<int>();
        //    for (int i = 0; i < GridMain.VisibleRowCount; i++)
        //    {
        //        foreach (var column in GridMain.Columns)
        //            Result += string.IsNullOrWhiteSpace(GridMain.GetCellValue(GridMain.GetRowHandleByVisibleIndex(i), column).ToString()).ToString() + " ,";

        //        if (Result.Contains("True"))
        //            break;
        //    }

        //    if (Result.Contains("True"))
        //    {
        //        if (SnackbarThreeDialog.MessageQueue is { } messageQueue)
        //            Task.Factory.StartNew(() => messageQueue.Enqueue("Debe llenar todos los campos"));
        //        return true;
        //    }
        //    else
        //        return false;

        //}

        //private void btnAceptar_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!ValCampoIncompleto() && ViewModel.EstadoVentana != "Modo Consulta")
        //    {
        //        GridMainMasivo.ItemsSource = null;
        //        MiniDialogo.IsOpen = false;
        //    }
        //}

        //private void btnAsignacionMasiva_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!ValCampoIncompleto() && ViewModel.EstadoVentana != "Modo Consulta")
        //    {
        //        ServiciosMasivosDialogo.IsOpen = true;
        //        MiniDialogo.IsOpen = false;
        //        AgregarModoGrid(GridMainMasivo, TablaGridMasivo, null, true);
        //    }
        //    else if (ViewModel.EstadoVentana == "Modo Consulta")
        //    {
        //        ServiciosMasivosDialogo.IsOpen = true;
        //        TablaGridMasivo.AllowEditing = false;
        //    }
        //}

        //private void ButtonCerrarMasivo_Click(object sender, RoutedEventArgs e)
        //{
        //    ServiciosMasivosDialogo.IsOpen = false;
        //    TablaGridMasivo.AllowEditing = true;
        //}

        //private void btnAceptarMasivo_Click(object sender, RoutedEventArgs e) => ServiciosMasivosDialogo.IsOpen = false;

    }
}
