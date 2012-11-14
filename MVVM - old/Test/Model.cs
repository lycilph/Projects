using MVVM.Observable;

namespace Test
{
    public class Model : ObservableObject
    {
        private int _Prop1;
        public int Prop1
        {
            get { return _Prop1; }
            set { SetAndRaiseIfChanged(() => Prop1, value); }
        }

        private int _Prop2;
        public int Prop2
        {
            get { return _Prop2; }
            set { SetAndRaiseIfChanged(() => Prop2, value); }
        }
        
        public Model(int p1, int p2)
        {
            Prop1 = p1;
            Prop2 = p2;
        }
    }
}
