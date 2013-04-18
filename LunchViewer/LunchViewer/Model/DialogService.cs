using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using FirstFloor.ModernUI.Windows.Controls;
using LunchViewer.Interfaces;

namespace LunchViewer.Model
{
    [Export(typeof(IDialogService))]
    public class DialogService : IDialogService
    {
        [Import]
        public ILocalizationService LocalizationService { get; set; }

        public bool? ShowYesNoMessage(string message, string title)
        {
            var dlg = new ModernDialog
            {
                Title = title,
                Content = new BBCodeBlock { BBCode = message, Margin = new Thickness(0, 0, 0, 8) },
                MinHeight = 0,
                MinWidth = 0,
                MaxHeight = 480,
                MaxWidth = 640,
            };

            // Localize buttons
            dlg.YesButton.Content = LocalizationService.Localize("Yes");
            dlg.NoButton.Content = LocalizationService.Localize("No");

            dlg.Buttons = new List<Button> { dlg.YesButton, dlg.NoButton };
            return dlg.ShowDialog();
        }
    }
}
