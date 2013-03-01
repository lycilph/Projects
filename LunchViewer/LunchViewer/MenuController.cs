using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using HtmlAgilityPack;

namespace LunchViewer
{
    public class MenuController
    {
        private const string main_page_url = @"http://www.kitenet.com";
        private readonly Action<string> notification_action;
        private readonly DispatcherTimer timer;

        public ObservableCollection<WeekMenu> WeekMenus { get; private set; }

        public MenuController(Action<string> notification_action)
        {
            this.notification_action = notification_action;
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            WeekMenus = new ObservableCollection<WeekMenu>();
        }

        public void StartCheckForUpdate()
        {
            timer.Tick += CheckForUpdates;
            timer.Start();
        }

        public void StopcheckForUpdate()
        {
            timer.Stop();
            timer.Tick -= CheckForUpdates;
        }

        private async void CheckForUpdates(object sender, EventArgs eventArgs)
        {
            notification_action("Checking for new menus...");
            var page = await DownloadPage(main_page_url);

            var menus = await ParseWeekMenus(page);
            foreach (var menu in menus)
                if (WeekMenus.All(e => e.Week != menu.Week))
                {
                    notification_action("Found new week menu - " + menu.Header);
                    var week_page = await DownloadPage(main_page_url + menu.Link);

                    var day_menus = await ParseMenu(week_page);
                    foreach (var day_menu in day_menus)
                        menu.Menus.Add(day_menu);

                    WeekMenus.Add(menu);
                }
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
            return Task.Run(() => (from link in doc.DocumentNode.SelectNodes("//a[@href]")
                                   let attribute = link.Attributes["href"]
                                   where link.InnerText.Contains("Menu uge")
                                   select new WeekMenu(link.InnerText, attribute.Value)).ToList());
        }

        private static Task<List<DayMenu>> ParseMenu(HtmlDocument doc)
        {
            return Task.Run(() =>
            {
                var menus = new List<DayMenu>();

                var nodes = doc.DocumentNode.SelectNodes("//p").Select(n => n.InnerText);
                StringBuilder sb = new StringBuilder();
                var current_menu = new DayMenu();
                foreach (var node in nodes)
                {
                    if (node == "&nbsp;")
                    {
                        current_menu.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                        sb.Clear();

                        menus.Add(current_menu);
                        current_menu = new DayMenu();
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
    }
}
