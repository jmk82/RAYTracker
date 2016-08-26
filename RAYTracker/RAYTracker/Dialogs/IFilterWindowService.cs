using RAYTracker.ViewModels;

namespace RAYTracker.Dialogs
{
    public interface IFilterWindowService
    {
        void ShowWindow(FilterViewModel viewModel);
        void CloseWindow();
    }
}