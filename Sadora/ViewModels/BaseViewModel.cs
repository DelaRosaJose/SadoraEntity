using Sadora.Clases;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sadora.ViewModels
{
    public class BaseViewModel<T> : INotifyPropertyChanging, INotifyPropertyChanged where T : class
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanging([CallerMemberName] string propetyName = "") => PropertyChanging(this, new PropertyChangingEventArgs(propetyName));

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        string _estadoVentana;
        public string EstadoVentana
        {
            get { return _estadoVentana; }
            set
            {
                if (_estadoVentana == value)
                    return;
                _estadoVentana = value;
                OnPropertyChanged(nameof(EstadoVentana));
            }

        }

        T _ventana;

        public T Ventana
        {
            get { return _ventana; }
            set
            {
                _ventana = value;
                OnPropertyChanged(nameof(Ventana));
            }

        }

    }
}
