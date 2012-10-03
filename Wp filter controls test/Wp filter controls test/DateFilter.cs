using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wp_filter_controls_test
{
    public class DateFilter : FilterBase
    {
        public DateFilter()
        {
            Name = "Date";
            Operators.Add(new FilterOperator("Equals", false));
            Operators.Add(new FilterOperator("Before", false));
            Operators.Add(new FilterOperator("After", false));
            Operators.Add(new FilterOperator("Between", true));
            CurrentOperator = Operators.First();
        }

        public override bool Filter(object o)
        {
            if (CurrentOperator == null)
                return true;

            // Return false if the input is NOT a model class or the text property is empty
            Model m = o as Model;
            if (m == null)
                return false;
            if (string.IsNullOrWhiteSpace(Input1))
                return true;
            if (CurrentOperator.IsRangeOperator && (string.IsNullOrWhiteSpace(Input1) || string.IsNullOrWhiteSpace(Input2)))
                return true;

            DateTime filter_date_1;
            DateTime filter_date_2;
            bool filter_date_1_valid = DateTime.TryParse(Input1, out filter_date_1);
            bool filter_date_2_valid = DateTime.TryParse(Input2, out filter_date_2);

            IsValid = (CurrentOperator.IsRangeOperator
                           ? filter_date_1_valid && filter_date_2_valid
                           : filter_date_1_valid);

            if (IsValid)
            {
                switch (CurrentOperator.Name)
                {
                    case "Equals":
                        return m.Date.Date == filter_date_1.Date;
                    case "Before":
                        return m.Date.Date.CompareTo(filter_date_1) < 0;
                    case "After":
                        return m.Date.Date.CompareTo(filter_date_1) > 0;
                    case "Between":
                        return m.Date.Date.CompareTo(filter_date_1.Date) > 0 &&
                               m.Date.Date.CompareTo(filter_date_2.Date) < 0;
                    default:
                        return false;
                }
            }

            return true;
        }
    }
}
