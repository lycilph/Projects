using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LunchViewer.Resources
{
    public static class NumericTextBoxBehavior
    {
        public static bool GetIsNumericOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericOnlyProperty);
        }
        public static void SetIsNumericOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericOnlyProperty, value);
        }
        public static readonly DependencyProperty IsNumericOnlyProperty = DependencyProperty.RegisterAttached(
            "IsNumericOnly",
            typeof(bool),
            typeof(NumericTextBoxBehavior),
            new PropertyMetadata(false, OnIsNumericOnlyChanged));

        private static void OnIsNumericOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            bool isNumericOnly = (bool)e.NewValue;

            TextBox textBox = (TextBox)d;

            if (isNumericOnly)
            {
                textBox.PreviewTextInput += BlockNonDigitCharacters;
                textBox.PreviewKeyDown += ReviewKeyDown;

                DataObject.AddPastingHandler(textBox, OnCancelCommand);
            }
            else
            {
                textBox.PreviewTextInput -= BlockNonDigitCharacters;
                textBox.PreviewKeyDown -= ReviewKeyDown;

                DataObject.RemovePastingHandler(textBox, OnCancelCommand);
            }
        }

        private static void BlockNonDigitCharacters(object sender, TextCompositionEventArgs e)
        {
            // Skip character if it is not a digit
            foreach (char ch in e.Text)
            {
                if (!Char.IsDigit(ch))
                    e.Handled = true;
            }

            // Check if text is longer than 5 characters
            var textbox = sender as TextBox;
            if (textbox != null && textbox.Text.Length >= 5)
                e.Handled = true;
        }

        private static void ReviewKeyDown(object sender, KeyEventArgs e)
        {
            // Disallow the space key, which doesn't raise a PreviewTextInput event.
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private static void OnCancelCommand(object sender, DataObjectEventArgs e)
        {
            e.CancelCommand();
        }
    }
}
