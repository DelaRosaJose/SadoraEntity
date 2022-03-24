using Sadora;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace Sadora.ViewModels.Clientes
{
    public class ClientesViewModel : BaseViewModel
    {
        #region Properties

        private List<Models.TcliCliente> _clientes;

        public List<Models.TcliCliente> Clientes
        {
            get
            {
                return _clientes;
            }
            set
            {
                if (_clientes == value)
                {
                    return;
                }
                _clientes = value;
                OnPropertyChanged("Clientes");
            }
        } 
        private Models.TcliCliente _cliente;

        public Models.TcliCliente Cliente
        {
            get
            {
                return _cliente;
            }
            set
            {
                if (_cliente == value)
                {
                    return;
                }
                _cliente = value;
                OnPropertyChanged("Cliente");
            }
        }

        #endregion

        #region Commands

        private ICommand _customerCommand;

        public ICommand CustomerCommand
        {
            get
            {
                if (_customerCommand == null)
                {
                    _customerCommand = new RelayCommand(param => this.CustomerCommandExecute(), null);
                }

                return _customerCommand;
            }
        }

        #endregion

        public ClientesViewModel()
        {
        }

        private void CustomerCommandExecute()
        {
            using (Models.SadoraEntities db = new Models.SadoraEntities())
            {
                Clientes = db.TcliClientes.ToList();
                Cliente = db.TcliClientes.OrderByDescending(x=>x.ClienteID).FirstOrDefault();

            }

            //var result = db.
            //var lst = (from d in db.TcliClientes
            //           select new Models.TcliCliente
            //           {
            //               Name = d.Name,
            //               Age = d.Age,
            //               Id = d.Id
            //           }).ToList();




            //var customerService = new CustomerService();

            //var result = customerService.GetCustomer();

            //Customers = new List<Customer>(result);
        }
    }
}
