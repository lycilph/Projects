using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ObservableObject
{
    public class ViewModel : ObservableObjectLibrary.ObservableObject
    {
        private readonly ObservableCollection<string> all_categories = new ObservableCollection<string>();

        protected Model _WrappedModel;
        public Model WrappedModel
        {
            get { return _WrappedModel; }
            set { SetAndRaiseIfChanged(() => WrappedModel, value); }
        }

        public ObservableCollection<string> AllCategories
        {
            get
            {
                all_categories.Clear();
                all_categories.Add("***");
                foreach (var category in WrappedModel.Categories)
                    all_categories.Add(category);
                return all_categories;
            }
        }

        public int AllCategoriesCount
        {
            get { return all_categories.Count; }
        }

        public ViewModel(Model model)
        {
            WrappedModel = model;

            AddDependency(() => WrappedModel.Categories, () => AllCategories);
            AddDependency(() => AllCategories, () => AllCategoriesCount);
        }
    }
}
