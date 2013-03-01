using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer
{
    public partial class MainWindow
    {
        private bool shutdown_requested;
        private readonly NotificationWindow notification_window = new NotificationWindow();
        private readonly MenuController menu_controller;

        public MenuViewModel MenuViewModel { get; private set; }
        public ICommand ShowCommand { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            menu_controller = new MenuController(notification_window.AddNotification);
            menu_controller.StartCheckForUpdate();

            MenuViewModel = new MenuViewModel(menu_controller.WeekMenus);
            ShowCommand = new ActionCommand(ShowWindow);

            notification_window.Show();
        }

        private void ShowClick(object sender, RoutedEventArgs e)
        {
            ShowWindow();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            shutdown_requested = true;
            Close();
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;

            if (Visibility == Visibility.Collapsed)
                Show();
            Activate();
        }

        private void UpdateWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    notify_icon.Visibility = Visibility.Visible;
                    break;
                case WindowState.Normal:
                    ShowInTaskbar = true;
                    notify_icon.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            UpdateWindowState();
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            if (shutdown_requested) return;

            WindowState = WindowState.Minimized;
            e.Cancel = true;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            notification_window.Close();

            menu_controller.StopcheckForUpdate();
        }
    }
}
