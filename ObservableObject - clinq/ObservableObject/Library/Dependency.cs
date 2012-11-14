using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ObservableObject.Library
{
    internal interface IDependency
    {
        void CreateSubscriptions(INotifyPropertyChanged subject, ref List<SubscriptionTree> listToAppendTo);
    }

    public class Dependency<T> : IDependency where T : class, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private List<DependencyMethod<T>> Methods { get; set; }

        internal Dependency()
        {
            Methods = new List<DependencyMethod<T>>();
        }

        public void CreateSubscriptions(INotifyPropertyChanged subject, ref List<SubscriptionTree> listToAppendTo)
        {
            foreach (var dependency_method in Methods)
                dependency_method.CreateSubscriptions(subject, ref listToAppendTo);
        }

        public DependencyMethod<T> Call(Action<T> callback)
        {
            var dependency_method = new DependencyMethod<T>(callback);
            Methods.Add(dependency_method);

            return dependency_method;
        }
    }
}
