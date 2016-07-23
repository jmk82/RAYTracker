using System.Collections.ObjectModel;

namespace RAYTracker
{
    public class SessionViewModel
    {
        public ObservableCollection<Session> Sessions { get; set; }

        public SessionViewModel(Program p)
        {
            Sessions = new ObservableCollection<Session>(p.GetSessions());
        }
    }
}
