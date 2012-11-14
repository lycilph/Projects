using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MVVM.Observable;
using System.ComponentModel;

namespace Test
{
    public class ViewModel : ViewModelBase
    {
        private ModelWrapped _WrappedModel;
        public ModelWrapped WrappedModel
        {
            get { return _WrappedModel; }
            set { SetAndRaiseIfChanged(() => WrappedModel, value); }
        }

        public int TotalVM
        {
            get { return WrappedModel.Model.Prop1 + WrappedModel.Model.Prop2; }
        }

        public ViewModel(Model m)
        {
            WrappedModel = new ModelWrapped(m);

            // Add "proxy" properties
            AllProperties(m);

            // Add dynamic properties
            Property("Total", () => m.Prop1 + m.Prop2);
            Property("Total2", () => WrappedModel.Model.Prop1 + WrappedModel.Model.Prop2);

            // Add dependency for "native" view model property
            Dependency(() => TotalVM, () => WrappedModel.Model.Prop1 + WrappedModel.Model.Prop2);
        }
    }
}
