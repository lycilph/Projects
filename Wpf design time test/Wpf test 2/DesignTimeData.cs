using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Wpf_test_2
{
    public class DesignTimeData : ObservableCollection<Tuple<string, int>>
    {
        public DesignTimeData()
        {
            Add(Tuple.Create("Designtime string 1", 11));
            Add(Tuple.Create("Designtime string 2", 22));
            Add(Tuple.Create("Designtime string 3", 33));
        }
    }
}
