using System;
using System.Collections.Generic;
using MVVM.Expressions;
using NLog;

namespace MVVM.Subscribers
{
    internal class ConstantNodeSubscriber : INodeSubscriber
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly Action notification_callback;
        private readonly ConstantNode constant_node;
        private readonly List<PropertyNodeSubscriber> children;

        public ConstantNodeSubscriber(ConstantNode node, Action callback)
        {
            constant_node = node;
            notification_callback = callback;
            children = new List<PropertyNodeSubscriber>();

            BuildChildren();
        }

        private void BuildChildren()
        {
            foreach (var node in constant_node.Children)
            {
                var property_node = node as PropertyNode;
                if (property_node == null)
                    throw new ArgumentException("Node must be of type PropertyNode");

                var property_subscriber = new PropertyNodeSubscriber(property_node, notification_callback);
                children.Add(property_subscriber);
            }
        }

        public void Subscribe()
        {
            foreach (var subscriber in children)
                subscriber.Subscribe(constant_node.Value);
        }

        public void Unsubscribe()
        {
            foreach (var subscriber in children)
                subscriber.Unsubscribe(constant_node.Value);
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Constant subscriber ({0}, {1})", constant_node.Name, children.Count));
            foreach (var child in children)
                child.DumpToLog();
        }
    }
}
