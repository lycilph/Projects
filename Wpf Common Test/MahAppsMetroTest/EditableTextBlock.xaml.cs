using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MahAppsMetroTest
{
    /// <summary>
    /// Interaction logic for EditableTextBlock.xaml
    /// </summary>
    public partial class EditableTextBlock : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(EditableTextBlock), new UIPropertyMetadata(string.Empty));

        public EditableTextBlock()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void text_block_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                SetEditable(true);
            }
        }

        private void text_box_LostFocus(object sender, RoutedEventArgs e)
        {
            SetEditable(false);
        }

        private void text_box_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetEditable(false);
                e.Handled = true;
            }
        }

        private void SetEditable(bool is_editable)
        {
            if (is_editable)
            {
                text_block.Visibility = Visibility.Hidden;
                text_box.Visibility = Visibility.Visible;

                Keyboard.Focus(text_box);
                text_box.CaretIndex = text_box.Text.Length;
            }
            else
            {
                text_block.Visibility = Visibility.Visible;
                text_box.Visibility = Visibility.Hidden;

                Keyboard.ClearFocus();
            }
        }
    }
}
