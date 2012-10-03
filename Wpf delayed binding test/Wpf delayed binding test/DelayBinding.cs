using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Wpf_delayed_binding_test
{
    public class DelayBinding
    {
        private readonly BindingExpressionBase binding_expression;
        private readonly DispatcherTimer timer;
        
        protected DelayBinding(BindingExpressionBase binding_expression, DependencyObject binding_target, DependencyProperty binding_target_property, TimeSpan delay)
        {
            this.binding_expression = binding_expression;
            
            // Subscribe to notifications for when the target property changes. This event handler will be
            // invoked when the user types, clicks, or anything else which changes the target property
            var descriptor = DependencyPropertyDescriptor.FromProperty(binding_target_property, binding_target.GetType());
            descriptor.AddValueChanged(binding_target, BindingTargetTargetPropertyChanged);
            
            // Add support so that the Enter key causes an immediate commit
            var frameworkElement = binding_target as FrameworkElement;
            if (frameworkElement != null)
            {
                frameworkElement.KeyUp += BindingTargetKeyUp;
            }
            
            // Setup the timer, but it won't be started until changes are detected
            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = delay;
        }
        
        private void BindingTargetKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            
            timer.Stop();
            binding_expression.UpdateSource();
        }
        
        private void BindingTargetTargetPropertyChanged(object sender, EventArgs e)
        {
            timer.Stop();
            timer.Start();
        }
        
        private void TimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            binding_expression.UpdateSource();
        }
        
        public static object SetBinding(DependencyObject binding_target, DependencyProperty binding_target_property, TimeSpan delay, Binding binding)
        {
            // Override some specific settings to enable the behavior of delay binding
            binding.Mode = BindingMode.TwoWay;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            
            // Apply and evaluate the binding
            var bindingExpression = BindingOperations.SetBinding(binding_target, binding_target_property, binding);
            
            // Setup the delay timer around the binding. This object will live as long as the target element lives, since it subscribes to the changing event,
            // and will be garbage collected as soon as the element isn't required (e.g., when it's Window closes) and the timer has stopped.
            new DelayBinding(bindingExpression, binding_target, binding_target_property, delay);
            
            // Return the current value of the binding (since it will have been evaluated because of the binding above)
            return binding_target.GetValue(binding_target_property);
        }
    }
}
