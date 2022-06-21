﻿using Sadora;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using Sadora.Clases;

namespace Sadora.ViewModels.Clientes
{
    public class ClientesViewModel : BaseViewModel
    {
        #region UscClientes ViewModel

        //private List<Models.TcliCliente> _clientes;

        //public List<Models.TcliCliente> Clientes
        //{
        //    get { return _clientes; }
        //    set 
        //    {
        //        if (_clientes == value)
        //            return;
        //        _clientes = value;
        //        OnPropertyChanged(nameof(Clientes));
        //    }
        //} 

        private Models.TcliCliente _cliente;

        public Models.TcliCliente Cliente
        {
            get { return _cliente; }
            set
            {
                if (_cliente == value)
                    return;
                _cliente = value;
                OnPropertyChanged(nameof(Cliente));
            }
        }

        string _estado;

        public string Estado
        {
            get { return _estado; }
            set
            {
                if (_estado == value)
                    return;
                _estado = value;
                OnPropertyChanged(nameof(Estado));
            }

        }

        #endregion

        #region Commands

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

        #endregion

        public ClientesViewModel()
        {
            Cliente = new Models.TcliCliente() { UsuarioID = ClassVariables.UsuarioID };
        }


        //private void CustomerCommandExecute()
        //{
        //    using (Models.SadoraEntity db = new Models.SadoraEntity())
        //        Cliente = db.TcliClientes.OrderByDescending(x=>x.ClienteID).FirstOrDefault();
        //    //{
        //    //    Clientes = db.TcliClientes.ToList();

        //    //}

        //}
    }
}
