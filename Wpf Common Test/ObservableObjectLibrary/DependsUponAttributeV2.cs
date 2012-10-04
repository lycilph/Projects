using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObservableObjectLibrary
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class DependsUponV2Attribute : Attribute
    {
        public string Source { get; private set; }
        public string Filter { get; private set; }

        public bool HasFilter
        {
            get { return !string.IsNullOrEmpty(Filter); }
        }

        public DependsUponV2Attribute(string source) : this(source, string.Empty) {}
        public DependsUponV2Attribute(string source, string filter)
        {
            Source = source;
            Filter = filter;
        }

        public override string ToString()
        {
            return (HasFilter ? string.Format("{0} - filtered by {1}", Source, Filter) : Source);
        }
    }
}
