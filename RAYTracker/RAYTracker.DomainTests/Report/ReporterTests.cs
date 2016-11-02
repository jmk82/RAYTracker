using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker.Domain.Model;
using RAYTracker.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Report.Tests
{
    [TestClass()]
    public class ReporterTests
    {
        [TestMethod()]
        public void FindLargestDropTest1()
        {
            var endTime = new DateTime(2016, 1, 1);
            Session[] sessions = new Session[]
            {
                new Session { Result = 10M, EndTime = new DateTime(2016, 1, 1) },
                new Session { Result = 20M, EndTime = new DateTime(2016, 1, 2) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 3) },
                new Session { Result = -5M, EndTime = new DateTime(2016, 1, 4) },
                new Session { Result = 10M, EndTime = new DateTime(2016, 1, 5) },
                new Session { Result = 30M, EndTime = new DateTime(2016, 1, 6) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 7) },
                new Session { Result = -15M, EndTime = new DateTime(2016, 1, 8) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 9) },
                new Session { Result = 60M, EndTime = new DateTime(2016, 1, 10) }
            };

            var result = Reporter.FindLargestDrop(sessions);

            Assert.AreEqual(35, result);
        }

        [TestMethod()]
        public void FindLargestDropTest2()
        {
            var endTime = new DateTime(2016, 1, 1);
            Session[] sessions = new Session[]
            {
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 1) },
                new Session { Result = -20M, EndTime = new DateTime(2016, 1, 2) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 3) },
                new Session { Result = -5M, EndTime = new DateTime(2016, 1, 4) },
                new Session { Result = 10M, EndTime = new DateTime(2016, 1, 5) },
                new Session { Result = 30M, EndTime = new DateTime(2016, 1, 6) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 7) },
                new Session { Result = -15M, EndTime = new DateTime(2016, 1, 8) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 9) },
                new Session { Result = 60M, EndTime = new DateTime(2016, 1, 10) }
            };

            var result = Reporter.FindLargestDrop(sessions);

            Assert.AreEqual(45, result);
        }

        [TestMethod()]
        public void FindLargestDropTest3()
        {
            var endTime = new DateTime(2016, 1, 1);
            Session[] sessions = new Session[]
            {
                new Session { Result = 10M, EndTime = new DateTime(2016, 1, 1) },
                new Session { Result = 20M, EndTime = new DateTime(2016, 1, 2) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 3) },
                new Session { Result = -5M, EndTime = new DateTime(2016, 1, 4) },
                new Session { Result = 10M, EndTime = new DateTime(2016, 1, 5) },
                new Session { Result = 30M, EndTime = new DateTime(2016, 1, 6) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 7) },
                new Session { Result = -15M, EndTime = new DateTime(2016, 1, 8) },
                new Session { Result = -10M, EndTime = new DateTime(2016, 1, 9) },
                new Session { Result = -60M, EndTime = new DateTime(2016, 1, 10) }
            };

            var result = Reporter.FindLargestDrop(sessions);

            Assert.AreEqual(95, result);
        }
    }
}