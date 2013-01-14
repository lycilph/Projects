using System.Collections.ObjectModel;

namespace CategorizationEngine
{
    public class CategoryViewModel : ObservableObject
    {
        private readonly Category category;
        private Post post;

        public string Name
        {
            get { return category.Name; }
        }

        public bool HasFilters
        {
            get { return category.Filters.Count > 0; }
        }

        private ObservableCollection<CategoryViewModel> _Categories;
        public ObservableCollection<CategoryViewModel> Categories
        {
            get { return _Categories; }
            set
            {
                if (_Categories == value) return;
                _Categories = value;
                NotifyPropertyChanged("Categories");
            }
        }

        private bool _IsMatch;
        public bool IsMatch
        {
            get { return _IsMatch; }
            set
            {
                if (_IsMatch == value) return;
                _IsMatch = value;
                NotifyPropertyChanged("IsMatch");
            }
        }

        public CategoryViewModel(Category category)
        {
            this.category = category;
            IsMatch = false;
            Categories = new ObservableCollection<CategoryViewModel>();
        }

        public void SetPost(Post p)
        {
            post = p;
            IsMatch = post.Categories.Contains(category);

            foreach (var vm in Categories)
                vm.SetPost(p);
        }

        public static CategoryViewModel Create(Category root)
        {
            var root_view_model = new CategoryViewModel(root);
            foreach (var c in root.Categories)
            {
                var vm = Create(c);
                root_view_model.Categories.Add(vm);
            }
            return root_view_model;
        }
    }
}
