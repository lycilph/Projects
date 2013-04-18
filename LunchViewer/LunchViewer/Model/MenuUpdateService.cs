using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using HtmlAgilityPack;
using LunchViewer.Interfaces;
using LunchViewer.Utils;

namespace LunchViewer.Model
{
    [Export(typeof(IMenuUpdateService))]
    public class MenuUpdateService : IMenuUpdateService, IPartImportsSatisfiedNotification
    {
        private const string main_page_url = @"http://www.kitenet.com";

        private readonly DispatcherTimer timer;

        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }
        [Import]
        public INotificationService NotificationService { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }
        [Import]
        public IMainWindow MainWindow { get; set; }

        public MenuUpdateService()
        {
            timer = new DispatcherTimer();
            timer.Tick += CheckForUpdates;
        }

        public void OnImportsSatisfied()
        {
            timer.Interval = TimeSpan.FromSeconds(Settings.UpdateInterval);

            Settings.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "UpdateInterval")
                        timer.Interval = TimeSpan.FromSeconds(Settings.UpdateInterval);
                };
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void CheckNow()
        {
            CheckForUpdates(null, EventArgs.Empty);
        }

        private async void CheckForUpdates(object sender, EventArgs args)
        {
            if (Settings.ShowNotificationOnUpdate)
            {
                var checking_menus_message = LocalizationService.Localize("CheckingMenusNotification");
                NotificationService.ShowNotification(checking_menus_message);
            }

            try
            {
                var page = await DownloadPage(main_page_url);

                var menus = await ParseWeekMenus(page);
                foreach (var menu in menus.Where(menu => !MenuRepository.HasMenuforWeek(menu.Week)))
                {
                    var week_page = await DownloadPage(menu.Link);
                    var start_of_week_date = DateUtils.FirstDateOfWeek(menu.Year, menu.Week);

                    var day_menus = await ParseMenu(week_page, start_of_week_date);
                    foreach (var day_menu in day_menus)
                        menu.Add(day_menu);

                    // Set the correct (localized) header
                    var menu_header = LocalizationService.Localize("WeeklyMenuHeader");
                    menu.SetLanguage(menu_header);

                    MenuRepository.Add(menu);

                    if (Settings.ShowNotificationOnUpdate)
                    {
                        // Make a copy of the menu reference for the notification action
                        var temp_menu = menu;
                        var new_menus_message = LocalizationService.Localize("NewMenusNotification") + menu.Header;
                        NotificationService.ShowNotification(new_menus_message, () => MainWindow.Open(temp_menu));
                    }
                }
            }
            catch (WebException we)
            {
                timer.Stop();
                MessageBox.Show(string.Format("Couldn't check for menus... ({0})", we.Message));
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

        private static Task<List<WeeklyMenu>> ParseWeekMenus(HtmlDocument doc)
        {
            return Task.Run(() =>
            {
                var links = doc.DocumentNode.SelectNodes("//a[@href]");
                var menu_links = links.Where(link => Regex.IsMatch(link.InnerText.ToLower(), "menu.*uge"));

                var menus = new List<WeeklyMenu>();
                foreach (var link in menu_links)
                {
                    var week = 0;
                    var match = Regex.Match(link.InnerText, @"[\d].");
                    if (match.Success)
                        week = Convert.ToInt32(match.Value);

                    var year = DateTime.Today.Year;
                    
                    // If the week of this menu is less than current week, we assume that it is from next year
                    if (DateUtils.GetCurrentWeekNumber() > week)
                        year += 1;

                    var menu_link = main_page_url + link.Attributes["href"].Value;

                    menus.Add(new WeeklyMenu(year, week, menu_link));
                }
                return menus;
            });
        }

        private static Task<List<DailyMenu>> ParseMenu(HtmlDocument doc, DateTime start_of_week)
        {
            return Task.Run(() =>
            {
                // Output
                var menus = new List<DailyMenu>();

                // Find nodes to parse
                var nodes = doc.DocumentNode.SelectNodes("//p").Select(n => n.InnerText);

                StringBuilder sb = new StringBuilder();
                DateTime date = DateTime.Today;
                string day = string.Empty;
                string text;
                foreach (var node in nodes)
                {
                    if (node == "&nbsp;")
                    {
                        text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());

                        sb.Clear();
                        day = string.Empty;

                        if (!string.IsNullOrEmpty(text))
                        {
                            var current_menu = new DailyMenu("da-DK", date, text);
                            menus.Add(current_menu);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(day))
                        {
                            day = node;
                            date = DateUtils.DateFromDayOfWeek(start_of_week, day);
                        }
                        else
                            sb.AppendLine(node);
                    }
                }
                if (sb.Length > 0)
                {
                    text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());

                    var current_menu = new DailyMenu("da-DK", date, text);
                    menus.Add(current_menu);
                }

                return menus;
            });
        }
    }
}
