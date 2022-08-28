using Sadora.Clases;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.CustomElements
{
    public partial class UscComboBoxGeneral : UserControl
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
        public int HeightLabel
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public List<ComboBoxItem> Items
        {
            get { return (List<ComboBoxItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
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

        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscComboBoxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscComboBoxGeneral), new PropertyMetadata(new Thickness(15)));

        public static new readonly DependencyProperty HeightProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscComboBoxGeneral), new PropertyMetadata(27));

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register(nameof(Items), typeof(List<ComboBoxItem>), typeof(UscComboBoxGeneral), new PropertyMetadata(new List<ComboBoxItem>()));
        
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscComboBoxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscComboBoxGeneral), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscComboBoxGeneral), new PropertyMetadata(true));

        #endregion
        public UscComboBoxGeneral()
        {
            InitializeComponent();

            Items = new List<ComboBoxItem>();
        }

        private void Root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainCBX.TabIndex = root.TabIndex;
            MainCBX.Focus();
        }

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscComboBoxGeneral instance = dependencyObject as UscComboBoxGeneral;

            instance.MainCBX.IsEnabled = instance.EstadoMainWindows == "Modo Consulta" ? false : true;
        }

        private void MainCBX_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);
    }
}
