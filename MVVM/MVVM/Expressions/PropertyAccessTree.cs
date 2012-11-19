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
