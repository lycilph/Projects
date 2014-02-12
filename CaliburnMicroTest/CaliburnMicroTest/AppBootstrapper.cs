﻿using Caliburn.Micro;
using CaliburnMicroTest.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CaliburnMicroTest
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        static AppBootstrapper()
        {
            LogManager.GetLog = type => new DebugLogger(type);
        }

        public AppBootstrapper()
        {
            Start();
        }

        protected override void Configure()
        {
            var catalog = new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>());
            container = new CompositionContainer(catalog);

            var batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var startupTasks =
                this.GetAllInstances(typeof(StartupTask))
                    .Cast<ExportedDelegate>()
                    .Select(exportedDelegate => (StartupTask)exportedDelegate.CreateDelegate(typeof(StartupTask)));
            startupTasks.Apply(s => s());
            DisplayRootViewFor<IShell>();
        }
    }
}
