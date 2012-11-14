using MVVM.Observable;

namespace Test
{
    public class ModelWrapped : ObservableObject
    {
        private Model _Model;
        public Model Model
        {
            get { return _Model; }
            set { SetAndRaiseIfChanged(() => Model, value); }
        }

        public ModelWrapped(Model m)
        {
            Model = m;
        }
    }
}
