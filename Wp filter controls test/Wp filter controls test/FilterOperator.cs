using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wp_filter_controls_test
{
    public class FilterOperator
    {
        public string Name { get; private set; }
        public bool IsRangeOperator { get; private set; }
        public FilterOperator(string n, bool is_range)
        {
            Name = n;
            IsRangeOperator = is_range;
        }
    }
}
