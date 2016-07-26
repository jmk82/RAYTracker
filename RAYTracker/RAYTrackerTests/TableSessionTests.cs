using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;
using System;
using RAYTracker.Model;

namespace RAYTrackerTests
{
    [TestClass]
    public class TableSessionTests
    {
        [TestMethod]
        public void TestCreateTableSession()
        {
            TableSession session = new TableSession();
            session.StartTime = new DateTime(2016, 7, 1, 10, 0, 0);
            session.EndTime = new DateTime(2016, 7, 1, 11, 0, 0);

            var expectedHour = 11;

            Assert.IsNotNull(session);
            Assert.AreEqual(expectedHour, session.EndTime.Hour);
        }
    }
}
