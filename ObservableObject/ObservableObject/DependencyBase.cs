using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObservableObject
{
    public class DependencyBase
    {
        public string property_name;

        public DependencyBase(string name)
        {
            property_name = name;
        }

        public virtual void Remove() {}
    }
}
