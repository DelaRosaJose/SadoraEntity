using Sadora.Clases;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.CustomElements
{
    public partial class UscDatePickerGeneral : UserControl
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
        public int HeightLabel
        {
            get { return (int)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
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

        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscDatePickerGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscDatePickerGeneral), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscDatePickerGeneral), new PropertyMetadata(new Thickness(15)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscDatePickerGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(UscDatePickerGeneral), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscDatePickerGeneral), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscDatePickerGeneral), new PropertyMetadata(true));

        #endregion

        public UscDatePickerGeneral() => InitializeComponent();

        private void Root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainDP.TabIndex = root.TabIndex;
            MainDP.Focus();
        }

        private void MainDP_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscDatePickerGeneral instance = dependencyObject as UscDatePickerGeneral;

            instance.MainDP.IsEnabled = instance.EstadoMainWindows == "Modo Consulta" ? false : true;
        }



    }
}
