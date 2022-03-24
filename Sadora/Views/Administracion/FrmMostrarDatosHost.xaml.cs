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
    /// Lógica de interacción para FrmMostrarDatosHost.xaml
    /// </summary>
    public partial class FrmMostrarDatosHost : Window
    {
        private DataTable dt;

        public FrmMostrarDatosHost()
        { }

        public FrmMostrarDatosHost(string Lista, DataTable tabla, List<String> ListName = null)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            if (Lista == null)
            {
                dt = tabla;
            }
            else
            {
                dt = Clases.ClassData.runDataTable(Lista, null, "CommandText");
            }

            GridMuestra.ItemsSource = dt.DefaultView;
            if (ListName != null)
            {
                bool Validator = false;
                for (int i = 0; i < GridMuestra.Columns.Count; i++)
                {
                    foreach (String Valor in ListName)
                    {
                        if (GridMuestra.Columns[i].HeaderCaption.ToString() == Valor)
                        {
                            Validator = true;
                            break;
                            //GridMuestra.Columns[i].Visible = true;
                        }
                        else
                        {
                            Validator = false;
                            //GridMuestra.Columns[i].Visible = false;
                        }
                    }
                    if (Validator)
                    {
                        GridMuestra.Columns[i].Visible = true;
                    }
                    else
                    {
                        GridMuestra.Columns[i].Visible = false;
                    }
                }
            }

            ClassControl.SetGridReadOnly(GridMuestra);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //MiniDialogo.IsOpen = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            //MiniDialogo.IsOpen = false;
            GridMuestra.SelectedItem = null;
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            if (GridMuestra.SelectedItem == null)
            {
                FrmCompletarCamposHost frm = new FrmCompletarCamposHost("Debe Seleccionar un registro.");
                frm.ShowDialog();
            }
            else
            {

                this.Hide();
                //Clientes.UscClientes clientes = new Clientes.UscClientes();
                //clientes.txtClaseID.Text = GridMuestra.
            }
        }

        private void TablaGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (GridMuestra.SelectedItem == null)
                {
                    FrmCompletarCamposHost frm = new FrmCompletarCamposHost("Debe Seleccionar un registro.");
                    frm.ShowDialog();
                }
                else
                    this.Hide();
            }
        }
    }
}
