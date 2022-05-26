using Sadora.Clases;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.CustomElements
{
    public partial class UscCheckBoxGeneral : UserControl
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
        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }
        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscCheckBoxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscCheckBoxGeneral), new PropertyMetadata(new Thickness(15,34,15,15)));

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(UscCheckBoxGeneral), new PropertyMetadata(false));

        #endregion

        public UscCheckBoxGeneral() => InitializeComponent();

        private void root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainCheck.TabIndex = root.TabIndex;
            MainCheck.Focus();
        }
    }
}
