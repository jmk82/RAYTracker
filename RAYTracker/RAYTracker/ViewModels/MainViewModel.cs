using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAYTracker.ViewModels
{
    public class MainViewModel
    {
        public CashGameViewModel CashGameViewModel { get; set; }

        public MainViewModel(CashGameViewModel cashGameViewModel)
        {
            CashGameViewModel = cashGameViewModel;
        }
    }
}
