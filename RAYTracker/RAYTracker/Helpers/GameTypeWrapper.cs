using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAYTracker.Domain.Model;

namespace RAYTracker.Helpers
{
    public class GameTypeWrapper
    {
        public GameType GameType { get; set; }
        public bool IsSelected { get; set; } = true;
    }
}
