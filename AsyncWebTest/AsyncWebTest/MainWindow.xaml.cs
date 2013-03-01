using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using HtmlAgilityPack;

namespace AsyncWebTest
{
    public partial class MainWindow
    {
        const string main_page_url = @"http://www.kitenet.com";
        private readonly ViewModel view_model = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = view_model;
        }

        private static Task<HtmlDocument> DownloadPage(string url)
        {
            return Task.Run(() =>
                {
                    HtmlDocument doc = new HtmlDocument();

                    using (var web_client = new WebClient())
                    {
                        web_client.UseDefaultCredentials = true;
                        var page = web_client.DownloadString(url);
                        
                        doc.LoadHtml(page);
                    }

                    return doc;
                });
        }

        private static Task<List<WeekMenu>> ParseWeekMenus(HtmlDocument doc)
        {
            return Task.Run(() =>
                {
                    var menus = new List<WeekMenu>();
                    
                    foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        HtmlAttribute att = link.Attributes["href"];
                        if (link.InnerText.Contains("Menu uge"))
                            menus.Add(new WeekMenu {Text = link.InnerText, Link = att.Value});
                    }

                    return menus;
                });
        }

        private static Task<WeekMenu> FindCurrentWeekMenu(IEnumerable<WeekMenu> week_menus)
        {
            return Task.Run(() =>
                {
                    var current_week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    return week_menus.First(l => l.Text.Contains(current_week.ToString(CultureInfo.InvariantCulture)));
                });
        }

        private static Task<List<Menu>> ParseMenu(HtmlDocument doc)
        {
            return Task.Run(() =>
                {
                    var menus = new List<Menu>();

                    var nodes = doc.DocumentNode.SelectNodes("//p").Select(n => n.InnerText);
                    StringBuilder sb = new StringBuilder();
                    var current_menu = new Menu();
                    foreach (var node in nodes)
                    {
                        if (node == "&nbsp;")
                        {
                            current_menu.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                            sb.Clear();

                            menus.Add(current_menu);
                            current_menu = new Menu();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(current_menu.Day))
                                current_menu.Day = node;
                            else
                                sb.AppendLine(node);
                        }
                    }
                    if (sb.Length > 0)
                    {
                        current_menu.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                        menus.Add(current_menu);
                    }

                    return menus;
                });
        }

        private static Task<Menu> FindCurrentDayMenu(IEnumerable<Menu> menus)
        {
            return Task.Run(() =>
                {
                    var weekday = DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture);
                    return menus.FirstOrDefault(menu => menu.Day.ToLowerInvariant().Contains(weekday));
                });
        }

        private static string GetDateString()
        {
            var current_week = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            var date = DateTime.Now.ToString("dd. MMMM yyyy", CultureInfo.CurrentCulture);
            var day = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(DateTime.Now.ToString("dddd", CultureInfo.CurrentCulture));

            return string.Format("{0} {1} (uge {2})", day, date, current_week);
        }

        private async void MainWindow_OnContentRendered(object sender, EventArgs e)
        {
            view_model.IsBusy = true;
            view_model.Date = GetDateString();

            view_model.BusyText = "Downloading menu...";
            var main_page = await DownloadPage(main_page_url);

            view_model.BusyText = "Parsing menus...";
            var week_menus = await ParseWeekMenus(main_page);
            foreach (var week_menu in week_menus)
                view_model.WeekMenus.Add(week_menu);

            view_model.BusyText = "Finding current week menu...";
            var current_week_menu = await FindCurrentWeekMenu(week_menus);
            CollectionViewSource.GetDefaultView(view_model.WeekMenus).MoveCurrentTo(current_week_menu);

            view_model.IsBusy = false;
        }

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            view_model.IsBusy = true;

            // Get currently selected week menu
            var current_week_menu = CollectionViewSource.GetDefaultView(view_model.WeekMenus).CurrentItem as WeekMenu;
            if (current_week_menu == null)
                throw new Exception("No menu selected");

            view_model.BusyText = "Downloading menu...";
            var week_page = await DownloadPage(main_page_url + current_week_menu.Link);

            view_model.BusyText = "Parsing menu...";
            var menus = await ParseMenu(week_page);
            view_model.Menus.Clear();
            foreach (var day_menu in menus)
                view_model.Menus.Add(day_menu);

            view_model.BusyText = "Finding menu for today...";
            var menu = await FindCurrentDayMenu(menus);
            CollectionViewSource.GetDefaultView(view_model.Menus).MoveCurrentTo(menu);

            view_model.IsBusy = false;
        }
    }
}
