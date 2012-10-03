using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;

namespace Wpf_test_2
{
    public class DesignAndRunTimeDatacontext : DataSourceProvider
    {
        public object RuntimeDatacontext { get; set; }
        public object DesigntimeDatacontext { get; set; }

        protected override void BeginQuery()
        {
            object result = null;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                result = DesigntimeDatacontext;
            else
                result = RuntimeDatacontext;

            base.OnQueryFinished(result);
        }
    }
}
