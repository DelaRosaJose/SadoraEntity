using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sadora.CustomElements
{
    public partial class UscBotonesGenerales : UserControl
    {
        string Estado;

        public UscBotonesGenerales()
        {
            InitializeComponent();
        }

        private void BtnGeneral_Click(object sender, RoutedEventArgs e)
        {
            string ButtonName = ((Button)e.OriginalSource).Name;

            switch (ButtonName)
            {
                case string name when "BtnPrimerRegistro, BtnAnteriorRegistro, BtnProximoRegistro, BtnUltimoRegistro".Contains(name):
                    HabilitadorDesabilitadorBotones(EstadoVentana: "Modo Consulta", BotonEstadoConsultaEjecutado: ButtonName);
                    break;
                case "BtnBuscar":
                    HabilitadorDesabilitadorBotones(EstadoVentana: "Modo Busqueda");
                    break;
                case "BtnAgregar":
                    HabilitadorDesabilitadorBotones(EstadoVentana: "Modo Agregar");
                    break;
                case "BtnEditar":
                    HabilitadorDesabilitadorBotones(EstadoVentana: "Modo Editar");
                    break;
                case "BtnCancelar":
                    HabilitadorDesabilitadorBotones(EstadoVentana: "Modo Consulta", BotonEstadoConsultaEjecutado: "BtnUltimoRegistro");
                    break;
                //case "BtnGuardar":
                //    HabilitadorDesabilitadorBotones(EstadoVentana: "Modo Consulta", BotonEstadoConsultaEjecutado: "BtnUltimoRegistro");
                //    break;

                    //case "BtnAnteriorRegistro":
                    //    SetEnabledButton("Modo Consulta", e);
                    //    //BtnPrimerRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = true;
                    //    //BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = false;
                    //    break;

                    //case "BtnProximoRegistro":
                    //    SetEnabledButton("Modo Consulta", e);
                    //    //BtnPrimerRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = true;
                    //    //BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = false;
                    //    break;

                    //case "BtnUltimoRegistro":
                    //    SetEnabledButton("Modo Consulta", e);
                    //    //BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = false;
                    //    //BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = true;
                    //    break;
            }
        }

        private void HabilitadorDesabilitadorBotones(string EstadoVentana, string BotonEstadoConsultaEjecutado = null) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        {
            Estado = EstadoVentana;
            lIconEstado.ToolTip = Estado;

            BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnBuscar.IsEnabled =
            BtnImprimir.IsEnabled = BtnAgregar.IsEnabled = BtnEditar.IsEnabled = BtnAnular.IsEnabled = BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = true;

            if (!new string[] { "Modo Agregar", "Modo Editar" }.Contains(Estado)) //Si el sistema se encuentra en modo consulta o busqueda entra el validador
            {
                BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = false;
                IconEstado.Kind = "Modo Consulta".Contains(Estado) ? MaterialDesignThemes.Wpf.PackIconKind.EyeOutline : MaterialDesignThemes.Wpf.PackIconKind.Search;

                if (!"Modo Consulta".Contains(Estado)) //Si el estado es modo busqueda enviamos a cambiamos el estado de los botones
                    BtnProximoRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnImprimir.IsEnabled = BtnEditar.IsEnabled = false;
                else if ("Modo Consulta".Contains(Estado)) //Si el estado es modo consulta evaluamos el boton ejecutado y cambiamos el estado de los botones
                {
                    if (BotonEstadoConsultaEjecutado == "BtnPrimerRegistro") BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = false;
                    else if (BotonEstadoConsultaEjecutado == "BtnAnteriorRegistro") BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = false;
                    else if (BotonEstadoConsultaEjecutado == "BtnProximoRegistro") BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = false;
                    else if (BotonEstadoConsultaEjecutado == "BtnUltimoRegistro") BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = false;
                }
            }
            else if (new string[] { "Modo Agregar", "Modo Editar" }.Contains(Estado)) //Si el sistema se encuentra en modo Agregar o Editar entra el validador
            {
                BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnBuscar.IsEnabled =
                BtnImprimir.IsEnabled = BtnAgregar.IsEnabled = BtnEditar.IsEnabled = false;

                IconEstado.Kind = Estado == "Modo Agregar" ? MaterialDesignThemes.Wpf.PackIconKind.AddThick : IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            }


            //if (Imprime == false)
            //    BtnImprimir.IsEnabled = Imprime;
            //if (Agrega == false)
            //    BtnAgregar.IsEnabled = Agrega;
            //if (Modifica == false)
            //    BtnEditar.IsEnabled = Modifica;
        }

        //public static readonly DependencyProperty FirstButtonStyleProperty =
        //DependencyProperty.Register("FirstButtonStyle", typeof(Style), typeof(UscBotonesGenerales));

        //public Style FirstButtonStyle
        //{
        //    get { return (Style)GetValue(FirstButtonStyleProperty); }
        //    set { SetValue(FirstButtonStyleProperty, value); }
        //}

        //public static readonly DependencyProperty SecondButtonStyleProperty =
        //    DependencyProperty.Register("SecondButtonStyle", typeof(Style), typeof(UscBotonesGenerales));

        //public Style SecondButtonStyle
        //{
        //    get { return (Style)GetValue(SecondButtonStyleProperty); }
        //    set { SetValue(SecondButtonStyleProperty, value); }
        //}
    }
}
