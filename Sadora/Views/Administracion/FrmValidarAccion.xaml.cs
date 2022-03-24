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
using Sadora.Clases;

namespace Sadora.Administracion
{
    /// <summary>
    /// Lógica de interacción para FrmValidarAccion.xaml
    /// </summary>
    public partial class FrmValidarAccion : Window
    {

        public FrmValidarAccion()
        { }

        public FrmValidarAccion(string Lista)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            lAviso.Text = Lista;
        }

        private void BtnSi_Click(object sender, RoutedEventArgs e)
        {
            ClassVariables.ValidarAccion = true;
            this.Close();
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            ClassVariables.ValidarAccion = false;
            this.Close();
        }
    }
}
