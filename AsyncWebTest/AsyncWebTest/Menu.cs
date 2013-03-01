using System.Collections.ObjectModel;

namespace AsyncWebTest
{
    public class Menu
    {
        public string Day { get; set; }
        public string Text { get; set; }
    }

    public class MenuCollection : ObservableCollection<Menu> { }
}
