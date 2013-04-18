using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFLocalizeExtension.Extensions;

namespace WpfLocalizationTest
{
    public partial class MainWindow
    {
        private readonly TranslationService.LanguageServiceClient translation_service_client;
        private readonly AuthenticationProvider authentication_provider;

        public ObservableCollection<string> Cultures
        {
            get { return (ObservableCollection<string>)GetValue(CulturesProperty); }
            set { SetValue(CulturesProperty, value); }
        }
        public static readonly DependencyProperty CulturesProperty =
            DependencyProperty.Register("Cultures", typeof(ObservableCollection<string>), typeof(MainWindow), new PropertyMetadata(null));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public string TranslatedText
        {
            get { return (string)GetValue(TranslatedTextProperty); }
            set { SetValue(TranslatedTextProperty, value); }
        }
        public static readonly DependencyProperty TranslatedTextProperty =
            DependencyProperty.Register("TranslatedText", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path != null)
            {
                var resource_folders = Directory.EnumerateDirectories(path);
                var resource_names = resource_folders.Select(Path.GetFileName);
                Cultures = new ObservableCollection<string>(resource_names);
            }

            translation_service_client = new TranslationService.LanguageServiceClient();
            authentication_provider = new AuthenticationProvider("LunchViewerID", "5uv19IyTKaEGCvB10Lq3xGInfyf1M2+UTeDVCHLUWDA=");
        }

        private void UpdateTranslations()
        {
            var culture = WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture.TwoLetterISOLanguageName;

            // Microsoft translate service https://datamarket.azure.com/developer/applications
            // - Getting started http://msdn.microsoft.com/en-us/library/hh454949.aspx

            var text = "Kylling med peanutsauce, nudler og lyn stegte grøntsager.";
            if (culture != "da")
            {
                //await 

                //HttpRequestMessageProperty http_request_property = new HttpRequestMessageProperty { Method = "POST" };
                //http_request_property.Headers.Add("Authorization", "Bearer " + authentication_provider.GetAccessToken().access_token);

                //// Creates a block within which an OperationContext object is in scope.
                //// ReSharper disable UnusedVariable
                //using (var scope = new OperationContextScope(translation_service_client.InnerChannel))
                //{
                //    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = http_request_property;
                //    text = translation_service_client.Translate("", text, "da", culture, "text/plain", "");
                //}
                //// ReSharper restore UnusedVariable
            }

            TranslatedText = text;
        }

        private Task<string> GetTranslationAsync(string text, string culture)
        {
            return Task.Run(() =>
                {
                    HttpRequestMessageProperty http_request_property = new HttpRequestMessageProperty { Method = "POST" };
                    http_request_property.Headers.Add("Authorization", "Bearer " + authentication_provider.GetAccessToken().access_token);

                    // Creates a block within which an OperationContext object is in scope.
                    // ReSharper disable UnusedVariable
                    using (var scope = new OperationContextScope(translation_service_client.InnerChannel))
                    {
                        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = http_request_property;
                        return translation_service_client.Translate("", text, "da", culture, "text/plain", "");
                    }
                    // ReSharper restore UnusedVariable
                });
        }

        private void UpdateStrings()
        {
            string str;
            LocExtension loc = new LocExtension("WpfLocalizationTest:Strings:code_property");
            loc.ResolveLocalizedValue(out str);
            Text = str;
        }

        private void CultureChanged(object sender, SelectionChangedEventArgs e)
        {
            var culture = e.AddedItems[0] as string;
            if (culture == null) return;

            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = CultureInfo.CreateSpecificCulture(culture);
            UpdateStrings();
            UpdateTranslations();
        }
    }
}
