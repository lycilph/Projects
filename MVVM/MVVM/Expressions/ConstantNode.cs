using System;
using System.ComponentModel;
using NLog;

namespace MVVM.Expressions
{
    public class ConstantNode : Node
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public INotifyPropertyChanged Value { get; set; }

        public ConstantNode(INotifyPropertyChanged value, string name)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; private set; }

        public override Type Type
        {
            get { return Value.GetType(); }
        }

        public override bool IsDuplicate(Node other)
        {
            var other_as_constant_node = other as ConstantNode;

            return other_as_constant_node != null &&
                   other_as_constant_node != this &&
                   other_as_constant_node.Value == Value;
        }

        public override void DumpToLog()
        {
            log.Trace(string.Format("Constant node ({0}, {1}, {2})", Name, Type.ToString(), Children.Count));
            foreach (var child in Children)
                child.DumpToLog();
        }

        #region Subscription

        public override void Subscribe(INotifyPropertyChanged subject, Action callback)
        {
            foreach (var child in Children)
                child.Subscribe(Value, callback);
        }

        public override void Unsubscribe(INotifyPropertyChanged subject)
        {
            foreach (var child in Children)
                child.Unsubscribe(Value);
        } 

        #endregion
    }
}
