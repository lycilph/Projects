using System;
using System.Reflection;
using NLog;
using System.ComponentModel;

namespace MVVM.Expressions
{
    public class PropertyNode : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private Action notification_callback;
        public PropertyInfo Property { get; private set; }

        public string PropertyName
        {
            get { return Property.Name; }
        }

        public override Type Type
        {
            get { return Property.PropertyType; }
        }

        public PropertyNode(PropertyInfo property)
        {
            this.Property = property;
        }

        public object GetPropertyValue(object obj)
        {
            return Property.GetValue(obj, null);
        }

        public override bool IsDuplicate(Node other)
        {
            var other_as_property_node = other as PropertyNode;

            return other_as_property_node != null &&
                   other_as_property_node != this &&
                   other_as_property_node.Property == Property;
        }

        public override void DumpToLog()
        {
            log.Trace(string.Format("Property node ({0}, {1}, {2})", PropertyName, Type.ToString(), Children.Count));
            foreach (var child in Children)
                child.DumpToLog();
        }

        #region Subscription

        private void OnPropertyChanged(object subject, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == PropertyName)
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
            if (args.PropertyName == PropertyName)
            {
                log.Trace("OnPropertyChanging " + args.PropertyName);
                // Unsubscribe from children
                UnsubscribeFromChildren(subject);
            }
        }

        public override void Subscribe(INotifyPropertyChanged subject, Action callback)
        {
            log.Trace("Subscribe " + PropertyName);

            notification_callback = callback;

            // Add property changed tracking callback
            subject.PropertyChanged += OnPropertyChanged;

            // Add property changing tracking callback
            var subject_as_notify_changing = subject as INotifyPropertyChanging;
            if (subject_as_notify_changing == null)
                throw new Exception("Subject must implement INotifyPropertyChanging");
            subject_as_notify_changing.PropertyChanging += OnPropertyChanging;

            SubscribeToChildren(subject);
        }

        public override void Unsubscribe(INotifyPropertyChanged subject)
        {
            log.Trace("Unsubscribe " + PropertyName);

            notification_callback = null;

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
            if (Children.Count == 0)
                return;

            INotifyPropertyChanged child_subject = (INotifyPropertyChanged)GetPropertyValue(subject);
            if (child_subject == null)
                return;

            log.Trace("SubscribeToChildren " + PropertyName);
            foreach (var child in Children)
                child.Subscribe(child_subject, notification_callback);
        }

        public void UnsubscribeFromChildren(object subject)
        {
            if (Children.Count == 0)
                return;

            INotifyPropertyChanged child_subject = (INotifyPropertyChanged)GetPropertyValue(subject);
            if (child_subject == null)
                return;

            log.Trace("UnsubscribeToChildren " + PropertyName);
            foreach (var child in Children)
                child.Unsubscribe(child_subject);
        }

        #endregion
    }
}
