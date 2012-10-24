using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ObservableObject
{
    public class Model : ObservableObject
    {
        private int _ModelProp1;
        public int ModelProp1
        {
            get { return _ModelProp1; }
            set
            {
                NotifyPropertyChanging("ModelProp1");
                _ModelProp1 = value;
                NotifyPropertyChanged("ModelProp1");
            }
        }

        private ObservableCollection<int> _ModelProp2;
        public ObservableCollection<int> ModelProp2
        {
            get { return _ModelProp2; }
            set
            {
                NotifyPropertyChanging("ModelProp2");
                _ModelProp2 = value;
                NotifyPropertyChanged("ModelProp2");
            }
        }

        public Model()
        {
            ModelProp1 = 0;
            ModelProp2 = new ObservableCollection<int>();
        }
    }
}
