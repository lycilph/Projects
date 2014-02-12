using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.Windows.Controls;

namespace EditableTextBlockV2
{
    public partial class MainWindow : IDropTarget
    {
        private AdvancedDropHandler advanced_drop_handler = new AdvancedDropHandler();
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();

        public ObservableViewModelCollection<CategoryViewModel, Category> CategoryViewModels
        {
            get { return (ObservableViewModelCollection<CategoryViewModel, Category>)GetValue(CategoryViewModelsProperty); }
            set { SetValue(CategoryViewModelsProperty, value); }
        }
        public static readonly DependencyProperty CategoryViewModelsProperty =
            DependencyProperty.Register("CategoryViewModels", typeof(ObservableViewModelCollection<CategoryViewModel, Category>), typeof(MainWindow), new PropertyMetadata(null));

        public CategoryViewModel SelectedCategoryViewModel
        {
            get { return (CategoryViewModel)GetValue(SelectedCategoryViewModelProperty); }
            set { SetValue(SelectedCategoryViewModelProperty, value); }
        }
        public static readonly DependencyProperty SelectedCategoryViewModelProperty =
            DependencyProperty.Register("SelectedCategoryViewModel", typeof(CategoryViewModel), typeof(MainWindow), new PropertyMetadata(null));

        public PatternViewModel SelectedPatternViewModel
        {
            get { return (PatternViewModel)GetValue(SelectedPatternViewModelProperty); }
            set { SetValue(SelectedPatternViewModelProperty, value); }
        }
        public static readonly DependencyProperty SelectedPatternViewModelProperty =
            DependencyProperty.Register("SelectedPatternViewModel", typeof(PatternViewModel), typeof(MainWindow), new PropertyMetadata(null));
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            categories = new ObservableCollection<Category>();
            for (int c = 1; c < 6; c++)
            {
                var cc = new Category { Name = string.Format("Category {0}", c) };
                categories.Add(cc);
                for (int p = 1; p < 6; p++)
                {
                    var cp = new Pattern(string.Format("Pattern {0}-{1}", c, p), string.Format("*{0}*", p));
                    cc.Patterns.Add(cp);
                }
            }

            CategoryViewModels = new ObservableViewModelCollection<CategoryViewModel, Category>(categories, m => new CategoryViewModel(m));
            CategoryViewModels.First().IsSelected = true;
        }

        private void CategoriesListBoxPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F2) return;

            SelectedCategoryViewModel.IsEditing = true;
            e.Handled = true;
        }

        private void PatternsListBoxPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F2) return;

            SelectedPatternViewModel.ToggleEditType();
            e.Handled = true;
        }

        private void ChangeClick(object sender, RoutedEventArgs e)
        {
            categories[0].Name = categories[0].Name + "*";
        }

        private void AddCategoryClick(object sender, RoutedEventArgs e)
        {
            var c = new Category { Name = "New category" };
            c.Patterns.Add(new Pattern("New pattern", "*"));
            categories.Add(c);
        }

        private void WriteClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.Print("Dumping models");
            foreach (var c in categories)
            {
                System.Diagnostics.Debug.Print(c.Name);
                foreach (var p in c.Patterns)
                    System.Diagnostics.Debug.Print(" - " + p.Name);
            }
        }

        public new void DragOver(IDropInfo drop_info)
        {
            advanced_drop_handler.DragOver(drop_info);
        }

        public new void Drop(IDropInfo drop_info)
        {
            advanced_drop_handler.Drop(drop_info);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listbox = sender as ListBox;
            if (listbox == null) return;

            var selected_categories = listbox.SelectedItems.OfType<CategoryViewModel>();
            if (selected_categories.Any())
            {
                SelectedCategoryViewModel = selected_categories.Last();
                return;
            }

            var selected_patterns = listbox.SelectedItems.OfType<PatternViewModel>();
            if (selected_patterns.Any())
            {
                SelectedPatternViewModel = selected_patterns.Last();
                return;
            }
        }
    }
}
