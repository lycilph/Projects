using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Wpf_test
{
    public class AggregateFilter
    {
        private List<Predicate<object>> filters = new List<Predicate<object>>();
        private ICollectionView collection_view = null;

        public AggregateFilter(ICollectionView cv)
        {
            collection_view = cv;
            collection_view.Filter = Filter;
        }

        private bool Filter(object o)
        {
            foreach (var f in filters)
                if (!f(o))
                    return false;
            return true;
        }

        public void Add(Predicate<object> filter_to_add)
        {
            filters.Add(filter_to_add);
            collection_view.Refresh();
        }

        public void Remove(Predicate<object> filter_to_remove)
        {
            if (filters.Contains(filter_to_remove))
            {
                filters.Remove(filter_to_remove);
                collection_view.Refresh();
            }
        }
    }
}
