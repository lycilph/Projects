using System;
using System.Collections.ObjectModel;
using NLog;
using System.ComponentModel;
using System.Collections.Specialized;

namespace MVVM.Expressions
{
    public class CollectionNode<T> : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private Action notification_callback;
        private PropertyNode property_node;
        public readonly ReactiveCollection<T> collection = new ReactiveCollection<T>();

        public override Type Type
        {
            get { return typeof(ObservableCollection<T>); }
        }

        public CollectionNode(PropertyNode node)
        {
            property_node = node;
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

        #region Subscription

        private void OnCollectionChanged(object subject, NotifyCollectionChangedEventArgs args)
        {
            log.Trace("OnCollectionChanged " + property_node.PropertyName + " - " + args.Action);

            // Invoke notification callback
            notification_callback();
        }

        public override void Subscribe(INotifyPropertyChanged subject, Action callback)
        {
            log.Trace("Subscribe " + property_node.PropertyName + " (collection)");

            notification_callback = callback;

            // Check that subject is INotifyCollectionChanged
            var subject_as_notify_collection = subject as INotifyCollectionChanged;
            if (subject_as_notify_collection == null)
                throw new Exception("Subject must implement INotifyCollectionChanged");
            subject_as_notify_collection.CollectionChanged += OnCollectionChanged;

            // Check that subject is INotifyCollectionChanged
            // - Generate wrappers for each element
            // - Subscribe to children

            throw new NotImplementedException();
        }

        public override void Unsubscribe(INotifyPropertyChanged subject)
        {
            log.Trace("Unsubscribe " + property_node.PropertyName + " (collection)");

            notification_callback = null;

            // Check that subject is INotifyCollectionChanged
            var subject_as_notify_collection = subject as INotifyCollectionChanged;
            if (subject_as_notify_collection == null)
                throw new Exception("Subject must implement INotifyCollectionChanged");
            subject_as_notify_collection.CollectionChanged -= OnCollectionChanged;

            // Check that subject is INotifyCollectionChanged
            // - Unsubscribe to children
            // - Clear collection

            throw new NotImplementedException();
        }

        #endregion
    }
}
