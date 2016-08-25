using System.Windows;
using RAYTracker.Views;

namespace RAYTracker.Dialogs
{
    public class WaitDialogService : IWaitDialogService
    {
        private WaitWindow _waitDialog;

        public void ShowWaitDialog()
        {
            _waitDialog = new WaitWindow
            {
                Owner = Application.Current.MainWindow,
                FetchProgressBar = { IsIndeterminate = true }
            };
            _waitDialog.Show();
        }

        public void CloseWaitDialog()
        {
            _waitDialog.Close();
        }
    }
}
