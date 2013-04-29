using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LunchViewer.Resources
{
    public static class TextBoxFocusBehavior
    {
        public static bool GetScrollToEndOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollToEndOnFocusProperty);
        }

        public static void SetScrollToEndOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToEndOnFocusProperty, value);
        }
        public static readonly DependencyProperty ScrollToEndOnFocusProperty = DependencyProperty.RegisterAttached(
            "ScrollToEndOnFocus",
            typeof(bool), 
            typeof(TextBoxFocusBehavior), 
            new PropertyMetadata(false, ScrollToEndOnFocusChangedCallback));

        private static void ScrollToEndOnFocusChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var textbox = obj as TextBox;
            if (textbox == null) return;

            textbox.GotKeyboardFocus += TextboxOnGotKeyboardFocus;
        }

        private static void TextboxOnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs keyboardFocusChangedEventArgs)
        {
            var textbox = sender as TextBox;
            if (textbox == null) return;

            textbox.CaretIndex = textbox.Text.Length;  
            textbox.ScrollToEnd();
        }
    }
}
