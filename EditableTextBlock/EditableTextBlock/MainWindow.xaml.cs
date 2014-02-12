using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EditableTextBlock
{
    public partial class MainWindow
    {
        public ObservableCollection<Category> Categories
        {
            get { return (ObservableCollection<Category>)GetValue(CategoriesProperty); }
            set { SetValue(CategoriesProperty, value); }
        }
        public static readonly DependencyProperty CategoriesProperty =
            DependencyProperty.Register("Categories", typeof(ObservableCollection<Category>), typeof(MainWindow), new PropertyMetadata(null));
                
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Categories = new ObservableCollection<Category>();
            for (int c = 1; c < 6; c++)
            {
                var cc = new Category { Name = string.Format("Category {0}", c) };
                Categories.Add(cc);
                for (int p = 1; p < 6; p++)
                {
                    var cp = new Pattern(string.Format("Pattern {0}-{1}", c, p), string.Format("*{0}*", p));
                    cc.Patterns.Add(cp);
                }
            }
        }

        private void ListBoxMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listbox = GetVisualParent<ListBox>((DependencyObject)sender);
            if (listbox == null) return;

            ListBoxItem item = listbox.ItemContainerGenerator.ContainerFromIndex(listbox.SelectedIndex) as ListBoxItem;
            if (item != null)
            {
                // Get the item's template parent
                ContentPresenter templateParent = GetFrameworkChild<ContentPresenter>(item);

                DataTemplate dataTemplate = listbox.ItemTemplate;
                if (dataTemplate != null && templateParent != null)
                {
                    var ec = dataTemplate.FindName("editable_control", templateParent) as EditableControl;
                    if (ec == null) return;

                    ec.IsEditing = true;
                    e.Handled = true;
                }
            }
        }

        private void ListBoxPreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F2) return;

            var listbox = GetVisualParent<ListBox>((DependencyObject)sender);
            if (listbox == null) return;

            ListBoxItem item = listbox.ItemContainerGenerator.ContainerFromIndex(listbox.SelectedIndex) as ListBoxItem;
            if (item != null)
            {
                // Get the item's template parent
                ContentPresenter templateParent = GetFrameworkChild<ContentPresenter>(item);

                DataTemplate dataTemplate = listbox.ItemTemplate;
                if (dataTemplate != null && templateParent != null)
                {
                    var ec = dataTemplate.FindName("editable_control", templateParent) as EditableControl;
                    if (ec == null) return;

                    ec.IsEditing = true;
                    e.Handled = true;
                }
            }
        }

        private static T GetVisualParent<T>(DependencyObject obj) where T : Visual
        {
            Visual parent_object = VisualTreeHelper.GetParent(obj) as Visual;
            if (parent_object == null) return null;
            
            return parent_object is T ? parent_object as T : GetVisualParent<T>(parent_object);
        }

        private static T GetFrameworkChild<T>(FrameworkElement referenceElement) where T : FrameworkElement
        {
            FrameworkElement child = null;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(referenceElement); i++)
            {
                child = VisualTreeHelper.GetChild(referenceElement, i) as FrameworkElement;
                if (child != null && child.GetType() == typeof(T))
                {
                    break;
                }
                else if (child != null)
                {
                    child = GetFrameworkChild<T>(child);
                    if (child != null && child.GetType() == typeof(T))
                        break;
                }
            }
            return child as T;
        }
    }
}
