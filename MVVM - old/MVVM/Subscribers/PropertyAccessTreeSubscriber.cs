using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MVVM.Expressions;
using NLog;

namespace MVVM.Subscribers
{
    public class PropertyAccessTreeSubscriber<TListener>
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly PropertyAccessTree property_access_tree;
        private readonly List<ParameterNodeSubscriber<TListener>> subscriptions;

        public PropertyAccessTreeSubscriber(PropertyAccessTree tree, Action<TListener, object> subscriber_callback)
        {
            property_access_tree = tree;
            subscriptions = new List<ParameterNodeSubscriber<TListener>>();

            foreach (var node in property_access_tree.Children)
            {
                var parameter_node = node as ParameterNode;
                if (parameter_node == null)
                    throw new ArgumentException("Root nodes must be of type ParameterNode");

                var root_subscription = new ParameterNodeSubscriber<TListener>(parameter_node, subscriber_callback);
                subscriptions.Add(root_subscription);
            }
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Property access tree subscriber ({0} roots)", subscriptions.Count));
            foreach (var root in subscriptions)
                root.DumpToLog();
        }

        public void SubscribeToChanges(INotifyPropertyChanged subject, TListener listener)
        {
            foreach (var root_subscription_node in subscriptions)
                root_subscription_node.SubscribeToChanges(subject, listener);
        }

        public void UnsubscribeFromChanges(INotifyPropertyChanged subject)
        {
            foreach (var root_subscription_node in subscriptions)
                root_subscription_node.UnsubscribeFromChanges(subject);
        }

        #region Graph debug output

        public PropertyAccessTree GraphDebug_GetPropertyAccessTree()
        {
            return property_access_tree;
        }

        #endregion
    }
}
