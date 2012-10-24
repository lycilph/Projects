using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableObjectLibrary
{
  public delegate void DependenciesUpdatedEventHandler(object sender, DependenciesUpdatedEventArgs args);

  public interface INotifyDependenciesUpdated
  {
    event DependenciesUpdatedEventHandler DependenciesUpdated;
  }
}
