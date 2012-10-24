using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObservableObject
{
    public class ViewModel : ObservableObject
    {
        private readonly Model model;

        public int ViewModelProp1
        {
            get { return model.ModelProp1*2; }
        }

        public int ViewModelProp2
        {
            get { return model.ModelProp2.Count; }
        }

        public int ViewModelProp3
        {
            get { return 42; }
        }

        public ViewModel(Model model)
        {
            this.model = model;

            AddDependency(() => model.ModelProp1, () => ViewModelProp1);
            AddDependency(() => model.ModelProp2, () => ViewModelProp2);
            AddDependency(() => model, () => ViewModelProp3);
        }
    }
}
