using System;
using System.ComponentModel;
using NLog;

namespace MVVM.Expressions
{
    public static class PropertyAccessTreeFactory
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static PropertyAccessTree Create(object obj, string property_name)
        {
            log.Trace("Building property access tree");

            PropertyAccessTree tree = new PropertyAccessTree();

            if (!TypeHelper.DoesTypeImplementINotifyPropertyChangedAndChanging(obj.GetType()))
                throw new ArgumentException("Object must implement INotifyPropertyChanging and INotifyPropertyChanged");

            var constant_node = new ConstantNode((INotifyPropertyChanged)obj, "this");
            tree.Children.Add(constant_node);

            var property_info = obj.GetType().GetProperty(property_name);
            var property_node = new PropertyNode(property_info);
            constant_node.Children.Add(property_node);

            tree.DumpToLog();
            return tree;
        }
    }
}
