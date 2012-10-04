using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ObservableObjectLibrary;
using System.Collections;

namespace ValidationTest
{
    public class ValidationModelBase : ObservableObject, INotifyDataErrorInfo
    {
        private Dictionary<String, String> errors = new Dictionary<String, String>();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (errors.ContainsKey(propertyName))
                return errors[propertyName];
            return null;
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        public void AddError(string property, string error)
        {
            errors[property] = error;
            NotifyErrorsChanged(property);
        }

        public void RemoveError(string property)
        {
            if (errors.ContainsKey(property))
                errors.Remove(property);
            NotifyErrorsChanged(property);
        }

        public void NotifyErrorsChanged(string property)
        {
            var handler = ErrorsChanged;
            if (handler != null)
                handler(this, new DataErrorsChangedEventArgs(property));
        }
    }
}
