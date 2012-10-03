using System.Collections.ObjectModel;

namespace Hierarchy_Test
{
    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }
        public Node(string n) { Name = n; Nodes = new ObservableCollection<Node>(); }
    }
}
