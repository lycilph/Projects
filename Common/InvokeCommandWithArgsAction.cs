using System.Windows;
using System.Windows.Input;

namespace Wpf_toolkit_test
{
    public sealed class InvokeCommandWithArgsAction : TriggerAction<DependencyObject>
    {
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(InvokeCommandWithArgsAction), new UIPropertyMetadata(null));

        protected override void Invoke(object parameter)
        {
            object event_args = parameter;

            if (this.AssociatedObject != null)
            {
                ICommand cmd = Command;
                if (cmd != null && cmd.CanExecute(event_args))
                    cmd.Execute(event_args);
            }
        }
    }
}
