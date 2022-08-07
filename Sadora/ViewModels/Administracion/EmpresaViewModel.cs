using Sadora.Clases;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Sadora.ViewModels.Administracion
{
    internal class EmpresaViewModel : BaseViewModel<Models.TcliCliente>
    {
        #region UscEmpresa ViewModel

        //private List<Models.TsysEmpresa> _empresas;

        //public List<Models.TsysEmpresa> Empresas
        //{
        //    get { return _empresas; }
        //    set
        //    {
        //        if (_empresas == value)
        //            return;
        //        _empresas = value;
        //        OnPropertyChanged(nameof(Empresas));
        //    }
        //}
        private Models.TsysEmpresa _empresa;

        public Models.TsysEmpresa Empresa
        {
            get { return _empresa; }
            set
            {
                if (_empresa == value)
                    return;
                _empresa = value;
                OnPropertyChanged(nameof(Empresa));
            }
        }


        #endregion

        public EmpresaViewModel()
        {
            Empresa = new Models.TsysEmpresa() { UsuarioID = ClassVariables.UsuarioID };
        }

        //#region Commands

        //private ICommand _customerCommand;

        //public ICommand CustomerCommand
        //{
        //    get
        //    {
        //        if (_customerCommand == null)
        //        {
        //            _customerCommand = new RelayCommand(param => this.CustomerCommandExecute(), null);
        //        }

        //        return _customerCommand;
        //    }
        //}

        //#endregion

        //public ClientesViewModel() { }

        //private void CustomerCommandExecute()
        //{
        //    using (Models.SadoraEntity db = new Models.SadoraEntity())
        //        Cliente = db.TcliClientes.OrderByDescending(x => x.ClienteID).FirstOrDefault();
        //    //{
        //    //    Clientes = db.TcliClientes.ToList();

        //    //}

        //}

    }
}
