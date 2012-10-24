using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObservableObjectLibrary
{
  public class ObservableObject : INotifyPropertyChanging, INotifyPropertyChanged, INotifyDependenciesUpdated
  {
    #region Add dependency

    public void AddDependency<T1, T2>(Expression<Func<T1>> source, Expression<Func<T2>> target)
    {
      var is_notifycollectionchanged = typeof(T1).GetInterfaces().Contains(typeof(INotifyCollectionChanged));
      var is_observableobject = typeof(T1).IsSubclassOf(typeof(ObservableObject));

      if (is_notifycollectionchanged)
      {
        INotifyCollectionChanged source_object = FindSourceObject(source) as INotifyCollectionChanged;
        string target_property_name = ExpressionHelper.FindPropertyName(target);

        // Check that the source object is an INotifyCollectionChanged
        if (source_object == null)
          throw new ArgumentException("Source object not of type " + typeof(INotifyCollectionChanged).ToString());

        // Check that the source property is actually is a property
        ExpressionHelper.CheckPropertyName(source);

        // Create dependency event handler
        NotifyCollectionChangedEventHandler handler = (sender, args) => NotifyPropertyChanged(target_property_name);

        // Add dependency to the object itself
        source_object.CollectionChanged += handler;

        // Cache the dependency
        //var dependency = DependencyHelper.CreateCollectionDependency(source, target, source_object, handler);
        //dependencies.Add(dependency);
      }
      else if (is_observableobject)
      {
        ObservableObject source_object = FindSourceObject(source) as ObservableObject;
        string target_property_name = ExpressionHelper.FindPropertyName(target);

        // Check that the source object is an ObservableObject
        if (source_object == null)
          throw new ArgumentException("Source object not of type " + typeof(ObservableObject).ToString());

        // Check that the source property is actually is a property
        ExpressionHelper.CheckPropertyName(source);

        // Create dependency event handler
        PropertyChangedEventHandler handler = (sender, args) => NotifyPropertyChanged(target_property_name);

        // Add dependency to the object itself
        source_object.PropertyChanged += handler;

        // Cache the dependency
        //var dependency = DependencyHelper.CreatePropertyDependency(source, target, source_object, handler);
        //dependencies.Add(dependency);
      }
      else
      {
        // Parse input
        ObservableObject source_object = FindSourceObject(source, 1) as ObservableObject; // stop_level = 1, as the source property will be the last of the stack
        string source_property_name = ExpressionHelper.FindPropertyName(source);
        string target_property_name = ExpressionHelper.FindPropertyName(target);

        // Check that the source object is an ObservableObject
        if (source_object == null)
          throw new ArgumentException("Source object not of type " + typeof(ObservableObject).ToString());

        // Create dependency event handler
        PropertyChangedEventHandler handler = (sender, args) =>
        {
          if (args.PropertyName == source_property_name)
            NotifyPropertyChanged(target_property_name);
        };

        // Add dependency to the object itself
        source_object.PropertyChanged += handler;

        // Cache the dependency
        //var dependency = DependencyHelper.CreatePropertyDependency(source, target, source_object, handler);
        //dependencies.Add(dependency);
      }
    }

    private object FindSourceObject<T>(Expression<Func<T>> source_expression, int stop_level = 0)
    {
      // Unpack lambda expression
      var expr = source_expression.Body;

      // "descend" toward's the root object reference and push on stack
      var member_stack = new Stack<MemberInfo>();
      while (expr is MemberExpression)
      {
        var member_expr = expr as MemberExpression;
        member_stack.Push(member_expr.Member);
        expr = member_expr.Expression;
      }

      // Find root object
      var root_expression = expr as ConstantExpression;
      if (root_expression == null)
        throw new ArgumentException("No root object found");
      var root_object = root_expression.Value;

      // "ascend" back whence we came from and resolve object references along the way
      while (member_stack.Count > stop_level)
      {
        var mi = member_stack.Pop();
        if (mi.MemberType == MemberTypes.Field)
          root_object = root_object.GetType().GetField(mi.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(root_object);
        else if (mi.MemberType == MemberTypes.Property)
          root_object = root_object.GetType().GetProperty(mi.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(root_object, null);
      }

      return root_object;
    }

    #endregion

    #region Interface implementation

    public event PropertyChangingEventHandler PropertyChanging;
    public void NotifyPropertyChanging(string property_name)
    {
      PropertyChangingEventHandler handler = PropertyChanging;
      if (handler != null)
        handler(this, new PropertyChangingEventArgs(property_name));
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void NotifyPropertyChanged(string property_name)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(property_name));
    }

    public event DependenciesUpdatedEventHandler DependenciesUpdated;
    public void NotifyDependenciesUpdated(string property_name)
    {
      DependenciesUpdatedEventHandler handler = DependenciesUpdated;
      if (handler != null)
        handler(this, new DependenciesUpdatedEventArgs(property_name));
    } 

    #endregion
  }
}
