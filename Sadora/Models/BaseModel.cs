using Sadora.Clases;
using Sadora.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Sadora.Models
{
    public class BaseModel
    {

        public static async Task<Tuple<T, bool, string>> Procesar<T, J>(string BotonPulsado, J viewModel, string IdRegistro, Func<T, IComparable> getProp,
            Expression<Func<T, bool>> getExpresion, UIElementCollection view, int? lastRegistro) where T : class where J : BaseViewModel<T>
        {
            string EstadoVentana = viewModel.EstadoVentana;
            T ViewModel = viewModel.Ventana;
            T UnChangedViewModel = viewModel.Ventana;
            bool Cansave = true;

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
                            Tsql = db.Set<T>().Where(getExpresion).OrderByDescending(getProp).FirstOrDefault();
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;
                            break;

                        case "BtnProximoRegistro":
                            intValue = int.TryParse(IdRegistro, out intValue) ? intValue : 0;
                            Tsql = db.Set<T>().Where(getExpresion).OrderBy(getProp).FirstOrDefault();
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;
                            break;

                        case "BtnUltimoRegistro":
                            Tsql = db.Set<T>().OrderByDescending(getProp).FirstOrDefault();
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;
                            break;

                        case "BtnAgregar":
                            ClassControl.LimpiadorGeneral(view);
                            break;

                        case "BtnCancelar":
                            ClassControl.SetDefaultColorELement(view);
                            Tsql = lastRegistro == 0 ? default : db.Set<T>().FindAsync(lastRegistro).Result;
                            ViewModel = Tsql != default ? Tsql : UnChangedViewModel;
                            break;

                        case "BtnGuardar":
                            if (!ClassControl.CanSaveView(view))
                                return new Tuple<T, bool, string>(ViewModel, false, "Debe completar los campos vacios");

                            if (EstadoVentana == "Modo Agregar")
                                db.Set<T>().Add(ViewModel);
                            else if (EstadoVentana == "Modo Editar")
                                db.Entry(ViewModel).State = EntityState.Modified;

                            await db.SaveChangesAsync();
                            break;
                    }
                    #endregion
                }

                return new Tuple<T, bool, string>(ViewModel, Cansave = true, string.Empty);
            }
            catch (Exception ex)
            {
                new Administracion.FrmCompletarCamposHost($"Ha ocurrido un error:\n {ex}").ShowDialog();
                return new Tuple<T, bool, string>(UnChangedViewModel, Cansave = false, $"Ha ocurrido un error no puede guardar");
            }
        }
    }
}
