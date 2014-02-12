using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Windows;

namespace GongDragDropTest
{
    public partial class MainWindow : Window, IDropTarget
    {
        private AdvancedDropHandler drop_handler = new AdvancedDropHandler();

        public ObservableCollection<Group> Groups
        {
            get { return (ObservableCollection<Group>)GetValue(GroupsProperty); }
            set { SetValue(GroupsProperty, value); }
        }
        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register("Groups", typeof(ObservableCollection<Group>), typeof(MainWindow), new PropertyMetadata(null));
        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Groups = new ObservableCollection<Group>();

            for (int g = 1; g < 6; g++)
            {
                var cg = new Group { Name = string.Format("Group {0}", g) };
                Groups.Add(cg);
                for (int c = 1; c < 6; c++)
                {
                    var cc = new Category { Name = string.Format("Category {0}-{1}", g, c) };
                    cg.Categories.Add(cc);
                    for (int p = 1; p < 6; p++)
                    {
                        var cp = new Pattern { Name = string.Format("Pattern {0}-{1}-{2}", g, c, p) };
                        cc.Patterns.Add(cp);
                    }
                }
            }
        }

        void IDropTarget.DragOver(IDropInfo dropInfo)
        {
            drop_handler.DragOver(dropInfo);
        }

        void IDropTarget.Drop(IDropInfo dropInfo)
        {
            drop_handler.Drop(dropInfo);
        }
    }
}
