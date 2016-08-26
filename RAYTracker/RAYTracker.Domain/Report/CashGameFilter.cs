using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAYTracker.Domain.Model;

namespace RAYTracker.Domain.Report
{
    public class CashGameFilter
    {
        public IEnumerable<GameType> GameTypes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
