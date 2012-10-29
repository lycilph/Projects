using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomTypeProviderTest
{
  public class CustomType : Type
  {
    private readonly Type internal_type;
    private readonly List<PropertyInfo> internal_properties = new List<PropertyInfo>();

    public CustomType(Type internal_type)
    {
      if (internal_type == null)
        throw new ArgumentNullException("internal_type");
      this.internal_type = internal_type;
    }

    public void AddProperty(PropertyInfo property_info)
    {
      internal_properties.Add(property_info);
    }

    #region Type overrides

    public override Assembly Assembly
    {
      get { return internal_type.Assembly; }
    }

    public override string AssemblyQualifiedName
    {
      get { return internal_type.AssemblyQualifiedName; }
    }

    public override Type BaseType
    {
      get { return internal_type.BaseType; }
    }

    public override string FullName
    {
      get { return internal_type.FullName; }
    }

    public override Guid GUID
    {
      get { return internal_type.GUID; }
    }

    protected override TypeAttributes GetAttributeFlagsImpl()
    {
      throw new NotImplementedException();
    }

    protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
    {
      return internal_type.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);
    }

    public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
    {
      return internal_type.GetConstructors(bindingAttr);
    }

    public override Type GetElementType()
    {
      return internal_type.GetElementType();
    }

    public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
    {
      return internal_type.GetEvent(name, bindingAttr);
    }

    public override EventInfo[] GetEvents(BindingFlags bindingAttr)
    {
      return internal_type.GetEvents(bindingAttr);
    }

    public override FieldInfo GetField(string name, BindingFlags bindingAttr)
    {
      return internal_type.GetField(name, bindingAttr);
    }

    public override FieldInfo[] GetFields(BindingFlags bindingAttr)
    {
      return internal_type.GetFields(bindingAttr);
    }

    public override Type GetInterface(string name, bool ignoreCase)
    {
      return internal_type.GetInterface(name, ignoreCase);
    }

    public override Type[] GetInterfaces()
    {
      return internal_type.GetInterfaces();
    }

    public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
    {
      return internal_type.GetMembers(bindingAttr);
    }

    protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
    {
      throw new NotImplementedException();
    }

    public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
    {
      return internal_type.GetMethods(bindingAttr);
    }

    public override Type GetNestedType(string name, BindingFlags bindingAttr)
    {
      return internal_type.GetNestedType(name, bindingAttr);
    }

    public override Type[] GetNestedTypes(BindingFlags bindingAttr)
    {
      return internal_type.GetNestedTypes(bindingAttr);
    }

    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
    {
      //return internal_type.GetProperties(bindingAttr).Concat(internal_properties).ToArray();
      return internal_properties.ToArray();
    }

    protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
    {
      //return GetProperties(bindingAttr).FirstOrDefault(prop => prop.Name == name) ??
      return internal_properties.FirstOrDefault(p => p.Name == name);
    }

    protected override bool HasElementTypeImpl()
    {
      return internal_type.HasElementType;
    }

    public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
    {
      return internal_type.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
    }

    protected override bool IsArrayImpl()
    {
      return internal_type.IsArray;
    }

    protected override bool IsByRefImpl()
    {
      return internal_type.IsByRef;
    }

    protected override bool IsCOMObjectImpl()
    {
      return internal_type.IsCOMObject;
    }

    protected override bool IsPointerImpl()
    {
      return internal_type.IsPointer;
    }

    protected override bool IsPrimitiveImpl()
    {
      return internal_type.IsPrimitive;
    }

    public override Module Module
    {
      get { return internal_type.Module; }
    }

    public override string Namespace
    {
      get { return internal_type.Namespace; }
    }

    public override Type UnderlyingSystemType
    {
      get { return internal_type.UnderlyingSystemType; }
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
      return internal_type.GetCustomAttributes(attributeType, inherit);
    }

    public override object[] GetCustomAttributes(bool inherit)
    {
      return internal_type.GetCustomAttributes(inherit);
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
      return internal_type.IsDefined(attributeType, inherit);
    }

    public override string Name
    {
      get { return internal_type.Name; }
    }

    public override bool ContainsGenericParameters
    {
      get { return internal_type.ContainsGenericParameters; }
    }

    public override IEnumerable<CustomAttributeData> CustomAttributes
    {
      get { return internal_type.CustomAttributes; }
    }

    public override MethodBase DeclaringMethod
    {
      get { return internal_type.DeclaringMethod; }
    }

    public override Type DeclaringType
    {
      get { return internal_type.DeclaringType; }
    }

    public override bool Equals(Type o)
    {
      return internal_type == o;
    }

    public override Type[] FindInterfaces(TypeFilter filter, object filterCriteria)
    {
      return internal_type.FindInterfaces(filter, filterCriteria);
    }

    public override MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
    {
      return internal_type.FindMembers(memberType, bindingAttr, filter, filterCriteria);
    }

    public override GenericParameterAttributes GenericParameterAttributes
    {
      get { return internal_type.GenericParameterAttributes; }
    }

    public override int GenericParameterPosition
    {
      get { return internal_type.GenericParameterPosition; }
    }

    public override Type[] GenericTypeArguments
    {
      get { return internal_type.GenericTypeArguments; }
    }

    public override int GetArrayRank()
    {
      return internal_type.GetArrayRank();
    }

    public override System.Collections.Generic.IList<CustomAttributeData> GetCustomAttributesData()
    {
      return internal_type.GetCustomAttributesData();
    }

    public override MemberInfo[] GetDefaultMembers()
    {
      return internal_type.GetDefaultMembers();
    }

    public override string GetEnumName(object value)
    {
      return internal_type.GetEnumName(value);
    }

    public override string[] GetEnumNames()
    {
      return internal_type.GetEnumNames();
    }

    public override Type GetEnumUnderlyingType()
    {
      return internal_type.GetEnumUnderlyingType();
    }

    public override Array GetEnumValues()
    {
      return internal_type.GetEnumValues();
    }

    public override EventInfo[] GetEvents()
    {
      return internal_type.GetEvents();
    }

    public override Type[] GetGenericArguments()
    {
      return internal_type.GetGenericArguments();
    }

    public override Type[] GetGenericParameterConstraints()
    {
      return internal_type.GetGenericParameterConstraints();
    }

    public override Type GetGenericTypeDefinition()
    {
      return internal_type.GetGenericTypeDefinition();
    }

    public override InterfaceMapping GetInterfaceMap(Type interfaceType)
    {
      return internal_type.GetInterfaceMap(interfaceType);
    }

    public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
    {
      return internal_type.GetMember(name, bindingAttr);
    }

    public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
    {
      return internal_type.GetMember(name, type, bindingAttr);
    }

    public override bool IsAssignableFrom(Type c)
    {
      return internal_type.IsAssignableFrom(c);
    }

    public override bool IsConstructedGenericType
    {
      get { return internal_type.IsConstructedGenericType; }
    }

    public override bool IsEnum
    {
      get { return internal_type.IsEnum; }
    }

    public override bool IsEnumDefined(object value)
    {
      return internal_type.IsEnumDefined(value);
    }

    public override bool IsEquivalentTo(Type other)
    {
      return internal_type.IsEquivalentTo(other);
    }

    public override bool IsGenericParameter
    {
      get { return internal_type.IsGenericParameter; }
    }

    public override bool IsGenericType
    {
      get { return internal_type.IsGenericType; }
    }

    public override bool IsGenericTypeDefinition
    {
      get { return internal_type.IsGenericTypeDefinition; }
    }

    public override bool IsInstanceOfType(object o)
    {
      return internal_type.IsInstanceOfType(o);
    }

    public override bool IsSerializable
    {
      get { return internal_type.IsSerializable; }
    }

    public override bool IsSubclassOf(Type c)
    {
      return internal_type.IsSubclassOf(c);
    }

    public override RuntimeTypeHandle TypeHandle
    {
      get { return internal_type.TypeHandle; }
    }

    public override Type MakeArrayType()
    {
      return internal_type.MakeArrayType();
    }

    public override Type MakeArrayType(int rank)
    {
      return internal_type.MakeArrayType(rank);
    }

    public override Type MakeByRefType()
    {
      return internal_type.MakeByRefType();
    }

    public override Type MakeGenericType(params Type[] typeArguments)
    {
      return internal_type.MakeGenericType(typeArguments);
    }

    public override Type MakePointerType()
    {
      return internal_type.MakePointerType();
    }

    public override MemberTypes MemberType
    {
      get { return internal_type.MemberType; }
    }

    public override int MetadataToken
    {
      get { return internal_type.MetadataToken; }
    }

    public override Type ReflectedType
    {
      get { return internal_type.ReflectedType; }
    }

    public override StructLayoutAttribute StructLayoutAttribute
    {
      get { return internal_type.StructLayoutAttribute; }
    }

    public override string ToString()
    {
      return internal_type.ToString();
    } 

    #endregion
  }
}
