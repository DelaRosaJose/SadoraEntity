using Sadora.Clases;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sadora.CustomElements
{
    public partial class UscTextboxButtonGeneral : UserControl
    {
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
        public event RoutedEventHandler SearchClick
        {
            add { AddHandler(SearchClickEvent, value); }
            remove { RemoveHandler(SearchClickEvent, value); }
        }
        //public bool FieldNumeric
        //{
        //    get { return (bool)GetValue(FieldNumericProperty); }
        //    set { SetValue(FieldNumericProperty, value); }
        //}

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(15));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly RoutedEvent SearchClickEvent =
            EventManager.RegisterRoutedEvent(nameof(SearchClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UscTextboxButtonGeneral));
         
        //public static readonly DependencyProperty FieldNumericProperty =
        //    DependencyProperty.Register("FieldNumeric", typeof(bool), typeof(UscTextboxGeneral), new PropertyMetadata(false));

        public UscTextboxButtonGeneral()
        {
            InitializeComponent();
        }

        //private void root_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (FieldNumeric)
        //        ClassControl.ValidadorNumeros(e);
        //}

        private void txtFieldID_KeyDown(object sender, KeyEventArgs e) => ClassControl.ValidadorNumeros(e);

        private void btnSearch_Click(object sender, RoutedEventArgs e) => RaiseEvent(new RoutedEventArgs(SearchClickEvent));
    }
}
