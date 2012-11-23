using System;
using System.Collections.ObjectModel;
using NLog;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace MVVM.Expressions
{
    public class CollectionNode<T> : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private Action notification_callback;
        private PropertyNode property_node;
        private ReactiveCollection<T> reactive_collection;

        public override Type Type
        {
            get { return typeof(ObservableCollection<T>); }
        }

        public CollectionNode(PropertyNode node, ReactiveCollection<T> collection)
        {
            property_node = node;
            reactive_collection = collection;
        }

        public override bool IsDuplicate(Node other)
        {
            var other_as_property_node = other as CollectionNode<T>;

            return other_as_property_node != null &&
                   other_as_property_node != this &&
                   other_as_property_node.reactive_collection == reactive_collection;
        }

        public override void DumpToLog()
        {
            log.Trace(string.Format("Collection node ({0}, {1})", Type.ToString(), Children.Count));
            foreach (var child in Children)
                child.DumpToLog();
        }

        #region Subscription

        public override void Subscribe(INotifyPropertyChanged subject, Action callback)
        {
            log.Trace("Subscribe " + property_node.PropertyName + " (collection)");

            notification_callback = callback;

            // Check that subject is INotifyCollectionChanged
            var subject_as_notify_collection = subject as INotifyCollectionChanged;
            if (subject_as_notify_collection == null)
                throw new Exception("Subject must implement INotifyCollectionChanged");

            // Subscribe to subject collection
            reactive_collection.Subscribe(subject_as_notify_collection, callback);
        }

        public override void Unsubscribe(INotifyPropertyChanged subject)
        {
            log.Trace("Unsubscribe " + property_node.PropertyName + " (collection)");

            notification_callback = null;

            reactive_collection.Unsubscribe();
        }

        #endregion
    }
}
