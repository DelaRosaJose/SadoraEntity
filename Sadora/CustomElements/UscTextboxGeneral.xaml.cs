using Sadora.Clases;
using System;
using System.Collections;
using System.Data.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sadora.CustomElements
{
    public partial class UscTextboxGeneral : UserControl
    {
        #region Creacion de Propiedades
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
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
            set { SetValue(EstadoMainWindowsProperty, value);}
        }
        public bool EnterPasarProximoCampo
        {
            get { return (bool)GetValue(EnterPasarProximoCampoProperty); }
            set { SetValue(EnterPasarProximoCampoProperty, value); }
        }
        public bool GuardarCampoVacio
        {
            get { return (bool)GetValue(GuardarCampoVacioProperty); }
            set { SetValue(GuardarCampoVacioProperty, value); }
        }

        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscTextboxGeneral), new PropertyMetadata(new Thickness(15)));
        
        public static readonly DependencyProperty ColorCampoVacioProperty =
            DependencyProperty.Register(nameof(ColorCampoVacio), typeof(SolidColorBrush), typeof(UscTextboxGeneral), new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"))));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscTextboxGeneral), new PropertyMetadata(true));
        
        public static readonly DependencyProperty GuardarCampoVacioProperty =
            DependencyProperty.Register(nameof(GuardarCampoVacio), typeof(bool), typeof(UscTextboxGeneral), new PropertyMetadata(false));

        #endregion

        public UscTextboxGeneral() => InitializeComponent();

        private void Root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainText.TabIndex = root.TabIndex;
            MainText.Focus();
        }

        private void MainText_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscTextboxGeneral instance = dependencyObject as UscTextboxGeneral;

            instance.MainText.IsReadOnly = instance.EstadoMainWindows == "Modo Consulta" ? true : false;
        }

    }
}
