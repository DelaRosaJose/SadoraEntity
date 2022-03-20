using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para FrmMain.xaml
    /// </summary>
    public partial class FrmMain : Window
    {
        public FrmMain()
        {
            InitializeComponent();
            this.DataContext = ClassVariables.ClasesVariables;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += timer_Tick;
            timer.Start();

            string SobreNosotros = "";

            DateTime fecha = DateTime.Today;
            string mes = fecha.ToString("MMMM"); //te da el nombre completo en la cultura default

            string dia = fecha.ToString("dddd");


            btnModuloClientes.MouseEnter += new MouseEventHandler(btnModuloClientes_MouseEnter);

            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Clases de clientes"},
                new Clases.ClassVariables() { Nombre = "Registro de clientes"}, new Clases.ClassVariables() { Nombre = "Cuentas por cobrar"},
                new Clases.ClassVariables() { Nombre = "Estado de cuentas por clientes"}, new Clases.ClassVariables() { Nombre = "Estado de cuentas por cobrar"},
                new Clases.ClassVariables() { Nombre = "Consulta de movimientos"}//, new Clases.ClassVariables() { Nombre = "Estado de cuentas por cobrar"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();

            ClassControl.UpdateNameUser();

        }

        public void AbrirFormulario<MiForm>(bool Pass = false) where MiForm : Window, new()
        {
            if (Pass)
            {
                if (Application.Current.Windows.OfType<MiForm>().Any())
                {
                    MiForm Win = Application.Current.Windows.OfType<MiForm>().First();//this.OwnedWindows.OfType<MiForm>().First();
                    Win.Activate();
                    Win.Show();
                    this.Close();
                }
                else
                {
                    MiForm Win = new MiForm();
                    Win.Show();
                    this.Close();
                }
            }
            else
            {
                MiniDialogo.IsOpen = true;
            }
        }

        private void ButtonCerrar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        void UpdateSobreModulo(List<ClassVariables> list)
        {
            SPanelSobreModulo.Children.Clear();

            foreach (ClassVariables user in list)
            {
                Ellipse myEllipse = new Ellipse()
                {
                    Height = 5,
                    Width = 5,
                    Fill = Brushes.Black
                };

                TextBlock myTextBlock = new TextBlock()
                {
                    Margin = new Thickness(5, 0, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 18.5, //FontWeight = FontWeights.SemiBold,
                    FontFamily = new FontFamily("pack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto"),
                    Text = user.Nombre
                };

                BulletDecorator myBulletDecorator = new BulletDecorator()
                {
                    Margin = new Thickness(5, 2, 2, 0),
                    Bullet = myEllipse,
                    Child = myTextBlock
                };

                SPanelSobreModulo.Children.Add(myBulletDecorator);
            }
        }

        private void btnModuloClientes_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Clientes.FrmMenu>(true);
        }

        private void btnModuloProveedores_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Proveedores.FrmMenu>(true);
        }

        private void btnModuloInventarios_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Inventario.FrmMenu>(true);
        }

        private void btnModuloVentas_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Ventas.FrmMenu>(true);
        }

        private void btnModuloContabilidad_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Contabilidad.FrmMenu>(true);
        }

        private void btnModuloBancos_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Clientes.FrmMenu>();
        }

        private void btnModuloAdministracion_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<FrmMenu>(true);
        }

        private void btnModuloRecursosHumanos_Click(object sender, RoutedEventArgs e)
        {
            AbrirFormulario<Recursos_Humanos.FrmMenu>(true);
        }

        void btnModuloClientes_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Clases de clientes"},
                new Clases.ClassVariables() { Nombre = "Registro de clientes"}, new Clases.ClassVariables() { Nombre = "Cuentas por cobrar"},
                new Clases.ClassVariables() { Nombre = "Estado de cuentas por clientes"}, new Clases.ClassVariables() { Nombre = "Estado de cuentas por cobrar"},
                new Clases.ClassVariables() { Nombre = "Consulta de movimientos"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }
        private void btnModuloProveedores_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Clases de proveedores"},
                new Clases.ClassVariables() { Nombre = "Registro de proveedores"}, new Clases.ClassVariables() { Nombre = "Cuentas por pagar"},
                new Clases.ClassVariables() { Nombre = "Estado de cuentas por proveedor"}, new Clases.ClassVariables() { Nombre = "Estado de cuentas por pagar"},
                new Clases.ClassVariables() { Nombre = "Consulta de movimientos"},new Clases.ClassVariables() { Nombre = "Solicitud de pagos"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }
        private void btnModuloInventarios_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Clases de articulos"},
                new Clases.ClassVariables() { Nombre = "Clases de Activos"},new Clases.ClassVariables() { Nombre = "Registro de articulos"},
                new Clases.ClassVariables() { Nombre = "Registro de Activos"}, new Clases.ClassVariables() { Nombre = "Movimientos de inventario"},
                new Clases.ClassVariables() { Nombre = "Recepcion de mercancia"}, new Clases.ClassVariables() { Nombre = "Despacho de mercancia"},
                new Clases.ClassVariables() { Nombre = "Toma fisica de inventario"},new Clases.ClassVariables() { Nombre = "Consulta de movimientos"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }

        private void btnModuloVentas_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Clases de ventas"},
                new Clases.ClassVariables() { Nombre = "Facturacion"}, new Clases.ClassVariables() { Nombre = "Cotizacion"},
                new Clases.ClassVariables() { Nombre = "Pedidos"}, new Clases.ClassVariables() { Nombre = "Recibo de ingresos"},
                new Clases.ClassVariables() { Nombre = "Devoluciones"}//,new Clases.ClassVariables() { Nombre = "Solicitud de pagos"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }

        private void btnModuloContabilidad_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Diario General"},
                new Clases.ClassVariables() { Nombre = "Mayor general"}, new Clases.ClassVariables() { Nombre = "Entrada de diario"},
                new Clases.ClassVariables() { Nombre = "Estados Financieros"}, new Clases.ClassVariables() { Nombre = "606"},
                new Clases.ClassVariables() { Nombre = "607"},new Clases.ClassVariables() { Nombre = "608"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }

        private void btnModuloBancos_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Clases de cuentas"},
                new Clases.ClassVariables() { Nombre = "Registro de pagos"}, new Clases.ClassVariables() { Nombre = "Conciliacion de banco"},
                new Clases.ClassVariables() { Nombre = "Pagos de tarjetas"}, new Clases.ClassVariables() { Nombre = "Depositos bancarios"},
                new Clases.ClassVariables() { Nombre = "Consulta de movimientos"},new Clases.ClassVariables() { Nombre = "Estado del banco"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }

        private void btnModuloAdministracion_MouseEnter(object sender, MouseEventArgs e)
        {
            List<Clases.ClassVariables> listOfUsers = new List<Clases.ClassVariables>()
            {   new Clases.ClassVariables() { Nombre = "Este Modulo cuenta con las siguientes opciones: "}, new Clases.ClassVariables() { Nombre = "Perfiles de usuarios"},
                new Clases.ClassVariables() { Nombre = "Registro de usuarios"}, new Clases.ClassVariables() { Nombre = "Registro de sucursales"},
                new Clases.ClassVariables() { Nombre = "Configuracion de empresas"}, new Clases.ClassVariables() { Nombre = "Controles de usuarios"}
            };

            UpdateSobreModulo(listOfUsers);
            listOfUsers.Clear();
        }
        private void btnModuloRecursosHumanos_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void ButtonMinimizar_Click(object sender, RoutedEventArgs e)
        {
            //btnModuloClientes.MouseEnter += new MouseEventHandler(btnModuloClientes_MouseEnter);
            this.WindowState = WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ClassVariables.LogoEmpresa != null)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new System.IO.MemoryStream(ClassVariables.LogoEmpresa);//new System.IO.MemoryStream(ClassVariables.LogoEmpresa);
                    bitmap.EndInit();
                    ImagePickture.Source = bitmap;
                }
            }
            catch (Exception) { }
        }
    }
}
