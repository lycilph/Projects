using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ObservableObject.WeakEvents;

namespace ObservableObject.Library
{
    public interface IPropertyAccessTreeSubscriber<TListener>
    {
        void SubscribeToChanges(INotifyPropertyChanged subject, TListener listener);
        void UnsubscribeFromChanges(INotifyPropertyChanged subject, TListener listener);
    }

    public class PropertyAccessTree
    {
        internal List<PropertyAccessTreeNode> Children { get; set; }

        public PropertyAccessTree()
        {
            this.Children = new List<PropertyAccessTreeNode>();
        }

        public bool IsMonitoringChildProperties
        {
            get { return this.Children.Count != 0 && this.Children[0].Children.Count > 0; }
        }

        public bool DoesEntireTreeSupportINotifyPropertyChanging
        {
            get { return this.Children.All(t => t.DoesEntireSubtreeSupportINotifyPropertyChanging); }
        }

        public SubscriptionTree CreateSubscriptionTree(INotifyPropertyChanged parameter)
        {
            List<SubscriptionNode> subscribers = new List<SubscriptionNode>(this.Children.Count);
            for (int i = 0; i < this.Children.Count; i++)
            {
                PropertyAccessTreeNode child = this.Children[i];
                if (child.Children.Count > 0)
                {
                    var subscriptionNode = child.CreateSubscription(parameter);
                    subscribers.Add(subscriptionNode);
                }
            }
            var subscriptionTree = new SubscriptionTree(parameter, subscribers);
            return subscriptionTree;
        }

        public IPropertyAccessTreeSubscriber<TListener> CreateCallbackSubscription<TListener>(Action<TListener, object> onPropertyChanged)
        {
            if (!this.DoesEntireTreeSupportINotifyPropertyChanging)
            {
                throw new Exception();
            }

            return new PropertyAccessTreeSubscriberINotifyPropertyChanging<TListener>(this, onPropertyChanged);
        }

        private class PropertyAccessTreeSubscriberINotifyPropertyChanging<TListener> : IPropertyAccessTreeSubscriber<TListener>
        {
            private class SubscriptionNode
            {
                private readonly Action<TListener, object> _subscriberCallback;
                private readonly PropertyAccessNode _propertyAccessNode;

                private readonly Action<TListener, object, object, PropertyChangedEventArgs> _onChangedCallback;
                private readonly Action<TListener, object, object, PropertyChangingEventArgs> _onChangingCallback;

                private readonly List<SubscriptionNode> _children;

                public SubscriptionNode(Action<TListener, object> subscriberCallback, PropertyAccessNode propertyAccessNode)
                {
                    _children = new List<SubscriptionNode>();
                    _subscriberCallback = subscriberCallback;
                    _propertyAccessNode = propertyAccessNode;

                    _onChangedCallback = OnPropertyChanged;
                    _onChangingCallback = OnPropertyChanging;

                    BuildChildren();
                }

                private void BuildChildren()
                {
                    foreach (PropertyAccessTreeNode t in _propertyAccessNode.Children)
                    {
                        var childSubscriptionNode = new SubscriptionNode(_subscriberCallback, (PropertyAccessNode)t);
                        _children.Add(childSubscriptionNode);
                    }
                }

                private void OnPropertyChanging(TListener listener, object subject, object rootSubject, PropertyChangingEventArgs args)
                {
                    UnsubscribeFromChildren(listener, subject, rootSubject);
                }

                private void OnPropertyChanged(TListener listener, object subject, object rootSubject, PropertyChangedEventArgs args)
                {
                    SubscribeToChildren(listener, subject, rootSubject);
                    _subscriberCallback(listener, rootSubject);
                }

                public void Subscribe(INotifyPropertyChanged subject, object rootSubject, TListener listener)
                {
                    WeakPropertyChangedEventManager.Register(
                        subject,
                        rootSubject,
                        _propertyAccessNode.PropertyName,
                        listener,
                        _onChangingCallback,
                        _onChangedCallback);

                    SubscribeToChildren(listener, subject, rootSubject);
                }

                private void SubscribeToChildren(TListener listener, object subject, object rootSubject)
                {
                    if (_children.Count == 0)
                        return;

                    INotifyPropertyChanged newChildSubject = (INotifyPropertyChanged)_propertyAccessNode.GetPropertyValue(subject);

                    if (newChildSubject == null)
                    {
                        return;
                    }

                    foreach (SubscriptionNode t in _children)
                        t.Subscribe(newChildSubject, rootSubject, listener);
                }

                public void Unsubscribe(INotifyPropertyChanged subject, object rootSubject, TListener listener)
                {
                    WeakPropertyChangedEventManager.Unregister(subject, _propertyAccessNode.PropertyName, listener, rootSubject);

                    UnsubscribeFromChildren(listener, subject, rootSubject);
                }

                private void UnsubscribeFromChildren(TListener listener, object subject, object rootSubject)
                {
                    if (_children.Count == 0)
                        return;

                    INotifyPropertyChanged oldChildSubject = (INotifyPropertyChanged)_propertyAccessNode.GetPropertyValue(subject);

                    if (oldChildSubject == null)
                    {
                        return;
                    }

                    foreach (SubscriptionNode t in _children)
                        t.Unsubscribe(oldChildSubject, rootSubject, listener);
                }
            }

            private class RootSubscription
            {
                private readonly Action<TListener, object> _subscriberCallback;
                private readonly PropertyAccessTreeNode _propertyAccessTreeNode;
                private readonly List<SubscriptionNode> _children;

                public RootSubscription(Action<TListener, object> subscriberCallback, PropertyAccessTreeNode parameterNode)
                {
                    _propertyAccessTreeNode = parameterNode;
                    _children = new List<SubscriptionNode>();
                    _subscriberCallback = subscriberCallback;

                    BuildChildren(parameterNode);
                }

                private void BuildChildren(PropertyAccessTreeNode root)
                {
                    foreach (PropertyAccessTreeNode t in root.Children)
                    {
                        var childSubscriptionNode = new SubscriptionNode(_subscriberCallback, (PropertyAccessNode)t);
                        _children.Add(childSubscriptionNode);
                    }
                }

                public void Subscribe(INotifyPropertyChanged subject, TListener listener)
                {
                    INotifyPropertyChanged subjectToSubscribe = subject;
                    ConstantNode constantNode = _propertyAccessTreeNode as ConstantNode;
                    if (constantNode != null)
                    {
                        subjectToSubscribe = constantNode.Value;
                    }

                    foreach (var child in _children)
                        child.Subscribe(subjectToSubscribe, subject, listener);
                }

                public void Unsubscribe(INotifyPropertyChanged subject, TListener listener)
                {
                    INotifyPropertyChanged subjectToUnsubscribe = subject;
                    ConstantNode constantNode = _propertyAccessTreeNode as ConstantNode;
                    if (constantNode != null)
                    {
                        subjectToUnsubscribe = constantNode.Value;
                    }

                    for (int i = 0; i < _children.Count; i++)
                    {
                        _children[i].Unsubscribe(subjectToUnsubscribe, subject, listener);
                    }
                }
            }

            private readonly List<RootSubscription> _subscriptions;

            private PropertyAccessTree _propertyAccessTree;

            public PropertyAccessTreeSubscriberINotifyPropertyChanging(PropertyAccessTree propertyAccessTree, Action<TListener, object> subscriberCallback)
            {
                _subscriptions = new List<RootSubscription>();

                _propertyAccessTree = propertyAccessTree;

                foreach (PropertyAccessTreeNode t in propertyAccessTree.Children)
                {
                    var rootSubscription = new RootSubscription(subscriberCallback, t);
                    _subscriptions.Add(rootSubscription);
                }
            }

            public void SubscribeToChanges(INotifyPropertyChanged subject, TListener listener)
            {
                foreach (RootSubscription t in _subscriptions)
                    t.Subscribe(subject, listener);
            }

            public void UnsubscribeFromChanges(INotifyPropertyChanged subject, TListener listener)
            {
                foreach (RootSubscription t in _subscriptions)
                    t.Unsubscribe(subject, listener);
            }
        }
    }
}
