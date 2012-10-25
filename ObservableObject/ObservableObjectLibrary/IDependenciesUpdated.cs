namespace ObservableObjectLibrary
{
    public delegate void DependenciesUpdatedEventHandler(object sender, DependenciesUpdatedEventArgs args);

    public interface INotifyDependenciesUpdated
    {
        event DependenciesUpdatedEventHandler DependenciesUpdated;
    }
}
