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

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para UscMantenimientoGeneral.xaml
    /// </summary>
    public partial class UscMantenimientoGeneral : UserControl
    {
        public UscMantenimientoGeneral(string Tabla)
        {
            InitializeComponent();
            Name = "UscMantenimientoGeneral";
            Table = Tabla;
            try
            {
                Tag = "Clases de " + Table.Substring(4).Replace("Clase", "").Replace("clase", "");
            }
            catch
            { }
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
        string Table;
        string Id;

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

                FinId();
                lTransaccionID.Text = Id;

                this.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                Inicializador = false;
            }

        }

        private void BtnPrimerRegistro_Click(object sender, RoutedEventArgs e)
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles limpiados.
            {
               txtNombre
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
               txtNombre
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
               txtNombre
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
               txtNombre
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
                setDatos(0, null);

                SetEnabledButton("Modo Consulta");

                if (tabla.Rows.Count > 1)
                {
                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla);
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
            }
        }

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

        //private void txtRepresentante_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        //private void txtTransaccionID_KeyDown(object sender, KeyEventArgs e)
        //{
        //    ClassControl.ValidadorNumeros(e);
        //}

        //private void txtDireccion_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        //private void txtCorreoElectronico_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        //private void txtTelefono_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        //private void txtCelular_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        //private void cActivar_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (Estado != "Modo Consulta")
        //    {
        //        if (e.Key == Key.Enter)
        //        {
        //            ((CheckBox)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        //        }
        //    }
        //}

        void setDatos(int Flag, string Cliente) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            if (Cliente == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
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
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del cliente
                    }
                }
            }
            else //Si pasamos un cliente, lo convertimos actualizamos la variable cliente principal
            {
                TransaccionID = Convert.ToInt32(Cliente);
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@ClaseID",TransaccionID),
                new SqlParameter("@Nombre",txtNombre.Text),
                new SqlParameter("@Tabla",Table),
                new SqlParameter("@UsuarioID",ClassVariables.UsuarioID)
            };

            tabla = Clases.ClassData.runDataTable("sp_sysClases", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                frm.ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
            {
                txtTransaccionID.Text = tabla.Rows[0][Id].ToString();
                txtNombre.Text = tabla.Rows[0]["Nombre"].ToString();

                if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
                {
                    try
                    {
                        LastTransaccionID = Convert.ToInt32(txtTransaccionID.Text); //intentamos convertir el id del cliente
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error al intentar convertirlo lo enviamos
                    }
                }
            }
            listSqlParameter.Clear(); //Limpiamos la lista de parametros.
        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtTransaccionID,txtNombre
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {

            };

            List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que validaremos al dar click en el boton guardar.
            {
                txtTransaccionID,txtNombre
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

        void setValidador(string Consulta, TextBox Enviador, TextBox Recibidor) //Este metodo se encarga de validar cualquier campo de la ventana que este llamando otro mantenimiento, Ejemplo(Campo clase de Administracion contiene(TransaccionID, Detalle de la clase que es el nombre)), 
        {                                                                       // el metodo recibe el textbox que le envia la info, la consulta que debe buscar con esa info y el textbox donde debe depositar el resultado.

            reader = Clases.ClassData.runSqlDataReader(Consulta + Enviador.Text, null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

            if (reader.HasRows) //Validamos si el datareader trajo data.
            {
                if (reader.Read()) //Si puede leer la informacion
                {
                    Recibidor.Text = reader["Nombre"].ToString(); //Asignamos el resultado de la columna "Nombre" del datareader en el textbox que le indicamos en el parametro previamente identificado.
                    reader.NextResult();
                }
                reader.Close(); //Cerramos el datareader
                reader.Dispose(); //Cortamos la conexion del datareader
            }
            else //Si no trajo data
            {
                Recibidor.Clear(); //Dejamos en blanco el campo que recibe
                reader.Close(); //limpiamos el reader
                reader.Dispose();
            }

        }

        void FinId()
        {
            SqlDataReader reader = Clases.ClassData.runSqlDataReader("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + Table + "' and ORDINAL_POSITION = 2", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

            if (reader.HasRows) //Validamos si el datareader trajo data.
            {
                if (reader.Read()) //Si puede leer la informacion
                {
                    Id = reader["COLUMN_NAME"].ToString(); //Asignamos el resultado de la columna "Nombre" del datareader en el textbox que le indicamos en el parametro previamente identificado.
                    reader.NextResult();
                }
                reader.Close(); //Cerramos el datareader
                reader.Dispose(); //Cortamos la conexion del datareader
            }
            else //Si no trajo data
            {
                reader.Close(); //limpiamos el reader
                reader.Dispose();
            }
        }

    }
}
