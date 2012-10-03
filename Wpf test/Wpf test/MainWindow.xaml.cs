using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using DragAndDrop;
using NLog;
using System.Windows.Input;
using System;

namespace Wpf_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDropTarget
    {
        private static Logger log = LogManager.GetCurrentClassLogger();
        private DefaultDropHandler default_drop_handler = new DefaultDropHandler();

        public ObservableCollection<Account> Accounts
        {
            get { return (ObservableCollection<Account>)GetValue(AccountsProperty); }
            set { SetValue(AccountsProperty, value); }
        }
        public static readonly DependencyProperty AccountsProperty =
            DependencyProperty.Register("Accounts", typeof(ObservableCollection<Account>), typeof(MainWindow), new UIPropertyMetadata(new ObservableCollection<Account>()));

        public ObservableCollection<string> WordList
        {
            get { return (ObservableCollection<string>)GetValue(WordListProperty); }
            set { SetValue(WordListProperty, value); }
        }
        public static readonly DependencyProperty WordListProperty =
            DependencyProperty.Register("WordList", typeof(ObservableCollection<string>), typeof(MainWindow), new UIPropertyMetadata(new ObservableCollection<string>()));

        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set { SetValue(FilterTextProperty, value); }
        }
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(MainWindow), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(OnFilterTextChanged)));

        private AggregateFilter aggregate_filter;

        public MainWindow()
        {
            log.Debug("MainWindow constructor");

            Accounts.Add(new Account("Account a"));
            Accounts.Add(new Account("Account b"));
            Accounts.Add(new Account("Account c"));
            Accounts[0].Posts.Add(new Post("Post a1"));
            Accounts[0].Posts.Add(new Post("Post a2"));
            Accounts[0].Posts.Add(new Post("Post a3"));
            Accounts[1].Posts.Add(new Post("Post b1"));
            Accounts[1].Posts.Add(new Post("Post b2"));
            Accounts[2].Posts.Add(new Post("Post c1"));

            #region Word list

            WordList.Add("through");
            WordList.Add("another");
            WordList.Add("because");
            WordList.Add("picture");
            WordList.Add("America");
            WordList.Add("between");
            WordList.Add("country");
            WordList.Add("thought");
            WordList.Add("example");
            WordList.Add("without");
            WordList.Add("banging");
            WordList.Add("bathtub");
            WordList.Add("blasted");
            WordList.Add("blended");
            WordList.Add("bobsled");
            WordList.Add("camping");
            WordList.Add("contest");
            WordList.Add("dentist");
            WordList.Add("disrupt");
            WordList.Add("himself");
            WordList.Add("jumping");
            WordList.Add("lending");
            WordList.Add("pinball");
            WordList.Add("planted");
            WordList.Add("plastic");
            WordList.Add("problem");
            WordList.Add("ringing");
            WordList.Add("shifted");
            WordList.Add("sinking");
            WordList.Add("sunfish");
            WordList.Add("trusted");
            WordList.Add("twisted");
            WordList.Add("nothing");
            WordList.Add("chuckle");
            WordList.Add("fishing");
            WordList.Add("forever");
            WordList.Add("grandpa");
            WordList.Add("grinned");
            WordList.Add("grumble");
            WordList.Add("library");
            WordList.Add("perfect");
            WordList.Add("Sanchez");
            WordList.Add("snuggle");
            WordList.Add("sparkle");
            WordList.Add("sunburn");
            WordList.Add("swimmer");
            WordList.Add("tadpole");
            WordList.Add("twinkle");
            WordList.Add("whisper");
            WordList.Add("whistle");
            WordList.Add("writing");
            WordList.Add("applaud");
            WordList.Add("awesome");
            WordList.Add("bedtime");
            WordList.Add("beehive");
            WordList.Add("begging");
            WordList.Add("broiler");
            WordList.Add("careful");
            WordList.Add("collect");
            WordList.Add("crinkle");
            WordList.Add("cupcake");
            WordList.Add("delight");
            WordList.Add("explore");
            WordList.Add("giraffe");
            WordList.Add("gumball");
            WordList.Add("harmful");
            WordList.Add("helpful");
            WordList.Add("herself");
            WordList.Add("highway");
            WordList.Add("hopeful");
            WordList.Add("instead");
            WordList.Add("kneecap");
            WordList.Add("leather");
            WordList.Add("necktie");
            WordList.Add("nowhere");
            WordList.Add("oatmeal");
            WordList.Add("outside");
            WordList.Add("painful");
            WordList.Add("pancake");
            WordList.Add("pennies");
            WordList.Add("popcorn");
            WordList.Add("pretzel");
            WordList.Add("rainbow");
            WordList.Add("recycle");
            WordList.Add("restful");
            WordList.Add("sandbox");
            WordList.Add("scratch");
            WordList.Add("shaking");
            WordList.Add("silence");
            WordList.Add("smiling");
            WordList.Add("snowman");
            WordList.Add("someone");
            WordList.Add("splotch");
            WordList.Add("startle");
            WordList.Add("stiffer");
            WordList.Add("strange");
            WordList.Add("stretch");
            WordList.Add("sunrise");
            WordList.Add("teacher");
            WordList.Add("thimble");
            WordList.Add("thinner");
            WordList.Add("topcoat");
            WordList.Add("traffic");
            WordList.Add("treetop");
            WordList.Add("trouble");
            WordList.Add("trumpet");
            WordList.Add("Tuesday");
            WordList.Add("tugboat");
            WordList.Add("visitor");
            WordList.Add("weather");
            WordList.Add("weekend");
            WordList.Add("wettest");
            WordList.Add("wriggle");
            WordList.Add("wrinkle");
            WordList.Add("written");
            WordList.Add("brother");
            WordList.Add("present");
            WordList.Add("program");
            WordList.Add("twitter");
            WordList.Add("against");
            WordList.Add("general");
            WordList.Add("however");
            WordList.Add("airport");
            WordList.Add("anybody");
            WordList.Add("balloon");
            WordList.Add("bedroom");
            WordList.Add("bicycle");
            WordList.Add("brownie");
            WordList.Add("cartoon");
            WordList.Add("ceiling");
            WordList.Add("channel");
            WordList.Add("chicken");
            WordList.Add("garbage");
            WordList.Add("promise");
            WordList.Add("squeeze");
            WordList.Add("address");
            WordList.Add("blanket");
            WordList.Add("earache");
            WordList.Add("excited");
            WordList.Add("grandma");
            WordList.Add("grocery");
            WordList.Add("indoors");
            WordList.Add("January");
            WordList.Add("kitchen");
            WordList.Add("lullaby");
            WordList.Add("monster");
            WordList.Add("morning");
            WordList.Add("naughty");
            WordList.Add("October");
            WordList.Add("pajamas");
            WordList.Add("pretend");
            WordList.Add("quarter");
            WordList.Add("shampoo");
            WordList.Add("stomach");
            WordList.Add("tonight");
            WordList.Add("unhappy");
            WordList.Add("pumpkin");
            WordList.Add("printed");
            WordList.Add("planned");
            WordList.Add("spilled");
            WordList.Add("smelled");
            WordList.Add("grilled");
            WordList.Add("slammed");
            WordList.Add("spelled");

            #endregion
            aggregate_filter = new AggregateFilter(CollectionViewSource.GetDefaultView(WordList));
            aggregate_filter.Add(FilterOnText);

            InitializeComponent();
            DataContext = this;
        }

        private void AddCategoryClick(object sender, RoutedEventArgs e)
        {
            PromptDialog prompt_dialog = new PromptDialog();
            prompt_dialog.Owner = this;
            prompt_dialog.Title = "Account";
            prompt_dialog.Message = "Enter name for new account";

            bool? result = prompt_dialog.ShowDialog();
            if (result.HasValue && result.Value && !string.IsNullOrEmpty(prompt_dialog.Output))
            {
                Accounts.Add(new Account(prompt_dialog.Output));

                var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
                accounts_view.MoveCurrentToLast();
            }
        }

        private void RemoveCategoryClick(object sender, RoutedEventArgs e)
        {
            // Get selected account
            var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
            Account current_account = accounts_view.CurrentItem as Account;
            if (current_account == null)
                return;

            // Remove account from list
            Accounts.Remove(current_account);
        }

        private void EditCategoryClick(object sender, RoutedEventArgs e)
        {
            // Get selected account
            var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
            Account current_account = accounts_view.CurrentItem as Account;
            if (current_account == null)
                return;

            PromptDialog prompt_dialog = new PromptDialog();
            prompt_dialog.Owner = this;
            prompt_dialog.Title = "Account";
            prompt_dialog.Message = "Enter name for account";
            prompt_dialog.Output = current_account.Name;

            bool? result = prompt_dialog.ShowDialog();
            if (result.HasValue && result.Value && !string.IsNullOrEmpty(prompt_dialog.Output))
                current_account.Name = prompt_dialog.Output;
        }

        private void AddPostClick(object sender, RoutedEventArgs e)
        {
            // Get selected account
            var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
            Account current_account = accounts_view.CurrentItem as Account;
            if (current_account == null)
                return;

            PromptDialog prompt_dialog = new PromptDialog();
            prompt_dialog.Owner = this;
            prompt_dialog.Title = "Post";
            prompt_dialog.Message = "Enter text for new post";

            bool? result = prompt_dialog.ShowDialog();
            if (result.HasValue && result.Value && !string.IsNullOrEmpty(prompt_dialog.Output))
            {
                current_account.Posts.Add(new Post(prompt_dialog.Output));

                var posts_view = CollectionViewSource.GetDefaultView(current_account.Posts);
                posts_view.MoveCurrentToLast();
            }
        }

        private void RemovePostClick(object sender, RoutedEventArgs e)
        {
            // Get selected account
            var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
            Account current_account = accounts_view.CurrentItem as Account;
            if (current_account == null)
                return;

            // Get selected post
            var posts_view = CollectionViewSource.GetDefaultView(current_account.Posts);
            Post current_post = posts_view.CurrentItem as Post;
            if (current_post == null)
                return;

            // Remove post from list
            current_account.Posts.Remove(current_post);
        }

        private void EditPostClick(object sender, RoutedEventArgs e)
        {
            // Get selected account
            var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
            Account current_account = accounts_view.CurrentItem as Account;
            if (current_account == null)
                return;

            // Get selected post
            var posts_view = CollectionViewSource.GetDefaultView(current_account.Posts);
            Post current_post = posts_view.CurrentItem as Post;
            if (current_post == null)
                return;

            PromptDialog prompt_dialog = new PromptDialog();
            prompt_dialog.Owner = this;
            prompt_dialog.Title = "Post";
            prompt_dialog.Message = "Enter text for post";
            prompt_dialog.Output = current_post.Text;

            bool? result = prompt_dialog.ShowDialog();
            if (result.HasValue && result.Value && !string.IsNullOrEmpty(prompt_dialog.Output))
                current_post.Text = prompt_dialog.Output;
        }

        void IDropTarget.DragOver(DropInfo drop_info)
        {
            default_drop_handler.DragOver(drop_info);
        }

        void IDropTarget.Drop(DropInfo drop_info)
        {
            if (drop_info.TargetItem == null)
                log.Trace("Dropping on " + drop_info.TargetCollection.ToString());
            else
            {
                if (drop_info.TargetItem is Account)
                    log.Trace("Dropping on " + (drop_info.TargetItem as Account).Name);
                else
                    log.Trace("Dropping on " + (drop_info.TargetItem as Post).Text);
            }

            default_drop_handler.Drop(drop_info);

            if (drop_info.Data is Account)
            {
                var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
                accounts_view.MoveCurrentTo(drop_info.Data);
            }
            else if (drop_info.Data is Post)
            {
                var accounts_view = CollectionViewSource.GetDefaultView(Accounts);
                Account current_account = accounts_view.CurrentItem as Account;
                if (current_account == null)
                    return;
                // Get selected post
                var posts_view = CollectionViewSource.GetDefaultView(current_account.Posts);
                posts_view.MoveCurrentTo(drop_info.Data);
            }
        }

        private void DataTemplatesClick(object sender, RoutedEventArgs e)
        {
            DataTemplatesWindow dtw = new DataTemplatesWindow();
            dtw.Owner = this;
            dtw.ShowDialog();
        }

        private static void OnFilterTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as MainWindow;
            if (window == null)
                return;

            var word_list_view = CollectionViewSource.GetDefaultView(window.WordList);
            word_list_view.Refresh();
        }

        private bool FilterOnText(object o)
        {
            if (string.IsNullOrEmpty(FilterText))
                return true;

            string str = o as string;
            if (string.IsNullOrEmpty(str))
                return false;

            int index = str.IndexOf(FilterText, 0, StringComparison.InvariantCultureIgnoreCase);
            return index > -1;
        }

        private bool FilterOnStartsWithA(object o)
        {
            string str = o as string;
            if (string.IsNullOrEmpty(str))
                return false;

            return str.StartsWith("a", StringComparison.InvariantCultureIgnoreCase);
        }

        private void FilterChecked(object sender, RoutedEventArgs e)
        {
            aggregate_filter.Add(FilterOnStartsWithA);
        }

        private void FilterUnchecked(object sender, RoutedEventArgs e)
        {
            aggregate_filter.Remove(FilterOnStartsWithA);
        }
    }
}
