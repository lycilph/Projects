using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Wpf_test
{
    public class EnumTypeConverter : EnumConverter
    {
        public EnumTypeConverter(Type enum_type) : base(enum_type) { }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destination_type)
        {
            if (destination_type == typeof(string) && value != null)
            {
                var enumType = value.GetType();
                if (enumType.IsEnum)
                    return GetDisplayName(value);
            }

            return base.ConvertTo(context, culture, value, destination_type);
        }

        private string GetDisplayName(object enum_value)
        {
            var displayNameAttribute = EnumType.GetField(enum_value.ToString()).GetCustomAttributes(typeof(EnumDisplayNameAttribute), false).FirstOrDefault() as EnumDisplayNameAttribute;
            if (displayNameAttribute != null)
                return displayNameAttribute.DisplayName;

            return Enum.GetName(EnumType, enum_value);
        }
    }
}
