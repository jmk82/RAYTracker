using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace RAYTracker.ViewModels
{
    public sealed class InfoDialogViewModel : ViewModelBase
    {
        public string InfoText { get; private set; }
        public string CancelButtonVisibility { get; set; }

        public RelayCommand CloseDialogCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }

        public InfoDialogViewModel(string message, bool showCancelButton = false)
        {
            InfoText = message;
            CancelButtonVisibility = showCancelButton ? "Visible" : "Hidden";

            CloseDialogCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CloseInfoDialog"));
            });

            ConfirmCommand = new RelayCommand(() =>
            {
                Messenger.Default.Send(new NotificationMessage("CloseInfoDialog"));
                Messenger.Default.Send(new NotificationMessage("InfoDialogConfirmed"));
            });
        }
    }
}
