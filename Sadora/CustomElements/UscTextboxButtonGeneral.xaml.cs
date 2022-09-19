using Sadora.Clases;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Sadora.CustomElements
{
    public partial class UscTextboxButtonGeneral : UserControl
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
        public string BuscarPorTabla
        {
            get { return (string)GetValue(SearchByTableProperty); }
            set { SetValue(SearchByTableProperty, value); }
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
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(2));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscTextboxButtonGeneral), new PropertyMetadata(new Thickness(15)));

        public static readonly DependencyProperty ColorCampoVacioProperty =
            DependencyProperty.Register(nameof(ColorCampoVacio), typeof(SolidColorBrush), typeof(UscTextboxButtonGeneral), new PropertyMetadata((SolidColorBrush)(new BrushConverter().ConvertFrom("#4CEDEDED"))));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty SearchByTableProperty =
            DependencyProperty.Register(nameof(BuscarPorTabla), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null, EstadoMainWindowsPropertyChanged));

        public static readonly DependencyProperty EnterPasarProximoCampoProperty =
            DependencyProperty.Register(nameof(EnterPasarProximoCampo), typeof(bool), typeof(UscTextboxButtonGeneral), new PropertyMetadata(true));

        private static void EstadoMainWindowsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ea)
        {
            UscTextboxButtonGeneral instance = dependencyObject as UscTextboxButtonGeneral;

            instance.MainText.IsReadOnly = instance.EstadoMainWindows == "Modo Consulta" ? true : false;
        }

        #endregion

        public UscTextboxButtonGeneral() => InitializeComponent();

        private void btnSearch_Click(object sender, RoutedEventArgs e) => ProcesadorCampo();

        private void MainText_TextChanged(object sender, TextChangedEventArgs e) => ProcesadorCampo(e);

        async void ProcesadorCampo(TextChangedEventArgs e = null)
        {
            if (EstadoMainWindows == default)
                return;

            try
            {
                if (EstadoMainWindows != "Modo Consulta" || e != default)
                {
                    string ValueColumn = e != null ? (e.Source as TextBox).Text : default;

                    if (new string[] { "", "0", string.Empty }.Contains(ValueColumn))
                    {
                        ResultText.Text = string.Empty;
                        return;
                    }

                    using (Models.SadoraEntity db = new Models.SadoraEntity())
                    {
                        var ColumnAsync = db.Database.SqlQuery<string>($"select top 1 COLUMN_NAME from Information_Schema.COLUMNS " +
                        $"where TABLE_NAME = '{BuscarPorTabla}' " +
                        $"and COLUMN_NAME not in ('RowID', 'UsuarioID') " +
                        $"and COLUMN_NAME like '%ID'").FirstOrDefaultAsync();

                        if (e == default)
                        {
                            Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost($"Select * from {BuscarPorTabla}", null);
                            frm.ShowDialog();

                            if (frm.GridMuestra.SelectedItem != null)
                            {
                                DataRowView item = frm.GridMuestra.SelectedItem as DataRowView;
                                ValueColumn = MainText.Text = item.Row.ItemArray[0].ToString();
                            }
                        }

                        ValueColumn =
                            int.TryParse(ValueColumn, out int intValue) ? ValueColumn :
                            new string[] {string.Empty, null}.Contains(Text) ? 0.ToString() : Text;

                        ResultText.Text = await db.Database.SqlQuery<string>($"select Nombre from {BuscarPorTabla} where {await ColumnAsync} = {ValueColumn}").FirstOrDefaultAsync();

                    }
                }
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }

        private void root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainText.TabIndex = root.TabIndex;
            MainText.Focus();
        }

        private void MainText_KeyUp(object sender, KeyEventArgs e) => ClassControl.PasarConEnterProximoCampo(e, EstadoMainWindows, EnterPasarProximoCampo);

        private void MainText_PreviewKeyDown(object sender, KeyEventArgs e) => ClassControl.CampoSoloPermiteNumeros(e);


    }
}
