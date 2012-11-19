using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MVVM.Expressions
{
    public abstract class Node
    {
        private Action notification_callback;
        public abstract Type Type { get; }
        public List<Node> Children { get; set; }

        protected Node()
        {
            Children = new List<Node>();
        }

        public abstract bool IsDuplicate(Node other);
        public abstract void DumpToLog();

        public abstract void Subscribe(INotifyPropertyChanged subject, Action callback);
        public abstract void Unsubscribe(INotifyPropertyChanged subject);
    }
}
