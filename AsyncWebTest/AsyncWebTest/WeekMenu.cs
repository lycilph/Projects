using System.Collections.ObjectModel;

namespace AsyncWebTest
{
    public class WeekMenu
    {
        public string Text { get; set; }
        public string Link { get; set; }
    }

    public class WeekMenuCollection : ObservableCollection<WeekMenu> {}
}
