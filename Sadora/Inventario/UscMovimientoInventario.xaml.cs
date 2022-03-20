using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.Inventario
{
    /// <summary>
    /// Lógica de interacción para UscMovimientoInventario.xaml
    /// </summary>
    public partial class UscMovimientoInventario : UserControl
    {
        public UscMovimientoInventario()
        {
            InitializeComponent();
            Name = "UscMovimientoInventario";
        }

        bool Imprime;
        bool Agrega;
        bool Modifica;

        bool Inicializador = false;
        DataTable reader;
        DataTable tabla;
        DataTable TableGrid;
        //SqlDataReader reader;
        string Estado;
        string Lista;
        int MovimientoID;
        int LastMovimientoID;
        string last;

        private void UserControl_Initialized(object sender, EventArgs e) => Inicializador = true;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Inicializador == true)
            {
                Imprime = ClassVariables.Imprime;
                Agrega = ClassVariables.Agrega;
                Modifica = ClassVariables.Modifica;

                this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                Inicializador = false;
            }
        }

        private void BtnPrimerRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
                /*txtMontoGravado,txtMontoGravado*/
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            setDatos(0, "1");
            BtnPrimerRegistro.IsEnabled = false;
            BtnAnteriorRegistro.IsEnabled = false;
        }

        private void BtnAnteriorRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
                /*txtMontoGravado,txtMontoGravado*/
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                MovimientoID = Convert.ToInt32(txtMovimientoID.Text) - 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }


            if (MovimientoID <= 1)
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                setDatos(0, "1");
            }
            else
            {
                setDatos(0, MovimientoID.ToString());
            }
        }

        private void BtnProximoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
                /*txtMontoGravado,txtMontoGravado*/
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                MovimientoID = Convert.ToInt32(txtMovimientoID.Text) + 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }

            if (MovimientoID >= LastMovimientoID)
            {
                BtnUltimoRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                setDatos(0, LastMovimientoID.ToString());
            }
            else
            {
                setDatos(0, MovimientoID.ToString());
            }
        }

        private void BtnUltimoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
                /*txtMontoGravado,txtMontoGravado*/
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            setDatos(-1, "1");
            BtnUltimoRegistro.IsEnabled = false;
            BtnProximoRegistro.IsEnabled = false;
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {

            if (Estado == "Modo Consulta")
            {
                last = txtMovimientoID.Text;
                SetEnabledButton("Modo Busqueda");
            }
            else if (Estado == "Modo Busqueda")
            {
                List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
                {
                    //txtProveedorID,tbxProveedorID,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular//,cActivar
                };
                Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

                setDatos(0, null);

                List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
                {
                    "Proveedor ID","ITBIS","Nomenclatura","Desde","Direccion","Activo"
                };

                SetEnabledButton("Modo Consulta");

                if (tabla.Rows.Count > 1)
                {
                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
                    frm.ShowDialog();

                    if (frm.GridMuestra.SelectedItem != null)
                    {
                        DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                        txtMovimientoID.Text = item.Row.ItemArray[0].ToString();
                        setDatos(0, txtMovimientoID.Text);
                        frm.Close();
                    }
                    else
                        setDatos(0, last);

                }
                else if (tabla.Rows.Count < 1)
                {
                    BtnProximoRegistro.IsEnabled = false;
                    BtnAnteriorRegistro.IsEnabled = false;
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("No se encontraron datos"));
                }

            }
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            SetEnabledButton("Modo Agregar");
            AgregarModoGrid();
            cbxEstado.SelectedIndex = 1;
            cbxTipoMovimiento.Focus();
            dtpFechaMovimiento.Text = DateTime.Today.ToString();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Editar");
            AgregarModoGrid(null, true);
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Consulta");
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SetControls(false, "Validador", false);
            if (Lista != "Debe Completar los Campos: ")
                new Administracion.FrmCompletarCamposHost(Lista).ShowDialog();
            else if (ValidaCanSaverArticulo())
            {
                if (Estado == "Modo Editar")
                    setDatos(2, null);
                else
                    setDatos(1, null);

                SetEnabledButton("Modo Consulta");
                setDatos(0, txtMovimientoID.Text);
            }
        }

        private void txtMovimientoID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void cbxTipoMovimiento_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void dtpFechaMovimiento_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                cbxEstado.Focus();
        }

        private void cbxEstado_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        void setDatos(int Flag, string Transaccion) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                if (Transaccion == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
                {
                    if (txtMovimientoID.Text == "")
                        MovimientoID = 0;
                    else
                    {
                        try
                        {
                            MovimientoID = Convert.ToInt32(txtMovimientoID.Text);
                        }
                        catch (Exception exception)
                        {
                            ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del Proveedor
                        }
                    }
                }
                else //Si pasamos un Proveedor, lo convertimos actualizamos la variable Proveedor principal
                    MovimientoID = Convert.ToInt32(Transaccion);

            })).Wait();


            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el Nomenclatura en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@MovimientoID",MovimientoID),
                new SqlParameter("@TipoMovimiento",cbxTipoMovimiento.Text),
                new SqlParameter("@FechaMovimiento",dtpFechaMovimiento.Text),
                new SqlParameter("@Estado",cbxEstado.Text),
                new SqlParameter("@UsuarioID",ClassVariables.UsuarioID)
            };


            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                tabla = Clases.ClassData.runDataTable("sp_invMovimientoInventario", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

                if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
                {
                    Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                    frm.ShowDialog();
                    ClassVariables.GetSetError = null;
                }

                if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
                {
                    txtMovimientoID.Text = tabla.Rows[0]["MovimientoID"].ToString();
                    cbxTipoMovimiento.Text = tabla.Rows[0]["TipoMovimiento"].ToString();
                    dtpFechaMovimiento.Text = tabla.Rows[0]["FechaMovimiento"].ToString();
                    cbxEstado.Text = tabla.Rows[0]["Estado"].ToString();

                    BtnEditar.IsEnabled = cbxEstado.Text == "Abierta";
                    if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
                    {
                        try
                        {
                            LastMovimientoID = Convert.ToInt32(txtMovimientoID.Text); //intentamos convertir el id del Proveedor
                        }
                        catch (Exception exception)
                        {
                            ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error al intentar convertirlo lo enviamos
                        }
                    }
                }
            })).Wait();

            listSqlParameter.Clear(); //Limpiamos la lista de parametros.

            if (Estado == "Modo Consulta")
                Dispatcher.BeginInvoke(new ThreadStart(() => { setDatosGrid(0); })).Wait();
            else if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                GridMain.View.MoveFirstRow();
                for (int i = 0; i < GridMain.VisibleRowCount; i++)
                {
                    setDatosGrid(Flag);
                    GridMain.View.MoveNextRow();
                }
            }
        }

        void setDatosGrid(int Flag) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            string ArticuloID = "";
            string Tarjeta = "";
            int Cantidad = 0;
            int CantidadPrevia = 0;
            int CantidadPostMovimiento = 0;

            if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                ArticuloID = GridMain.GetFocusedRowCellValue("ArticuloID").ToString();
                Tarjeta = GridMain.GetFocusedRowCellValue("Tarjeta").ToString();
                Cantidad = Convert.ToInt32(GridMain.GetFocusedRowCellValue("Cantidad").ToString());
                CantidadPrevia = Convert.ToInt32(GridMain.GetFocusedRowCellValue("CantidadPrevia").ToString());
                CantidadPostMovimiento = Convert.ToInt32(GridMain.GetFocusedRowCellValue("CantidadPostMovimiento").ToString());
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el NCF en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@MovimientoID", txtMovimientoID.Text),
                new SqlParameter("@ArticuloID", ArticuloID),
                new SqlParameter("@TarjetaID", Tarjeta),//string.IsNullOrEmpty(Tarjeta) || Tarjeta == "" ? "1" : Tarjeta),
                new SqlParameter("@Cantidad", Cantidad),
                new SqlParameter("@CantidadPrevia", CantidadPrevia),
                new SqlParameter("@CantidadPostMovimiento", CantidadPostMovimiento),
                new SqlParameter("@UsuarioID", ClassVariables.UsuarioID)
            };

            TableGrid = Clases.ClassData.runDataTable("sp_invMovimientoInventarioDetalle", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
                {
                    new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError).ShowDialog();
                    ClassVariables.GetSetError = null;
                }

                if (TableGrid.Rows.Count > 0) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
                    AgregarModoGrid(TableGrid);

                if (Estado != "Modo Agregar" && Estado != "Modo Editar")
                    ClassControl.SetGridReadOnly(GridMain);
            })).Wait();

            if (cbxEstado.Text == "Cerrada" && (Estado == "Modo Agregar" || Estado == "Modo Editar"))
                Clases.ClassData.runSqlCommand(string.Format("update TinvArticulos set Cantidad = {0} where ArticuloID = {1}", CantidadPostMovimiento, ArticuloID)); //Aqui actualizamos la cantidad de existencia de los articulos.
            
            listSqlParameter.Clear();  //Limpiamos la lista de parametros.
        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtMovimientoID,cbxTipoMovimiento,dtpFechaMovimiento,cbxEstado
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                txtArticuloID, cbxEstado, dtpFechaMovimiento
            };

            List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
            {
                txtMovimientoID
            };

            if (Modo == null) //si no trae ningun modo entra el validador
            {
                if (Estado == "Modo Busqueda")
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, listaControles);
                else if (Estado == "Modo Agregar")
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, true, null);
                else
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, null);
            }
            else if (Modo == "Validador") //si el parametro modo es igual a validador ingresa.
                Lista = Clases.ClassControl.ValidadorControles(listaControlesValidar); //Este metodo se encarga de validar que cada unos de los controles que se les indica en la lista no se dejen vacios.

            listaControl.Clear(); //limpiamos ambas listas
            listaControles.Clear();
            listaControlesValidar.Clear();
        }

        void SetEnabledButton(String status) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        {

            Estado = status;
            lIconEstado.ToolTip = Estado;

            if (Estado != "Modo Agregar" && Estado != "Modo Editar") //Si el sistema se encuentra en modo consulta o busqueda entra el validador
            {
                BtnPrimerRegistro.IsEnabled = true;
                BtnAnteriorRegistro.IsEnabled = true;
                BtnProximoRegistro.IsEnabled = true;
                BtnUltimoRegistro.IsEnabled = true;
                BtnBuscar.IsEnabled = true;
                BtnImprimir.IsEnabled = true;
                BtnAgregar.IsEnabled = true;
                BtnEditar.IsEnabled = true;

                BtnCancelar.IsEnabled = false;
                BtnGuardar.IsEnabled = false;
                if (Estado == "Modo Consulta") //Si el estado es modo consulta enviamos a ejecutar otro metodo parametizado de forma especial
                {
                    SetControls(true, null, false);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
                }
                else //Si el estado es modo busqueda enviamos a ejecutar el mismo metodo parametizado de forma especial y cambiamos el estado de los botones
                {
                    BtnProximoRegistro.IsEnabled = false;
                    BtnAnteriorRegistro.IsEnabled = false;
                    BtnImprimir.IsEnabled = false;
                    BtnEditar.IsEnabled = false;
                    SetControls(false, null, false);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Search;
                }
            }
            else  //Si el sistema se encuentra en modo Agregar o Editar entra el validador
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                BtnUltimoRegistro.IsEnabled = false;

                BtnBuscar.IsEnabled = false;
                BtnImprimir.IsEnabled = false;

                BtnAgregar.IsEnabled = false;
                BtnEditar.IsEnabled = false;

                BtnCancelar.IsEnabled = true;
                BtnGuardar.IsEnabled = true;
                if (Estado == "Modo Agregar") //Si el estado es modo Agregar enviamos a ejecutar otro metodo parametizado de forma especial
                {
                    SetControls(false, null, false);
                    cbxEstado.SelectedIndex = 0;
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.AddThick;
                    txtMovimientoID.Text = (LastMovimientoID + 1).ToString();
                }
                else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
                {
                    SetControls(true, null, true);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                }
                txtMovimientoID.IsReadOnly = true;
                txtArticuloID.IsReadOnly = false;
            }
            if (Imprime == false)
                BtnImprimir.IsEnabled = Imprime;
            if (Agrega == false)
                BtnAgregar.IsEnabled = Agrega;
            if (Modifica == false)
                BtnEditar.IsEnabled = Modifica;

        }

        private void AgregarModoGrid(DataTable table = null, bool ModeEditar = false)
        {
            if (!ModeEditar)
            {
                if (table == null)
                {
                    reader = Clases.ClassData.runDataTable("select ArticuloID, TarjetaID as Tarjeta, '' as Nombre, CantidadPrevia, Cantidad, CantidadPostMovimiento from TinvMovimientoInventarioDetalle  where MovimientoID = '" + txtMovimientoID.Text + "' ", null, "CommandText");
                    GridMain.ItemsSource = reader;
                }
                else
                    GridMain.ItemsSource = table;
            }

            foreach (DevExpress.Xpf.Grid.GridColumn Col in GridMain.Columns)
            {
                switch (Col.HeaderCaption)
                {
                    case "Articulo ID":
                        Col.Visible = false;
                        Col.ReadOnly = true;
                        break;

                    case "Tarjeta":
                        Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(1, DevExpress.Xpf.Grid.GridColumnUnitType.Star);
                        Col.ReadOnly = true;
                        break;

                    case "Nombre":
                        Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(1, DevExpress.Xpf.Grid.GridColumnUnitType.Star);
                        Col.ReadOnly = true;
                        break;

                    case "Cantidad":
                        Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(1, DevExpress.Xpf.Grid.GridColumnUnitType.Star);
                        Col.EditSettings.DisplayFormat = "N";
                        Col.ReadOnly = false;
                        break;

                    case "Cantidad Previa":
                        Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(1, DevExpress.Xpf.Grid.GridColumnUnitType.Star);
                        Col.ReadOnly = true;
                        Col.EditSettings.DisplayFormat = "N";
                        break;

                    case "Cantidad Post Movimiento":
                        Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(1, DevExpress.Xpf.Grid.GridColumnUnitType.Star);
                        Col.ReadOnly = true;
                        Col.EditSettings.DisplayFormat = "N";
                        break;
                }
            }
        }

        private void TablaGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) && TablaGrid.FocusedColumn.HeaderCaption.ToString() == "Cantidad")
            {
                double Cantidad = Convert.ToDouble(GridMain.GetCellValue(TablaGrid.FocusedRowHandle, "Cantidad").ToString());
                double CantidadPrevia = Convert.ToDouble(GridMain.GetCellValue(TablaGrid.FocusedRowHandle, "CantidadPrevia"));

                if (cbxTipoMovimiento.Text == "Entrada de inventario")
                    GridMain.SetCellValue(TablaGrid.FocusedRowHandle, "CantidadPostMovimiento", (Cantidad + CantidadPrevia));
                else if (cbxTipoMovimiento.Text == "Salida de inventario")
                {
                    if ((CantidadPrevia - Cantidad) < 0 && SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Debe cambiar la cantidad ingresada; La cantidad no puede ser menor a 0"));
                        GridMain.SetCellValue(TablaGrid.FocusedRowHandle, "CantidadPostMovimiento", CantidadPrevia);
                        GridMain.SetCellValue(TablaGrid.FocusedRowHandle, "Cantidad", 0);
                        return;
                    }
                    else
                        GridMain.SetCellValue(TablaGrid.FocusedRowHandle, "CantidadPostMovimiento", (CantidadPrevia - Cantidad));
                }

                txtArticuloID.Focus();
            }
        }

        private bool ValidaCanSaverArticulo()
        {
            bool resultado = true;


            for (int i = 0; i < GridMain.VisibleRowCount; i++)
            {
                int rowHandle = GridMain.GetRowHandleByVisibleIndex(i);
                double Cantidad = Convert.ToDouble(GridMain.GetCellValue(rowHandle, "Cantidad").ToString());
                double CantidadPrevia = Convert.ToDouble(GridMain.GetCellValue(rowHandle, "CantidadPrevia"));

                if (cbxTipoMovimiento.Text == "Entrada de inventario")
                    GridMain.SetCellValue(rowHandle, "CantidadPostMovimiento", (Cantidad + CantidadPrevia));
                else if (cbxTipoMovimiento.Text == "Salida de inventario")
                {
                    if ((CantidadPrevia - Cantidad) < 0 && SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Debe cambiar la cantidad ingresada; La cantidad no puede ser menor a 0"));
                        GridMain.SetCellValue(rowHandle, "CantidadPostMovimiento", CantidadPrevia);
                        GridMain.SetCellValue(rowHandle, "Cantidad", 0);
                        resultado = false;
                        break;
                    }
                    else
                        GridMain.SetCellValue(rowHandle, "CantidadPostMovimiento", (CantidadPrevia - Cantidad));
                }
            }
            return resultado;
        }

        private bool ValidaArticulosGrid(string ReaderValue = null/*, double Cantidad = 0*/)
        {
            bool resultado = true;

            for (int i = 0; i < GridMain.VisibleRowCount; i++)
            {
                int rowHandle = GridMain.GetRowHandleByVisibleIndex(i);

                string cellValue = GridMain.GetCellValue(rowHandle, "Tarjeta").ToString();

                string rowTarjeta = reader.Rows[0]["Tarjeta"].ToString();

                if (ReaderValue != null)
                    rowTarjeta = ReaderValue;

                if (cellValue == rowTarjeta || cellValue.Contains(rowTarjeta))
                {
                    TablaGrid.FocusedRowHandle = rowHandle;

                    if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Ya se habia agregado este articulo"));

                    resultado = false;
                    break;
                }
            }

            return resultado;
        }

        private void txtArticuloID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado == "Modo Agregar" && e.Key == Key.Enter)
            {
                reader = Clases.ClassData.runDataTable("Select a.ArticuloID ,a.Tarjeta, a.Nombre,a.Cantidad as CantidadPrevia, 0 as Cantidad, 0 as CantidadPostMovimiento " +
                            " from TinvArticulos a where a.Tarjeta = '" + txtArticuloID.Text + "' or a.Nombre like '%" + txtArticuloID.Text + "%'" +
                            " order by a.ArticuloID", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

                List<String> ColumCaption = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
                    {
                        "Tarjeta", "Nombre", "Cantidad Previa"
                    };

                if (reader.Rows.Count == 1)
                {
                    if (ValidaArticulosGrid(reader.Rows[0]["Tarjeta"].ToString()/*, (double)ReadTable.Rows[0]["Cantidad"]) == false*/))
                    {
                        TablaGrid.AddNewRow();

                        int newRowHandle = DevExpress.Xpf.Grid.DataControlBase.NewItemRowHandle;

                        GridMain.SetCellValue(newRowHandle, "ArticuloID", reader.Rows[0]["ArticuloID"]);
                        GridMain.SetCellValue(newRowHandle, "Tarjeta", reader.Rows[0]["Tarjeta"]);
                        GridMain.SetCellValue(newRowHandle, "Nombre", reader.Rows[0]["Nombre"]);
                        GridMain.SetCellValue(newRowHandle, "CantidadPrevia", reader.Rows[0]["CantidadPrevia"]);
                        GridMain.SetCellValue(newRowHandle, "Cantidad", reader.Rows[0]["Cantidad"]);
                        GridMain.SetCellValue(newRowHandle, "CantidadPostMovimiento", reader.Rows[0]["CantidadPostMovimiento"]);

                        ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                        GridMain.CurrentColumn = TablaGrid.VisibleColumns[3];
                        TablaGrid.FocusedRowHandle = newRowHandle;
                        GridMain.SelectItem(newRowHandle);
                        TablaGrid.FocusedColumn = TablaGrid.VisibleColumns[3];
                    }

                    txtArticuloID.Clear();
                }


                else if (reader.Rows.Count > 1)
                {

                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, reader, ColumCaption);
                    frm.ShowDialog();

                    string ArticuloID = "";
                    string Tarjeta = "";
                    string Nombre = null;
                    int CantidadPrevia = 0;
                    int CantidadPostMovimiento = 0;

                    if (frm.GridMuestra.SelectedItem != null)
                    {
                        DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                        ArticuloID = item.Row.ItemArray[0].ToString();
                        Tarjeta = item.Row.ItemArray[1].ToString();
                        Nombre = item.Row.ItemArray[2].ToString();
                        CantidadPrevia = Convert.ToInt32(item.Row.ItemArray[3].ToString());
                        frm.Close();

                        if (ValidaArticulosGrid(Tarjeta))
                        {
                            TablaGrid.AddNewRow();

                            int newRowHandle = DevExpress.Xpf.Grid.DataControlBase.NewItemRowHandle;

                            GridMain.SetCellValue(newRowHandle, "ArticuloID", ArticuloID);
                            GridMain.SetCellValue(newRowHandle, "Tarjeta", Tarjeta);
                            GridMain.SetCellValue(newRowHandle, "Nombre", Nombre);
                            GridMain.SetCellValue(newRowHandle, "Cantidad", 0);
                            GridMain.SetCellValue(newRowHandle, "CantidadPrevia", CantidadPrevia);
                            GridMain.SetCellValue(newRowHandle, "CantidadPostMovimiento", CantidadPostMovimiento);

                            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                            GridMain.CurrentColumn = TablaGrid.VisibleColumns[3];
                            TablaGrid.FocusedRowHandle = newRowHandle;
                            GridMain.SelectItem(newRowHandle);
                            TablaGrid.FocusedColumn = TablaGrid.VisibleColumns[3];
                        }
                    }
                    else if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("No se selecciono ningun articulo"));

                    txtArticuloID.Clear();
                }


                else if (reader.Rows.Count < 1 && SnackbarThree.MessageQueue is { } messageQueue)
                    Task.Factory.StartNew(() => messageQueue.Enqueue("No se encontro el articulo"));

            }
        }

        private void TablaGrid_FocusedColumnChanged(object sender, DevExpress.Xpf.Grid.FocusedColumnChangedEventArgs e)
        {
            if (TablaGrid.FocusedColumn != null && TablaGrid.FocusedColumn.FieldName.ToString() == "CantidadPostMovimiento")
                TablaGrid.FocusedColumn = TablaGrid.VisibleColumns[3];
        }
    }
}
