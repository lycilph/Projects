using System.Collections.ObjectModel;
using MVVM.Observable;

namespace Test
{
    public class ViewModel : ObservableObject
    {
        private Model _WrappedModel;
        public Model WrappedModel
        {
            get { return _WrappedModel; }
            set { SetAndRaiseIfChanged(() => WrappedModel, value); }
        }

        public int Total
        {
            get { return WrappedModel.Prop1 + WrappedModel.Prop2; }
        }

        private ObservableCollection<string> _Items;
        public ObservableCollection<string> Items
        {
            get { return _Items; }
            set { SetAndRaiseIfChanged(() => Items, value); }
        }

        public int ItemsCount
        {
            get { return Items.Count; }
        }

        static ViewModel()
        {
            var dependency = Register<ViewModel>();

            dependency.Call(obj => obj.NotifyPropertyChanged("Total"))
                      .OnChanged(obj => obj.WrappedModel.Prop1 + obj.WrappedModel.Prop2);

            dependency.Call(obj => obj.NotifyPropertyChanged("ItemsCount"))
                      .OnChanged(obj => obj.Items);
        }

        public ViewModel(Model m)
        {
            WrappedModel = m;
            Items = new ObservableCollection<string>();
        }
    }
}
