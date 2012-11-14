using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableObjectLibrary
{
    public class DependenciesUpdatedEventArgs : EventArgs
    {
        public string SourceName { get; set; }

        public DependenciesUpdatedEventArgs(string source_name)
        {
            SourceName = source_name;
        }
    }
}
