using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomTypeProviderTest
{
  public class CustomPropertyInfo : PropertyInfo
  {
    private object internal_object;
    private PropertyInfo internal_property_info;

    public CustomPropertyInfo(object obj, PropertyInfo property_info)
    {
      internal_object = obj;
      internal_property_info = property_info;
    }

    public override PropertyAttributes Attributes
    {
      get { throw new NotImplementedException(); }
    }

    public override bool CanRead
    {
      get { return true; }
    }

    public override bool CanWrite
    {
      get { return true; }
    }

    public override MethodInfo[] GetAccessors(bool nonPublic)
    {
      throw new NotImplementedException();
    }

    public override MethodInfo GetGetMethod(bool nonPublic)
    {
      return internal_property_info.GetMethod;
    }

    public override ParameterInfo[] GetIndexParameters()
    {
      return internal_property_info.GetIndexParameters();
    }

    public override MethodInfo GetSetMethod(bool nonPublic)
    {
      return internal_property_info.SetMethod;
    }

    public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
      return internal_property_info.GetValue(internal_object, invokeAttr, binder, index, culture);
    }

    public override Type PropertyType
    {
      get { return internal_property_info.PropertyType; }
    }

    public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
      internal_property_info.SetValue(internal_object, value, invokeAttr, binder, index, culture);
    }

    public override Type DeclaringType
    {
      get { throw new NotImplementedException(); }
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return internal_property_info.GetCustomAttributes(attributeType, inherit);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return internal_property_info.GetCustomAttributes(inherit);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return internal_property_info.IsDefined(attributeType, inherit);
    }

    public override string Name
    {
      get { return internal_property_info.Name; }
    }

    public override Type ReflectedType
    {
      get { throw new NotImplementedException(); }
    }
  }
}
