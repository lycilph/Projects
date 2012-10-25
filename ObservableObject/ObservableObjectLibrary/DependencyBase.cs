using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace ObservableObjectLibrary
{
    public class DependencyBase
    {
        public string property_name;

        protected DependencyBase(string name)
        {
            property_name = name;
        }

        protected virtual void Remove() { }
        protected virtual void Add(ObservableObject obj) { }

        public void Update(ObservableObject obj)
        {
            Remove();
            Add(obj);
        }

        public virtual bool DependensOn(string property)
        {
            return false;
        }
    }
}
