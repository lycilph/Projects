using System;

namespace ObservableObjectLibrary
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    public class DependsUponAttribute : Attribute
    {
        public string Object { get; private set; }
        public string Source { get; private set; }

        public bool HasObject
        {
            get { return !string.IsNullOrEmpty(Object); }
        }

        public DependsUponAttribute(string input)
        {
            if (input.Contains("."))
            {
                var index = input.LastIndexOf('.');

                Object = input.Substring(0, index);
                Source = input.Substring(index + 1, input.Length - index - 1);
            }
            else
            {
                Source = input;
            }
        }

        public override string ToString()
        {
            return (HasObject ? string.Format("{0}.{1}", Object, Source) : Source);
        }
    }
}
