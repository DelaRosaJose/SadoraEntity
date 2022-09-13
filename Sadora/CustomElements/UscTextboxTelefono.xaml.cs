using Sadora.Clases;
using System;
using System.Collections;
using System.Data.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sadora.CustomElements
{
    public partial class UscTextboxTelefono : UserControl
    {

        #region Creacion de Propiedades
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public Thickness MarginBorder
        {
            get { return (Thickness)GetValue(MarginBorderProperty); }
            set { SetValue(MarginBorderProperty, value); }
        }
        public SolidColorBrush ColorCampoVacio
        {
            get { return (SolidColorBrush)GetValue(ColorCampoVacioProperty); }
            set { SetValue(ColorCampoVacioProperty, value); }
        }
        public int HeightLabel
        {
            get { return (int)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string EstadoMainWindows
        {
            get { return (string)GetValue(EstadoMainWindowsProperty); }
            set { SetValue(EstadoMainWindowsProperty, value); }
        }
        public bool EnterPasarProximoCampo
        {
            get { return (bool)GetValue(EnterPasarProximoCampoProperty); }
            set { SetValue(EnterPasarProximoCampoProperty, value); }
        }
        public TextBoxMask Mask
        {
            get { return (TextBoxMask)GetValue(MaskProperty); }
            set { SetValue(MaskProperty, value); }
        }

        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxTelefono), new PropertyMetadata(null));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscTextboxTelefono), new PropertyMetadata(new Thickness(15)));

        public static readonly DependencyProperty ColorCampoVacioProperty =
            DependencyProperty.Register(nameof(ColorCampoVacio), typeof(SolidColorBrush), typeof(UscTextboxTelefono), new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"))));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxTelefono), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxTelefono), new PropertyMetadata(null));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscTextboxTelefono), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscTextboxTelefono), new PropertyMetadata(true));

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register(nameof(Mask), typeof(TextBoxMask), typeof(UscTextboxTelefono), new PropertyMetadata(TextBoxMask.Telefono));

        #endregion

        public UscTextboxTelefono() => InitializeComponent();

        private void Root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainText.TabIndex = root.TabIndex;
            MainText.Focus();
        }

        private void MainText_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);

        private void MainText_PreviewKeyDown(object sender, KeyEventArgs e) => ClassControl.CampoSoloPermiteNumeros(e);

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscTextboxTelefono instance = dependencyObject as UscTextboxTelefono;

            instance.MainText.IsReadOnly = instance.EstadoMainWindows == "Modo Consulta" ? true : false;
        }
        
        private void MainText_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainText.CaretIndex = MainText.Text.Length;

            var tbEntry = sender as TextBox;

            if (tbEntry != null && tbEntry.Text.Length > 0)
                tbEntry.Text = formatNumber(tbEntry.Text, Mask);
        }

        public static string formatNumber(string MaskedNum, TextBoxMask phoneFormat)
        {
            int x;
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            if (MaskedNum != null)
            {
                for (int i = 0; i < MaskedNum.Length; i++)
                {
                    if (int.TryParse(MaskedNum.Substring(i, 1), out x))
                        sb.Append(x.ToString());
                }
                switch (phoneFormat)
                {
                    case TextBoxMask.Telefono:
                        return FormatForTelefono(sb.ToString()).ToString();

                    //case TextBoxMask.Cedula:
                    //    return FormatForCedula(sb.ToString()).ToString();

                    default:
                        break;
                }

            }
            return sb.ToString();
        }

        static StringBuilder FormatForTelefono(string sb)
        {
            StringBuilder sb2 = new StringBuilder();

            if (sb.Length > 0) sb2.Append("(");

            if (sb.Length > 0) sb2.Append(sb.Substring(0, 1));
            if (sb.Length > 1) sb2.Append(sb.Substring(1, 1));
            if (sb.Length > 2) sb2.Append(sb.Substring(2, 1));

            if (sb.Length > 3) sb2.Append(") ");

            if (sb.Length > 3) sb2.Append(sb.Substring(3, 1));
            if (sb.Length > 4) sb2.Append(sb.Substring(4, 1));
            if (sb.Length > 5) sb2.Append(sb.Substring(5, 1));

            if (sb.Length > 6) sb2.Append("-");

            if (sb.Length > 6) sb2.Append(sb.Substring(6, 1));
            if (sb.Length > 7) sb2.Append(sb.Substring(7, 1));
            if (sb.Length > 8) sb2.Append(sb.Substring(8, 1));
            if (sb.Length > 9) sb2.Append(sb.Substring(9, 1));

            return sb2;
        }

        //static StringBuilder FormatForCedula(string sb)
        //{
        //    StringBuilder sb2 = new StringBuilder();

        //    if (sb.Length > 0) sb2.Append(sb.Substring(0, 1));
        //    if (sb.Length > 1) sb2.Append(sb.Substring(1, 1));
        //    if (sb.Length > 2) sb2.Append(sb.Substring(2, 1));

        //    if (sb.Length > 3) sb2.Append("-");

        //    if (sb.Length > 3) sb2.Append(sb.Substring(3, 1));
        //    if (sb.Length > 4) sb2.Append(sb.Substring(4, 1));

        //    if (sb.Length > 5) sb2.Append(sb.Substring(5, 1));
        //    if (sb.Length > 6) sb2.Append(sb.Substring(6, 1));
        //    if (sb.Length > 7) sb2.Append(sb.Substring(7, 1));
        //    if (sb.Length > 8) sb2.Append(sb.Substring(8, 1));
        //    if (sb.Length > 9) sb2.Append(sb.Substring(9, 1));

        //    if (sb.Length > 10) sb2.Append("-");

        //    if (sb.Length > 10) sb2.Append(sb.Substring(10, 1));

        //    return sb2;
        //}
    }
    public enum TextBoxMask
    {
        Telefono//,
        //Cedula
    }

}
