using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveUI
{
    public class Data : ReactiveObject
    {
        private string _Text;
        public string Text
        {
            get { return _Text; }
            set { this.RaiseAndSetIfChanged(ref _Text, value); }
        }
    }
}
