using Sadora.Clases;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sadora.CustomElements
{
    /// <summary>
    /// Lógica de interacción para UscTextboxGeneral.xaml
    /// </summary>
    public partial class UscTextboxGeneral : UserControl
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
        public bool FieldNumeric
        {
            get { return (bool)GetValue(FieldNumericProperty); }
            set { SetValue(FieldNumericProperty, value); }
        }


        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(UscTextboxGeneral),
                new PropertyMetadata(null));
        
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(UscTextboxGeneral),
                new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register("MarginBorder", typeof(int), typeof(UscTextboxGeneral),
                new PropertyMetadata(15));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("HeightLabel", typeof(int), typeof(UscTextboxGeneral),
                new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UscTextboxGeneral),
                new PropertyMetadata(null));
        public static readonly DependencyProperty FieldNumericProperty =
            DependencyProperty.Register("FieldNumeric", typeof(bool), typeof(UscTextboxGeneral),
                new PropertyMetadata(false));



        //public static readonly RoutedEvent JoinKeyUpEvent =
        //    EventManager.RegisterRoutedEvent(nameof(JoinKeyUp), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TierCard));
        
        //public static readonly RoutedEvent JoinKeyUpEvent =
        //    EventManager.RegisterRoutedEvent(nameof(JoinKeyUp), RoutingStrategy.Bubble, typeof(KeyEventHandler), typeof(UscTextboxGeneral));
        

        public UscTextboxGeneral()
        {
            InitializeComponent();
        }

        //private void Field_KeyUp(object sender, KeyEventArgs e)
        //{
        //    //base.OnKeyUp(e);
        //    RaiseEvent(new RoutedEventArgs(JoinKeyUpEvent));
        //}

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
            if (FieldNumeric)
                ClassControl.ValidadorNumeros(e);
        }
    }
}
