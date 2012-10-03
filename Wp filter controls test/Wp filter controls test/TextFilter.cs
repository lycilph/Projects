using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Wp_filter_controls_test
{
    public class TextFilter : FilterBase
    {
        public TextFilter()
        {
            Name = "Text";
            Operators.Add(new FilterOperator("Contains", false));
            Operators.Add(new FilterOperator("StartsWith", false));
            Operators.Add(new FilterOperator("EndsWith", false));
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
            if (string.IsNullOrWhiteSpace(m.Text))
                return false;

            switch (CurrentOperator.Name)
            {
                case "Contains":
                    // Return true if no text is defined in the filter
                    if (string.IsNullOrEmpty(Input1))
                        return true;
                    // Returns true if the input contains the filter text defined in Input1
                    int index = m.Text.IndexOf(Input1, 0, StringComparison.InvariantCultureIgnoreCase);
                    return index > -1;
                case "StartsWith":
                    // Return true if no text is defined in the filter
                    if (string.IsNullOrEmpty(Input1))
                        return true;
                    // Returns true if the input starts with the filter text defined in Input1
                    return m.Text.StartsWith(Input1, StringComparison.InvariantCultureIgnoreCase);
                case "EndsWith":
                    // Return true if no text is defined in the filter
                    if (string.IsNullOrEmpty(Input1))
                        return true;
                    // Returns true if the input starts with the filter text defined in Input1
                    return m.Text.EndsWith(Input1, StringComparison.InvariantCultureIgnoreCase);
                default:
                    return false;
            }
        }
    }
}
