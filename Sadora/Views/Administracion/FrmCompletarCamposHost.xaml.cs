using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para FrmCompletarCamposHost.xaml
    /// </summary>
    public partial class FrmCompletarCamposHost : Window
    {
        public FrmCompletarCamposHost()
        { }

        public FrmCompletarCamposHost(string Lista)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            lAviso.Text = Lista;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MiniDialogo.IsOpen = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //MiniDialogo.IsOpen = true;
        }
    }
}
