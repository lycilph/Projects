using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using System.Collections.ObjectModel;

namespace MEFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IPartImportsSatisfiedNotification
    {
#if DEBUG
        private bool debug_mode = true;
#else
        private bool debug_mode = false;
#endif

        [ImportMany]
        public List<Lazy<IModule, IModuleType>> ImportModules { get; set; }

        public ObservableCollection<IModule> Modules { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Modules = new ObservableCollection<IModule>();
        }

        public void OnImportsSatisfied()
        {
#if DEBUG
            var imports = ImportModules.Select(m => m.Value);
#else
            var imports = ImportModules.Where(m => m.Metadata.Type == ModuleTypes.Import).Select(m => m.Value);
#endif
            Modules.Clear();
            foreach (var module in imports)
                Modules.Add(module);
        }

        private void UpdateModules()
        {
            IEnumerable<IModule> imports;

            if (debug_mode)
                imports = ImportModules.Select(m => m.Value);
            else
                imports = ImportModules.Where(m => m.Metadata.Type == ModuleTypes.Import).Select(m => m.Value);

            Modules.Clear();
            foreach (var module in imports)
                Modules.Add(module);
        }

        private void DebugModeClick(object sender, RoutedEventArgs e)
        {
            debug_mode = !debug_mode;
            UpdateModules();
        }
    }
}
