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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para FrmMenu.xaml
    /// </summary>
    public partial class FrmMenu : Window
    {

        bool FrmMenuClosed = true;
        bool Pasador = false;
        string Modulo = "";

        public FrmMenu()
        {
            InitializeComponent();
            this.DataContext = ClassVariables.ClasesVariables; 
            Modulo = TblModulo.Text;
            TextService();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        void TextService(bool cerrado = true)
        {
            if (cerrado == true)
            {
                TblModulo.Text = String.Join(Environment.NewLine, TblModulo.Text.Select(c => new String(c, 1)).ToArray());
                TblModulo.FontSize = 19.5;
            }
            else
                TblModulo.Text = Modulo;
        }

        void OpenUsercontrol(UserControl Usc, MaterialDesignThemes.Wpf.PackIconKind icon = MaterialDesignThemes.Wpf.PackIconKind.FileOutline)
        {
            if (Usc == null)
            {
                MiniDialogo.IsOpen = true;
            }
            else
            {
                string Titulo = "";
                string Modulo = "";
                for (int i = 0; i < TabMain.Items.Count; i++)
                {
                    TabMain.SelectedIndex = i;
                    string name = (TabMain.SelectedItem as TabItem).Header.ToString();

                    System.Reflection.Assembly assembly;
                    assembly = System.Reflection.Assembly.GetExecutingAssembly();

                    foreach (Type t in assembly.GetTypes())
                    {
                        var nombreTipo = t.BaseType.Name;
                        if (nombreTipo.ToLower().Contains("usercontrol") || nombreTipo.ToLower().Contains("window"))
                        {
                            if (t.Name == Usc.Name)
                            {
                                Modulo = t.Namespace.Replace("Sadora.", string.Empty);
                                break;
                            }
                        }
                    }
                    Titulo = Usc.Tag.ToString();

                    if (name == Titulo)
                    {
                        Pasador = true;
                        break;
                    }
                    else
                    {
                        Pasador = false;
                    }
                }

                if (Pasador == true)
                {
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        var message = "Dicha ventana se encuentra abierta";
                        Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                    }
                    Pasador = false;
                }
                else
                {
                    List<SqlParameter> listSqlParameter = new List<SqlParameter>() //Creamos una lista de parametros con cada parametro de sql, donde indicamos el nombre en sql y le indicamos el valor o el campo de donde sacara el valor que enviaremos.
                        {
                            new SqlParameter("Flag",-1),
                            new SqlParameter("@UsuarioID", ClassVariables.UsuarioID),
                            new SqlParameter("@Nombre", Usc.Name),
                            new SqlParameter("@Modulo", Modulo),
                            new SqlParameter("@Titulo", Titulo),
                            new SqlParameter("@Visualiza", ""),
                            new SqlParameter("@Imprime", ""),
                            new SqlParameter("@Agrega", ""),
                            new SqlParameter("@Modifica", ""),
                            new SqlParameter("@Anula", "")
                        };

                    DataTable TablaGrid = Clases.ClassData.runDataTable("sp_sysAccesos", listSqlParameter, "StoredProcedure"); //recibimos el resultado que nos retorne la transaccion digase, consulta, agregar,editar,eliminar en una tabla.

                    if (ClassVariables.GetSetError != null) //Si el intento anterior presenta algun error aqui aparece el mismo
                    {
                        Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost(ClassVariables.GetSetError);
                        frm.ShowDialog();
                        ClassVariables.GetSetError = null;
                    }

                    if (TablaGrid.Rows.Count == 1) //evaluamos si la tabla actualizada previamente tiene datos, de ser asi actualizamos los controles en los que mostramos esa info.
                    {
                        if (Convert.ToBoolean(TablaGrid.Rows[0]["Visualiza"]))
                        {
                            ClassVariables.Imprime = Convert.ToBoolean(TablaGrid.Rows[0]["Imprime"]);
                            ClassVariables.Agrega = Convert.ToBoolean(TablaGrid.Rows[0]["Agrega"]);
                            ClassVariables.Modifica = Convert.ToBoolean(TablaGrid.Rows[0]["Modifica"]);

                            var packIconMaterial = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = icon,
                                Width = 24,
                                Height = 24,
                                Margin = new Thickness(7, 0, 0, 0),
                            };

                            Wpf.Controls.TabItem tabItem = new Wpf.Controls.TabItem()
                            {
                                FontSize = 18.5,
                                FontWeight = FontWeights.SemiBold,
                                FontFamily = new FontFamily("pack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto"),
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Header = Titulo,
                                Content = Usc,
                                Icon = packIconMaterial
                            };
                            TabMain.Items.Add(tabItem);
                        }
                        else
                        {
                            new Administracion.FrmCompletarCamposHost("Usted no tiene acceso a dicha ventana").ShowDialog();
                        }
                    }
                    else
                    {
                        new Administracion.FrmCompletarCamposHost("Dicha ventana no existe o se encontro mas de una vez, por favor comuniquese con su proveedor de sistemas").ShowDialog();
                    }
                    listSqlParameter.Clear();
                }
            }
        }

        private void ButtonOpenMenu_Click_1(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;

            if (FrmMenuClosed)
            {
                Storyboard openFrmMenu = (Storyboard)ButtonOpenMenu.FindResource("OpenMenu");
                openFrmMenu.Begin();

                TextService(false);
            }
            else
            {
                Storyboard closeFrmMenu = (Storyboard)ButtonOpenMenu.FindResource("CloseMenu");
                closeFrmMenu.Begin();

                TextService();
            }
            FrmMenuClosed = !FrmMenuClosed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;

            if (FrmMenuClosed)
            {
                Storyboard openFrmMenu = (Storyboard)ButtonOpenMenu.FindResource("OpenMenu");
                openFrmMenu.Begin();

                TextService(false);
            }
            else
            {
                Storyboard closeFrmMenu = (Storyboard)ButtonOpenMenu.FindResource("CloseMenu");
                closeFrmMenu.Begin();

                TextService();
            }
            FrmMenuClosed = !FrmMenuClosed;
        }

        private void ButtonCerrar_Click(object sender, RoutedEventArgs e)
        {
            new FrmMain().Show();
            this.Hide();
        }

        private void ButtonMinimizar_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void btnMenuRegistroCajas_MouseUp(object sender, MouseButtonEventArgs e) => OpenUsercontrol(null);

        private void btnMenuRegistroUsuarios_MouseUp(object sender, MouseButtonEventArgs e) => OpenUsercontrol(new UscUsuarios(), iconMenuRegistroUsuarios.Kind);

        private void btnMenuRegistroGruposUsuarios_MouseUp(object sender, MouseButtonEventArgs e) => OpenUsercontrol(new UscGruposUsuarios(), iconMenuRegistroUsuarios.Kind);
      
        private void btnMenuRegistroEmpresa_MouseUp(object sender, MouseButtonEventArgs e) => OpenUsercontrol(new UscEmpresa(), iconMenuRegistroEmpresa.Kind);

        private void btnMenuConsultaLogs_MouseUp(object sender, MouseButtonEventArgs e) => OpenUsercontrol(new UscConsultorLogs(), iconMenuConsultaLogs.Kind);

    }
}
