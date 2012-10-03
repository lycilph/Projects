using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Module_Interface;
using System;

namespace Test_Import_Module
{
    public class TestImportModule : IModuleInterface
    {
        public string Name
        {
            get { return "Test Import Module"; }
        }
        [Import(typeof(Account))]
        public Account CurrentAccount { get; set; }

        private RelayCommand _ImportCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (_ImportCommand == null)
                    _ImportCommand = new RelayCommand(_ => DoImport());
                return _ImportCommand;
            }
        }

        private void DoImport()
        {
            CurrentAccount.Name = "Account (Test)";
            CurrentAccount.Posts.Add(new Post(DateTime.Now, "Post 1"));
            CurrentAccount.Posts.Add(new Post(DateTime.Now.AddDays(1), "Post 2"));
            CurrentAccount.Posts.Add(new Post(DateTime.Now.AddDays(2), "Post 3"));
            CurrentAccount.Posts.Add(new Post(DateTime.Now.AddDays(3), "Post 4"));
            CurrentAccount.Posts.Add(new Post(DateTime.Now.AddDays(4), "Post 5"));
        }

        public void Execute(string filename, Account account)
        {
            DoImport();
        }
    }
}
