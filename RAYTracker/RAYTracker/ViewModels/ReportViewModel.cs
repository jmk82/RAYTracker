using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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
