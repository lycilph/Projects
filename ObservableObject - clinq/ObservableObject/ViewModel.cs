using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObservableObject
{
    public class ViewModel : Library.ObservableObject
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

        static ViewModel()
        {
            var dependency = Register<ViewModel>();

            dependency.Call(obj => obj.NotifyPropertyChanged("Total"))
                      .OnChanged(obj => obj.WrappedModel.Prop1 + obj.WrappedModel.Prop2);
        }

        public ViewModel(Model m)
        {
            WrappedModel = m;
        }
    }
}
