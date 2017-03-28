using System;

namespace Sangmado.Inka.Extensions
{
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);

    public interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }

    public class PropertyChangedEventArgs : EventArgs
    {
        private readonly string _propertyName;

        public PropertyChangedEventArgs(string propertyName)
        {
            _propertyName = propertyName;
        }

        public virtual string PropertyName
        {
            get
            {
                return _propertyName;
            }
        }
    }
}
