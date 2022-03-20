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

namespace Sadora.Clientes
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

        bool Imprime, Agrega, Modifica, Inicializador = false;
        DataTable tabla;
        SqlDataReader reader;
        string Estado, Lista, last;
        int TransaccionID, LastTransaccionID;


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
                setDatos(0, TransaccionID.ToString());
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
                setDatos(0, TransaccionID.ToString());
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
                    txtClienteID,tbxClienteID//,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular//,cActivar
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
                        setDatos(0, last);
                }
                else if (tabla.Rows.Count < 1)
                {
                    BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = false;
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("No se encontraron datos"));
                }
            }
        }

        private void BtnImprimir_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            DevExpress.Xpf.Printing.PrintHelper.ShowPrintPreview(this, new Reportes.RpCuentasXCobrar(tabla)).WindowState = WindowState.Maximized;
            Cursor = Cursors.Arrow;
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            SetEnabledButton("Modo Agregar");
            cbxTipoTransaccion.SelectedIndex = 0;
            dtpFechaTransaccion.Text = DateTime.Now.ToShortDateString();
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e) => SetEnabledButton("Modo Editar");

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            SetEnabledButton("Modo Consulta");
            this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (txtMontoGravado.Text == "0" && txtMontoExcento.Text == "0")
                txtMontoGravado.Text = txtMontoExcento.Text = null;

            SetControls(false, "Validador", false);

            if (Lista != "Debe Completar los Campos: ")
                new Administracion.FrmCompletarCamposHost(Lista).ShowDialog();
            else
            {
                if (Estado == "Modo Editar")
                    setDatos(2, null);
                else
                    setDatos(1, null);

                SetEnabledButton("Modo Consulta");
                setDatos(0, txtTransaccionID.Text);
            }

        }

        private void txtTransaccionID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtFactura_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtObservacion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void cbxTipoTransaccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void dtpFechaTransaccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                cbxEstado.Focus();
        }

        private void cbxEstado_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtClienteID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
            {
                if (String.IsNullOrWhiteSpace(txtClienteID.Text))
                    txtClienteID.Text = 0.ToString();

                ClassControl.setValidador("select Nombre from TcliClientes where ClienteID =", txtClienteID, tbxClienteID);
                ClassControl.setValidador("select DiasCredito Nombre from TcliClientes where ClienteID =", txtClienteID, txtDiasCredito);
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
            }
        }

        private void txtClienteID_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

        private void btnClienteID_Click(object sender, RoutedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost("Select ClienteID, RNC, Nombre, Representante, DiasCredito, Direccion,Activo from TcliClientes where Activo = 1", null);
                frm.ShowDialog();

                if (frm.GridMuestra.SelectedItem != null)
                {
                    DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                    txtClienteID.Text = item.Row.ItemArray[0].ToString();
                    tbxClienteID.Text = item.Row.ItemArray[2].ToString();
                    txtDiasCredito.Text = item.Row.ItemArray[4].ToString();
                }

            }
        }

        private void txtMontoExcento_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Estado != "Modo Consulta" && txtMontoGravado.Text != "0" && txtMontoExcento.Text != "0")
            {
                var Result = Convert.ToDouble(((TextBox)sender).Text == "" ? "0" : ((TextBox)sender).Text);

                if (Result > 0)
                {
                    txtMontoGravado.IsEnabled = txtITBIS.IsEnabled = false;
                    txtMontoGravado.Text = txtITBIS.Text = "0";
                }
                else
                {
                    txtMontoGravado.IsEnabled = txtITBIS.IsEnabled = true;
                    txtMontoExcento.Text = "0";
                }
            }
            else if (Estado != "Modo Consulta" && txtMontoExcento.Text != "0")
            {
                var Result = Convert.ToDouble(((TextBox)sender).Text == "" ? "0" : ((TextBox)sender).Text);

                if (Result > 0)
                {
                    txtMontoGravado.IsEnabled = txtITBIS.IsEnabled = false;
                    txtMontoGravado.Text = txtITBIS.Text = "0";
                }
                else
                {
                    txtMontoGravado.IsEnabled = txtITBIS.IsEnabled = true;
                    txtMontoExcento.Text = "0";
                }
            }
        }

        private void txtDiasCredito_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta" && e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtDiasCredito_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

        private void txtMontoGravado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Estado != "Modo Consulta")
            {
                var Result = Convert.ToDouble(((TextBox)sender).Text == "" ? "0" : ((TextBox)sender).Text);

                if (Result > 0)
                {
                    txtMontoExcento.IsEnabled = false;
                    txtMontoExcento.Text = "0";
                    txtITBIS.Text = (Result * 0.18).ToString();
                }
                else
                {
                    txtMontoExcento.IsEnabled = true;
                    txtMontoGravado.Text = txtITBIS.Text = "0";
                }
            }
        }

        private void txtMontoExcento_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

        private void txtMontoGravado_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

        void setDatos(int Flag, string Transaccion) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            if (Transaccion == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
            {
                if (txtTransaccionID.Text == "")
                    TransaccionID = 0;
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
                TransaccionID = Convert.ToInt32(Transaccion);

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el Nomenclatura en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@TransaccionID",TransaccionID),
                new SqlParameter("@TipoTransaccion",cbxTipoTransaccion.Text),
                new SqlParameter("@ClienteID",txtClienteID.Text),
                new SqlParameter("@DiasCredito",txtDiasCredito.Text),
                new SqlParameter("@Fecha",dtpFechaTransaccion.Text),
                new SqlParameter("@MontoExcento",txtMontoExcento.Text),
                new SqlParameter("@MontoGravado",txtMontoGravado.Text),
                new SqlParameter("@ITBIS",txtITBIS.Text),
                new SqlParameter("@Estado",cbxEstado.Text),
                new SqlParameter("@FacturaID",txtFactura.Text),
                new SqlParameter("@Observacion",txtObservacion.Text),
                new SqlParameter("@UsuarioID",ClassVariables.UsuarioID)
            };

            tabla = Clases.ClassData.runDataTable("sp_cliTransacciones", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

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
                txtClienteID.Text = tabla.Rows[0]["ClienteID"].ToString();
                txtDiasCredito.Text = tabla.Rows[0]["DiasCredito"].ToString();
                dtpFechaTransaccion.Text = tabla.Rows[0]["Fecha"].ToString();
                txtMontoExcento.Text = tabla.Rows[0]["MontoExcento"].ToString();
                txtMontoGravado.Text = tabla.Rows[0]["MontoGravado"].ToString();
                txtITBIS.Text = tabla.Rows[0]["ITBIS"].ToString();
                cbxEstado.Text = tabla.Rows[0]["Estado"].ToString();
                txtFactura.Text = tabla.Rows[0]["FacturaID"].ToString();
                txtObservacion.Text = tabla.Rows[0]["Observacion"].ToString();

                BtnEditar.IsEnabled = cbxEstado.Text == "Abierta";

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
                ClassControl.setValidador("select * from TcliClientes where ClienteID =", txtClienteID, tbxClienteID); //ejecutamos el metodo validador con el campo seleccionado para que lo busque y muestre una vez se guarde el registro
            }

            listSqlParameter.Clear(); //Limpiamos la lista de parametros.
        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtTransaccionID,txtMontoGravado,txtMontoExcento,txtClienteID,cbxTipoTransaccion,dtpFechaTransaccion,cbxEstado, txtFactura, txtObservacion,tbxClienteID,txtDiasCredito
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                txtClienteID,tbxClienteID,txtDiasCredito//,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular//,cActivar
            };

            List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
            {
                txtTransaccionID,txtMontoGravado,txtClienteID,tbxClienteID,txtMontoExcento,txtMontoGravado,txtITBIS,txtDiasCredito//,txtDireccion,txtCorreoElectronico,txtTelefono,txtCelular
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
                BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnBuscar.IsEnabled =
                BtnImprimir.IsEnabled = BtnAgregar.IsEnabled = BtnEditar.IsEnabled = true;

                BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = false;

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
                BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnBuscar.IsEnabled =
                BtnImprimir.IsEnabled = BtnAgregar.IsEnabled = BtnEditar.IsEnabled = false;

                BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = true;

                if (Estado == "Modo Agregar") //Si el estado es modo Agregar enviamos a ejecutar otro metodo parametizado de forma especial
                {
                    SetControls(false, null, false);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.AddThick;
                    txtTransaccionID.Text = (LastTransaccionID + 1).ToString();
                }
                else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
                {
                    SetControls(true, null, true);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                }
                txtTransaccionID.IsReadOnly = true;
            }
            if (Imprime == false)
                BtnImprimir.IsEnabled = Imprime;
            if (Agrega == false)
                BtnAgregar.IsEnabled = Agrega;
            if (Modifica == false)
                BtnEditar.IsEnabled = Modifica;
        }

    }
}
