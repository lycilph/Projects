using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObservableObjectLibrary;

namespace ObservableObjectTest
{
    public class PropertyToPropertyDependencyTestObject : ObservableObject
    {
        public int Prop1
        {
            get { return Get(() => Prop1); }
            set { Set(() => Prop1, value); }
        }

        [DependsUpon("Prop1")]
        public int Prop2
        {
            get { return Prop1; }
        }

        public PropertyToPropertyDependencyTestObject(int p1)
        {
            Prop1 = p1;
        }
    }
}
