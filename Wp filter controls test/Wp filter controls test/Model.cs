using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wp_filter_controls_test
{
    public class Model
    {
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Value { get; set; }
        public string Match { get; set; }
        public Model(string t)
        {
            Date = DateTime.Now;
            Text = t;
            Value = 42.0;
            Match = "Category";
        }
    }
}
