using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

namespace Sadora.Inventario
{
    /// <summary>
    /// Lógica de interacción para UscArticulos.xaml
    /// </summary>
    public partial class UscArticulos : UserControl
    {
        public UscArticulos()
        {
            InitializeComponent();
            Name = "UscArticulos";
        }

        bool Imprime;
        bool Agrega;
        bool Modifica;

        bool Inicializador = false;
        DataTable tabla;
        SqlDataReader reader;
        DataTable TableGrid;
        string Estado;
        string Lista;
        int ArticuloID;
        int LastArticuloID;
        string last;

        List<ClassVariables> ListVariables = new List<ClassVariables>();

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
               txtNombre, txtTarjeta
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
               txtNombre, txtTarjeta
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                ArticuloID = Convert.ToInt32(txtArticuloID.Text) - 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }


            if (ArticuloID <= 1)
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                setDatos(0, "1");
            }
            else
            {
                setDatos(0, ArticuloID.ToString());
            }
        }

        private void BtnProximoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre, txtTarjeta
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                ArticuloID = Convert.ToInt32(txtArticuloID.Text) + 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }

            if (ArticuloID >= LastArticuloID)
            {
                BtnUltimoRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                setDatos(0, LastArticuloID.ToString());
            }
            else
            {
                setDatos(0, ArticuloID.ToString());
            }
        }

        private void BtnUltimoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre, txtTarjeta
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
                last = txtArticuloID.Text;
                SetEnabledButton("Modo Busqueda");
            }
            else if (Estado == "Modo Busqueda")
            {
                List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
                {
                    txtModelo,txtDepartamentoID,tbxDepartamentoID,txtMarcaID,tbxMarcaID,txtCosto,txtPrecio,txtDescripcion,txtCantidad
                };
                Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

                setDatos(0, null);

                List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
                {
                    "Articulo ID","Tarjeta","Nombre","Descripcion","Modelo","Costo","Precio"
                };

                SetEnabledButton("Modo Consulta");

                if (tabla.Rows.Count > 1)
                {
                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
                    frm.ShowDialog();

                    if (frm.GridMuestra.SelectedItem != null)
                    {
                        DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                        txtArticuloID.Text = item.Row.ItemArray[0].ToString();
                        setDatos(0, txtArticuloID.Text);
                        frm.Close();
                    }
                    else
                    {
                        setDatos(0, last);
                    }
                }
                else if (tabla.Rows.Count < 1)
                {
                    BtnProximoRegistro.IsEnabled = false;
                    BtnAnteriorRegistro.IsEnabled = false;
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        var message = "No se encontraron datos";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
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
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e) => SetEnabledButton("Modo Editar");

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Consulta");
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            SetControls(false, "Validador", false);
            if (Lista != "Debe Completar los Campos: ")
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(Lista);
                frm.ShowDialog();
            }
            else
            {
                if (Estado == "Modo Editar")
                {
                    setDatos(2, null);
                }
                else
                {
                    setDatos(1, null);
                }
                SetEnabledButton("Modo Consulta");
                setDatos(0, txtArticuloID.Text);
            }

        }

        private void txtCosto_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtArticuloID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDescripcion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtPrecio_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtPrecio_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtModelo_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDireccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtCorreoElectronico_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtTelefono_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtTelefono_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtCelular_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtCelular_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void cActivar_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((CheckBox)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void cbxTipoArticulo_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void cbxAuxiliar_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void dtpFechaArticulo_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    lDescripcion.Focus();
                }
            }
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDisponibles_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtTarjeta_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDepartamentoID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtDepartamentoID.Text == "")
                    {
                        txtDepartamentoID.Text = 0.ToString();
                    }
                    ClassControl.setValidador("select Nombre from TinvDepartamento where DepartamentoID = ", txtDepartamentoID, tbxDepartamentoID);
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtDepartamentoID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void btnDepartamentoID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("Select DepartamentoID,Nombre from TinvDepartamento", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtDepartamentoID.Text = item.Row.ItemArray[0].ToString();

                    ClassControl.setValidador("select Nombre from TinvDepartamento where DepartamentoID =", txtDepartamentoID, tbxDepartamentoID);
                }

            }
        }

        private void btnMarcaID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("select MarcaID,Nombre from TinvMarca", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtMarcaID.Text = item.Row.ItemArray[0].ToString();

                    ClassControl.setValidador("select Nombre from TinvMarca where MarcaID = ", txtMarcaID, tbxMarcaID);
                }

            }
        }

        private void txtMarcaID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtMarcaID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtMarcaID.Text == "")
                    {
                        txtMarcaID.Text = 0.ToString();
                    }
                    ClassControl.setValidador("select Nombre from TinvMarca where MarcaID =", txtMarcaID, tbxMarcaID);
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }


        private void txtClaseID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtClaseID.Text == "")
                    {
                        txtClaseID.Text = 0.ToString();
                    }
                    ClassControl.setValidador("select Nombre from TinvClaseArticulos where ClaseID =", txtClaseID, tbxClaseID);
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtClaseID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void btnClaseID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("select ClaseID, Nombre, Porcentaje from TinvClaseArticulos", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtClaseID.Text = item.Row.ItemArray[0].ToString();

                    ClassControl.setValidador("select Nombre from TinvClaseArticulos where ClaseID =", txtClaseID, tbxClaseID);
                }

            }
        }

        void setDatos(int Flag, string Articulo) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            if (Articulo == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
            {
                if (txtArticuloID.Text == "")
                {
                    ArticuloID = 0;
                }
                else
                {
                    try
                    {
                        ArticuloID = Convert.ToInt32(txtArticuloID.Text);
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del Proveedor
                    }
                }
            }
            else //Si pasamos un Proveedor, lo convertimos actualizamos la variable Proveedor principal
            {
                ArticuloID = Convert.ToInt32(Articulo);
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el Descripcion en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@ArticuloID",ArticuloID),
                new SqlParameter("@Nombre",txtNombre.Text),
                new SqlParameter("@Descripcion",txtDescripcion.Text),
                new SqlParameter("@Modelo",txtModelo.Text),
                new SqlParameter("@ClaseArticuloID",txtClaseID.Text),
                new SqlParameter("@DepartamentoID",txtDepartamentoID.Text),
                new SqlParameter("@MarcaID",txtMarcaID.Text),
                new SqlParameter("@Tarjeta",txtTarjeta.Text),
                new SqlParameter("@Costo",txtCosto.Text),
                new SqlParameter("@Precio",txtPrecio.Text),
                new SqlParameter("@UsuarioID",ClassVariables.UsuarioID),
                new SqlParameter("@Cantidad",txtCantidad.Text)
            };

            tabla = Clases.ClassData.runDataTable("sp_invArticulos", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la Articulo digase, consulta, agregar,editar,eliminar en una tabla.

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                frm.ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
            {
                txtArticuloID.Text = tabla.Rows[0]["ArticuloID"].ToString();
                txtNombre.Text = tabla.Rows[0]["Nombre"].ToString();
                txtDescripcion.Text = tabla.Rows[0]["Descripcion"].ToString();
                txtModelo.Text = tabla.Rows[0]["Modelo"].ToString();
                txtClaseID.Text = tabla.Rows[0]["ClaseArticuloID"].ToString();
                txtDepartamentoID.Text = tabla.Rows[0]["DepartamentoID"].ToString();
                txtMarcaID.Text = tabla.Rows[0]["MarcaID"].ToString();
                txtTarjeta.Text = tabla.Rows[0]["Tarjeta"].ToString();
                txtCosto.Text = tabla.Rows[0]["Costo"].ToString();
                txtPrecio.Text = tabla.Rows[0]["Precio"].ToString();
                txtCantidad.Text = tabla.Rows[0]["Cantidad"].ToString();

                if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
                {
                    try
                    {
                        LastArticuloID = Convert.ToInt32(txtArticuloID.Text); //intentamos convertir el id del Proveedor
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error al intentar convertirlo lo enviamos
                    }
                }
                ClassControl.setValidador("select Nombre from TinvDepartamento where DepartamentoID = ", txtDepartamentoID, tbxDepartamentoID);
                ClassControl.setValidador("select Nombre from TinvMarca where MarcaID = ", txtMarcaID, tbxMarcaID);
                ClassControl.setValidador("select Nombre from TinvClaseArticulos where ClaseID =", txtClaseID, tbxClaseID);

            }
            listSqlParameter.Clear(); //Limpiamos la lista de parametros.


            if (Estado == "Modo Consulta")
            {
                GridMain.ItemsSource = null;
                setDatosGrid(0);
                if (GridMain.ItemsSource != null && ((DataTable)GridMain.ItemsSource).Rows.Count > 0)
                {
                    GridMain.Columns["RowID"].Visible = false;
                    GridMain.Columns["ArticuloID"].Visible = false;
                    GridMain.Columns["UsuarioID"].Visible = false;

                    TablaGrid.AutoWidth = true;
                }

            }
            else if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                GridMain.View.MoveFirstRow();
                for (int i = 0; i < GridMain.VisibleRowCount; i++)
                {
                    setDatosGrid(1);
                    GridMainMasivo.View.MoveFirstRow();
                    for (int j = 0; j < GridMainMasivo.VisibleRowCount; j++)
                    {
                        if (!(bool)GridMainMasivo.GetFocusedRowCellValue("Alta"))
                        {
                            GridMainMasivo.View.MoveNextRow();
                            continue;
                        }

                        string ArticuloMasivo = GridMainMasivo.GetFocusedRowCellValue("ArticuloID").ToString();
                        setDatosGrid(1, ArticuloMasivo);
                        GridMainMasivo.View.MoveNextRow();
                    }

                    GridMain.View.MoveNextRow();
                }

            }

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError).ShowDialog();
                ClassVariables.GetSetError = null;
            }
        }

        void setDatosGrid(int Flag, string ArticuloID = null) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            string NombreServicio = "";
            double Precio = 0;
            bool Alta = false;

            if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                NombreServicio = GridMain.GetFocusedRowCellValue("NombreServicio").ToString();
                Precio = (double)GridMain.GetFocusedRowCellValue("Precio");
                Alta = (bool)GridMain.GetFocusedRowCellValue("Alta");
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el NCF en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@ArticuloID", ArticuloID == null ? txtArticuloID.Text : ArticuloID),
                new SqlParameter("@NombreServicio", NombreServicio),
                new SqlParameter("@Precio", Precio),
                new SqlParameter("@Alta", Alta),
                new SqlParameter("@UsuarioID", ClassVariables.UsuarioID)
            };

            TableGrid = Clases.ClassData.runDataTable("sp_invServicioArticulos", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                frm.ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (TableGrid.Rows.Count > 0) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
                AgregarModoGrid(GridMain, TablaGrid, TableGrid);


            //else
            //{
            //    GridMain.ItemsSource = null;
            //}

            List<String> listaColumnas = new List<String>() //Estos son los controles que seran controlados, readonly, enable.
            {
                //"Visualiza","Agrega","Modifica","Imprime","Anula"
            };

            if (Estado == "Modo Agregar" && Estado == "Modo Editar")
            {
                //Clases.ClassControl.SetGridReadOnly(GridMain, listaColumnas, false);
            }
            else
            {
                ClassControl.SetGridReadOnly(GridMain);
            }

            listSqlParameter.Clear(); listaColumnas.Clear(); //Limpiamos la lista de parametros.
        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtArticuloID,txtTarjeta,txtNombre,txtModelo,txtMarcaID,txtDepartamentoID,txtCosto,txtPrecio,txtDescripcion,txtCantidad
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                txtModelo,txtMarcaID,tbxMarcaID,txtDepartamentoID,tbxDepartamentoID,txtCosto,txtPrecio,txtDescripcion,txtCantidad
            };

            List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
            {
                txtArticuloID,txtTarjeta,txtNombre,txtModelo,txtMarcaID,tbxMarcaID,txtDepartamentoID,tbxDepartamentoID,txtCosto,txtPrecio,txtCantidad
            };

            if (Modo == null) //si no trae ningun modo entra el validador
            {
                if (Estado == "Modo Busqueda")
                {
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, listaControles);
                }
                else if (Estado == "Modo Agregar")
                {
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, true, null);
                }
                else
                {
                    Clases.ClassControl.ActivadorControlesReadonly(listaControl, Habilitador, Editando, false, null);
                }
            }
            else if (Modo == "Validador") //si el parametro modo es igual a validador ingresa.
            {
                Lista = Clases.ClassControl.ValidadorControles(listaControlesValidar); //Este metodo se encarga de validar que cada unos de los controles que se les indica en la lista no se dejen vacios.
            }
            listaControl.Clear(); //limpiamos ambas listas
            listaControles.Clear();
            listaControlesValidar.Clear();
        }

        void SetEnabledButton(String status) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        {

            Estado = status;
            lIconEstado.ToolTip = Estado;
            BorderCantidad.IsEnabled = true;

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
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.AddThick;
                    txtArticuloID.Text = (LastArticuloID + 1).ToString();
                }
                else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
                {
                    SetControls(true, null, true);
                    BorderCantidad.IsEnabled = false;
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                }
                txtArticuloID.IsReadOnly = true;
            }
            if (Imprime == false)
                BtnImprimir.IsEnabled = Imprime;
            if (Agrega == false)
                BtnAgregar.IsEnabled = Agrega;
            if (Modifica == false)
                BtnEditar.IsEnabled = Modifica;
        }

        private void txtCantidad_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtCantidad_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);


        private void BtnConfiguracionServicios_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta" && Estado != "Modo Busqueda")
            {
                MiniDialogo.IsOpen = true;
                AgregarModoGrid(GridMain, TablaGrid);
            }
            else if (Estado == "Modo Consulta")
            {
                MiniDialogo.IsOpen = true;
                TablaGrid.AllowEditing = false;
            }
        }

        private void ButtonCerrar_Click(object sender, RoutedEventArgs e)
        {
            GridMain.ItemsSource = null;
            MiniDialogo.IsOpen = false;
            TablaGrid.AllowEditing = true;
        }

        private void AgregarModoGrid(DevExpress.Xpf.Grid.GridControl Grid, DevExpress.Xpf.Grid.TableView tableView, DataTable table = null, bool IsServiciosMasivos = false)
        {
            if (table == null)
            {
                DataTable reader;

                if (!IsServiciosMasivos)
                    reader = Clases.ClassData.runDataTable("SELECT [NombreServicio], [Precio], [Alta] FROM[Sadora].[dbo].[TinvServicioArticulos] where ArticuloID = " + txtArticuloID.Text, null, "CommandText");
                else
                    reader = Clases.ClassData.runDataTable("declare @Alta bit = 0; select ArticuloID, Tarjeta, Nombre, Descripcion, precio, @Alta as Alta from TinvArticulos where ArticuloID <> " + txtArticuloID.Text /*+ " and HaveServices <> 1"*/, null, "CommandText");

                Grid.ItemsSource = reader;
                tableView.AutoWidth = true;
                if (!IsServiciosMasivos)
                    tableView.AddNewRow();
            }
            else
                Grid.ItemsSource = table;

            if (!IsServiciosMasivos)
                foreach (DevExpress.Xpf.Grid.GridColumn Col in Grid.Columns)
                {
                    switch (Col.HeaderCaption)
                    {
                        case "Precio":
                            Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(50, DevExpress.Xpf.Grid.GridColumnUnitType.Pixel);
                            break;

                        case "Alta":
                            Col.Width = new DevExpress.Xpf.Grid.GridColumnWidth(20, DevExpress.Xpf.Grid.GridColumnUnitType.Pixel);
                            break;
                    }
                }
            
        }

        private void TablaGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter && TablaGrid.FocusedColumn.HeaderCaption.ToString() == "Alta")
                {
                    if (!ValCampoIncompleto())
                        TablaGrid.AddNewRow();
                }
                else if (e.Key == Key.F5 && TablaGrid.SelectedRows.Count == 1)
                {
                    new Administracion.FrmValidarAccion("Esta seguro que desea eliminar esta fila?").ShowDialog(); ;
                    if (ClassVariables.ValidarAccion == true)
                        TablaGrid.DeleteRow(TablaGrid.FocusedRowHandle);
                }
            }
        }

        bool ValCampoIncompleto()
        {
            string Result = "";

            List<int> rowHandles = new List<int>();
            for (int i = 0; i < GridMain.VisibleRowCount; i++)
            {
                foreach (var column in GridMain.Columns)
                    Result += string.IsNullOrWhiteSpace(GridMain.GetCellValue(GridMain.GetRowHandleByVisibleIndex(i), column).ToString()).ToString() + " ,";

                if (Result.Contains("True"))
                    break;
            }

            if (Result.Contains("True"))
            {
                if (SnackbarThreeDialog.MessageQueue is { } messageQueue)
                    Task.Factory.StartNew(() => messageQueue.Enqueue("Debe llenar todos los campos"));
                return true;
            }
            else
                return false;

        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (!ValCampoIncompleto() && Estado != "Modo Consulta")
            {
                GridMainMasivo.ItemsSource = null;
                MiniDialogo.IsOpen = false;
            }
        }

        private void btnAsignacionMasiva_Click(object sender, RoutedEventArgs e)
        {
            if (!ValCampoIncompleto() && Estado != "Modo Consulta")
            {
                ServiciosMasivosDialogo.IsOpen = true;
                MiniDialogo.IsOpen = false;
                AgregarModoGrid(GridMainMasivo, TablaGridMasivo, null, true);
            }
            else if (Estado == "Modo Consulta")
            {
                ServiciosMasivosDialogo.IsOpen = true;
                TablaGridMasivo.AllowEditing = false;
            }
        }

        private void ButtonCerrarMasivo_Click(object sender, RoutedEventArgs e)
        {
            ServiciosMasivosDialogo.IsOpen = false;
            TablaGridMasivo.AllowEditing = true;
        }

        private void btnAceptarMasivo_Click(object sender, RoutedEventArgs e) => ServiciosMasivosDialogo.IsOpen = false;

    }
}
