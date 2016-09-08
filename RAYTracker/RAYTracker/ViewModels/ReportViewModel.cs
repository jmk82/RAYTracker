using GalaSoft.MvvmLight;

namespace RAYTracker.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private string _report;

        public string Report
        {
            get { return _report; }
            set
            {
                _report = value;
                RaisePropertyChanged();
            }
        }
    }
}
