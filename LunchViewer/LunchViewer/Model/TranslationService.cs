using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;
using NLog;

namespace LunchViewer.Model
{
    [Export(typeof(ITranslationService))]
    public class TranslationService : ITranslationService, IPartImportsSatisfiedNotification
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly BingTranslatorService.LanguageServiceClient translation_service_client;
        private readonly AuthenticationProvider authentication_provider;

        [Import]
        private ISettings Settings { get; set; }
        [Import]
        private IMenuRepository MenuRepository { get; set; }
        [Import]
        private ILocalizationService LocalizationService { get; set; }
        [Import]
        private IMenuUpdateService MenuUpdateService { get; set; }

        public TranslationService()
        {
            translation_service_client = new BingTranslatorService.LanguageServiceClient();
            authentication_provider = new AuthenticationProvider("LunchViewerID", "9J9NlGfKUUU4j4UFNmbDUHz/oV2bQXBATevKDix8f/8=");
        }

        public void OnImportsSatisfied()
        {
            Settings.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Culture" || args.PropertyName == "TranslateMenus")
                        Translate();
                };
            MenuUpdateService.MenusUpdated += (sender, args) => Translate();
        }

        private async void Translate()
        {
            var culture_info = System.Globalization.CultureInfo.CreateSpecificCulture(Settings.Culture);

            logger.Debug("Translating menus to: " + culture_info.TwoLetterISOLanguageName);

            foreach (var weekly_menu in MenuRepository.WeeklyMenus)
            {
                var menu_header = Settings.TranslateMenus ? LocalizationService.Localize("WeeklyMenuHeader")
                                                          : LocalizationService.Localize("WeeklyMenuHeader", Settings.OriginalCulture);
                weekly_menu.SetLanguage(menu_header);

                foreach (var daily_menu in weekly_menu.Menus)
                {
                    if (Settings.TranslateMenus)
                    {
                        if (!daily_menu.HasLanguage(culture_info.Name))
                        {
                            logger.Debug(string.Format("Adding new translation for {0}, week {1} - {2}", daily_menu.Date.ToShortDateString(), weekly_menu.Week, weekly_menu.Year));

                            var original_text = daily_menu.GetTranslation(Settings.OriginalCulture);
                            var translated_text = await GetTranslationAsync(culture_info.TwoLetterISOLanguageName, original_text);
                            daily_menu.AddNewTranslation(culture_info.Name, translated_text);
                        }
                        daily_menu.SetLanguage(culture_info.Name);
                    }
                    else
                    {
                        daily_menu.SetLanguage(Settings.OriginalCulture);
                    }
                }
            }
        }

        private Task<string> GetTranslationAsync(string culture, string text)
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
    }
}
