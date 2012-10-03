using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Microsoft.Expression.Media.Effects;

namespace Wpf_drilldown_listbox_test
{
    public class AnimatedListBox : ListBox
    {
        // Parts
        private Border control_border = null;

        // Helper variables for the animation
        private Brush old_image;

        public IEnumerable AnimatedItemsSource
        {
            get { return (IEnumerable)GetValue(AnimatedItemsSourceProperty); }
            set { SetValue(AnimatedItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty AnimatedItemsSourceProperty =
            DependencyProperty.Register("AnimatedItemsSource", typeof(IEnumerable), typeof(AnimatedListBox), new UIPropertyMetadata(new PropertyChangedCallback(OnAnimatedItemsSourcePropertyChanged)));

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register("Duration", typeof(TimeSpan), typeof(AnimatedListBox), new UIPropertyMetadata(TimeSpan.FromMilliseconds(250)));

        public IEasingFunction EasingFunction
        {
            get { return (IEasingFunction)GetValue(EasingFunctionProperty); }
            set { SetValue(EasingFunctionProperty, value); }
        }
        public static readonly DependencyProperty EasingFunctionProperty =
            DependencyProperty.Register("EasingFunction", typeof(IEasingFunction), typeof(AnimatedListBox), new UIPropertyMetadata(null));

        public TransitionEffect TransitionEffect
        {
            get { return (TransitionEffect)GetValue(TransitionEffectProperty); }
            set { SetValue(TransitionEffectProperty, value); }
        }
        public static readonly DependencyProperty TransitionEffectProperty =
            DependencyProperty.Register("TransitionEffect", typeof(TransitionEffect), typeof(AnimatedListBox), new UIPropertyMetadata(null));        

        static AnimatedListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedListBox), new FrameworkPropertyMetadata(typeof(AnimatedListBox)));
            ItemsSourceProperty.OverrideMetadata(typeof(AnimatedListBox), new FrameworkPropertyMetadata(OnItemsSourcePropertyChanged));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            control_border = Template.FindName("PART_Border", this) as Border;
            if (control_border == null)
                throw new Exception("AnimatedListBox must contain the part [PART_Border]");
        }

        private static void OnAnimatedItemsSourcePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            var animated_listbox = dp as AnimatedListBox;
            if (animated_listbox == null)
                return;

            // Render old content to an image, so it doesn't change during animation
            animated_listbox.old_image = RenderToBrush(animated_listbox.control_border);

            // Set itemsSource to trigger animation
            animated_listbox.ItemsSource = animated_listbox.AnimatedItemsSource;
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs args)
        {
            var animated_listbox = dp as AnimatedListBox;
            if (animated_listbox == null || args.OldValue == null)
                return;

            animated_listbox.AnimateContent();
        }

        private void AnimateContent()
        {
            // Only do the transition if the AnimatedItemsSource is used
            if (AnimatedItemsSource == null || this.TransitionEffect == null)
                return;

            // Is the GPU capable of handling the transition effect
            var tier = (RenderCapability.Tier >> 16);
            if (tier < 2)
                return;

            var da = new DoubleAnimation(0.0, 1.0, new Duration(Duration), FillBehavior.HoldEnd);
            da.Completed += (s, e) => Effect = null;
            if (EasingFunction != null)
                da.EasingFunction = EasingFunction;

            var transition_effect = this.TransitionEffect.Clone() as TransitionEffect;
            if (transition_effect == null)
                throw new InvalidOperationException("TransitionEffect must be of type TransitionEffect");

            transition_effect.OldImage = old_image;
            transition_effect.BeginAnimation(TransitionEffect.ProgressProperty, da);

            this.Effect = transition_effect;
        }

        private static Brush RenderToBrush(FrameworkElement fe)
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
