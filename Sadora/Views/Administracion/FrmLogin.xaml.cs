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
using System.Windows.Shapes;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para FrmLogin.xaml
    /// </summary>
    public partial class FrmLogin : Window
    {
        List<ClassVariables> VariablesList = new List<ClassVariables>();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BotonSalir_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private DataTable SetDatos(bool Activo = false)
        {
            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
            {
                new SqlParameter("Flag",3),
                new SqlParameter("@UsuarioID",txtUsuarioID.Text),
                new SqlParameter("@Nombre"," "),
                new SqlParameter("@EmpleadoID"," "),
                new SqlParameter("@GrupoID"," "),
                new SqlParameter("@Contraseña",Clases.ClassControl.GetSHA256(txtPassword.Password)),
                new SqlParameter("@Activo", Activo)
            };

            DataTable tabla = Clases.ClassData.runDataTable("sp_sysUsuarios", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.
            listSqlParameter.Clear();
            return tabla;
        }

        private void BotonAcceder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuarioID.Text))
            {
                if (SnackbarThree.MessageQueue is { } messageQueue)
                    Task.Factory.StartNew(() => messageQueue.Enqueue("Debe ingresar el usuario"));
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                if (SnackbarThree.MessageQueue is { } messageQueue)
                    Task.Factory.StartNew(() => messageQueue.Enqueue("Debe ingresar la contraseña"));
                return;
            }
            #region FindingDB

            DataTable tabla = SetDatos();

            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
            {
                new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError).ShowDialog();
                ClassVariables.GetSetError = null;
            }

            if (tabla.Rows.Count == 1 && tabla.Columns.Contains("Resultado")) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
            {
                //txtUsuarioID.Text = tabla.Rows[0]["UsuarioID"].ToString();
                if (tabla.Rows[0]["Resultado"].ToString() == "Acceso Permitido")
                {
                    Clases.ClassVariables.UsuarioID = Convert.ToInt32(txtUsuarioID.Text);

                    DataTable reader = Clases.ClassData.runDataTable("select top 1 Nombre, RNC, Razon_Social, Direccion, Logo, Telefono from TsysEmpresa", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.

                    if (reader.Rows.Count == 1 && reader.Columns.Contains("Nombre") && reader.Columns.Contains("Logo"))
                    {
                        ClassVariables.ClasesVariables.NombreEmpresa = reader.Rows[0]["Nombre"].ToString();
                        ClassVariables.LogoEmpresa = (byte[])reader.Rows[0]["Logo"]; //(byte[])reader.Rows[0]["Nombre"];
                        ClassVariables.ClasesVariables.RNCEmpresa = reader.Rows[0]["RNC"].ToString();
                        ClassVariables.ClasesVariables.TelefonoEmpresa = reader.Rows[0]["Telefono"].ToString();
                        ClassVariables.ClasesVariables.DireccionEmpresa = reader.Rows[0]["Direccion"].ToString();
                    }

                    new FrmMain().Show();
                    this.Close();
                }

                else if (tabla.Rows[0]["Resultado"].ToString() == "Usuario desactivado")
                {
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Usuario desactivado"));
                }
                else if (tabla.Rows[0]["Resultado"].ToString() == "Acceso denegado")
                {
                    Counter();

                    if (VariablesList.Where(x => x.UserID == txtUsuarioID.Text).Select(x => x.CountIntent).FirstOrDefault() >= 3)
                    {
                        DataTable tablaV2 = SetDatos(true);

                        if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
                        {
                            new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError).ShowDialog();

                            ClassVariables.GetSetError = null;
                        }
                        if (tablaV2.Rows.Count == 1 && tablaV2.Columns.Contains("Resultado"))
                        {
                            if (tablaV2.Rows[0]["Resultado"].ToString() == "El usuario ha sido bloqueado, pidale a TI que restablesca el usuario")
                            {
                                if (SnackbarThree.MessageQueue is { } messageQueuev2)
                                    Task.Factory.StartNew(() => messageQueuev2.Enqueue("El usuario ha sido bloqueado, comuniquese con soporte"));
                            }
                        }
                    }
                    else
                    {
                        if (SnackbarThree.MessageQueue is { } messageQueue)
                            Task.Factory.StartNew(() => messageQueue.Enqueue("Acceso denegado"));
                    }

                }

                else if (tabla.Rows[0]["Resultado"].ToString() == "Error de Usuario")
                {
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Error de Usuario"));
                }

            }
            #endregion
            //Limpiamos la lista de parametros.

        }

        private void Counter()
        {
            var FormasDePago = VariablesList.FindAll(x => x.UserID == txtUsuarioID.Text);//.Where(x => x.FormaPago == FormaPagoAplicada);//new ClassVariables().FormaPago;

            if (FormasDePago.Any())
                FormasDePago.ForEach(c => c.CountIntent++);
            else
                VariablesList.Add(new ClassVariables() { UserID = txtUsuarioID.Text, CountIntent = 1 });

        }

        private void txtUsuarioID_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }

        private void txtUsuarioID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.BotonAcceder.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsuarioID.Focus();
        }
    }
}
