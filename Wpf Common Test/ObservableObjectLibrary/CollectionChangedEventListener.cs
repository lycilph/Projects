using System;
using System.Collections.Specialized;
using System.Windows;

namespace ObservableObjectLibrary
{
    internal class CollectionChangedEventListener : IWeakEventListener
    {
        private readonly INotifyCollectionChanged _source;
        public INotifyCollectionChanged Source { get { return _source; } }
        
        private readonly NotifyCollectionChangedEventHandler _handler;
        public NotifyCollectionChangedEventHandler Handler { get { return _handler; } }

        public CollectionChangedEventListener(INotifyCollectionChanged source, NotifyCollectionChangedEventHandler handler)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (handler == null) { throw new ArgumentNullException("handler"); }

            _source = source;
            _handler = handler;
        }

        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            var handler = _handler;
            handler(sender, (NotifyCollectionChangedEventArgs)e);
            return true;
        }
    }
}
