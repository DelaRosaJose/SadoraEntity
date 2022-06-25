using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sadora.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanging([CallerMemberName] string propetyName = "") => PropertyChanging(this, new PropertyChangingEventArgs(propetyName));

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") 
        {
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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

    }
}
