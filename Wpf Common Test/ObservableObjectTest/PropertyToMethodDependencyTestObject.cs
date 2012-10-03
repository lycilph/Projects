using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObservableObjectLibrary;

namespace ObservableObjectTest
{
    public class PropertyToMethodDependencyTestObject : ObservableObject
    {
        public int Prop1
        {
            get { return Get(() => Prop1); }
            set { Set(() => Prop1, value); }
        }

        public int Prop2
        {
            get { return Get(() => Prop2); }
            set { Set(() => Prop2, value); }
        }

        [DependsUpon("Prop1")]
        public void Method()
        {
            Prop2 = Prop1;
        }

        public PropertyToMethodDependencyTestObject(int p1, int p2)
        {
            Prop1 = p1;
            Prop2 = p2;
        }
    }
}
