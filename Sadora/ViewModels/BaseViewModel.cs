using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Sadora.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Dictionary<string, object> Dictionary;

        public BaseViewModel() => Dictionary = new Dictionary<string, object>();

        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        //protected void OnPropertyChanging([CallerMemberName] string propetyName = "") => PropertyChanging(this, new PropertyChangingEventArgs(propetyName));

        //protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        public void Set<T>(T value, [CallerMemberName] string propertyName = null, bool siempreNotificar = false)
        {
            T backingStore = default;

            if (Dictionary.TryGetValue(propertyName, out var valueInDict))
            {
                backingStore = (T)valueInDict;
                Dictionary[propertyName] = value;
            }
            else
                Dictionary.Add(propertyName, value);

            if (siempreNotificar || !EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                var notify = typeof(T) != typeof(string) || new string[] { (string)(object)backingStore, (string)(object)value }.Any(x => !string.IsNullOrWhiteSpace(x));
            
                if (notify)
                    OnPropertyChanged(propertyName);
            }

        }

        public T Get<T>([CallerMemberName] string propertyName = null)
        {
            if (Dictionary.TryGetValue(propertyName, out var valueInDict))
                return (T)valueInDict;
            else
                return default;
        }
    }
}
