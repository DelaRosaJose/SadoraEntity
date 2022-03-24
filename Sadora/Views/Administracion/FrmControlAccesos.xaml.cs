using Sadora.Clases;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para FrmControlAccesos.xaml
    /// </summary>
    public partial class FrmControlAccesos : Window
    {
        public bool Resultado;

        public FrmControlAccesos()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e) => this.Hide();

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsuarioID.Text == "1" && txtPassword.Text == "123")
            {
                Resultado = true;
                this.Hide();
            }
        }
        private void txtUsuarioID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ((Control)sender).MoveFocus(new TraversalRequest(new FocusNavigationDirection()));
            }
        }


    }
}
