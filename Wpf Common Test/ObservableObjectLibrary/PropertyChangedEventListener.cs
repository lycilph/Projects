using System;
using System.ComponentModel;
using System.Windows;

namespace ObservableObjectLibrary
{
    internal class PropertyChangedEventListener : IWeakEventListener
    {
        private readonly INotifyPropertyChanged _Source;
        public INotifyPropertyChanged Source { get { return _Source; } }

        private readonly PropertyChangedEventHandler _Handler;
        public PropertyChangedEventHandler Handler { get { return _Handler; } }

        public PropertyChangedEventListener(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (handler == null) throw new ArgumentNullException("handler");

            _Source = source;
            _Handler = handler;
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            var handler = _Handler;
            handler(sender, (PropertyChangedEventArgs)e);
            return true;
        }
    }
}
