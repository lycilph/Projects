using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomTypeProviderTest
{
  public class Post : INotifyPropertyChanged
  {
    private string _Text;
    public string Text
    {
      get { return _Text; }
      set { _Text = value; NotifyPropertyChanged(); }
    }

    public Post()
    {
      Text = "post";
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void NotifyPropertyChanged([CallerMemberName] string property_name = "")
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null)
        handler(this, new PropertyChangedEventArgs(property_name));
    }
  }
}
