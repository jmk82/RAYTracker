using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;
using System;
using RAYTracker.Domain;
using RAYTracker.Domain.Model;

namespace RAYTrackerTests
{
    [TestClass]
    public class TableSessionTests
    {
        [TestMethod]
        public void TestCreateTableSession()
        {
            Session session = new Session();
            session.StartTime = new DateTime(2016, 7, 1, 10, 0, 0);
            session.EndTime = new DateTime(2016, 7, 1, 11, 0, 0);

            var expectedHour = 11;

            Assert.IsNotNull(session);
            Assert.AreEqual(expectedHour, session.EndTime.Hour);
        }
    }
}
