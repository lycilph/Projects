using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ObservableObject
{
    public class Model : ObservableObjectLibrary.ObservableObject
    {
        protected ObservableCollection<string> _Categories;
        public ObservableCollection<string> Categories
        {
            get { return _Categories; }
            protected set { SetAndRaiseIfChanged(() => Categories, value); }
        }

        public int CategoriesCount
        {
            get { return Categories.Count; }
        }

        public Model()
        {
            Categories = new ObservableCollection<string>();

            AddDependency(() => Categories, () => CategoriesCount);
        }
    }
}
