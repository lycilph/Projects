using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using LunchViewer.Annotations;

namespace LunchViewer
{
    public class Notification : INotifyPropertyChanged
    {
        private readonly DispatcherTimer removal_timer;
        private readonly Func<Notification, bool> removal_action;
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

        public Notification(string text, Func<Notification, bool> removal_action, TimeSpan message_duration, TimeSpan fade_out_duration)
        {
            Text = text;
            IsRemoving = false;
            this.removal_action = removal_action;
            this.fade_out_duration = fade_out_duration;

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

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
