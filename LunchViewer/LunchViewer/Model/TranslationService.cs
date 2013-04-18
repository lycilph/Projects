using System.ComponentModel.Composition;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using LunchViewer.Infrastructure;
using LunchViewer.Interfaces;

namespace LunchViewer.Model
{
    [Export(typeof(ITranslationService))]
    public class TranslationService : ITranslationService, IPartImportsSatisfiedNotification
    {
        private readonly BingTranslatorService.LanguageServiceClient translation_service_client;
        private readonly AuthenticationProvider authentication_provider;

        [Import]
        public ISettings Settings { get; set; }
        [Import]
        public ILocalizationService LocalizationService { get; set; }
        [Import]
        public IMenuRepository MenuRepository { get; set; }

        public TranslationService()
        {
            translation_service_client = new BingTranslatorService.LanguageServiceClient();
            authentication_provider = new AuthenticationProvider("LunchViewerID", "5uv19IyTKaEGCvB10Lq3xGInfyf1M2+UTeDVCHLUWDA=");
        }

        public void OnImportsSatisfied()
        {
            Settings.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Culture" || args.PropertyName == "TranslateMenus")
                        Translate();
                };
        }

        private async void Translate()
        {
            var culture_info = System.Globalization.CultureInfo.CreateSpecificCulture(Settings.Culture);

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
