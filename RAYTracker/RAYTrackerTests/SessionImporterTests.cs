using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using RAYTracker.Domain;
using RAYTracker.Domain.Model;

namespace RAYTrackerTests
{
    [TestClass]
    public class SessionImporterTests
    {
        [TestMethod]
        public void TestOrderTableSessions1()
        {
            // Arrange
            IList<Session> sessions = new Collection<Session>();

            Session session1 = new Session();
            session1.StartTime = new DateTime(2016, 7, 1, 10, 0, 0);
            session1.EndTime = new DateTime(2016, 7, 1, 11, 0, 0);

            Session session2 = new Session();
            session2.StartTime = new DateTime(2016, 7, 1, 11, 0, 0);
            session2.EndTime = new DateTime(2016, 7, 1, 12, 0, 0);

            Session session3 = new Session();
            session3.StartTime = new DateTime(2016, 7, 1, 10, 0, 0);
            session3.EndTime = new DateTime(2016, 7, 1, 12, 0, 0);

            sessions.Add(session1);
            sessions.Add(session2);
            sessions.Add(session3);

            // Act

            var orderedSessions = PlayingSession.GroupToPlayingSessions(sessions);

            // Assert
            var expectedStartHour1 = 10;
            var expectedStartHour2 = 10;
            var expectedStartHour3 = 11;

            var expectedEndHour1 = 11;
            var expectedEndHour2 = 12;
            var expectedEndHour3 = 12;

            Assert.AreEqual(expectedStartHour1, orderedSessions.First().StartTime.Hour);
            Assert.AreEqual(expectedStartHour2, orderedSessions.Skip(1).First().StartTime.Hour);
            Assert.AreEqual(expectedStartHour3, orderedSessions.Skip(2).First().StartTime.Hour);

            Assert.AreEqual(expectedEndHour1, orderedSessions.First().EndTime.Hour);
            Assert.AreEqual(expectedEndHour2, orderedSessions.Skip(1).First().EndTime.Hour);
            Assert.AreEqual(expectedEndHour3, orderedSessions.Skip(2).First().EndTime.Hour);
        }

        [TestMethod]
        public void TestSessionOrderingWithRandomTableSessionAndPrintOutput()
        {
            IList<Session> sessions = new Collection<Session>();

            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                var randomHour = random.Next(0, 24);
                var randomMinute = random.Next(0, 60);
                var randomDuration = new TimeSpan(0, random.Next(180), 0);

                var tableSession = new Session();
                tableSession.StartTime = new DateTime(2016, 7, 1, randomHour, randomMinute, 0);
                tableSession.EndTime = tableSession.StartTime + randomDuration;

                sessions.Add(tableSession);
            }

            foreach (var session in sessions)
            {
                Debug.WriteLine(session.ToString());
            }

            Debug.WriteLine("----- After ordering: -----");
            var orderedSessions = PlayingSession.GroupToPlayingSessions(sessions);

            foreach (var session in orderedSessions)
            {
                Debug.WriteLine(session.ToString());
            }
        }

        [TestMethod()]
        public void CreateSessionsTest1()
        {
            // Arrange
            IList<Session> sessions = new Collection<Session>();

            Session session1 = new Session();
            session1.StartTime = new DateTime(2016, 7, 1, 10, 0, 0);
            session1.EndTime = new DateTime(2016, 7, 1, 11, 0, 0);

            Session session2 = new Session();
            session2.StartTime = new DateTime(2016, 7, 1, 10, 30, 0);
            session2.EndTime = new DateTime(2016, 7, 1, 12, 0, 0);

            Session session3 = new Session();
            session3.StartTime = new DateTime(2016, 7, 1, 11, 0, 0);
            session3.EndTime = new DateTime(2016, 7, 1, 12, 30, 0);

            sessions.Add(session1);
            sessions.Add(session2);
            sessions.Add(session3);

            var created = PlayingSession.GroupToPlayingSessions(sessions);

            foreach (var session in created)
            {
                Debug.WriteLine(session.ToString());
            }

            Assert.AreEqual(1, created.Count);
        }

        [TestMethod()]
        public void CreateSessionsTest2()
        {
            // Arrange
            IList<Session> sessions = new Collection<Session>();

            Session session1 = new Session();
            session1.StartTime = new DateTime(2016, 7, 1, 10, 0, 0);
            session1.EndTime = new DateTime(2016, 7, 1, 11, 0, 0);

            Session session2 = new Session();
            session2.StartTime = new DateTime(2016, 7, 1, 10, 30, 0);
            session2.EndTime = new DateTime(2016, 7, 1, 12, 0, 0);

            Session session3 = new Session();
            session3.StartTime = new DateTime(2016, 7, 1, 11, 0, 0);
            session3.EndTime = new DateTime(2016, 7, 1, 12, 30, 0);

            Session session4 = new Session();
            session4.StartTime = new DateTime(2016, 7, 1, 13, 0, 0);
            session4.EndTime = new DateTime(2016, 7, 1, 14, 0, 0);

            Session session5 = new Session();
            session5.StartTime = new DateTime(2016, 7, 1, 13, 30, 0);
            session5.EndTime = new DateTime(2016, 7, 1, 14, 30, 0);

            sessions.Add(session1);
            sessions.Add(session2);
            sessions.Add(session3);
            sessions.Add(session4);
            sessions.Add(session5);

            var created = PlayingSession.GroupToPlayingSessions(sessions);

            foreach (var session in created)
            {
                Debug.WriteLine(session.ToString());
            }

            Assert.AreEqual(2, created.Count);
        }

        [TestMethod]
        public void AnotherRandomTest()
        {
            IList<Session> sessions = new List<Session>();

            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var randomHour = random.Next(0, 24);
                var randomMinute = random.Next(0, 60);
                var randomDuration = new TimeSpan(0, random.Next(120), 0);

                var tableSession = new Session();
                tableSession.StartTime = new DateTime(2016, 1, 1, randomHour, randomMinute, 0);
                tableSession.EndTime = tableSession.StartTime + randomDuration;

                sessions.Add(tableSession);
            }

            var created = PlayingSession.GroupToPlayingSessions(sessions);

            Debug.WriteLine("Total sessions: " + created.Count);

            //foreach (var session in generator.OrderTableSessions(sessions))
            //{
            //    Debug.WriteLine(session.ToString());
            //}

            //Debug.WriteLine("-----------------------------");

            //foreach (var session in created)
            //{
            //    Debug.WriteLine(session.ToString());
            //}
        }

    }
}