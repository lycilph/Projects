using System;
using System.Collections.Generic;
using MVVM.Expressions;
using NLog;

namespace MVVM.Subscribers
{
    internal class PropertyAccessTreeSubscriber
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly PropertyAccessTree property_access_tree;
        private readonly List<ConstantNodeSubscriber> subscribers;

        public PropertyAccessTreeSubscriber(PropertyAccessTree tree, Action notification_callback)
        {
            property_access_tree = tree;
            subscribers = new List<ConstantNodeSubscriber>();

            foreach (var node in property_access_tree.Children)
            {
                var constant_node = node as ConstantNode;
                if (constant_node == null)
                    throw new ArgumentException("Root nodes must be of type ConstantNode");

                var root = new ConstantNodeSubscriber(constant_node, notification_callback);
                subscribers.Add(root);
            }
        }

        public void Subscribe()
        {
            foreach (var subscriber in subscribers)
                subscriber.Subscribe();
        }

        public void Unsubscribe()
        {
            foreach (var subscriber in subscribers)
                subscriber.Unsubscribe();
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Property access tree subscriber ({0} roots)", subscribers.Count));
            foreach (var root in subscribers)
                root.DumpToLog();
        }
    }
}
