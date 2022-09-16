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
    public partial class UscTextboxNumerico : UserControl
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
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        public int HeightLabel
        {
            get { return (int)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public string Number
        {
            get { return (string)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }
        public bool FieldDecimal
        {
            get { return (bool)GetValue(FieldDecimalProperty); }
            set { SetValue(FieldDecimalProperty, value); }
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
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxNumerico), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxNumerico), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscTextboxNumerico), new PropertyMetadata(new Thickness(15)));

        public static readonly DependencyProperty ColorCampoVacioProperty =
            DependencyProperty.Register(nameof(ColorCampoVacio), typeof(SolidColorBrush), typeof(UscTextboxNumerico), new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"))));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxNumerico), new PropertyMetadata(30));

        public static readonly DependencyProperty NumberProperty =
            DependencyProperty.Register(nameof(Number), typeof(string), typeof(UscTextboxNumerico), new PropertyMetadata(null));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscTextboxNumerico), new PropertyMetadata(true));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscTextboxNumerico), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty FieldDecimalProperty =
            DependencyProperty.Register(nameof(FieldDecimal), typeof(bool), typeof(UscTextboxNumerico), new PropertyMetadata(false));

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register(nameof(Mask), typeof(TextBoxMask), typeof(UscTextboxNumerico), new PropertyMetadata(TextBoxMask.Telefono));

        #endregion

        public UscTextboxNumerico() => InitializeComponent();

        private void Root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainText.TabIndex = root.TabIndex;
            MainText.Focus();
        }

        private void MainText_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);

        private void MainText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (FieldDecimal)
                ClassControl.CampoSoloPermiteDecimales(e);
            else
                ClassControl.CampoSoloPermiteNumeros(e);
        }

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscTextboxNumerico instance = dependencyObject as UscTextboxNumerico;

            instance.MainText.IsReadOnly = instance.EstadoMainWindows == "Modo Consulta" ? true : false;
        }


    }

}
