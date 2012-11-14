using System;
using System.Reflection;
using NLog;

namespace MVVM.Expressions
{
    public class PropertyNode : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

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
    }
}
