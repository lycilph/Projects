using System.Collections.Generic;
using System.ComponentModel;

namespace SortUtils
{
    public abstract class BaseComparer<T> : Comparer<T>
    {
        public string Property { get; set; }
        public ListSortDirection Direction { get; set; }

        protected BaseComparer() : this(string.Empty, ListSortDirection.Ascending) {}
        protected BaseComparer(string prop, ListSortDirection dir)
        {
            Property = prop;
            Direction = dir;
        }
    }
}
