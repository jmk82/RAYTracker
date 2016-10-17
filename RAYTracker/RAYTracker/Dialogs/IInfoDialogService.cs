using RAYTracker.ViewModels;

namespace RAYTracker.Dialogs
{
    public interface IInfoDialogService
    {
        void ShowInfoDialog(InfoDialogViewModel vm);
        void CloseInfoDialog();
    }
}