using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditableTextBlock
{
    public class EditableControl : ContentControl
    {
        public object EditContent
        {
            get { return (object)GetValue(EditContentProperty); }
            set { SetValue(EditContentProperty, value); }
        }
        public static readonly DependencyProperty EditContentProperty =
            DependencyProperty.Register("EditContent", typeof(object), typeof(EditableControl), new UIPropertyMetadata(null, OnEditContentChangedCallback));

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(EditableControl), new PropertyMetadata(false));
        static EditableControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditableControl), new FrameworkPropertyMetadata(typeof(EditableControl)));
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (!IsEditing)
                IsEditing = true;
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.F2)
                IsEditing = true;
            if (e.Key == Key.Enter || e.Key == Key.Escape)
                IsEditing = false;
        }

        public static void OnEditContentChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var c = d as EditableControl;
            if (c == null) return;

            if (e.NewValue != null)
            {
                var textbox = e.NewValue as TextBox;
                if (textbox == null) return;

                textbox.IsVisibleChanged += (s, arg) =>
                {
                    Keyboard.Focus(textbox);
                    textbox.CaretIndex = textbox.Text.Length;
                };
                textbox.LostFocus += (a, arg) => 
                {
                    c.IsEditing = false;
                };
            }
        }
    }
}
