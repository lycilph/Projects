using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Wpf_toolkit_test
{
    public class AggregateComparer: Comparer<Data>
    {
        private ListCollectionView view = null;
        public readonly List<Comparer<Data>> comparers = new List<Comparer<Data>>();

        public AggregateComparer(ListCollectionView v)
        {
            view = v;
            view.CustomSort = this;
        }

        public void AddComparer(Comparer<Data> c)
        {
            comparers.Add(c);
        }

        public void Refresh()
        {
            view.Refresh();
        }

        public override int Compare(Data x, Data y)
        {
            int result = 0;

            foreach (var c in comparers)
            {
                result = c.Compare(x, y);
                if (result != 0)
                    return result;
            }

            return result;
        }
    }
}
