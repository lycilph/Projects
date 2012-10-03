using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Wp_filter_controls_test
{
    public class AggregateFilter : FilterBase
    {
        public readonly List<FilterBase> filters = new List<FilterBase>();

        public int Count { get { return filters.Count; } }

        public override bool Filter(object o)
        {
            // Returns true if all elements is true
            return filters.All(f => f.Filter(o));
        }

        public void Add(FilterBase filter_to_add)
        {
            filters.Add(filter_to_add);
            filter_to_add.PropertyChanged += FilterChanged;
            Refresh();
        }

        public void Remove(FilterBase filter_to_remove) { Remove(filter_to_remove, true); }
        public void Remove(FilterBase filter_to_remove, bool do_refresh)
        {
            if (filters.Contains(filter_to_remove))
            {
                filters.Remove(filter_to_remove);
                filter_to_remove.PropertyChanged -= FilterChanged;

                if (do_refresh)
                    Refresh();
            }
        }

        public void Clear()
        {
            foreach (var filter in filters)
                filter.PropertyChanged -= FilterChanged;
            filters.Clear();
            Refresh();
        }

        private void FilterChanged(object sender, PropertyChangedEventArgs e)
        {
            Refresh();
        }
    }
}
