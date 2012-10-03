using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Wpf_fluidmove_test
{
    public class Data
    {
        public string Text { get; set; }
        public Brush Color { get; set; }
        public Data(string t, Brush c)
        {
            Text = t;
            Color = c;
        }
    }
}
