using ObservableObjectLibrary;

namespace ObservableObjectTest
{
    public class ChainedPropertyDependencyTestObject : ObservableObject
    {
        public int Prop1
        {
            get { return Get(() => Prop1); }
            set { Set(() => Prop1, value); }
        }

        [DependsUpon("Prop1")]
        public int Prop2
        {
            get { return Get(() => Prop2); }
            set { Set(() => Prop2, value); }
        }

        [DependsUpon("Prop2")]
        public int Prop3
        {
            get { return Get(() => Prop3); }
            set { Set(() => Prop3, value); }
        }

        public ChainedPropertyDependencyTestObject(int p1, int p2, int p3)
        {
            Prop1 = p1;
            Prop2 = p2;
            Prop3 = p3;
        }
    }
}
