using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using NLog;

namespace Hierarchy_Test
{
    public class DesignAndRunTimeDatacontext : DataSourceProvider
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public object RuntimeDatacontext { get; set; }
        public object DesigntimeDatacontext { get; set; }

        protected override void BeginQuery()
        {
            log.Debug("BeginQuery");

            object result = null;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                result = DesigntimeDatacontext;
            else
                result = RuntimeDatacontext;

            base.OnQueryFinished(result);
        }
    }
}
