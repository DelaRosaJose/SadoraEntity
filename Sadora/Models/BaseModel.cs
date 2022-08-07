using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sadora.Models
{
    public class BaseModel
    {

        public static async Task<T> Procesar<T, J>(string BotonPulsado, T Viewmodel, Func<T, IComparable> getProp, string IdRegistro, string EstadoVentana, J Viewmodels) where T : class where J : ViewModels.BaseViewModel<J>
        {
            var NewViewModel = Viewmodel;
            try
            {
                int intValue;

                using (SadoraEntity db = new SadoraEntity())
                {

                    //var id = Viewmodels;
                    //var ididi = id.Ventana;
                    T Tsql = default;

                    #region MyRegion
                    switch (BotonPulsado)
                    {
                        case "BtnPrimerRegistro":
                            Tsql = db.Set<T>().OrderBy(getProp).FirstOrDefault();
                            NewViewModel = /*ididi =*/ Tsql != default ? Tsql : Viewmodel;
                            //_FistClienteID = ViewModel.Cliente.ClienteID;
                            //ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            break;

                        case "BtnAnteriorRegistro":
                            //intValue = int.TryParse(IdRegistro, out intValue) ? intValue : 0;
                            //Tsql = db.TcliClientes.Where(x => x.ClienteID == (intValue - 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                            //ViewModel.Cliente = Tsql != default ? Tsql : ViewModel.Cliente;
                            //ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Cliente.ClienteID > _FistClienteID ? ButtonName : "BtnPrimerRegistro");
                            break;

                        case "BtnProximoRegistro":
                            //intValue = int.TryParse(ViewModel.Cliente.ClienteID.ToString(), out intValue) ? intValue : 0;
                            //Tsql = db.TcliClientes.Where(x => x.ClienteID == (intValue + 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                            //ViewModel.Cliente = Tsql != default ? Tsql : ViewModel.Cliente;
                            //ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ViewModel.Cliente.ClienteID < _LastClienteID ? ButtonName : "BtnUltimoRegistro");
                            break;

                        case "BtnUltimoRegistro":
                            Tsql = db.Set<T>().OrderByDescending(getProp).FirstOrDefault();
                            NewViewModel = Tsql != default ? Tsql : Viewmodel;
                            //_LastClienteID = ViewModel.Cliente.ClienteID;
                            //ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            break;

                            //case "BtnAgregar":
                            //    ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            //    ClassControl.LimpiadorGeneral(MainView.Children);
                            //    break;

                            //case "BtnEditar":
                            //    ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: ButtonName);
                            //    break;

                            //case "BtnCancelar":
                            //    ControlesGenerales.BtnUltimoRegistro.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                            //    break;

                            //case "BtnGuardar":
                            //    if (ViewModel.EstadoVentana == "Modo Agregar")
                            //        db.TcliClientes.Add(ViewModel.Cliente);
                            //    else if (ViewModel.EstadoVentana == "Modo Editar")
                            //        db.Entry(ViewModel.Cliente).State = System.Data.Entity.EntityState.Modified;

                            //    await db.SaveChangesAsync();

                            //    ControlesGenerales.HabilitadorDesabilitadorBotones(BotonEstadoConsultaEjecutado: "BtnUltimoRegistro");
                            //    break;
                    }
                    #endregion
                }

                //if (Imprime == false)
                //    ControlesGenerales.BtnImprimir.IsEnabled = Imprime;
                //if (Agrega == false)
                //    ControlesGenerales.BtnAgregar.IsEnabled = Agrega;
                //if (Modifica == false)
                //    ControlesGenerales.BtnEditar.IsEnabled = Modifica;
                //if (PuedeUsarBotonAnular == false)
                //    ControlesGenerales.BtnAnular.IsEnabled = PuedeUsarBotonAnular;

                //ViewModel.EstadoVentana = ControlesGenerales.EstadoVentana;
                return NewViewModel;
            }
            catch (System.Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
                return Viewmodel;
            }
        }
    }
}
