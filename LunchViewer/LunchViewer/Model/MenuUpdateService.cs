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
using NLog;

namespace LunchViewer.Model
{
    [Export(typeof(IMenuUpdateService))]
    public class MenuUpdateService : IMenuUpdateService, IPartImportsSatisfiedNotification
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string main_page_url = @"http://www.kitenet.com";

        private readonly DispatcherTimer timer;
        private bool is_updating;

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

        public event EventHandler MenusUpdated;

        public MenuUpdateService()
        {
            timer = new DispatcherTimer();
            timer.Tick += CheckForUpdates;

            is_updating = false;
        }

        public void OnImportsSatisfied()
        {
            timer.Interval = TimeSpan.FromSeconds(Settings.UpdateInterval);

            Settings.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "UpdateInterval")
                        timer.Interval = TimeSpan.FromSeconds(Settings.UpdateInterval);
                    if (args.PropertyName == "AutomaticMenuUpdate")
                        UpdateStatus();
                };
        }

        public void Start()
        {
            logger.Debug("Enabling automatic menu updates");
            timer.Start();
        }

        public void Stop()
        {
            logger.Debug("Disabling automatic menu updates");
            timer.Stop();
        }

        private void UpdateStatus()
        {
            if (Settings.AutomaticMenuUpdate)
                Start();
            else
                Stop();
        }

        public void CheckNow()
        {
            CheckForUpdates(null, EventArgs.Empty);
        }

        private async void CheckForUpdates(object sender, EventArgs args)
        {
            // Make sure we don't start multiple updates simultaneously
            if (is_updating) return;
            is_updating = true;

            logger.Debug("Checking for new menus...");

            if (Settings.ShowNotificationOnUpdate)
            {
                var checking_menus_message = LocalizationService.Localize("CheckingMenusNotification");
                NotificationService.ShowNotification(checking_menus_message);
            }

            var notify_updates = false;
            try
            {
                logger.Debug("Downloading page: " + main_page_url);
                var page = await DownloadPage(main_page_url);

                var menus = await ParseWeekMenus(page);
                foreach (var menu in menus.Where(menu => !MenuRepository.HasMenuforWeek(menu.Week)))
                {
                    logger.Debug("Downloading page: " + menu.Link);
                    var week_page = await DownloadPage(menu.Link);
                    var start_of_week_date = DateUtils.FirstDateOfWeek(menu.Year, menu.Week);

                    var day_menus = await ParseMenu(week_page, start_of_week_date);
                    foreach (var day_menu in day_menus)
                        menu.Add(day_menu);

                    // Set the correct (localized) header
                    var menu_header = LocalizationService.Localize("WeeklyMenuHeader");
                    menu.SetLanguage(menu_header);

                    MenuRepository.Add(menu);
                    notify_updates = true;

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
                Settings.AutomaticMenuUpdate = false;

                // Create message
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Couldn't check for menus... ({0})", we.Message));
                sb.AppendLine();
                sb.Append("Disabling automatic updates (fix and reenable in settings)");
                // Show and log message
                logger.Debug(sb.ToString());
                MessageBox.Show(sb.ToString());
            }
            finally
            {
                if (notify_updates)
                    NotifyMenusUpdated();

                is_updating = false;
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
                var menus = new List<DailyMenu>();

                // Find nodes to parse
                var nodes = doc.DocumentNode.SelectNodes("//p").Select(n => n.InnerText);
                // Check for days
                var days = new List<string> {"mandag", "tirsdag", "onsdag", "torsdag", "fredag"};
                Func<string, bool> is_day = x => days.Any(d => x.ToLower().Contains(d));

                var day = string.Empty;
                string text;
                var date = DateTime.Today;
                var sb = new StringBuilder();

                foreach (var node in nodes)
                {
                    var node_text = HtmlEntity.DeEntitize(node);

                    //if (IsDay(node_text, days))
                    if (is_day(node_text))
                    {
                        if (!string.IsNullOrEmpty(day))
                        {
                            text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                            menus.Add(new DailyMenu("da-DK", date, text));
                            sb.Clear();
                        }

                        day = node_text;
                        date = DateUtils.DateFromDayOfWeek(start_of_week, day);
                    }
                    else
                    {

                        if (!string.IsNullOrWhiteSpace(node_text))
                            sb.AppendLine(node_text.Trim());
                    }
                }
                // Add the last menu
                text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
                menus.Add(new DailyMenu("da-DK", date, text));

                return menus;
            });
        }

        protected virtual void NotifyMenusUpdated()
        {
            var handler = MenusUpdated;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
