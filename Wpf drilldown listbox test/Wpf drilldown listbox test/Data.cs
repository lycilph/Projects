using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Wpf_drilldown_listbox_test
{
    public class Node
    {
        public string Text { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }

        public Node(string t)
        {
            Text = t;
            Nodes = new ObservableCollection<Node>();
        }
    }
}
