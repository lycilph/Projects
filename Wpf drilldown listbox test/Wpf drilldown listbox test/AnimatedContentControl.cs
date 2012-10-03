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
using System.Windows.Media.Animation;
using Microsoft.Expression.Media.Effects;
using System.IO;

namespace Wpf_drilldown_listbox_test
{
    public class AnimatedContentControl : ContentControl
    {
        // Parts
        private ContentPresenter content_presenter;

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AnimatedContentControl), new UIPropertyMetadata(TimeSpan.FromMilliseconds(250)));

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(AnimatedContentControl), new UIPropertyMetadata(null));

        public TransitionEffect TransitionEffect
        {
            get { return (TransitionEffect)GetValue(TransitionEffectProperty); }
            set { SetValue(TransitionEffectProperty, value); }
        }
        public static readonly DependencyProperty TransitionEffectProperty =
            DependencyProperty.Register("TransitionEffect", typeof(TransitionEffect), typeof(AnimatedContentControl), new UIPropertyMetadata(null));        

        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
            ContentProperty.OverrideMetadata(typeof(AnimatedContentControl), new FrameworkPropertyMetadata(OnContentPropertyChanged));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            content_presenter = Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
            if (content_presenter == null)
                throw new Exception("AnimatedListBox must contain the part [PART_ContentPresenter]");
        }

        private static void OnContentPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            var old_content = args.OldValue;
            var new_content = args.NewValue;

            if (old_content != null && new_content != null)
            {
                var animated_content_control = (AnimatedContentControl)dp;
                animated_content_control.AnimateContent(old_content, new_content);
            }
            else
            {
                var animated_content_control = (AnimatedContentControl)dp;
                XamlHelper.ExecuteOnLoaded(animated_content_control, () => animated_content_control.content_presenter.Content = new_content );
            }
        }

        private void AnimateContent(object old_content, object new_content)
        {
            var old_visual_content = VisualTreeHelper.GetChild(content_presenter, 0) as FrameworkElement;
            if (old_visual_content == null || this.TransitionEffect == null)
            {
                content_presenter.Content = new_content;
                return;
            }

            // Is the GPU capable of handling the transition effect
            var tier = (RenderCapability.Tier >> 16);
            if (tier < 2)
                return;

            var da = new DoubleAnimation(0.0, 1.0, new Duration(this.Duration), FillBehavior.HoldEnd);
            da.Completed += (s, e) => content_presenter.Effect = null;
            if (EasingFunction != null)
                da.EasingFunction = EasingFunction;

            var transition_effect = this.TransitionEffect.Clone() as TransitionEffect;
            if (transition_effect == null)
                throw new InvalidOperationException("TransitionEffect must be of type TransitionEffect");

            transition_effect.OldImage = RenderToBrush(old_visual_content);
            transition_effect.BeginAnimation(TransitionEffect.ProgressProperty, da);

            content_presenter.Content = new_content;
            content_presenter.Effect = transition_effect;
        }

        private static VisualBrush RenderToBrush(FrameworkElement fe)
        {
            // Render current content to an image
            var render_target = new RenderTargetBitmap((int)fe.ActualWidth, (int)fe.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            render_target.Render(fe);
            var img = new Image() { Source = render_target };

            // Return image as a VisualBrush
            return new VisualBrush(img);
        }
    }
}
