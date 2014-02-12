using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaliburnMicroTest.Utils
{
    public delegate void StartupTask();

    public class StartupTasks
    {
        [Export(typeof(StartupTask))]
        public void ApplyBindingScopeOverride()
        {
            var getNamedElements = BindingScope.GetNamedElements;
            BindingScope.GetNamedElements = o =>
            {
                var metroWindow = o as MetroWindow;
                if (metroWindow == null)
                {
                    return getNamedElements(o);
                }

                var list = new List<FrameworkElement>(getNamedElements(o));
                var type = o.GetType();
                var fields = o.GetType()
                              .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                              .Where(f => f.DeclaringType == type);
                var flyouts = fields.Where(f => f.FieldType == typeof(FlyoutsControl))
                                    .Select(f => f.GetValue(o))
                                    .Cast<FlyoutsControl>();
                list.AddRange(flyouts);
                return list;
            };
        }

    }
}
