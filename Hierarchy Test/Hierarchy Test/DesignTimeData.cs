using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using NLog;

namespace Hierarchy_Test
{
    public class DesignTimeData : ObservableCollection<Node>
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public DesignTimeData()
        {
            log.Debug("Constructor");

            // Level 1
            Add(new Node("Level 1, A"));
            Add(new Node("Level 1, B"));
            Add(new Node("Level 1, C"));

            // Level 2
            Node n = this[0];
            n.Nodes.Add(new Node("Level 2, A, A"));
            n.Nodes.Add(new Node("Level 2, A, B"));
            n.Nodes.Add(new Node("Level 2, A, C"));
            n = this[1];
            n.Nodes.Add(new Node("Level 2, B, A"));
            n.Nodes.Add(new Node("Level 2, B, B"));
            n.Nodes.Add(new Node("Level 2, B, C"));
            n = this[2];
            n.Nodes.Add(new Node("Level 2, C, A"));
            n.Nodes.Add(new Node("Level 2, C, B"));
            n.Nodes.Add(new Node("Level 2, C, C"));

            // Level 3
            n = this[0].Nodes[2];
            n.Nodes.Add(new Node("Level 3, A, C, A"));
            n.Nodes.Add(new Node("Level 3, A, C, B"));
            n.Nodes.Add(new Node("Level 3, A, C, C"));
            n = this[1].Nodes[1];
            n.Nodes.Add(new Node("Level 3, B, B, A"));
            n.Nodes.Add(new Node("Level 3, B, B, B"));
            n.Nodes.Add(new Node("Level 3, B, B, C"));
            n = this[2].Nodes[0];
            n.Nodes.Add(new Node("Level 3, C, A, A"));
            n.Nodes.Add(new Node("Level 3, C, A, B"));
            n.Nodes.Add(new Node("Level 3, C, A, C"));
        }
    }
}
