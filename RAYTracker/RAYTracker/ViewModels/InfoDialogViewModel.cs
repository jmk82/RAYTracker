using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace RAYTracker.ViewModels
{
    public sealed class InfoDialogViewModel : ViewModelBase
    {
        public string InfoText { get; private set; }

        public RelayCommand CloseDialogCommand { get; set; }

        public InfoDialogViewModel(string message)
        {
            InfoText = message;

            CloseDialogCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CloseInfoDialog"));
            });
        }
    }
}
