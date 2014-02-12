using System.Linq;

namespace EditableTextBlockV2
{
    public class CategoryViewModel : ViewModelBase<Category>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; }
        }

        private ObservableViewModelCollection<PatternViewModel, Pattern> _PatternViewModels;
        public ObservableViewModelCollection<PatternViewModel, Pattern> PatternViewModels
        {
            get { return _PatternViewModels; }
            set
            {
                if (_PatternViewModels == value) return;
                _PatternViewModels = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IsEditing = false;
        public bool IsEditing
        {
            get { return _IsEditing; }
            set
            {
                if (_IsEditing == value) return;
                _IsEditing = value;
                NotifyPropertyChanged();
            }
        }

        public CategoryViewModel(Category category) : base(category)
        {
            PatternViewModels = new ObservableViewModelCollection<PatternViewModel, Pattern>(Model.Patterns, p => new PatternViewModel(p));

            if (PatternViewModels.Any())
                PatternViewModels.First().IsSelected = true;
        }
    }
}
