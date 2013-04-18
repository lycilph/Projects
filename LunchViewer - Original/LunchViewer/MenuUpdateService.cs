using System.ComponentModel.Composition;

namespace LunchViewer
{
    [Export(typeof(IMenuUpdateService))]
    public class MenuUpdateService : IMenuUpdateService
    {
        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }


    //    private const string main_page_url = @"http://www.kitenet.com";

    //    private readonly INotificationService notification_service;
    //    private readonly IMenuRepository menu_repository;
    //    private readonly IMainWindow menu_viewer;

    //    private readonly DispatcherTimer timer;

    //    public MenuUpdater(INotificationService notification_service, IMenuRepository menu_repository, IMainWindow menu_viewer)
    //    {
    //        this.notification_service = notification_service;
    //        this.menu_repository = menu_repository;
    //        this.menu_viewer = menu_viewer;

    //        timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
    //    }

    //    public void StartCheckForUpdate()
    //    {
    //        timer.Tick += CheckForUpdates;
    //        timer.Start();
    //    }

    //    public void StopCheckForUpdate()
    //    {
    //        timer.Stop();
    //        timer.Tick -= CheckForUpdates;
    //    }

    //    private async void CheckForUpdates(object sender, EventArgs eventArgs)
    //    {
    //        notification_service.ShowNotification("Checking for new menus...");

    //        try
    //        {
    //            var page = await DownloadPage(main_page_url);

    //            var menus = await ParseWeekMenus(page);
    //            foreach (var menu in menus.Where(menu => !menu_repository.HasMenuforWeek(menu.Week)))
    //            {
    //                var week_page = await DownloadPage(main_page_url + menu.Link);

    //                var day_menus = await ParseMenu(week_page);
    //                foreach (var day_menu in day_menus)
    //                    menu.Add(day_menu);

    //                menu_repository.Add(menu);

    //                // Make a copy of the menu reference for the notification action
    //                var temp_menu = menu;
    //                notification_service.ShowNotification("Found new week menu - " + menu.Header, () => menu_viewer.Show(temp_menu));
    //            }
    //        }
    //        catch (WebException we)
    //        {
    //            timer.Stop();
    //            MessageBox.Show(string.Format("Couldn't check for menus... ({0})", we.Message));
    //        }
    //    }

    //    private static Task<HtmlDocument> DownloadPage(string url)
    //    {
    //        return Task.Run(() =>
    //        {
    //            HtmlDocument doc = new HtmlDocument();

    //            using (var web_client = new WebClient())
    //            {
    //                web_client.UseDefaultCredentials = true;
    //                var page = web_client.DownloadString(url);

    //                doc.LoadHtml(page);
    //            }

    //            return doc;
    //        });
    //    }

    //    private static Task<List<WeekMenu>> ParseWeekMenus(HtmlDocument doc)
    //    {
    //        return Task.Run(() => (from link in doc.DocumentNode.SelectNodes("//a[@href]")
    //                               let attribute = link.Attributes["href"]
    //                               where link.InnerText.Contains("Menu uge")
    //                               select new WeekMenu(link.InnerText, attribute.Value)).ToList());
    //    }

    //    private static Task<List<DayMenu>> ParseMenu(HtmlDocument doc)
    //    {
    //        return Task.Run(() =>
    //        {
    //            var menus = new List<DayMenu>();

    //            var nodes = doc.DocumentNode.SelectNodes("//p").Select(n => n.InnerText);
    //            StringBuilder sb = new StringBuilder();
    //            var current_menu = new DayMenu();
    //            foreach (var node in nodes)
    //            {
    //                if (node == "&nbsp;")
    //                {
    //                    current_menu.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    //                    sb.Clear();

    //                    menus.Add(current_menu);
    //                    current_menu = new DayMenu();
    //                }
    //                else
    //                {
    //                    if (string.IsNullOrEmpty(current_menu.Day))
    //                        current_menu.Day = node;
    //                    else
    //                        sb.AppendLine(node);
    //                }
    //            }
    //            if (sb.Length > 0)
    //            {
    //                current_menu.Text = sb.ToString().TrimEnd(Environment.NewLine.ToCharArray());
    //                menus.Add(current_menu);
    //            }

    //            return menus;
    //        });
    //    }
    }
}
