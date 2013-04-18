using System;
using System.Windows.Input;
using System.Windows.Threading;
using LunchViewer.Infrastructure;
using Microsoft.Expression.Interactivity.Core;

namespace LunchViewer.Model
{
    public class Notification : ObservableObject
    {
        private readonly DispatcherTimer removal_timer;
        private readonly Func<Notification, bool> removal_action;
        private readonly TimeSpan fade_out_duration;

        private object _Data;
        public object Data
        {
            get { return _Data; }
            set
            {
                if (Equals(value, _Data)) return;
                _Data = value;
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

        public ICommand ClickCommand { get; private set; }

        public Notification(object data, Func<Notification, bool> removal_action, Action click_action, TimeSpan message_duration, TimeSpan fade_out_duration)
        {
            Data = data;
            IsRemoving = false;
            this.removal_action = removal_action;
            this.fade_out_duration = fade_out_duration;

            ClickCommand = new ActionCommand(click_action);

            removal_timer = new DispatcherTimer { Interval = message_duration };
            removal_timer.Tick += MessageDurationTick;
            removal_timer.Start();
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
    }
}
