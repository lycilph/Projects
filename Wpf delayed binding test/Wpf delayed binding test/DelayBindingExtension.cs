using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Wpf_delayed_binding_test
{
    public class DelayBindingExtension : MarkupExtension
    {
        public DelayBindingExtension()
        {
            Delay = TimeSpan.FromSeconds(0.5);
        }   
        public DelayBindingExtension(PropertyPath path) : this()
        {
            Path = path;
        }

        // New parameter for the DelayBinding
        public TimeSpan Delay { get; set; }

        // Parameters for the Binding
        public IValueConverter Converter { get; set; }
        public object ConverterParameter { get; set; }
        public string ElementName { get; set; }
        public RelativeSource RelativeSource { get; set; }
        public object Source { get; set; }
        public bool ValidatesOnDataErrors { get; set; }
        public bool ValidatesOnExceptions { get; set; }
        [ConstructorArgument("path")]
        public PropertyPath Path { get; set; }
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var valueProvider = serviceProvider.GetService(typeof (IProvideValueTarget)) as IProvideValueTarget;
            if (valueProvider != null)
            {
                var binding_target = valueProvider.TargetObject as DependencyObject;
                var binding_property = valueProvider.TargetProperty as DependencyProperty;
                if (binding_property == null || binding_target == null)
                {
                    throw new NotSupportedException(string.Format(
                        "The property '{0}' on target '{1}' is not valid for a DelayBinding. The DelayBinding target must be a DependencyObject, "
                        + "and the target property must be a DependencyProperty.",
                        valueProvider.TargetProperty,
                        valueProvider.TargetObject));
                }

                var binding = new Binding()
                {
                    Path = Path,
                    Converter = Converter,
                    ConverterCulture = ConverterCulture,
                    ConverterParameter = ConverterParameter,
                    ValidatesOnDataErrors = ValidatesOnDataErrors,
                    ValidatesOnExceptions = ValidatesOnExceptions
                };
                if (ElementName != null) binding.ElementName = ElementName;
                if (RelativeSource != null) binding.RelativeSource = RelativeSource;
                if (Source != null) binding.Source = Source;
                
                return DelayBinding.SetBinding(binding_target, binding_property, Delay, binding);
            }
            return null;
        }
    }
}
