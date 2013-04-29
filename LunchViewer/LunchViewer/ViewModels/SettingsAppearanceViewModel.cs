using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using FirstFloor.ModernUI.Presentation;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using LunchViewer.Utils;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer.ViewModels
{
    [Export(typeof(SettingsAppearanceViewModel))]
    public class SettingsAppearanceViewModel : ObservableObject
    {
        [Import]
        private ILocalizationService LocalizationService { get; set; }

        // 9 accent colors from metro design principles
        private readonly Color[] _AccentColors = new[]{
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // blueish
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x33, 0x99, 0x33),   // green
            Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
            Color.FromRgb(0xf0, 0x96, 0x09),   // orange
            Color.FromRgb(0xff, 0x45, 0x00),   // orange red
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xff, 0x00, 0x97),   // magenta
            Color.FromRgb(0xa2, 0x00, 0xff),   // purple            
        };
        public Color[] AccentColors
        {
            get { return _AccentColors; }
        }

        private Color _SelectedAccentColor;
        public Color SelectedAccentColor
        {
            get { return _SelectedAccentColor; }
            set
            {
                if (_SelectedAccentColor == value) return;
                _SelectedAccentColor = value;
                NotifyPropertyChanged();

                // Find light accent color and apply BEFORE setting the normal AccentColor
                SetLightAccentColor(value);
                AppearanceManager.Current.AccentColor = value;
            }
        }

        private readonly LinkCollection _Themes = new LinkCollection();
        public LinkCollection Themes
        {
            get { return _Themes; }
        }

        private Link _SelectedTheme;
        public Link SelectedTheme
        {
            get { return _SelectedTheme; }
            set
            {
                if (_SelectedTheme == value) return;
                _SelectedTheme = value;
                NotifyPropertyChanged();

                AppearanceManager.Current.ThemeSource = value.Source;
            }
        }

        private readonly ObservableCollection<FontSizeItem> _FontSizes = new ObservableCollection<FontSizeItem>();
        public ObservableCollection<FontSizeItem> FontSizes
        {
            get { return _FontSizes; }
        }

        private FontSizeItem _SelectedFontSize;
        public FontSizeItem SelectedFontSize
        {
            get { return _SelectedFontSize; }
            set
            {
                if (_SelectedFontSize == value) return;
                _SelectedFontSize = value;
                NotifyPropertyChanged();

                AppearanceManager.Current.FontSize = value.FontSize;
            }
        }

        public ICommand LoadedCommand { get; private set; }

        public SettingsAppearanceViewModel()
        {
            // Add the default themes
            _Themes.Add(new Link { DisplayName = "Dark", Source = AppearanceManager.DarkThemeSource });
            _Themes.Add(new Link { DisplayName = "Light", Source = AppearanceManager.LightThemeSource });

            // Add Font sizes
            _FontSizes.Add(new FontSizeItem { DisplayName = "Small", FontSize = FontSize.Small });
            _FontSizes.Add(new FontSizeItem { DisplayName = "Large", FontSize = FontSize.Large });
            
            SyncData();

            LoadedCommand = new ActionCommand(UpdateLocalization);

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        private void UpdateLocalization()
        {
            // Localize themes
            foreach (var theme in _Themes)
            {
                var theme_source = Path.GetFileNameWithoutExtension(theme.Source.ToString()).Replace(".", string.Empty);
                var theme_localization_key = "Theme" + Path.GetFileNameWithoutExtension(theme_source);

                theme.DisplayName = LocalizationService.Localize(theme_localization_key);
            }
            // Localize font sizes
            foreach (var item in _FontSizes)
            {
                var font_size_localization_key = "FontSize" + item.FontSize.ToString();

                item.DisplayName = LocalizationService.Localize(font_size_localization_key);
            }
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor" || e.PropertyName == "FontSize")
                SyncData();
        }

        private void SyncData()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            SelectedTheme = _Themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));
            // and make sure font size is up-to-date
            SelectedFontSize = _FontSizes.FirstOrDefault(e => e.FontSize == AppearanceManager.Current.FontSize);
            // and make sure accent color is up-to-date
            SelectedAccentColor = AppearanceManager.Current.AccentColor;
        }

        private static void SetLightAccentColor(Color c)
        {
            var light_accent_color = ColorUtils.Lighten(c);

            Application.Current.Resources["AccentLightColor"] = light_accent_color;
            Application.Current.Resources["AccentLight"] = new SolidColorBrush(light_accent_color);
        }
    }
}
