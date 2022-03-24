using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para UscEmpresa.xaml
    /// </summary>
    public partial class UscEmpresa : UserControl
    {
        bool Imprime;
        bool Agrega;
        bool Modifica;

        public bool Inicializador = false;
        DataTable tabla;
        string Estado;
        string Lista;
        int EmpresaID;
        int LastEmpresaID;
        string last = "";

        byte[] pic = null;

        public UscEmpresa()
        {
            InitializeComponent();
            Name = "UscEmpresa";
        }

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
                EmpresaID = Convert.ToInt32(txtEmpresaID.Text) - 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }


            if (EmpresaID <= 1)
            {
                BtnPrimerRegistro.IsEnabled = false;
                BtnAnteriorRegistro.IsEnabled = false;
                setDatos(0, "1");
            }
            else
            {
                setDatos(0, EmpresaID.ToString());
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
                EmpresaID = Convert.ToInt32(txtEmpresaID.Text) + 1;
            }
            catch (Exception exception)
            {
                ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString();
            }

            if (EmpresaID >= LastEmpresaID)
            {
                BtnUltimoRegistro.IsEnabled = false;
                BtnProximoRegistro.IsEnabled = false;
                setDatos(0, LastEmpresaID.ToString());
            }
            else
            {
                setDatos(0, EmpresaID.ToString());
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
                last = txtEmpresaID.Text;
                SetEnabledButton("Modo Busqueda");
            }
            else if (Estado == "Modo Busqueda")
            {
                List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
                {
                    txtEmpresaID//,tbxEmpleadoID,txtPassword,txtGrupoID,tbxGrupoID,cActivar
                };
                Clases.ClassControl.ActivadorControlesReadonly(null, true, false, false, listaControles);

                setDatos(0, null);

                List<String> ListName = new List<String>() //Estos son los campos que saldran en la ventana de busqueda, solo si se le pasa esta lista de no ser asi, se mostrarian todos
                {
                    "Usuario ID","Nombre","Empleado ID","Grupo ID","Activo"
                };

                SetEnabledButton("Modo Consulta");

                if (tabla.Rows.Count > 1)
                {
                    Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost(null, tabla, ListName);
                    frm.ShowDialog();

                    if (frm.GridMuestra.SelectedItem != null)
                    {
                        DataRowView item = (frm.GridMuestra as DevExpress.Xpf.Grid.GridControl).SelectedItem as DataRowView;
                        txtEmpresaID.Text = item.Row.ItemArray[0].ToString();
                        setDatos(0, txtEmpresaID.Text);
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
            last = txtEmpresaID.Text;
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
                //GridMain.ClearColumnFilter("Visualiza");
                if (Estado == "Modo Editar")
                {
                    setDatos(2, null);
                }
                else
                {
                    Estado = "Modo Agregar";
                    setDatos(1, null);
                }
                SetEnabledButton("Modo Consulta");
                setDatos(0, txtEmpresaID.Text);
                ClassVariables.ClasesVariables.NombreEmpresa = txtNombre.Text;
                ClassVariables.ClasesVariables.RNCEmpresa = txtRNC.Text;
                ClassVariables.ClasesVariables.TelefonoEmpresa = txtTelefono.Text;
                ClassVariables.ClasesVariables.DireccionEmpresa = txtDireccion.Text;
                ClassVariables.LogoEmpresa = pic;
            }
        }

        private void txtEmpresaID_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtNombre_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtEmpresaID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        void setDatos(int Flag, string Usuario) //Este es el metodo principal del sistema encargado de conectar, enviar y recibir la informacion de sql
        {
            if (Usuario == null) //si el parametro llega nulo intentamos llenarlo para que no presente ningun error el sistema
            {
                if (txtEmpresaID.Text == "")
                    EmpresaID = 0;
                else
                {
                    try
                    {
                        EmpresaID = Convert.ToInt32(txtEmpresaID.Text);
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //Enviamos la excepcion que nos brinda el sistema en caso de que no pueda convertir el id del Usuario 
                    }
                }
            }
            else //Si pasamos un Usuario, lo convertimos actualizamos la variable Usuario principal
                EmpresaID = Convert.ToInt32(Usuario);

            if (pic == null)
            {
                pic = new MemoryStream().ToArray();
            }

            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",Flag),
                new SqlParameter("@EmpresaID",EmpresaID),
                new SqlParameter("@Nombre",txtNombre.Text),
                new SqlParameter("@RNC",txtRNC.Text),
                new SqlParameter("@Razon_Social",txtRazonSocial.Text),
                new SqlParameter("@Direccion",txtDireccion.Text),
                new SqlParameter("@Telefono",txtTelefono.Text),
                new SqlParameter("@Logo",pic),
                new SqlParameter("@UsuarioID", ClassVariables.UsuarioID)
            };

            tabla = Clases.ClassData.runDataTable("sp_sysEmpresa", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                frm.ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (tabla.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
            {
                txtEmpresaID.Text = tabla.Rows[0]["EmpresaID"].ToString();
                txtNombre.Text = tabla.Rows[0]["Nombre"].ToString();
                txtRNC.Text = tabla.Rows[0]["RNC"].ToString();
                txtRazonSocial.Text = tabla.Rows[0]["Razon_Social"].ToString();
                txtDireccion.Text = tabla.Rows[0]["Direccion"].ToString();
                txtTelefono.Text = tabla.Rows[0]["Telefono"].ToString();
                try
                {
                    pic = (byte[])tabla.Rows[0]["Logo"];
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream((byte[])tabla.Rows[0]["Logo"]);
                    bitmap.EndInit();
                    ImagePickture.Source = bitmap;
                }
                catch (Exception) { }

                if (Flag == -1) //si pulsamos el boton del ultimo registro se ejecuta el flag -1 es decir que tenemos una busqueda especial
                {
                    try
                    {
                        LastEmpresaID = Convert.ToInt32(txtEmpresaID.Text); //intentamos convertir el id del Usuario
                    }
                    catch (Exception exception)
                    {
                        ClassVariables.GetSetError = "Ha ocurrido un error: " + exception.ToString(); //si presenta un error ald intentar convertirlo lo enviamos
                    }
                }

            }
            listSqlParameter.Clear(); //Limpiamos la lista de parametros.

        }

        void SetControls(bool Habilitador, string Modo, bool Editando) //Este metodo se encarga de controlar cada unos de los controles del cuerpo de la ventana como los textbox
        {
            List<Control> listaControl = new List<Control>() //Estos son los controles que seran controlados, readonly, enable.
            {
                txtEmpresaID,txtNombre,txtRNC,txtRazonSocial,txtDireccion,txtTelefono
            };

            List<Control> listaControles = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                txtRNC,txtRazonSocial,txtDireccion,txtTelefono
            };

            List<Control> listaControlesValidar = new List<Control>() //Estos son los controles que desahilitaremos al dar click en el boton buscar, los controles que no esten en esta lista se quedaran habilitados para poder buscar un registro por ellos.
            {
                txtEmpresaID,txtNombre,txtDireccion,txtTelefono
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
                    txtEmpresaID.Text = (LastEmpresaID + 1).ToString();
                }
                else //Si el estado es modo Editar enviamos a ejecutar el mismo metodo parametizado de forma especial
                {
                    SetControls(true, null, true);
                    IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                }
                txtEmpresaID.IsReadOnly = true;
                List<String> listaColumnas = new List<String>() //Estos son los controles que seran controlados, readonly, enable.
                {
                    "Visualiza","Agrega","Modifica","Imprime","Anula"
                };
                //Clases.ClassControl.SetGridReadOnly(GridMain, listaColumnas, false);
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

        private void txtRazonSocial_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtRNC_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtDireccion_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void BtnCargarFotos_Click(object sender, RoutedEventArgs e)
        {
            if (Estado == "Modo Agregar" || Estado == "Modo Editar")
            {
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.InitialDirectory = "c:\\";
                dlg.Filter = "Image files (*.jpg;*.png)|*.jpg;*.png|All Files (*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedFileName = dlg.FileName;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFileName);
                    bitmap.EndInit();
                    ImagePickture.Source = bitmap;

                    Stream myStream = dlg.OpenFile();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        myStream.CopyTo(ms);
                        pic = ms.ToArray();
                    }
                }
            }
        }

        private void txtTelefono_KeyUp(object sender, KeyEventArgs e)
        {
            if (Estado != "Modo Consulta")
                if (e.Key == Key.Enter)
                    ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }
    }
}
