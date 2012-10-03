using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wp_filter_controls_test
{
    public class FilterFactory
    {
        public static List<string> GetAvailableFilters()
        {
            return new List<string>() {"Date", "Text"};
        }

        public static FilterBase Create(string filter)
        {
            switch (filter)
            {
                case "Date": return new DateFilter();
                case "Text": return new TextFilter();
                default: return null;
            }
        }
    }
}
