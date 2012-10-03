using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.Windows.Input;

namespace Module_Interface
{
    [InheritedExport(typeof(IModuleInterface))]
    public interface IModuleInterface
    {
        string Name { get; }
        Account CurrentAccount { get; set; }

        ICommand ImportCommand { get; }

        void Execute(string filename, Account account);
    }
}
