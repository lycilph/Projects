using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SortUtils
{
    public static class XamlHelper
    {
        public static void ExecuteOnLoaded(FrameworkElement fe, Action<FrameworkElement> action)
        {
            if (fe.IsLoaded)
            {
                if (action != null)
                {
                    action(fe);
                }
            }
            else
            {
                RoutedEventHandler handler = null;
                handler = delegate
                {
                    fe.Loaded -= handler;

                    if (action != null)
                    {
                        action(fe);
                    }
                };

                fe.Loaded += handler;
            }
        }
    }
}
