using Sadora.Clases;
using System;
using System.Collections;
using System.Data.Linq;
using System.Reflection;
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
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscTextboxGeneral), new PropertyMetadata(new Thickness(15)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null, TextPropertyChanged));

        public static readonly DependencyProperty FieldNumericProperty =
            DependencyProperty.Register(nameof(FieldNumeric), typeof(bool), typeof(UscTextboxGeneral), new PropertyMetadata(false));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscTextboxGeneral), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscTextboxGeneral), new PropertyMetadata(true));

        #endregion

        public UscTextboxGeneral() => InitializeComponent();

        private void Root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainText.TabIndex = root.TabIndex;
            MainText.Focus();
        }

        private void MainText_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);

        private void MainText_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (FieldNumeric)
                ClassControl.CampoSoloPermiteNumeros(e);
        }

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscTextboxGeneral instance = dependencyObject as UscTextboxGeneral;

            instance.MainText.IsReadOnly = instance.EstadoMainWindows == "Modo Consulta" ? true : false;
        }
        
        private static void TextPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscTextboxGeneral instance = dependencyObject as UscTextboxGeneral;

            var datacontex = instance.DataContext;
            System.Type types = datacontex.GetType();
            string typeq = datacontex.GetType().Name;
            var typefull = datacontex.GetType().FullName;
            ListToDataTable(datacontex);



            var his = instance.GetType() == Type.GetType("System.Int32");


            var his2 = datacontex.GetType() == Type.GetType("System.Int32");

            var obj222 = new Models.TcliCliente();

            var obj = datacontex;

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {

                foreach (PropertyInfo propss in prop.GetType().GetProperties())
                { 
                
                }

                    if (prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                {
                    //Console.Writeline("This prop's type is Ienumerable");
                }
            }


            //var ele = (datacontex as (types as System.Type)).  

            //    DataTable dataTable = new DataTable(typeof(T).Name);


            //instance.MainText.IsReadOnly = instance.EstadoMainWindows == "Modo Consulta" ? true : false;
        }
        public static void ListToDataTable<DataContext>(DataContext items)
        {
            //DataTable dataTable = new DataTable(typeof(T).Name);
            //PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            Type contextType = items.GetType();

            PropertyInfo providerProperty = contextType.GetProperty("Provider", BindingFlags.Instance | BindingFlags.NonPublic);
            if (providerProperty == null)
                return;

            var hi = providerProperty.GetValue(items, null);

            //var items2 = (IQueryable)db.GetType().GetMethod("GetTable").MakeGenericMethod(dbType).Invoke(db, null);

            //return GetData(items2, columns, page, count, filter, sort, sortdir);




            //foreach (PropertyInfo prop in Props)
            //    dataTable.Columns.Add(prop.Name);

            //foreach (T item in items)
            //{
            //    var values = new object[Props.Length];
            //    for (int i = 0; i < Props.Length; i++)
            //        values[i] = Props[i].GetValue(item, null);

            //    dataTable.Rows.Add(values);
            //}

            //return dataTable;
        }

    }
}
