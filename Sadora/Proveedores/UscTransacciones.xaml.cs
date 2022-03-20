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

namespace Sadora.Proveedores
{
    /// <summary>
    /// Lógica de interacción para UscTransacciones.xaml
    /// </summary>
    public partial class UscTransacciones : UserControl
    {
        public UscTransacciones()
        {
            InitializeComponent();
            Name = "UscTransacciones";
        }

        bool Imprime;
        bool Agrega;
        bool Modifica;

        bool Inicializador = false;
        DataTable tabla;
        SqlDataReader reader;
        string Estado;
        string Lista;
        int TransaccionID;
        int LastTransaccionID;
        string last;

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            Inicializador = true;
        }

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
               /*txtMontoGravado,*/txtMontoGravado
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
               /*txtMontoGravado,*/txtMontoGravado
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                TransaccionID = Convert.ToInt32(txtTransaccionID.Text) - 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }


            if (TransaccionID <= 1)
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                setDatos(0, "1");
            }
            else
            {
                setDatos(0, TransaccionID.ToString());
            }
        }

        private void BtnProximoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               /*txtMontoGravado,*/txtMontoGravado
            };
            ClassControl.ClearControl(listaControl);
            SetEnabledButton("Modo Consulta");
            try
            {
                TransaccionID = Convert.ToInt32(txtTransaccionID.Text) + 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }

            if (TransaccionID >= LastTransaccionID)
            {
                BtnUltimoRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                setDatos(0, LastTransaccionID.ToString());
            }
            else
            {
                setDatos(0, TransaccionID.ToString());
            }
        }

        private void BtnUltimoRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               /*txtMontoGravado,*/txtMontoGravado
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
                last = txtTransaccionID.Text;
                SetEnabledButton("Modo Busqueda");
            }
            else if (Estado == "Modo Busqueda")
            {
                List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
                {
                    txtProveedorID,tbxProveedorID//,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular//,cActivar
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
                        txtTransaccionID.Text = item.Row.ItemArray[0].ToString();
                        setDatos(0, txtTransaccionID.Text);
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
                    //List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
                    //{
                    //   txtMontoGravado,txtMontoGravado
                    //};
                    //ClassControl.ClearControl(listaControl);
                    //setDatos(0, last);
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

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Editar");
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
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(Lista);
                frm.ShowDialog();
            }
            else
            {
                //SqlDataReader tabla = ClassControl.getDatosCedula(txtMontoGravado.Text);
                //if (tabla != null)
                //{
                //    tabla.Close();
                //    tabla.Dispose();
                if (Estado == "Modo Editar")
                {
                    setDatos(2, null);
                }
                else
                {
                    setDatos(1, null);
                }
                SetEnabledButton("Modo Consulta");
                setDatos(0, txtTransaccionID.Text);
                //this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                //}

            }

        }

        private void btnProveedorID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("Select ProveedorID,RNC,Nombre,Representante,Activo from TsupProveedores", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtProveedorID.Text = item.Row.ItemArray[0].ToString();

                    ClassControl.setValidador("select * from TsupProveedores where ProveedorID =", txtProveedorID, tbxProveedorID);
                }

            }
        }

        //private void txtMontoGravado_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            reader = ClassControl.getDatosCedula(txtMontoGravado.Text);
        //            if (reader != null)
        //            {
        //                if (reader.HasRows)
        //                {
        //                    if (reader.Read())
        //                    {
        //                        txtMontoGravado.Text = reader["Nomenclaturas"].ToString() + " " + reader["Apellidos"].ToString();
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
        //            }
        //            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        private void txtTransaccionID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void cbxTipoTransaccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void dtpFechaTransaccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    cbxEstado.Focus();                    //((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void cbxEstado_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtProveedorID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtProveedorID.Text != "")
                    {
                        ClassControl.setValidador("select Nombre from TsupProveedores where ProveedorID =", txtProveedorID, tbxProveedorID);
                    }
                    else
                    {
                        txtProveedorID.Text = 0.ToString();
                        ClassControl.setValidador("select Nombre from TsupProveedores where ProveedorID =", txtProveedorID, tbxProveedorID);
                    }
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtProveedorID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtMontoExcento_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    if (txtMontoExcento.Text != "")
                    {
                        txtITBIS.IsReadOnly = true;
                        txtMontoGravado.IsReadOnly = true;
                        BtnGuardar.Focus();
                    }
                    else
                    {
                        ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                    }
                }
            }
        }

        private void txtMontoExcento_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Estado != "Modo Consulta" && Convert.ToDouble(txtMontoGravado.Text) > 0 && Convert.ToDouble(txtITBIS.Text) > 0)
                {
                    txtMontoExcento.Text = "0";
                    txtMontoExcento.IsReadOnly = true;
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
                else
                {
                    txtMontoExcento.IsReadOnly = false;
                    txtMontoGravado.IsReadOnly = true;
                }
            }
            catch //(Exception)
            {
                if (Estado != "Modo Consulta" && txtMontoGravado.Text != "" && txtITBIS.Text != "")
                {
                    txtMontoExcento.Text = "0";
                    txtMontoExcento.IsReadOnly = true;
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
                else
                {
                    txtMontoExcento.IsReadOnly = false;
                    //txtMontoGravado.IsReadOnly = true;
                }
            }
        }

        private void txtMontoExcento_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtMontoGravado_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                if (e.Key == Key.Enter)
                {
                    txtMontoExcento.IsReadOnly = false;
                    try
                    {
                        if (Convert.ToDouble(txtMontoGravado.Text) >= 0)
                        {
                            txtITBIS.Text = (Convert.ToInt32(txtMontoGravado.Text) * 0.18).ToString();
                            txtMontoExcento.IsReadOnly = true;
                        }
                        else
                        {
                            txtITBIS.Text = null;
                        }
                    }
                    catch
                    {
                        txtITBIS.Text = null;
                        txtMontoExcento.Focus();
                        txtMontoGravado.IsReadOnly = false;
                    }
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
                }
            }
        }

        private void txtMontoGravado_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        void setDatos(int Flag, string Transaccion) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            if (Transaccion == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
            {
                if (txtTransaccionID.Text == "")
                {
                    TransaccionID = 0;
                }
                else
                {
                    try
                    {
                        TransaccionID = Convert.ToInt32(txtTransaccionID.Text);
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del Proveedor
                    }
                }
            }
            else //Si pasamos un Proveedor, lo convertimos actualizamos la variable Proveedor principal
            {
                TransaccionID = Convert.ToInt32(Transaccion);
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el Nomenclatura en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@TransaccionID",txtTransaccionID.Text),
                new SqlParameter("@TipoTransaccion",cbxTipoTransaccion.Text),
                new SqlParameter("@ProveedorID",txtProveedorID.Text),
                new SqlParameter("@Fecha",dtpFechaTransaccion.Text),
                new SqlParameter("@MontoExcento",txtMontoExcento.Text),
                new SqlParameter("@MontoGravado",txtMontoGravado.Text),
                new SqlParameter("@ITBIS",txtITBIS.Text),
                new SqlParameter("@Estado",cbxEstado.Text),
                new SqlParameter("@UsuarioID",ClassVariables.UsuarioID)
            };

            tabla = Clases.ClassData.runDataTable("sp_supTransacciones", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                frm.ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
            {
                txtTransaccionID.Text = tabla.Rows[0]["TransaccionID"].ToString();
                cbxTipoTransaccion.Text = tabla.Rows[0]["TipoTransaccion"].ToString();
                txtProveedorID.Text = tabla.Rows[0]["ProveedorID"].ToString();
                dtpFechaTransaccion.Text = tabla.Rows[0]["Fecha"].ToString();
                txtMontoExcento.Text = tabla.Rows[0]["MontoExcento"].ToString();
                txtMontoGravado.Text = tabla.Rows[0]["MontoGravado"].ToString();
                txtITBIS.Text = tabla.Rows[0]["ITBIS"].ToString();
                cbxEstado.Text = tabla.Rows[0]["Estado"].ToString();

                if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
                {
                    try
                    {
                        LastTransaccionID = Convert.ToInt32(txtTransaccionID.Text); //intentamos convertir el id del Proveedor
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error al intentar convertirlo lo enviamos
                    }
                }
                ClassControl.setValidador("select * from TsupProveedores where ProveedorID =", txtProveedorID, tbxProveedorID); //ejecutamos el metodo validador con el campo seleccionado para que lo busque y muestre una vez se guarde el registro
            }
            //else
            //{
            //    if (SnackbarThree.MessageQueue is { } messageQueue)
            //    {
            //        var message = "No se encontraron datos";
            //        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            //    }
            //}
            listSqlParameter.Clear(); //Limpiamos la lista de parametros.
        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtTransaccionID,txtMontoGravado,txtMontoExcento,txtProveedorID,cbxTipoTransaccion,dtpFechaTransaccion,cbxEstado
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                txtProveedorID,tbxProveedorID//,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular//,cActivar
            };

            List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
            {
                txtTransaccionID,txtMontoGravado,txtProveedorID,tbxProveedorID//,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular
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
                    txtTransaccionID.Text = (LastTransaccionID + 1).ToString();
                    //txtMontoGravado.Focus();
                }
                else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
                {
                    SetControls(true, null, true);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                }
                txtTransaccionID.IsReadOnly = true;
            }
            if (Imprime == false)
            {
                BtnImprimir.IsEnabled = Imprime;
            }
            if (Agrega == false)
            {
                BtnAgregar.IsEnabled = Agrega;
            }
            if (Modifica == false)
            {
                BtnEditar.IsEnabled = Modifica;
            }
        }

    }
}
