using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sadora.CustomElements
{
    public partial class UscBotonesGenerales : UserControl
    {
        string Estado;

        public UscBotonesGenerales() => InitializeComponent();

        public void HabilitadorDesabilitadorBotones(string BotonEstadoConsultaEjecutado) //Este metodo se encarga de crear la interacion de los botones de la ventana segun el estado en el que se encuentra
        {
            lIconEstado.ToolTip = Estado =
                new string[] { "BtnPrimerRegistro", "BtnAnteriorRegistro", "BtnProximoRegistro", "BtnUltimoRegistro", "BtnCancelar" }.Contains(BotonEstadoConsultaEjecutado) ? "Modo Consulta" :
                BotonEstadoConsultaEjecutado.Contains("BtnBuscar") ? "Modo Busqueda" :
                BotonEstadoConsultaEjecutado.Contains("BtnAgregar") ? "Modo Agregar" :
                BotonEstadoConsultaEjecutado.Contains("BtnEditar") ? "Modo Editar" : default;

            BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnBuscar.IsEnabled =
            BtnImprimir.IsEnabled = BtnAgregar.IsEnabled = BtnEditar.IsEnabled = BtnAnular.IsEnabled = BtnCancelar.IsEnabled = BtnAnular.IsEnabled = BtnGuardar.IsEnabled = true;

            if (new string[] { "Modo Consulta", "Modo Busqueda" }.Contains(Estado)) //Si el sistema se encuentra en modo consulta o busqueda entra el validador
            {
                BtnCancelar.IsEnabled = BtnGuardar.IsEnabled = false;
                IconEstado.Kind = Estado == "Modo Consulta" ? MaterialDesignThemes.Wpf.PackIconKind.EyeOutline : MaterialDesignThemes.Wpf.PackIconKind.Search;

                if (Estado != "Modo Consulta") //Si el estado es modo busqueda enviamos a cambiamos el estado de los botones
                    BtnProximoRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnImprimir.IsEnabled = BtnEditar.IsEnabled = BtnAnular.IsEnabled = false;
                else if (Estado == "Modo Consulta") //Si el estado es modo consulta evaluamos el boton ejecutado y cambiamos el estado de los botones
                {
                    if (BotonEstadoConsultaEjecutado == "BtnPrimerRegistro") BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = false;
                    else if (BotonEstadoConsultaEjecutado == "BtnUltimoRegistro") BtnUltimoRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = false;
                    return;
                }
            }
            else if (new string[] { "Modo Agregar", "Modo Editar" }.Contains(Estado)) //Si el sistema se encuentra en modo Agregar o Editar entra el validador
            {
                BtnPrimerRegistro.IsEnabled = BtnAnteriorRegistro.IsEnabled = BtnProximoRegistro.IsEnabled = BtnUltimoRegistro.IsEnabled = BtnBuscar.IsEnabled =
                BtnImprimir.IsEnabled = BtnAgregar.IsEnabled = BtnEditar.IsEnabled = BtnAnular.IsEnabled = false;

                IconEstado.Kind = Estado == "Modo Agregar" ? MaterialDesignThemes.Wpf.PackIconKind.AddThick : IconEstado.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
            }
        }
    }
}
