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
        //public TournamentViewModel TournamentViewModel { get; set; }

        public MainViewModel(CashGameViewModel cashGameVM)
        {
            CashGameViewModel = cashGameVM;
            //TournamentViewModel = tournamentViewModel;
        }
    }
}
