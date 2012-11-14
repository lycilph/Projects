using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QuickGraph;
using MVVM.Expressions;
using System.Collections.ObjectModel;

namespace Test
{
    public partial class DebugGraphWindow : Window
    {
        public ObservableCollection<PropertyAccessTree> PropertyAccessTrees { get; private set; }

        public DebugGraphWindow()
        {
            PropertyAccessTrees = new ObservableCollection<PropertyAccessTree>();
            InitializeComponent();
            DataContext = this;
        }

        public void AddPropertyAccessTrees(IEnumerable<PropertyAccessTree> trees)
        {
            PropertyAccessTrees.Clear();

            foreach (var tree in trees)
                PropertyAccessTrees.Add(tree);

            ShowPropertyAccessTree(PropertyAccessTrees.First());
        }

        private void ShowPropertyAccessTree(PropertyAccessTree tree)
        {
            var g = new BidirectionalGraph<object, IEdge<object>>();

            foreach (var node in tree.GraphDebug_GetChildren())
            {
                var parameter_node = node as ParameterNode;
                if (parameter_node == null) continue;

                var parameter_node_name = string.Format("{0} - {1}", parameter_node.Type.UnderlyingSystemType.Name, parameter_node.Name);
                g.AddVertex(parameter_node_name);

                BuildPropertyAccessTreeGraph(g, parameter_node_name, parameter_node.Children);
            }
            
            graph_layout.Graph = g;
        }

        private void BuildPropertyAccessTreeGraph(BidirectionalGraph<object, IEdge<object>> g, string root, IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                var property_node = node as PropertyNode;
                if (property_node == null) continue;

                var property_node_name = string.Format("{0} - {1}", property_node.Type.UnderlyingSystemType.Name, property_node.PropertyName);
                g.AddVertex(property_node_name);
                g.AddEdge(new Edge<object>(root, property_node_name));

                if (property_node.Children.Count > 0)
                    BuildPropertyAccessTreeGraph(g, property_node_name, property_node.Children);
            }
        }

        private void ListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collection_source = CollectionViewSource.GetDefaultView(PropertyAccessTrees);
            var item = collection_source.CurrentItem as PropertyAccessTree;
            if (item != null)
                ShowPropertyAccessTree(item);
        }
    }
}
