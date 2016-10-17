using GalaSoft.MvvmLight.Messaging;
using RAYTracker.ViewModels;
using RAYTracker.Views;
using System.Windows;

namespace RAYTracker.Dialogs
{
    public class InfoDialogService : IInfoDialogService
    {
        private InfoDialog _infoDialog;

        public void ShowInfoDialog(InfoDialogViewModel vm)
        {
            _infoDialog = new InfoDialog()
            {
                Owner = Application.Current.MainWindow,
                DataContext = vm
            };

            Messenger.Default.Register<NotificationMessage>(this, message =>
            {
                if (message.Notification == "CloseInfoDialog") CloseInfoDialog();
            });

            _infoDialog.ShowDialog();
        }

        public void CloseInfoDialog()
        {
            _infoDialog.Close();
        }
    }
}
