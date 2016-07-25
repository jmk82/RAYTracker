using System.Collections.ObjectModel;

namespace RAYTracker
{
    public class TableSessionViewModel
    {
        public ObservableCollection<TableSession> TableSessions { get; set; }

        public TableSessionViewModel(Session session)
        {
            TableSessions = new ObservableCollection<TableSession>(session.TableSessions);
        }
    }
}
