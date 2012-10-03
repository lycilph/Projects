using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Wpf_drilldown_listbox_test
{
    public static class XamlHelper
    {
        public static void ExecuteOnLoaded(FrameworkElement fe, Action action)
        {
            if (fe.IsLoaded)
            {
                if (action != null)
                {
                    action();
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
                        action();
                    }
                };

                fe.Loaded += handler;
            }
        }
    }
}
