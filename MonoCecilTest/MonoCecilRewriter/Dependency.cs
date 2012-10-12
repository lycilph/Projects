using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoCecilRewriter
{
    public class Dependency
    {
        public string source;
        public PropertyType source_type;

        public List<string> targets = new List<string>();

        public Dependency(string source, string target) : this(source, target, PropertyType.AutoProperty) {}
        public Dependency(string source, string target, PropertyType source_type)
        {
            this.source = source;
            this.source_type = source_type;

            targets.Add(target);
        }
    }
}
