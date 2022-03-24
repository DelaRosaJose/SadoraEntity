using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Lógica de interacción para FrmSplash.xaml
    /// </summary>
    public partial class FrmSplash : Window
    {
        FrmLogin login = new FrmLogin();
        bool Estado = true;
        BackgroundWorker worker = new BackgroundWorker();

        public FrmSplash()
        {
            InitializeComponent();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync();

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            for (int i = 0; i <= 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(30);
            }

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Cursor = Cursors.AppStarting;
            if (Estado == true)
            {
                pbStatus.Value = e.ProgressPercentage;
            }


            if (pbStatus.Value == 10)
            {
                Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    System.Reflection.Assembly assembly;
                    assembly = System.Reflection.Assembly.GetExecutingAssembly();

                    foreach (Type t in assembly.GetTypes())
                    {

                        var nombreTipo = t.BaseType.Name;
                        Control control;

                        if (nombreTipo.ToLower().Contains("usercontrol") || nombreTipo.ToLower().Contains("window"))
                        {
                            try
                            {
                                control = Activator.CreateInstance(t) as Control;
                            }
                            catch
                            {
                                control = Activator.CreateInstance(t, "") as Control;
                            }

                            if (control is Window)
                            {
                                Window frm = control as Window;
                            }

                            else
                            {
                                UserControl frm = control as UserControl;
                            }

                            List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
                        {
                            new SqlParameter("Flag", 1),
                            new SqlParameter("@Nombre", t.Name),
                            new SqlParameter("@Modulo", t.Namespace.Replace("Sadora.", string.Empty)),
                            new SqlParameter("@Titulo", control.Tag)
                        };

                            if (control.Tag != null)
                            {
                                DataTable TablaGrid = Clases.ClassData.runDataTable("sp_sysFormularios", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.
                            }

                            if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
                            {
                                listBox1.Items.Add(string.Format(" "));
                                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                                frm.ShowDialog();

                                ClassVariables.GetSetError = null;

                                new FrmValidarAccion("Desea reiniciar la aplicacion").ShowDialog();

                                if (ClassVariables.ValidarAccion == true)
                                    System.Windows.Forms.Application.Restart();

                                System.Windows.Application.Current.Shutdown();

                                return;
                            }

                            listSqlParameter.Clear();
                        }

                    }
                }));

                //List<string> ListVentanas = new List<string>()
                //{
                //    "Clases de Marca","Clases de Departamento","Clases de Proveedores","Clases de MetodoPagos"
                //};
                Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    foreach (string item in new List<string>() { "Clases de Marca", "Clases de Departamento", "Clases de Proveedores", "Clases de MetodoPagos" })
                    {
                        List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
                    {
                        new SqlParameter("Flag", 1),
                        new SqlParameter("@Nombre", "UscMantenimientoGeneral"),
                        new SqlParameter("@Modulo", "Administracion"),
                        new SqlParameter("@Titulo", item)
                    };

                        DataTable TablaGrid = Clases.ClassData.runDataTable("sp_sysFormularios", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

                        if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
                        {
                            Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                            frm.ShowDialog();
                            ClassVariables.GetSetError = null;
                        }

                        listSqlParameter.Clear();
                    }
                }));

            }
            else if (pbStatus.Value == 100 && listBox1.Items.Count == 0)
            {
                Cursor = Cursors.Arrow;
                login.Show();
                this.Close();
            }
        }


    }
}
