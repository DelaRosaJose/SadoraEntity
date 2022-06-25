﻿using Sadora.Clases;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public string SearchByTable
        {
            get { return (string)GetValue(SearchByTableProperty); }
            set { SetValue(SearchByTableProperty, value); }
        }
        public string EstadoMainWindows
        {
            get { return (string)GetValue(EstadoMainWindowsProperty); }
            set { SetValue(EstadoMainWindowsProperty, value); }
        }
        #endregion

        #region Registro de Dependency Property

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register(nameof(MaxLength), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(0));

        public static readonly DependencyProperty MarginBorderProperty =
            DependencyProperty.Register(nameof(MarginBorder), typeof(Thickness), typeof(UscTextboxButtonGeneral), new PropertyMetadata(new Thickness(15)));

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(HeightLabel), typeof(int), typeof(UscTextboxButtonGeneral), new PropertyMetadata(30));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly RoutedEvent SearchClickEvent =
            EventManager.RegisterRoutedEvent(nameof(SearchClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UscTextboxButtonGeneral));

        public static readonly DependencyProperty SearchByTableProperty =
            DependencyProperty.Register(nameof(SearchByTable), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        public static readonly DependencyProperty EstadoMainWindowsProperty =
            DependencyProperty.Register(nameof(EstadoMainWindows), typeof(string), typeof(UscTextboxButtonGeneral), new PropertyMetadata(null));

        #endregion

        public UscTextboxButtonGeneral() => InitializeComponent();

        private void txtFieldID_KeyDown(object sender, KeyEventArgs e) => ClassControl.CampoSoloPermiteNumeros(e);

        private void btnSearch_Click(object sender, RoutedEventArgs e) => ProcesadorCampo();

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => ProcesadorCampo(e);

        async void ProcesadorCampo(TextChangedEventArgs e = null)
        {
            if (EstadoMainWindows == default)
                return;
            //{
            //    //new Administracion.FrmCompletarCamposHost($"El Elemento seleccionado no tiene bindeado el Estado !Notifique a TI! \n El titulo del elemento es: {Title}").ShowDialog();
            //    //throw new Exception();
            //}

            try
            {
                if (EstadoMainWindows != "Modo Consulta")
                {
                    string ValueColumn = e != null ? (e.Source as TextBox).Text : default;

                    if (new string[] { "", "0", string.Empty }.Contains(ValueColumn))
                    {
                        ResultText.Text = string.Empty;
                        return;
                    }

                    using (Models.SadoraEntity db = new Models.SadoraEntity())
                    {
                        string ColumnAsync = default;

                        ColumnAsync = await db.Database.SqlQuery<string>($"select top 1 COLUMN_NAME from Information_Schema.COLUMNS " +
                        $"where TABLE_NAME = '{SearchByTable}' " +
                        $"and COLUMN_NAME not in ('RowID', 'UsuarioID') " +
                        $"and COLUMN_NAME like '%ID'").FirstOrDefaultAsync();

                        if (e == default)
                        {
                            Administracion.FrmMostrarDatosHost frm = new Administracion.FrmMostrarDatosHost($"Select * from {SearchByTable}", null);
                            frm.ShowDialog();

                            if (frm.GridMuestra.SelectedItem != null)
                            {
                                DataRowView item = frm.GridMuestra.SelectedItem as DataRowView;
                                ValueColumn = MainText.Text = item.Row.ItemArray[0].ToString();
                            }
                        }

                        ResultText.Text = await db.Database.SqlQuery<string>($"select Nombre from {SearchByTable} where {ColumnAsync} = {ValueColumn}").FirstOrDefaultAsync();

                    }
                }
            }
            catch (System.Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
            }
        }

        private void root_GotFocus(object sender, RoutedEventArgs e)
        {
            MainText.TabIndex = root.TabIndex;
            MainText.Focus();
        }

    }
}
