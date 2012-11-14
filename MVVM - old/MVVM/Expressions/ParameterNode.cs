using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace MVVM.Expressions
{
    public class ParameterNode : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly Type _Type;
        public override Type Type
        {
            get { return _Type; }
        }

        public string Name { get; private set; }

        public ParameterNode(Type type, string name)
        {
            _Type = type;
            Name = name;
        }

        public override bool IsDuplicate(Node other)
        {
            var other_as_parameter_node = other as ParameterNode;

            return other_as_parameter_node != null &&
                   other_as_parameter_node != this && 
                   other_as_parameter_node.Name == Name;
        }

        public override void DumpToLog()
        {
            log.Trace(string.Format("Parameter node ({0}, {1}, {2})", Name, Type.ToString(), Children.Count));
            foreach (var child in Children)
            {
                child.DumpToLog();
            }
        }
    }
}
