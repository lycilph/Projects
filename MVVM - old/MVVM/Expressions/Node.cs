using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.ComponentModel;

namespace MVVM.Expressions
{
    public abstract class Node
    {
        public abstract Type Type { get; }
        public List<Node> Children { get; set; }

        protected Node()
        {
            Children = new List<Node>();
        }

        public bool DoesEntireTreeSupportINotifyPropertyChangedAndChanging()
        {
            if (Children.Count == 0)
                return true;

            if (!typeof(INotifyPropertyChanged).IsAssignableFrom(Type))
                return false;
            if (!typeof(INotifyPropertyChanging).IsAssignableFrom(Type))
                return false;

            return Children.All(child => child.DoesEntireTreeSupportINotifyPropertyChangedAndChanging());
        }

        public abstract bool IsDuplicate(Node other);
        public abstract void DumpToLog();
    }
}
