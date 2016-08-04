using System;
using System.Collections.Generic;
using System.Linq;

namespace RAYTracker.Domain.Model
{
    public class PlayingSession
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public IList<Session> Sessions { get; set; }
        public decimal Result { get; set; }
        public double MtRatio { get; set; }
        public decimal HourlyRate { get; set; }

        public PlayingSession(DateTime start, DateTime end, IList<Session> sessions)
        {
            StartTime = start;
            EndTime = end;
            Duration = end - start;
            Sessions = sessions;
            Result = Sessions.Sum(s => s.ChipsCashedOut - s.ChipsBought);
            MtRatio = Sessions.Sum(s => s.SessionDuration.TotalMinutes) / Duration.TotalMinutes;
            HourlyRate = (decimal)((double)Result / Duration.TotalHours);
        }

        public static IList<PlayingSession> GroupToPlayingSessions(IList<Session> sessions)
        {
            if (sessions == null || sessions.Count == 0)
            {
                return null;
            }

            var orderedSessions = Session.OrderSessions(sessions);
            IList<PlayingSession> allPlayingSessions = new List<PlayingSession>();
            IList<Session> sessionsToBeAdded = new List<Session>();
            sessionsToBeAdded.Add(orderedSessions.First());

            var currentSessionSetStartTime = orderedSessions.First().StartTime;
            var currentSessionSetEndTime = orderedSessions.First().EndTime;

            while (true)
            {
                sessionsToBeAdded = orderedSessions.Where(t => t.StartTime <= currentSessionSetEndTime).ToList();

                if (sessionsToBeAdded.Max(t => t.EndTime) == currentSessionSetEndTime)
                {
                    allPlayingSessions.Add(new PlayingSession(currentSessionSetStartTime, currentSessionSetEndTime,
                        new List<Session>(sessionsToBeAdded)));

                    foreach (var session in sessionsToBeAdded)
                    {
                        orderedSessions.Remove(session);
                    }

                    if (!orderedSessions.Any())
                    {
                        break;
                    }

                    sessionsToBeAdded.Clear();
                    currentSessionSetStartTime = orderedSessions.First().StartTime;
                    currentSessionSetEndTime = orderedSessions.First().EndTime;
                }
                else
                {
                    currentSessionSetEndTime = orderedSessions
                        .Where(t => t.StartTime <= currentSessionSetEndTime)
                        .Max(t => t.EndTime);
                }
            }

            return allPlayingSessions;
        }
    }
}
