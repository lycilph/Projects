using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Module_Interface;
using System;

namespace File_Import_Module
{
    public class FileImportModule : IModuleInterface
    {
        public string Name
        {
            get { return "File Import Module"; }
        }
        [Import(typeof(Account))]
        public Account CurrentAccount { get; set; }

        private RelayCommand _ImportCommand;
        public ICommand ImportCommand
        {
            get
            {
                if (_ImportCommand == null)
                    _ImportCommand = new RelayCommand(message => DoImport(message));
                return _ImportCommand;
            }
        }

        private void DoImport(object message)
        {
            string account_name;
            if (message is string)
                account_name = "Account (" + (message as string) + ")";
            else
                account_name = "Account (File)";

            Add(account_name, CurrentAccount);
        }

        public void Execute(string filename, Account account)
        {
            Add(filename, account);
        }

        private void Add(string account_name, Account account)
        {
            account.Name = account_name;
            account.Posts.Add(new Post(DateTime.Now, "Post 111"));
            account.Posts.Add(new Post(DateTime.Now.AddMonths(1), "Post 222"));
            account.Posts.Add(new Post(DateTime.Now.AddMonths(2), "Post 333"));
            account.Posts.Add(new Post(DateTime.Now.AddMonths(3), "Post 444"));
            account.Posts.Add(new Post(DateTime.Now.AddMonths(4), "Post 555"));
        }
    }
}
