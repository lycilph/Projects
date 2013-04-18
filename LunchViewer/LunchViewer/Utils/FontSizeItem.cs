using FirstFloor.ModernUI.Presentation;
using LunchViewer.Infrastructure;

namespace LunchViewer.Utils
{
    public class FontSizeItem : ObservableObject
    {
        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                if (value == _DisplayName) return;
                _DisplayName = value;
                NotifyPropertyChanged();
            }
        }

        private FontSize _FontSize;
        public FontSize FontSize
        {
            get { return _FontSize; }
            set
            {
                if (value == _FontSize) return;
                _FontSize = value;
                NotifyPropertyChanged();
            }
        }
    }
}
