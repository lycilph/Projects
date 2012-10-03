using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SortUtils
{
    public interface IComparerFactory<T>
    {
        BaseComparer<T> Create(SortArgument sort_argument);
        BaseComparer<T> Create(string property);
        BaseComparer<T> Create(string property, ListSortDirection direction);
    }
}
