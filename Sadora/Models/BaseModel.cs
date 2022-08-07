using Sadora.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sadora.Models
{
    public class BaseModel
    {

        public static async Task<T> Procesar<T, J>(string BotonPulsado, J viewModel, string IdRegistro, Func<T, IComparable> getProp, Expression<Func<T, bool>> getExpresion) where T : class where J : BaseViewModel<T>
        {
            string EstadoVentana = viewModel.EstadoVentana;
            T ViewModel = viewModel.Ventana;
            T UnChangedViewModel = viewModel.Ventana;

            try
            {

                int intValue;

                using (SadoraEntity db = new SadoraEntity())
                {

                    T Tsql = default;

                    #region MyRegion
                    switch (BotonPulsado)
                    {
                        case "BtnPrimerRegistro":
                            Tsql = db.Set<T>().OrderBy(getProp).FirstOrDefault();
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;
                            break;

                        case "BtnAnteriorRegistro":
                            intValue = int.TryParse(IdRegistro, out intValue) ? intValue : 0;
                            //Tsql = db.TcliClientes.Where(x => x.ClienteID == (intValue - 1)).OrderBy(x => x.ClienteID).FirstOrDefault();
                            Tsql = db.Set<T>().Where(getExpresion).OrderByDescending(getProp).FirstOrDefault();
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;

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
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;
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

                        case "BtnGuardar":
                            if (EstadoVentana == "Modo Agregar")
                                db.Set<T>().Add(ViewModel);
                            else if (EstadoVentana == "Modo Editar")
                                db.Entry(ViewModel).State = EntityState.Modified;

                            await db.SaveChangesAsync();
                            break;
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
                return ViewModel;
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
                return UnChangedViewModel;
            }
        }
    }
}
