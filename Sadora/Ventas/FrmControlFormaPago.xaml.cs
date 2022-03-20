using Sadora.Clases;
using Sadora.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Sadora.Ventas
{
    /// <summary>
    /// Lógica de interacción para FrmControlFormaPago.xaml
    /// </summary>
    public partial class FrmControlFormaPago : INotifyPropertyChanged
    {
        //public bool Resultado;
        //private int ClienteID;
        //private int ClaseComprobanteId;
        //private bool NotData = false;
        private string FormaPago;
        //private string montorestante;
        private List<Clases.ClassVariables> ListOfFormasPagos = new List<Clases.ClassVariables>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public string FormaPagoAplicada { get { return FormaPago; } set { FormaPago = value; OnPropertyChanged(); } }

        #region Variables para Eventos
        string Efect = "";
        string Tarj = "";
        string Trans = "";
        string Ck = "";
        //string FormaPago = "";
        #endregion

        public FrmControlFormaPago() { }

        public FrmControlFormaPago(string formaPago, double MontoPagar)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            FormaPago = formaPago;
            MiniDialogo.IsOpen = true;
            txtMontoDistribuido.Focus();
            lMonto.Text = MontoPagar.ToString("N");
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ControlEvent();
        }

        private void txtMontoDistribuido_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (!txtMontoDistribuido.IsReadOnly)
                {
                    txtMontoRestante.Text = default;
                    var result = "";

                    txtMontoRestante.Text = Convert.ToDouble(result = txtMontoDistribuido.Text != null && txtMontoDistribuido.Text != "" ? txtMontoDistribuido.Text : "0") == 0 ? (Convert.ToDouble(lMonto.Text)).ToString() :
                        (Convert.ToDouble(lMonto.Text) - Convert.ToDouble(txtMontoRestante.Text = (txtMontoRestante.Text != null && txtMontoRestante.Text != "" ? txtMontoRestante.Text : txtMontoDistribuido.Text))).ToString();
                }
            }
            catch (Exception exception)
            {
                Administracion.FrmCompletarCamposHost frm = new Administracion.FrmCompletarCamposHost("Ha ocurrido un error: " + exception.ToString());
                frm.ShowDialog();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((Convert.ToDouble(lMonto.Text) < Convert.ToDouble(txtMontoDistribuido.Text)) && SnackbarThree.MessageQueue is { } messageQueue && FormaPagoAplicada != Efect)
                    Task.Factory.StartNew(() => messageQueue.Enqueue("No puede distribuir mas del monto a pagar"));
                else if (FormaPagoAplicada == Efect && Convert.ToDouble(lMonto.Text) < Convert.ToDouble(txtMontoDistribuido.Text))
                {
                    new Administracion.FrmValidarAccion("Esta seguro que desea distribuir esta cantidad?").ShowDialog();
                    if (ClassVariables.ValidarAccion)
                    {
                        new Administracion.FrmCompletarCamposHost("Devuelta: " + txtMontoRestante.Text.Replace("-", "")).ShowDialog();

                        Counter();

                        FinishScreen();
                        //ClassVariables.IsFullFormaPago = true;
                        //this.Close();
                    }
                }
                else if (Convert.ToDouble(txtMontoRestante.Text) != 0 && Convert.ToDouble(lMonto.Text) != Convert.ToDouble(txtMontoDistribuido.Text))
                {
                    BtAceptarCancelar.Visibility = Visibility.Hidden;
                    PanelOpcionesPagos.Visibility = Visibility.Visible;

                    Counter();

                    lMonto.Text = txtMontoRestante.Text;
                    txtMontoDistribuido.IsReadOnly = true;

                    if (SnackbarThree.MessageQueue is { } messageQueues && Convert.ToDouble(lMonto.Text) > 0)
                        Task.Factory.StartNew(() => messageQueues.Enqueue("La distribucion aun no esta completa, debe culminarla."));
                }
                else if (txtMontoRestante.Text == "0")
                {
                    lMonto.Text = txtMontoRestante.Text;
                    Counter();
                    FinishScreen();
                    //new Administracion.FrmCompletarCamposHost("Devuelta: " + txtMontoRestante.Text).ShowDialog();
                    //ClassVariables.IsFullFormaPago = true;
                    //this.Close();
                    //new Administracion.FrmCompletarCamposHost("Devuelta: " + (Devuelta.Replace("-", ""))).ShowDialog();
                }
            }
            catch { }
        }

        private void Counter()
        {
            var FormasDePago = ListOfFormasPagos.FindAll(x => x.FormaPago == FormaPagoAplicada);//.Where(x => x.FormaPago == FormaPagoAplicada);//new ClassVariables().FormaPago;

            if (FormasDePago.Any())
                FormasDePago.ForEach(c => c.CantidadFormaPago = Convert.ToDouble(txtMontoDistribuido.Text));
            else
                ListOfFormasPagos.Add(new ClassVariables() { FormaPago = FormaPagoAplicada, CantidadFormaPago = Convert.ToDouble(txtMontoDistribuido.Text) });
        }

        private void FinishScreen()
        {
            ClassVariables.IsFullFormaPago = true;
            ClassVariables.ListFormasPagos = ListOfFormasPagos;
            this.Close();
        }

        private void ControlEvent()
        {
            var CajaConfigurada = (int)Settings.Default["Caja"];

            if (CajaConfigurada <= 0)
            {
                if (SnackbarThree.MessageQueue is { } messageQueue)
                    Task.Factory.StartNew(() => messageQueue.Enqueue("No hay caja configurada"));
            }
            else
            {
                string CreateNameButton = "";

                DataTable MetodoCaja = Clases.ClassData.runDataTable("select a.Nombre from TvenMetodoPagos a inner join TvenCajasDetalle b on a.MetodoID = b.MetodoID where b.CajaID = " + CajaConfigurada + " and Alta = 1", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.
                for (int i = 0; i < MetodoCaja.Rows.Count; i++)
                {
                    CreateNameButton = MetodoCaja.Rows[i]["Nombre"].ToString();

                    switch (CreateNameButton)
                    {
                        case string a when a.ToUpper().Contains("EFECTIVO"):
                            #region Create Border
                            Border myBorderEfect = new Border()
                            {
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(2),
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Padding = new Thickness(0),
                                CornerRadius = new CornerRadius(5),
                                Margin = new Thickness(3)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonEfect = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir metodo de pago " + CreateNameButton,
                                Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonEfect.Click += new RoutedEventHandler(handlerEfect_Click);

                            StackPanel MyStackEfect = new StackPanel();

                            Efect = CreateNameButton;

                            var packIconMaterialEfect = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.Cash,
                                Width = 36,
                                Height = 36,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextEfect = new TextBlock()
                            {
                                Text = CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 17
                            };
                            #endregion
                            #region Asing Childs
                            MyStackEfect.Children.Add(packIconMaterialEfect);
                            MyStackEfect.Children.Add(MyTextEfect);
                            MyButtonEfect.Content = MyStackEfect;
                            myBorderEfect.Child = MyButtonEfect;

                            PanelWrap.Children.Add(myBorderEfect);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("TARJETA"):
                            #region Create Border
                            Border myBorderTarj = new Border()
                            {
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(2),
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Padding = new Thickness(0),
                                CornerRadius = new CornerRadius(5),
                                Margin = new Thickness(3)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonTarj = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir metodo de pago " + CreateNameButton,
                                Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonTarj.Click += new RoutedEventHandler(handlerTarj_Click);

                            StackPanel MyStackTarj = new StackPanel();

                            Tarj = CreateNameButton;

                            var packIconMaterialTarj = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.CreditCardOutline,
                                Width = 36,
                                Height = 36,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextTarj = new TextBlock()
                            {
                                Text = CreateNameButton/*"Tarjeta"*/,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 17
                            };
                            #endregion
                            #region Asing Childs
                            MyStackTarj.Children.Add(packIconMaterialTarj);
                            MyStackTarj.Children.Add(MyTextTarj);
                            MyButtonTarj.Content = MyStackTarj;
                            myBorderTarj.Child = MyButtonTarj;

                            PanelWrap.Children.Add(myBorderTarj);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("TRANSFERENCIA"):
                            #region Create Border
                            Border myBorderTrans = new Border()
                            {
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(2),
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Padding = new Thickness(0),
                                CornerRadius = new CornerRadius(5),
                                Margin = new Thickness(3)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonTrans = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir metodo de pago " + CreateNameButton,
                                Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonTrans.Click += new RoutedEventHandler(handlerTrans_Click);

                            StackPanel MyStackTrans = new StackPanel();

                            Trans = CreateNameButton;

                            var packIconMaterialTrans = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.BankTransfer,
                                Width = 36,
                                Height = 36,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextTrans = new TextBlock()
                            {
                                Text = /*"Transferencia"*/ CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 17
                            };
                            #endregion
                            #region Asing Childs
                            MyStackTrans.Children.Add(packIconMaterialTrans);
                            MyStackTrans.Children.Add(MyTextTrans);
                            MyButtonTrans.Content = MyStackTrans;
                            myBorderTrans.Child = MyButtonTrans;

                            PanelWrap.Children.Add(myBorderTrans);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("CHEQUE"):
                            #region Create Border
                            Border myBorderCk = new Border()
                            {
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(2),
                                HorizontalAlignment = HorizontalAlignment.Right,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Padding = new Thickness(0),
                                CornerRadius = new CornerRadius(5),
                                Margin = new Thickness(3)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonCk = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir metodo de pago " + CreateNameButton,
                                Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonCk.Click += new RoutedEventHandler(handlerCk_Click);

                            StackPanel MyStackCk = new StackPanel();

                            Ck = CreateNameButton;

                            var packIconMaterialCk = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.Bank,
                                Width = 36,
                                Height = 36,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextCk = new TextBlock()
                            {
                                Text = CreateNameButton/*"Cheque"*/,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 17
                            };
                            #endregion
                            #region Asing Childs
                            MyStackCk.Children.Add(packIconMaterialCk);
                            MyStackCk.Children.Add(MyTextCk);
                            MyButtonCk.Content = MyStackCk;
                            myBorderCk.Child = MyButtonCk;

                            PanelWrap.Children.Add(myBorderCk);
                            #endregion
                            break;
                    }
                }
            }
        }

        private void handlerEfect_Click(object sender, RoutedEventArgs e)
        {
            ValidatorActionEvent(Efect);
        }

        private void handlerTarj_Click(object sender, RoutedEventArgs e)
        {
            ValidatorActionEvent(Tarj);
        }

        private void handlerTrans_Click(object sender, RoutedEventArgs e)
        {
            ValidatorActionEvent(Trans);
        }

        private void handlerCk_Click(object sender, RoutedEventArgs e)
        {
            ValidatorActionEvent(Ck);
        }
        void ValidatorActionEvent(string Formapago)
        {
            txtMontoDistribuido.IsReadOnly = false;
            txtMontoDistribuido.Text = default;
            txtMontoRestante.Text = default;

            FormaPagoAplicada = Formapago;
            BtAceptarCancelar.Visibility = Visibility.Visible;
            PanelOpcionesPagos.Visibility = Visibility.Hidden;

            var FormasDePago = ListOfFormasPagos.Select(x => new { x.FormaPago, x.CantidadFormaPago }).Where(x => x.FormaPago == Formapago);//new ClassVariables().FormaPago;

            if (FormasDePago.Any())
            {
                txtMontoDistribuido.Text = FormasDePago.FirstOrDefault().CantidadFormaPago.ToString();
                lMonto.Text = (Convert.ToDouble(lMonto.Text) + Convert.ToDouble(txtMontoDistribuido.Text)).ToString();
                txtMontoDistribuido_KeyUp(this, null);
            }
        }

        private void PanelOpcionesPagos_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtMontoDistribuido_KeyDown(object sender, KeyEventArgs e)
        {
            ClassControl.ValidadorNumeros(e);
        }
    }
}
