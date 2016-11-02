using RAYTracker.ViewModels;

namespace RAYTracker.Dialogs
{
    public interface IInfoDialogService
    {
        void ShowDialog(InfoDialogViewModel vm);
        void CloseInfoDialog();
    }
}