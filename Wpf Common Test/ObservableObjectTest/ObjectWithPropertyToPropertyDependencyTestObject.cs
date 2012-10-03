using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObservableObjectLibrary;

namespace ObservableObjectTest
{
    public class ObjectWithPropertyToPropertyDependencyTestObject : ObservableObject
    {
        public class Base : ObservableObject
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

            public Base(int p1, int p2)
            {
                Prop1 = p1;
                Prop2 = p2;
            }
        }

        public Base obj = new Base(0, 0);

        [DependsUpon("obj.Prop1")]
        public int Prop1
        {
            get { return obj.Prop1; }
        }

        public ObjectWithPropertyToPropertyDependencyTestObject(int p1, int p2)
        {
            obj.Prop1 = p1;
            obj.Prop2 = p2;
        }
    }
}
