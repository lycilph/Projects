using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObservableObject
{
    public class ViewModel : ObservableObject
    {
        private Model _model;
        private Model WrappedModel
        {
            get { return _model; }
            set
            {
                NotifyPropertyChanging("WrappedModel");
                _model = value;
                NotifyPropertyChanged("WrappedModel");
            }
        }

        public int ViewModelProp1
        {
            get { return WrappedModel.ModelProp1*2; }
        }

        public int ViewModelProp2
        {
            get { return WrappedModel.ModelProp2.Count; }
        }

        public int ViewModelProp3
        {
            get { return 42; }
        }

        public ViewModel(Model model)
        {
            this.WrappedModel = model;

            AddDependency(() => WrappedModel.ModelProp1, () => ViewModelProp1);
            AddDependency(() => WrappedModel.ModelProp2, () => ViewModelProp2);
            AddDependency(() => WrappedModel, () => ViewModelProp3);
        }
    }
}
