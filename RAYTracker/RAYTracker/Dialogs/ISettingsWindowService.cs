using RAYTracker.ViewModels;

namespace RAYTracker.Dialogs
{
    public interface ISettingsWindowService
    {
        void CloseWindow();
        void showWindow(SettingsViewModel viewModel);
    }
}