using System;
using System.Collections.ObjectModel;
using NLog;
using System.ComponentModel;

namespace MVVM.Expressions
{
    public class CollectionNode<T> : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private ObservableCollection<T> collection;

        public override Type Type
        {
            get { return typeof(ObservableCollection<T>); }
        }

        public override bool IsDuplicate(Node other)
        {
            var other_as_property_node = other as CollectionNode<T>;

            return other_as_property_node != null &&
                   other_as_property_node != this &&
                   other_as_property_node.collection == collection;
        }

        public override void DumpToLog()
        {
            log.Trace(string.Format("Collection node ({0}, {1})", Type.ToString(), Children.Count));
            foreach (var child in Children)
                child.DumpToLog();
        }

        public override void Subscribe(INotifyPropertyChanged subject, Action callback)
        {
            throw new NotImplementedException();
        }

        public override void Unsubscribe(INotifyPropertyChanged subject)
        {
            throw new NotImplementedException();
        }
    }
}
