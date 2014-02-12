using System.Windows;
using Caliburn.Micro;
using CaliburnMicroTest.ViewModels;

namespace CaliburnMicroTest
{
    public static class WindowManagerExtensions
    {
        public static MessageBoxResult ShowMetroMessageBox(this IWindowManager window_manager, string message, string title, MessageBoxButton buttons)
        {
            MessageBoxResult result = MessageBoxResult.None;
            var shell_view_model = IoC.Get<IShell>();

            try
            {
                shell_view_model.ShowOverlay();

                var messagebox_view_model = new MetroMessageBoxViewModel(message, title, buttons);
                window_manager.ShowDialog(messagebox_view_model);

                result = messagebox_view_model.Result;
            }
            finally
            {
                shell_view_model.HideOverlay();
            }

            return result;
        }

        public static void ShowMetroMessageBox(this IWindowManager window_manager, string message)
        {
            window_manager.ShowMetroMessageBox(message, "System Message", MessageBoxButton.OK);
        }

        public static void ShowMetroMessageBox(this IWindowManager window_manager, string message, string title)
        {
            window_manager.ShowMetroMessageBox(message, title, MessageBoxButton.OK);
        }
    }
}