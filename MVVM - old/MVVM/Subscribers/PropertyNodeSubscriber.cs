using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MVVM.Expressions;
using NLog;
using System.Collections.Specialized;

namespace MVVM.Subscribers
{
    internal class PropertyNodeSubscriber<TListener>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private static int subscription_count = 0;

        private readonly Action<TListener, object> subscriber_callback;
        private readonly PropertyNode property_node;
        private readonly List<PropertyNodeSubscriber<TListener>> children;

        private TListener subscriber;
        private object root_subject;

        public PropertyNodeSubscriber(PropertyNode node, Action<TListener, object> callback)
        {
            subscriber_callback = callback;
            property_node = node;
            children = new List<PropertyNodeSubscriber<TListener>>();

            BuildChildren();
        }

        private void BuildChildren()
        {
            foreach (var node in property_node.Children)
            {
                var current_node = node as PropertyNode;
                if (current_node == null)
                    throw new ArgumentException("Node must be of type PropertyNode");

                var child_subscription_node = new PropertyNodeSubscriber<TListener>(current_node, subscriber_callback);
                children.Add(child_subscription_node);
            }
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Subscription ({0}, {1})", property_node.PropertyName, children.Count));
            foreach (var child in children)
                child.DumpToLog();
        }

        private void OnPropertyChanged(object subject, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == property_node.PropertyName)
            {
                log.Trace("OnPropertyChanged " + args.PropertyName);

                // Subscribe to collection
                if (IsNotifyCollection(property_node))
                    SubscribeToCollectionChanges(subject);
                // Subscribe to children
                SubscribeToChildChanges(subject);

                subscriber_callback(subscriber, root_subject);
            }
        }

        private void OnPropertyChanging(object subject, PropertyChangingEventArgs args)
        {
            if (args.PropertyName == property_node.PropertyName)
            {
                log.Trace("OnPropertyChanging " + args.PropertyName);

                // Unsubscribe from collection
                if (IsNotifyCollection(property_node))
                    UnsubscribeFromCollectionChanges(subject);
                // Unsubscribe from children
                UnsubscribeFromChildChanges(subject);
            }
        }

        private void OnCollectionChanged(object subject, NotifyCollectionChangedEventArgs args)
        {
            log.Trace("OnCollectionChanged " + property_node.PropertyName);

            subscriber_callback(subscriber, root_subject);
        }

        public void SubscribeToChanges(INotifyPropertyChanged subject, object root, TListener listener)
        {
            log.Trace("SubscribeToChange " + property_node.PropertyName);

            subscriber = listener;
            root_subject = root;

            // Add property changed tracking callback
            subject.PropertyChanged += OnPropertyChanged;
            
            // Add property changing tracking callback
            var subject_as_notify_changing = subject as INotifyPropertyChanging;
            if (subject_as_notify_changing == null)
                throw new Exception("Subject must implement INotifyPropertyChanging");
            subject_as_notify_changing.PropertyChanging += OnPropertyChanging;

            // Subscribe to collection
            if (IsNotifyCollection(property_node))
                SubscribeToCollectionChanges(subject);

            // Subscribe to childen
            SubscribeToChildChanges(subject);

            // Update subscription counter
            subscription_count++;
            log.Debug(string.Format("Subscription count {0}", subscription_count));
        }

        public void UnsubscribeFromChanges(INotifyPropertyChanged subject)
        {
            log.Trace("UnsubscribeFromChanges " + property_node.PropertyName);

            // Add property changed tracking callback
            subject.PropertyChanged -= OnPropertyChanged;

            // Add property changing tracking callback
            var subject_as_notify_changing = subject as INotifyPropertyChanging;
            if (subject_as_notify_changing == null)
                throw new Exception("Subject must implement INotifyPropertyChanging");
            subject_as_notify_changing.PropertyChanging -= OnPropertyChanging;

            // Subscribe to collection
            if (IsNotifyCollection(property_node))
                UnsubscribeFromCollectionChanges(subject);

            // Unsubscribe from children
            UnsubscribeFromChildChanges(subject);

            // Update subscription counter
            subscription_count--;
            log.Debug(string.Format("Subscription count {0}", subscription_count));
        }

        private void SubscribeToChildChanges(object subject)
        {
            log.Trace("SubscribeToChildChanges " + property_node.PropertyName);

            if (children.Count == 0)
                return;

            INotifyPropertyChanged notify_property_child_subject = (INotifyPropertyChanged)property_node.GetPropertyValue(subject);
            if (notify_property_child_subject == null)
                return;

            foreach (var child in children)
                child.SubscribeToChanges(notify_property_child_subject, root_subject, subscriber);
        }

        private void UnsubscribeFromChildChanges(object subject)
        {
            log.Trace("UnsubscribeFromChildChanges " + property_node.PropertyName);

            if (children.Count == 0)
                return;

            INotifyPropertyChanged child_subject = (INotifyPropertyChanged)property_node.GetPropertyValue(subject);
            if (child_subject == null)
                return;

            foreach (var child in children)
                child.UnsubscribeFromChanges(child_subject);
        }

        private static bool IsNotifyCollection(PropertyNode node)
        {
            return typeof(INotifyCollectionChanged).IsAssignableFrom(node.Type);
        }

        private void SubscribeToCollectionChanges(object subject)
        {
            log.Trace("SubscribeToCollectionChanges " + property_node.PropertyName);

            INotifyCollectionChanged collection_subject = (INotifyCollectionChanged)property_node.GetPropertyValue(subject);
            if (collection_subject == null) return;

            collection_subject.CollectionChanged += OnCollectionChanged;
        }

        private void UnsubscribeFromCollectionChanges(object subject)
        {
            log.Trace("UnsubscribeFromCollectionChanges " + property_node.PropertyName);

            INotifyCollectionChanged collection_subject = (INotifyCollectionChanged)property_node.GetPropertyValue(subject);
            if (collection_subject == null) return;

            collection_subject.CollectionChanged -= OnCollectionChanged;
        }
    }
}
