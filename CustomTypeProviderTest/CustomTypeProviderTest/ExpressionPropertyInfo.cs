using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomTypeProviderTest
{
  class ExpressionPropertyInfo<T> : PropertyInfo
  {
    private string internal_name;

    //public TIn internal_object { get; set; }
    //public Func<TIn, TOut> GetFunc { get; set; }
    public Func<T> GetFunc { get; set; }

    public ExpressionPropertyInfo(string name)
    {
      internal_name = name;
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
      return null;
    }

    public override ParameterInfo[] GetIndexParameters()
    {
      var parameters = new List<ParameterInfo>();
      return parameters.ToArray();
    }

    public override MethodInfo GetSetMethod(bool nonPublic)
    {
      throw new NotImplementedException();
    }

    public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
      return GetFunc(/*internal_object*/);
    }

    public override Type PropertyType
    {
      get { return typeof(T); }
    }

    public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
    {
      throw new NotImplementedException();
    }

    public override Type DeclaringType
    {
      get { throw new NotImplementedException(); }
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      throw new NotImplementedException();
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      throw new NotImplementedException();
    }

    public override string Name
    {
      get { return internal_name; }
    }

    public override Type ReflectedType
    {
      get { throw new NotImplementedException(); }
    }
  }
}
