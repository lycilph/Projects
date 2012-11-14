using System;
using System.Collections.Generic;
using System.ComponentModel;
using MVVM.Expressions;

namespace MVVM.Observable
{
    internal interface IDependency
    {
        void SubscribeToChanges(INotifyPropertyChanged subject);
        void UnsubscribeFromChanges(INotifyPropertyChanged subject);

        #region Graph debug output

        IEnumerable<PropertyAccessTree> GraphDebug_GetPropertyAccessTrees();

        #endregion
    }

    public class Dependency<T> : IDependency where T : class, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private List<DependencyMethod<T>> Methods { get; set; }

        internal Dependency()
        {
            Methods = new List<DependencyMethod<T>>();
        }

        public DependencyMethod<T> Call(Action<T> callback)
        {
            var dependency_method = new DependencyMethod<T>(callback);
            Methods.Add(dependency_method);

            return dependency_method;
        }

        public void SubscribeToChanges(INotifyPropertyChanged subject)
        {
            foreach (var dependency_method in Methods)
                dependency_method.SubscribeToChanges(subject);
        }

        public void UnsubscribeFromChanges(INotifyPropertyChanged subject)
        {
            foreach (var dependency_method in Methods)
                dependency_method.UnsubscribeFromChanges(subject);
        }

        #region Graph debug output

        public IEnumerable<PropertyAccessTree> GraphDebug_GetPropertyAccessTrees()
        {
            List<PropertyAccessTree> trees = new List<PropertyAccessTree>();

            foreach (var dependency_method in Methods)
                trees.AddRange(dependency_method.GraphDebug_GetPropertyAccessTrees());

            return trees;
        }

        #endregion
    }
}
