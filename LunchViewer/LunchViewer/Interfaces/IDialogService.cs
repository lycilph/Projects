namespace LunchViewer.Interfaces
{
    public interface IDialogService
    {
        bool? ShowYesNoMessage(string message, string title);
    }
}
