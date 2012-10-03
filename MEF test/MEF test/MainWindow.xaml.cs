using System.Collections.Generic;
using System.Windows;
using Module_Interface;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Input;

namespace MEF_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [ImportMany]
        public IEnumerable<IModuleInterface> ImportModules { get; set; }

        [Export(typeof(Account))]
        public Account CurrentAccount
        {
            get { return (Account)GetValue(CurrentAccountProperty); }
            set { SetValue(CurrentAccountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentAccount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentAccountProperty =
            DependencyProperty.Register("CurrentAccount", typeof(Account), typeof(MainWindow), new UIPropertyMetadata(new Account("")));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MainWindow), new UIPropertyMetadata("Filename.ext"));

        private RelayCommand _ImportCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (_ImportCommand == null)
                    _ImportCommand = new RelayCommand(module => DoImport(module));
                return _ImportCommand;
            }
        }

        private void DoImport(object module)
        {
            if (module is IModuleInterface)
                (module as IModuleInterface).Execute(Message, CurrentAccount);
        }

        public MainWindow()
        {
            Compose();
            InitializeComponent();
        }

        public void Compose()
        {
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(MainWindow).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(System.Environment.CurrentDirectory));

            var container = new CompositionContainer(catalog);
            try
            {
                container.ComposeParts(this);
            }
            catch (CompositionException composition_exception)
            {
                MessageBox.Show(composition_exception.ToString());
            }
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            CurrentAccount.Clear();
        }
    }
}
