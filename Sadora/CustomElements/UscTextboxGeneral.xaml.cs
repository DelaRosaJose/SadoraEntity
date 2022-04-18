using Sadora.Clases;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public int MarginBorder
        {
            get { return (int)GetValue(MarginBorderProperty); }
            set { SetValue(MarginBorderProperty, value); }
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
        public bool FieldNumeric
        {
            get { return (bool)GetValue(FieldNumericProperty); }
            set { SetValue(FieldNumericProperty, value); }
        }

        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(15));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty FieldNumericProperty =
            DependencyProperty.Register(nameof(FieldNumeric), typeof(bool), typeof(UscTextboxGeneral), new PropertyMetadata(false));

        #endregion

        public UscTextboxGeneral() => InitializeComponent();

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
            if (FieldNumeric)
                ClassControl.ValidadorNumeros(e);
        }
    }
}
