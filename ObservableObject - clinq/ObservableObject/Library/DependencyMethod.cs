using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ObservableObject.Library
{
    public class DependencyMethod<T> where T : class, INotifyPropertyChanged, INotifyPropertyChanging
    {
        private readonly Action<T> callback;
        private List<IPropertyAccessTreeSubscriber<DependencyMethod<T>>> property_change_subscribers;

        internal DependencyMethod(Action<T> callback)
        {
            this.callback = callback;
        }

        public DependencyMethod<T> OnChanged<TResult>(Expression<Func<T, TResult>> propertyAccessor)
        {
            PropertyAccessTree property_access_tree = ExpressionPropertyAnalyzer.Analyze(propertyAccessor);
            if (!property_access_tree.DoesEntireTreeSupportINotifyPropertyChanging)
                throw new ArgumentException("All objects must implement INotifyPropertyChanging");

            if (property_change_subscribers == null)
                property_change_subscribers = new List<IPropertyAccessTreeSubscriber<DependencyMethod<T>>>();

            var subscriber = property_access_tree.CreateCallbackSubscription<DependencyMethod<T>>(OnAnyPropertyInSubscriptionChanges);
            property_change_subscribers.Add(subscriber);

            return this;
        }

        private static void OnAnyPropertyInSubscriptionChanges(DependencyMethod<T> me, object object_that_changed)
        {
            me.callback((T)object_that_changed);
        }

        internal void CreateSubscriptions(INotifyPropertyChanged subject, ref List<SubscriptionTree> listToAppendTo)
        {
            if (property_change_subscribers == null) return;

            foreach (var subscriber in property_change_subscribers)
                subscriber.SubscribeToChanges(subject, this);
        }
    }
}
