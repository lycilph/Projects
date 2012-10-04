using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ObservableObjectLibrary;

namespace ObservableObjectTest
{
    public class ReflectionDrillDownTestObject : ObservableObjectV2
    {
        public class Inner : ObservableObjectV2
        {
            public int InnerProp1
            {
                get { return Get(() => InnerProp1); }
                set { Set(() => InnerProp1, value); }
            }
        }

        public readonly Inner inner = new Inner();

        [DependsUponV2("inner.InnerProp1")]
        public int Prop1
        {
            get { return Get(() => Prop1); }
            set { Set(() => Prop1, value); }
        }

        public ReflectionDrillDownTestObject(int p1)
        {
            inner.InnerProp1 = p1;
            Prop1 = p1;
        }
    }
}
