using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace MVVM.Expressions
{
    public class PropertyAccessTree
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        internal List<Node> Children { get; set; }

        public PropertyAccessTree()
        {
            Children = new List<Node>();
        }

        public PropertyNode GetPropertyNode(string property_name)
        {
            return GetPropertyNode(property_name, Children);
        }

        private PropertyNode GetPropertyNode(string property_name, List<Node> nodes)
        {
            if (nodes == null || nodes.Count == 0)
                return null;

            PropertyNode property_node;
            foreach (var node in nodes)
            {
                if (node is PropertyNode)
                {
                    property_node = node as PropertyNode;
                    if (property_node.PropertyName == property_name)
                        return property_node;
                }

                property_node = GetPropertyNode(property_name, node.Children);
                if (property_node != null)
                    return property_node;
            }
            return null;
        }

        public void Subscribe(Action callback)
        {
            foreach (var child in Children)
                child.Subscribe(null, callback);
        }

        public void Unsubscribe()
        {
            foreach (var child in Children)
                child.Unsubscribe(null);
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Property access tree ({0} children)", Children.Count));
            foreach (var child in Children)
                child.DumpToLog();
        }
    }
}
