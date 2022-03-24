using Sadora.Clases;
using Sadora.Properties;
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

namespace Sadora.Ventas
{
    /// <summary>
    /// Lógica de interacción para FrmControlComprobantes.xaml
    /// </summary>
    public partial class FrmControlComprobantes : Window
    {
        //public bool Resultado;
        private int ClienteID;
        //private int ClaseComprobanteId;
        private bool NotData = false;
        string FormaPago;
        double MontoPagar = 0;
        ClassVariables ClasesVariab;
        #region Variables para Eventos
        string CompFiscal = "";
        string CompConsumo = "";
        string CompGubernamental = "";
        string CompRegimenEspecial = "";
        string SinComp = "";
        #endregion

        public FrmControlComprobantes()
        { }

        public FrmControlComprobantes(int clienteID, string formaPago, double montoPagar, ClassVariables ClasesVariables)
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;

            FormaPago = formaPago;

            ClienteID = clienteID != 1 ? clienteID : 0;

            MontoPagar = montoPagar;
            ClasesVariab = ClasesVariables;

            if (ClienteID != 0)
                txtRNC.IsReadOnly = true;


            ControlEvent();
            //ClaseComprobanteId = clienteID == 1 ? 1 : default;

            //ClienteID = clienteID;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FindRazonSocial();
        }

        private void ButtonCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnValorFiscal_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(1);
            PutTextbox(1);
        }

        private void BtnValorGubernamental_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(4);
            PutTextbox(4);
        }

        private void BtnConsumidorFinal_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(2);
            PutTextbox(2);
        }

        private void BtnRegimenEspecial_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(3);
            PutTextbox(3);
        }

        //void FindNCF()
        //{
        //    DataTable reader = Clases.ClassData.runDataTable("select RNC, RazonSocial from getCliente('" + txtRNC.Text + "'," + ClienteID + ")", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.
        //    if ((ClienteID != 0 && txtRNC.Text == "") || (ClienteID != 0 && txtRNC.Text == null))
        //    {
        //        if (reader.Rows.Count == 1)
        //        {
        //            txtRazonSocial.Text = reader.Rows[0]["RazonSocial"].ToString();
        //            txtRNC.Text = reader.Rows[0]["RNC"].ToString();
        //        }
        //    }
        //    else
        //    {
        //        if (reader.Rows.Count == 1)
        //        {
        //            txtRazonSocial.Text = reader.Rows[0]["RazonSocial"].ToString();
        //            txtRNC.Text = reader.Rows[0]["RNC"].ToString();
        //        }
        //        else if (reader.Rows.Count == 0)
        //        {
        //            if (SnackbarThree.MessageQueue is { } messageQueue)
        //            {
        //                Task.Factory.StartNew(() => messageQueue.Enqueue("No se encontraron datos"));
        //            }
        //        }
        //        else
        //        {
        //            if (SnackbarThree.MessageQueue is { } messageQueue)
        //            {
        //                Task.Factory.StartNew(() => messageQueue.Enqueue("Mas de una razon social con este RNC, comuniquese con su proveedor de servicios."));
        //            }
        //        }
        //    }
        //}

        void PutTextbox(int value, bool pass = false)
        {
            if ((ClienteID == 0 && !NotData) || pass)
            {
                if (value == 1)
                    TabItem.Header = "Valor Fiscal";
                else if (value == 2)
                    TabItem.Header = "Consumidor Final";
                else if (value == 3)
                    TabItem.Header = "Regimen Especial";
                else if (value == 4)
                    TabItem.Header = "Valor Gubernamental";
            }
        }

        void FindRazonSocial(int TipoComprobante = 0)
        {
            DataTable reader = Clases.ClassData.runDataTable("select RNC, RazonSocial from getCliente('" + txtRNC.Text + "'," + ClienteID + ")", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.
            if ((ClienteID != 0 && txtRNC.Text == "") || (ClienteID != 0 && txtRNC.Text == null) || (ClienteID == 0 && (txtRNC.Text.Length == 9 || txtRNC.Text.Length == 11)))
            {
                if (reader.Rows.Count == 1)
                {
                    txtRazonSocial.Text = reader.Rows[0]["RazonSocial"].ToString();
                    txtRNC.Text = reader.Rows[0]["RNC"].ToString();
                    NotData = false;
                }
            }
            else
            {
                if (reader.Rows.Count == 1)
                {
                    txtRazonSocial.Text = reader.Rows[0]["RazonSocial"].ToString();
                    txtRNC.Text = reader.Rows[0]["RNC"].ToString();
                }
                else if (reader.Rows.Count == 0)
                {
                    if (ClienteID != 0 || (txtRNC.Text.Length == 11 && txtRNC.Text.Length == 9))
                    {
                        if (SnackbarThree.MessageQueue is { } messageQueue)
                        {
                            Task.Factory.StartNew(() => messageQueue.Enqueue("No se encontraron datos"));
                            NotData = true;
                        }
                    }
                    else
                    {
                        if (SnackbarThree.MessageQueue is { } messageQueue)
                        {
                            Task.Factory.StartNew(() => messageQueue.Enqueue("RNC o cedula incorrecta"));
                            NotData = true;
                        }
                    }
                }
                else
                {
                    if (SnackbarThree.MessageQueue is { } messageQueue)
                    {
                        Task.Factory.StartNew(() => messageQueue.Enqueue("Mas de una razon social con este RNC, comuniquese con su proveedor de servicios."));
                    }
                }
            }

            string ClassComprobante = null;

            if (ClienteID != 0)
            {
                DataTable Result = Clases.ClassData.runDataTable("select * from TcliClientes where ClienteID = " + ClienteID, null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.
                                                                                                                                                    //{
                if (Result.Rows.Count == 1)
                    ClassComprobante = Result.Rows[0]["ClaseComprobanteID"].ToString();

                Result.Clear();
                Result.Dispose();
                ClassControl.setValidador("Select NCF as Nombre from getNextNCF(" + ClassComprobante + ",NULL) --", null, txtNCF, true);
                ClasesVariab.ClaseNCFDinamic = ClassControl.setPropBinding("Select ClaseID as Nombre from getNextNCF(" + ClassComprobante + ",NULL) --", null, true);
                if (txtNCF.Text.ToUpper().Contains("B01"))
                    PutTextbox(1, true);
                else if (txtNCF.Text.ToUpper().Contains("B02"))
                    PutTextbox(2, true);
                else if (txtNCF.Text.ToUpper().Contains("B14"))
                    PutTextbox(3, true);
                else if (txtNCF.Text.ToUpper().Contains("B15"))
                    PutTextbox(4, true);

                ClasesVariab.ClienteDinamic = txtRazonSocial.Text;
                txtFechaVencimiento.Text = "12/31/2021";
                getFinalView();
            }
            else if (TipoComprobante != 0 && !string.IsNullOrWhiteSpace(txtRNC.Text) && (txtRNC.Text.Length == 11 || txtRNC.Text.Length == 9))
            {
                ClassControl.setValidador("Select NCF as Nombre from getNextNCF(" + TipoComprobante + ",NULL) --", null, txtNCF, true);
                ClasesVariab.ClaseNCFDinamic = ClassControl.setPropBinding("Select ClaseID as Nombre from getNextNCF(" + TipoComprobante + ",NULL) --", null, true);
                if (txtNCF.Text.ToUpper().Contains("B01"))
                    PutTextbox(1);
                else if (txtNCF.Text.ToUpper().Contains("B02"))
                    PutTextbox(2);
                else if (txtNCF.Text.ToUpper().Contains("B14"))
                    PutTextbox(3);
                else if (txtNCF.Text.ToUpper().Contains("B15"))
                    PutTextbox(4);

                ClasesVariab.ClienteDinamic = txtRazonSocial.Text + " (CM)";
                txtFechaVencimiento.Text = "12/31/2021";
                getFinalView();
            }
        }

        void getFinalView()
        {
            if (string.IsNullOrWhiteSpace(txtNCF.Text) || string.IsNullOrWhiteSpace(txtRazonSocial.Text) || string.IsNullOrWhiteSpace(txtRNC.Text) || string.IsNullOrWhiteSpace(txtFechaVencimiento.Text) || TabItem.Header == "Seleccionar Tipo de comprobante")
            {
                if (SnackbarThree.MessageQueue is { } messageQueue)
                {
                    Task.Factory.StartNew(() => messageQueue.Enqueue("No se puede pasar a ventana desglose de metodo de pago"));
                }
            }
            else
            {

                ClasesVariab.RNCDinamic = txtRNC.Text;
                ClasesVariab.NCFDinamic = txtNCF.Text;
                this.Close();
                new FrmControlFormaPago(FormaPago, MontoPagar).ShowDialog();

            }
        }

        private void txtRNC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FindRazonSocial();
        }

        private void ControlEvent()
        {
            string CreateNameButton = "";

            DataTable MetodoCaja = Clases.ClassData.runDataTable("select * from TconComprobantes", null, "CommandText"); //En esta linea de codigo estamos ejecutando un metodo que recibe una consulta, la busca en sql y te retorna el resultado en un datareader.
            for (int i = 0; i < MetodoCaja.Rows.Count; i++)
            {
                CreateNameButton = MetodoCaja.Columns.Contains("Nombre") ? MetodoCaja.Rows[i]["Nombre"].ToString() : null;
                if (!string.IsNullOrWhiteSpace(CreateNameButton))
                {
                    switch (CreateNameButton)
                    {
                        case string a when a.ToUpper().Contains("VALOR FISCAL"):
                            #region Create Border
                            Border myBorderCompFiscal = new Border()
                            {
                                Padding = new Thickness(0),
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(1.5),
                                CornerRadius = new CornerRadius(5),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Margin = new Thickness(20, 20, 20, 2)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonCompFiscal = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir tipo de comprobante valor fiscal"/* + CreateNameButton*/,
                                //Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonCompFiscal.Click += new RoutedEventHandler(handlerCompFiscal_Click);

                            StackPanel MyStackCompFiscal = new StackPanel() { Orientation = Orientation.Horizontal };

                            CompFiscal = CreateNameButton;

                            var packIconMaterialCompFiscal = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.FileChartOutline,
                                Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF93636"),
                                Margin = new Thickness(0, 0, 20, 0),
                                Width = 45,
                                Height = 45,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextCompFiscal = new TextBlock()
                            {
                                Text = "Valor Fiscal",//CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 40
                            };
                            #endregion
                            #region Asing Childs
                            MyStackCompFiscal.Children.Add(packIconMaterialCompFiscal);
                            MyStackCompFiscal.Children.Add(MyTextCompFiscal);
                            MyButtonCompFiscal.Content = MyStackCompFiscal;
                            myBorderCompFiscal.Child = MyButtonCompFiscal;

                            PanelWrap.Children.Add(myBorderCompFiscal);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("CONSUMIDOR") || a.ToUpper().Contains("CONSUMO"):
                            #region Create Border
                            Border myBorderCompConsumo = new Border()
                            {
                                Padding = new Thickness(0),
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(1.5),
                                CornerRadius = new CornerRadius(5),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Margin = new Thickness(20, 20, 20, 2)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonCompConsumo = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir tipo de comprobante consumidor final"/* + CreateNameButton*/,
                                //Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonCompConsumo.Click += new RoutedEventHandler(handlerCompConsumo_Click);

                            StackPanel MyStackCompConsumo = new StackPanel() { Orientation = Orientation.Horizontal };

                            CompConsumo = CreateNameButton;

                            var packIconMaterialCompConsumo = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.FileChartOutline,
                                Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF93636"),
                                Margin = new Thickness(0, 0, 20, 0),
                                Width = 45,
                                Height = 45,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextCompConsumo = new TextBlock()
                            {
                                Text = "Consumidor Final",//CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 40
                            };
                            #endregion
                            #region Asing Childs
                            MyStackCompConsumo.Children.Add(packIconMaterialCompConsumo);
                            MyStackCompConsumo.Children.Add(MyTextCompConsumo);
                            MyButtonCompConsumo.Content = MyStackCompConsumo;
                            myBorderCompConsumo.Child = MyButtonCompConsumo;

                            PanelWrap.Children.Add(myBorderCompConsumo);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("GUBERNAMENTAL"):
                            #region Create Border
                            Border myBorderCompGubernamental = new Border()
                            {
                                Padding = new Thickness(0),
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(1.5),
                                CornerRadius = new CornerRadius(5),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Margin = new Thickness(20, 20, 20, 2)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonCompGubernamental = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir tipo de comprobante valor gubernamental"/* + CreateNameButton*/,
                                //Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonCompGubernamental.Click += new RoutedEventHandler(handlerCompGubernamental_Click);

                            StackPanel MyStackCompGubernamental = new StackPanel() { Orientation = Orientation.Horizontal };

                            CompGubernamental = CreateNameButton;

                            var packIconMaterialCompGubernamental = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.FileChartOutline,
                                Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF93636"),
                                Margin = new Thickness(0, 0, 20, 0),
                                Width = 45,
                                Height = 45,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextCompGubernamental = new TextBlock()
                            {
                                Text = "Regimen Especial",//CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 40
                            };
                            #endregion
                            #region Asing Childs
                            MyStackCompGubernamental.Children.Add(packIconMaterialCompGubernamental);
                            MyStackCompGubernamental.Children.Add(MyTextCompGubernamental);
                            MyButtonCompGubernamental.Content = MyStackCompGubernamental;
                            myBorderCompGubernamental.Child = MyButtonCompGubernamental;

                            PanelWrap.Children.Add(myBorderCompGubernamental);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("REGIMENESPECIAL"):
                            #region Create Border
                            Border myBorderCompRegimenEspecial = new Border()
                            {
                                Padding = new Thickness(0),
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(1.5),
                                CornerRadius = new CornerRadius(5),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Margin = new Thickness(20, 20, 20, 2)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonCompRegimenEspecial = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir tipo de comprobante valor gubernamental"/* + CreateNameButton*/,
                                //Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonCompRegimenEspecial.Click += new RoutedEventHandler(handlerCompRegimenEspecial_Click);

                            StackPanel MyStackCompRegimenEspecial = new StackPanel() { Orientation = Orientation.Horizontal };

                            CompRegimenEspecial = CreateNameButton;

                            var packIconMaterialCompRegimenEspecial = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.FileChartOutline,
                                Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF93636"),
                                Margin = new Thickness(0, 0, 20, 0),
                                Width = 45,
                                Height = 45,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextCompRegimenEspecial = new TextBlock()
                            {
                                Text = "Regimen Especial",//CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 40
                            };
                            #endregion
                            #region Asing Childs
                            MyStackCompRegimenEspecial.Children.Add(packIconMaterialCompRegimenEspecial);
                            MyStackCompRegimenEspecial.Children.Add(MyTextCompRegimenEspecial);
                            MyButtonCompRegimenEspecial.Content = MyStackCompRegimenEspecial;
                            myBorderCompRegimenEspecial.Child = MyButtonCompRegimenEspecial;

                            PanelWrap.Children.Add(myBorderCompRegimenEspecial);
                            #endregion
                            break;
                        case string a when a.ToUpper().Contains("SIN") && (MetodoCaja.Columns.Contains("SinComprobantes") ? (MetodoCaja.Rows[i]["SinComprobantes"].ToString() == "True" ? true : false) : false):
                            #region Create Border
                            Border myBorderSinComp = new Border()
                            {
                                Padding = new Thickness(0),
                                BorderBrush = (Brush)Application.Current.FindResource("PrimaryHueDarkBrush"),
                                BorderThickness = new Thickness(1.5),
                                CornerRadius = new CornerRadius(5),
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                VerticalAlignment = VerticalAlignment.Stretch,
                                Margin = new Thickness(20, 20, 20, 2)
                            };
                            #endregion
                            #region Create Button
                            Button MyButtonSinComp = new Button()
                            {
                                Padding = new Thickness(0),
                                Height = 59,
                                MinWidth = 100,
                                //Width = 100,
                                ToolTip = "Pulsar para elegir sin comprobantes"/* + CreateNameButton*/,
                                //Background = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#33C8C8C8"),
                                Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style
                            };
                            #endregion
                            #region Asing event, Create Stack and Icon with text
                            MyButtonSinComp.Click += new RoutedEventHandler(handlerSinComp_Click);

                            StackPanel MyStackSinComp = new StackPanel() { Orientation = Orientation.Horizontal };

                            SinComp = CreateNameButton;

                            var packIconMaterialSinComp = new MaterialDesignThemes.Wpf.PackIcon()
                            {
                                Kind = MaterialDesignThemes.Wpf.PackIconKind.FileChartOutline,
                                Foreground = (Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#FFF93636"),
                                Margin = new Thickness(0, 0, 20, 0),
                                Width = 45,
                                Height = 45,
                                HorizontalAlignment = HorizontalAlignment.Center
                            };

                            TextBlock MyTextSinComp = new TextBlock()
                            {
                                Text = "Sin Comprobantes",//CreateNameButton,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                FontSize = 40
                            };
                            #endregion
                            #region Asing Childs
                            MyStackSinComp.Children.Add(packIconMaterialSinComp);
                            MyStackSinComp.Children.Add(MyTextSinComp);
                            MyButtonSinComp.Content = MyStackSinComp;
                            myBorderSinComp.Child = MyButtonSinComp;

                            PanelWrap.Children.Add(myBorderSinComp);
                            #endregion
                            break;

                    }
                }
            }
            //}
        }

        private void handlerCompFiscal_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(1);
            PutTextbox(1);
        }

        private void handlerCompRegimenEspecial_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(3);
            PutTextbox(3);
        }

        private void handlerCompConsumo_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(2);
            PutTextbox(2);
        }

        private void handlerCompGubernamental_Click(object sender, RoutedEventArgs e)
        {
            FindRazonSocial(4);
            PutTextbox(4);
        }

        private void handlerSinComp_Click(object sender, RoutedEventArgs e)
        {
            new Administracion.FrmValidarAccion("Esta seguro que desea registrar esta factura sin comprobantes?").ShowDialog();
            if (ClassVariables.ValidarAccion)
            {
                this.Close();
                new FrmControlFormaPago(FormaPago, MontoPagar).ShowDialog();


                //new Administracion.FrmCompletarCamposHost("Devuelta: " + txtMontoRestante.Text.Replace("-", "")).ShowDialog();
                //ClassVariables.IsFullFormaPago = true;
                //this.Close();
            }

            //FindRazonSocial(0);
            //PutTextbox(0);
        }


    }
}
