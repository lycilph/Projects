using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;

namespace Wp_filter_controls_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Model> ModelList
        {
            get { return (ObservableCollection<Model>)GetValue(ModelListProperty); }
            set { SetValue(ModelListProperty, value); }
        }
        public static readonly DependencyProperty ModelListProperty =
            DependencyProperty.Register("ModelList", typeof(ObservableCollection<Model>), typeof(MainWindow), new UIPropertyMetadata(null));

        public FilterControlViewModel FilterControl { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            #region ModelList

            ModelList = new ObservableCollection<Model>();
            ModelList.Add(new Model("through"));
            ModelList.Add(new Model("another"));
            ModelList.Add(new Model("because"));
            ModelList.Add(new Model("picture"));
            ModelList.Add(new Model("America"));
            ModelList.Add(new Model("between"));
            ModelList.Add(new Model("country"));
            ModelList.Add(new Model("thought"));
            ModelList.Add(new Model("example"));
            ModelList.Add(new Model("without"));
            ModelList.Add(new Model("banging"));
            ModelList.Add(new Model("bathtub"));
            ModelList.Add(new Model("blasted"));
            ModelList.Add(new Model("blended"));
            ModelList.Add(new Model("bobsled"));
            ModelList.Add(new Model("camping"));
            ModelList.Add(new Model("contest"));
            ModelList.Add(new Model("dentist"));
            ModelList.Add(new Model("disrupt"));
            ModelList.Add(new Model("himself"));
            ModelList.Add(new Model("jumping"));
            ModelList.Add(new Model("lending"));
            ModelList.Add(new Model("pinball"));
            ModelList.Add(new Model("planted"));
            ModelList.Add(new Model("plastic"));
            ModelList.Add(new Model("problem"));
            ModelList.Add(new Model("ringing"));
            ModelList.Add(new Model("shifted"));
            ModelList.Add(new Model("sinking"));
            ModelList.Add(new Model("sunfish"));
            ModelList.Add(new Model("trusted"));
            ModelList.Add(new Model("twisted"));
            ModelList.Add(new Model("nothing"));
            ModelList.Add(new Model("chuckle"));
            ModelList.Add(new Model("fishing"));
            ModelList.Add(new Model("forever"));
            ModelList.Add(new Model("grandpa"));
            ModelList.Add(new Model("grinned"));
            ModelList.Add(new Model("grumble"));
            ModelList.Add(new Model("library"));
            ModelList.Add(new Model("perfect"));
            ModelList.Add(new Model("Sanchez"));
            ModelList.Add(new Model("snuggle"));
            ModelList.Add(new Model("sparkle"));
            ModelList.Add(new Model("sunburn"));
            ModelList.Add(new Model("swimmer"));
            ModelList.Add(new Model("tadpole"));
            ModelList.Add(new Model("twinkle"));
            ModelList.Add(new Model("whisper"));
            ModelList.Add(new Model("whistle"));
            ModelList.Add(new Model("writing"));
            ModelList.Add(new Model("applaud"));
            ModelList.Add(new Model("awesome"));
            ModelList.Add(new Model("bedtime"));
            ModelList.Add(new Model("beehive"));
            ModelList.Add(new Model("begging"));
            ModelList.Add(new Model("broiler"));
            ModelList.Add(new Model("careful"));
            ModelList.Add(new Model("collect"));
            ModelList.Add(new Model("crinkle"));
            ModelList.Add(new Model("cupcake"));
            ModelList.Add(new Model("delight"));
            ModelList.Add(new Model("explore"));
            ModelList.Add(new Model("giraffe"));
            ModelList.Add(new Model("gumball"));
            ModelList.Add(new Model("harmful"));
            ModelList.Add(new Model("helpful"));
            ModelList.Add(new Model("herself"));
            ModelList.Add(new Model("highway"));
            ModelList.Add(new Model("hopeful"));
            ModelList.Add(new Model("instead"));
            ModelList.Add(new Model("kneecap"));
            ModelList.Add(new Model("leather"));
            ModelList.Add(new Model("necktie"));
            ModelList.Add(new Model("nowhere"));
            ModelList.Add(new Model("oatmeal"));
            ModelList.Add(new Model("outside"));
            ModelList.Add(new Model("painful"));
            ModelList.Add(new Model("pancake"));
            ModelList.Add(new Model("pennies"));
            ModelList.Add(new Model("popcorn"));
            ModelList.Add(new Model("pretzel"));
            ModelList.Add(new Model("rainbow"));
            ModelList.Add(new Model("recycle"));
            ModelList.Add(new Model("restful"));
            ModelList.Add(new Model("sandbox"));
            ModelList.Add(new Model("scratch"));
            ModelList.Add(new Model("shaking"));
            ModelList.Add(new Model("silence"));
            ModelList.Add(new Model("smiling"));
            ModelList.Add(new Model("snowman"));
            ModelList.Add(new Model("someone"));
            ModelList.Add(new Model("splotch"));
            ModelList.Add(new Model("startle"));
            ModelList.Add(new Model("stiffer"));
            ModelList.Add(new Model("strange"));
            ModelList.Add(new Model("stretch"));
            ModelList.Add(new Model("sunrise"));
            ModelList.Add(new Model("teacher"));
            ModelList.Add(new Model("thimble"));
            ModelList.Add(new Model("thinner"));
            ModelList.Add(new Model("topcoat"));
            ModelList.Add(new Model("traffic"));
            ModelList.Add(new Model("treetop"));
            ModelList.Add(new Model("trouble"));
            ModelList.Add(new Model("trumpet"));
            ModelList.Add(new Model("Tuesday"));
            ModelList.Add(new Model("tugboat"));
            ModelList.Add(new Model("visitor"));
            ModelList.Add(new Model("weather"));
            ModelList.Add(new Model("weekend"));
            ModelList.Add(new Model("wettest"));
            ModelList.Add(new Model("wriggle"));
            ModelList.Add(new Model("wrinkle"));
            ModelList.Add(new Model("written"));
            ModelList.Add(new Model("brother"));
            ModelList.Add(new Model("present"));
            ModelList.Add(new Model("program"));
            ModelList.Add(new Model("twitter"));
            ModelList.Add(new Model("against"));
            ModelList.Add(new Model("general"));
            ModelList.Add(new Model("however"));
            ModelList.Add(new Model("airport"));
            ModelList.Add(new Model("anybody"));
            ModelList.Add(new Model("balloon"));
            ModelList.Add(new Model("bedroom"));
            ModelList.Add(new Model("bicycle"));
            ModelList.Add(new Model("brownie"));
            ModelList.Add(new Model("cartoon"));
            ModelList.Add(new Model("ceiling"));
            ModelList.Add(new Model("channel"));
            ModelList.Add(new Model("chicken"));
            ModelList.Add(new Model("garbage"));
            ModelList.Add(new Model("promise"));
            ModelList.Add(new Model("squeeze"));
            ModelList.Add(new Model("address"));
            ModelList.Add(new Model("blanket"));
            ModelList.Add(new Model("earache"));
            ModelList.Add(new Model("excited"));
            ModelList.Add(new Model("grandma"));
            ModelList.Add(new Model("grocery"));
            ModelList.Add(new Model("indoors"));
            ModelList.Add(new Model("January"));
            ModelList.Add(new Model("kitchen"));
            ModelList.Add(new Model("lullaby"));
            ModelList.Add(new Model("monster"));
            ModelList.Add(new Model("morning"));
            ModelList.Add(new Model("naughty"));
            ModelList.Add(new Model("October"));
            ModelList.Add(new Model("pajamas"));
            ModelList.Add(new Model("pretend"));
            ModelList.Add(new Model("quarter"));
            ModelList.Add(new Model("shampoo"));
            ModelList.Add(new Model("stomach"));
            ModelList.Add(new Model("tonight"));
            ModelList.Add(new Model("unhappy"));
            ModelList.Add(new Model("pumpkin"));
            ModelList.Add(new Model("printed"));
            ModelList.Add(new Model("planned"));
            ModelList.Add(new Model("spilled"));
            ModelList.Add(new Model("smelled"));
            ModelList.Add(new Model("grilled"));
            ModelList.Add(new Model("slammed"));
            ModelList.Add(new Model("spelled"));

            Random r = new Random(Guid.NewGuid().GetHashCode());
            foreach (var m in ModelList)
            {
                m.Date = DateTime.Now.AddDays((int)r.Next(0, 100));
                m.Value = r.NextDouble()*100.0;
                m.Match = m.Text.Substring(0, 3);
            }

            #endregion

            FilterControl = new FilterControlViewModel(CollectionViewSource.GetDefaultView(ModelList));
        }

        private void SortByTextClick(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            ListCollectionView view = CollectionViewSource.GetDefaultView(ModelList) as ListCollectionView;
            if (view == null || tb == null)
                return;

            if (tb.IsChecked.HasValue && tb.IsChecked.Value)
                view.CustomSort = new TextComparer();
            else
                view.CustomSort = null;
        }

        private void SortByDateClick(object sender, RoutedEventArgs e)
        {
            ToggleButton tb = sender as ToggleButton;
            ListCollectionView view = CollectionViewSource.GetDefaultView(ModelList) as ListCollectionView;
            if (view == null || tb == null)
                return;

            if (tb.IsChecked.HasValue && tb.IsChecked.Value)
                view.CustomSort = new DateComparer(ListSortDirection.Descending);
            else
                view.CustomSort = null;
        }
    }
}
