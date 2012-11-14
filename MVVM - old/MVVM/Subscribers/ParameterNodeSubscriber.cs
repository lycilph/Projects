using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MVVM.Expressions;
using NLog;

namespace MVVM.Subscribers
{
    internal class ParameterNodeSubscriber<TListener>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly Action<TListener, object> subscriber_callback;
        private readonly ParameterNode parameter_node;
        private readonly List<PropertyNodeSubscriber<TListener>> children;

        public ParameterNodeSubscriber(ParameterNode node, Action<TListener, object> callback)
        {
            parameter_node = node;
            subscriber_callback = callback;
            children = new List<PropertyNodeSubscriber<TListener>>();

            BuildChildren();
        }

        private void BuildChildren()
        {
            foreach (var node in parameter_node.Children)
            {
                var property_node = node as PropertyNode;
                if (property_node == null)
                    throw new ArgumentException("Node must be of type PropertyNode");

                var child_subscription_node = new PropertyNodeSubscriber<TListener>(property_node, subscriber_callback);
                children.Add(child_subscription_node);
            }
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Root subscription ({0}, {1})", parameter_node.Name, children.Count));
            foreach (var child in children)
                child.DumpToLog();
        }

        public void SubscribeToChanges(INotifyPropertyChanged subject, TListener listener)
        {
            log.Trace("SubscribeToChange " + parameter_node.Name);

            foreach (var node in children)
                node.SubscribeToChanges(subject, subject, listener);
        }

        public void UnsubscribeFromChanges(INotifyPropertyChanged subject)
        {
            log.Trace("UnsubscribeFromChanges " + parameter_node.Name);

            foreach (var node in children)
                node.UnsubscribeFromChanges(subject);
        }
    }
}
