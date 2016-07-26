using RAYTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker
{
    public class SessionGenerator
    {
        public IList<Session> GroupToSessions(IList<TableSession> tableSessions)
        {
            if (tableSessions == null)
            {
                throw new ArgumentNullException("Ei istuntoja!");
            }

            var orderedTableSessions = OrderTableSessions(tableSessions);
            IList<Session> allSessions = new List<Session>();
            IList<TableSession> tableSessionsToBeAdded = new List<TableSession>();
            tableSessionsToBeAdded.Add(orderedTableSessions.First());

            var currentSetStartTime = orderedTableSessions.First().StartTime;
            var currentSetEndTime = orderedTableSessions.First().EndTime;

            while (true)
            {
                tableSessionsToBeAdded = orderedTableSessions.Where(t => t.StartTime <= currentSetEndTime).ToList();

                if (tableSessionsToBeAdded.Max(t => t.EndTime) == currentSetEndTime)
                {
                    allSessions.Add(new Session(currentSetStartTime, currentSetEndTime, 
                        new List<TableSession>(tableSessionsToBeAdded)));

                    foreach (var tableSession in tableSessionsToBeAdded)
                    {
                        orderedTableSessions.Remove(tableSession);
                    }

                    if (!orderedTableSessions.Any())
                    {
                        break;
                    }
                    else
                    {
                        tableSessionsToBeAdded.Clear();
                        currentSetStartTime = orderedTableSessions.First().StartTime;
                        currentSetEndTime = orderedTableSessions.First().EndTime;
                    }
                }
                else
                {
                    currentSetEndTime = orderedTableSessions
                        .Where(t => t.StartTime <= currentSetEndTime)
                        .Max(t => t.EndTime);
                }
            }
            
            return allSessions;
        }

        public IList<TableSession> OrderTableSessions(ICollection<TableSession> tableSessions)
        {
            var orderedTableSessions = tableSessions.OrderBy(t => t.StartTime).ThenBy(t => t.EndTime).ToList();

            return orderedTableSessions;
        }
    }
}
