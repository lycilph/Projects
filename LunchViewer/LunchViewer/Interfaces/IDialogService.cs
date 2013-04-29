namespace LunchViewer.Interfaces
{
    public interface IDialogService
    {
        bool? ShowYesNoMessage(string message, string title);
        bool? ShowOkMessage(string message, string title);
    }
}
