using System;
using System.Collections.Generic;
using MVVM.Expressions;
using NLog;
using System.ComponentModel;

namespace MVVM.Subscribers
{
    internal class PropertyNodeSubscriber : INodeSubscriber
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly Action notification_callback;
        private readonly PropertyNode property_node;
        private readonly List<PropertyNodeSubscriber> children;

        public PropertyNodeSubscriber(PropertyNode node, Action callback)
        {
            notification_callback = callback;
            property_node = node;
            children = new List<PropertyNodeSubscriber>();

            BuildChildren();
        }

        private void BuildChildren()
        {
            foreach (var node in property_node.Children)
            {
                var current_node = node as PropertyNode;
                if (current_node == null)
                    throw new ArgumentException("Node must be of type PropertyNode");

                var property_subscriber = new PropertyNodeSubscriber(current_node, notification_callback);
                children.Add(property_subscriber);
            }
        }

        private void OnPropertyChanged(object subject, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == property_node.PropertyName)
            {
                log.Trace("OnPropertyChanged " + args.PropertyName);
                // Subscribe to children
                SubscribeToChildren(subject);
                // Invoke notification callback
                notification_callback();
            }
        }

        private void OnPropertyChanging(object subject, PropertyChangingEventArgs args)
        {
            if (args.PropertyName == property_node.PropertyName)
            {
                log.Trace("OnPropertyChanging " + args.PropertyName);
                // Unsubscribe from children
                UnsubscribeFromChildren(subject);
            }
        }

        public void Subscribe(INotifyPropertyChanged subject)
        {
            log.Trace("Subscribe " + property_node.PropertyName);

            // Add property changed tracking callback
            subject.PropertyChanged += OnPropertyChanged;

            // Add property changing tracking callback
            var subject_as_notify_changing = subject as INotifyPropertyChanging;
            if (subject_as_notify_changing == null)
                throw new Exception("Subject must implement INotifyPropertyChanging");
            subject_as_notify_changing.PropertyChanging += OnPropertyChanging;

            SubscribeToChildren(subject);
        }

        public void Unsubscribe(INotifyPropertyChanged subject)
        {
            log.Trace("Unsubscribe " + property_node.PropertyName);

            // Add property changed tracking callback
            subject.PropertyChanged -= OnPropertyChanged;

            // Add property changing tracking callback
            var subject_as_notify_changing = subject as INotifyPropertyChanging;
            if (subject_as_notify_changing == null)
                throw new Exception("Subject must implement INotifyPropertyChanging");
            subject_as_notify_changing.PropertyChanging -= OnPropertyChanging;

            UnsubscribeFromChildren(subject);
        }

        public void SubscribeToChildren(object subject)
        {
            if (children.Count == 0)
                return;

            INotifyPropertyChanged child_subject = (INotifyPropertyChanged)property_node.GetPropertyValue(subject);
            if (child_subject == null)
                return;

            log.Trace("SubscribeToChildren " + property_node.PropertyName);
            foreach (var child in children)
                child.Subscribe(child_subject);
        }

        public void UnsubscribeFromChildren(object subject)
        {
            if (children.Count == 0)
                return;

            INotifyPropertyChanged child_subject = (INotifyPropertyChanged)property_node.GetPropertyValue(subject);
            if (child_subject == null)
                return;

            log.Trace("UnsubscribeToChildren " + property_node.PropertyName);
            foreach (var child in children)
                child.Unsubscribe(child_subject);
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("property subscriber ({0}, {1})", property_node.PropertyName, children.Count));
            foreach (var child in children)
                child.DumpToLog();
        }
    }
}
