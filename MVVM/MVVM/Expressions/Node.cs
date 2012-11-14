using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public abstract bool IsDuplicate(Node other);
        public abstract void DumpToLog();
    }
}
