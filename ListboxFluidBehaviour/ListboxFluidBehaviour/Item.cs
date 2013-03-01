using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ListboxFluidBehaviour.Annotations;
using Microsoft.Expression.Interactivity.Core;

namespace ListboxFluidBehaviour
{
    public class Item : INotifyPropertyChanged
    {
        private readonly DispatcherTimer removal_timer;
        private readonly Func<Item, bool> removal_action;
        private readonly TimeSpan fade_out_duration;

        private string _Text;
        public string Text
        {
            get { return _Text; }
            set
            {
                if (value == _Text) return;
                _Text = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IsRemoving;
        public bool IsRemoving
        {
            get { return _IsRemoving; }
            private set
            {
                if (value.Equals(_IsRemoving)) return;
                _IsRemoving = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand PauseCommand { get; set; }
        public ICommand ResumeCommand { get; set; }
        public ICommand ClickedCommand { get; set; }

        public Item(string text, Func<Item, bool> action, TimeSpan message_duration, TimeSpan fade_out_duration)
        {
            Text = text;
            IsRemoving = false;
            removal_action = action;
            this.fade_out_duration = fade_out_duration;

            PauseCommand = new ActionCommand(_ => removal_timer.Stop());
            ResumeCommand = new ActionCommand(_ => removal_timer.Start());
            ClickedCommand = new ActionCommand(Clicked);

            removal_timer = new DispatcherTimer {Interval = message_duration};
            removal_timer.Tick += MessageDurationTick;
            removal_timer.Start();
        }

        private static void Clicked(Object o)
        {
            var item = o as Item;
            if (item == null) return;
            
            MessageBox.Show(string.Format("Showing main window ({0})", item.Text));
        }

        private void FadeOutTick(object sender, EventArgs e)
        {
            removal_timer.Stop();
            removal_timer.Tick -= FadeOutTick;
            removal_action(this);
        }

        private void MessageDurationTick(object sender, EventArgs e)
        {
            removal_timer.Stop();
            Remove();
        }

        public void Remove()
        {
            if (IsRemoving) return;
            IsRemoving = true;

            removal_timer.Tick -= MessageDurationTick;
            removal_timer.Tick += FadeOutTick;
            removal_timer.Interval = fade_out_duration;
            removal_timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
