using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using MVVM.Subscribers;

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

        public bool DoesEntireTreeSupportINotifyPropertyChangedAndChanging()
        {
            return Children.All(child => child.DoesEntireTreeSupportINotifyPropertyChangedAndChanging());
        }

        public void DumpToLog()
        {
            log.Trace(string.Format("Property access tree ({0})", Children.Count));
            foreach (var child in Children)
                child.DumpToLog();
        }

        public PropertyAccessTreeSubscriber<TListener> CreateSubscriptionTree<TListener>(Action<TListener, object> on_property_changed)
        {
            if (!DoesEntireTreeSupportINotifyPropertyChangedAndChanging())
                throw new ArgumentException("All objects must implement INotifyPropertyChanged and INotifyPropertyChanging");

            return new PropertyAccessTreeSubscriber<TListener>(this, on_property_changed);
        }

        #region Graph debug output

        public IEnumerable<Node> GraphDebug_GetChildren()
        {
            return Children;
        }

        #endregion
    }
}
