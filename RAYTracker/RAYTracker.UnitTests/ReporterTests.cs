using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker.Domain.Report;
using RAYTracker.Domain.Model;
using System.Linq;
using System.Diagnostics;

namespace RAYTracker.UnitTests
{
    [TestClass]
    public class ReporterTests
    {
        [TestMethod]
        public void TestCumulativeResults()
        {
            var sessions = new Session[]
            {
                new Session { Result = 10M },
                new Session { Result = 20M },
                new Session { Result = -15M },
                new Session { Result = 10M }
            };

            var expectedResult = new decimal[]
            {
                10M, 30M, 15M, 25M
            };

            var cumulativeResults = Reporter.CumulativeResults(sessions);

            CollectionAssert.AreEqual(expectedResult, cumulativeResults);
        }

        [TestMethod]
        public void TestCumulativeSessionResults()
        {
            var sessions = new Session[]
            {
                new Session { Result = 10M },
                new Session { Result = 20M },
                new Session { Result = -5M },
                new Session { Result = 10M }
            };

            var expectedResult = new decimal[]
            {
                10M, 30M, 25M, 35M
            };

            var cumulativeSessionResults = new Reporter.CumulativeSessionResults(sessions);

            for (int i = 0; i < expectedResult.Length; i++)
            {
                Debug.WriteLine("expected: {0}, result: {1}", expectedResult[i], cumulativeSessionResults.CumulativeResults[i]);
            }

            CollectionAssert.AreEqual(expectedResult, cumulativeSessionResults.CumulativeResults);
        }

        [TestMethod]
        public void TestFindHighPoints()
        {
            var sessions = new Session[]
            {
                new Session { Result = 10M },
                new Session { Result = 20M },
                new Session { Result = -5M },
                new Session { Result = 10M },
                new Session { Result = 0M },
                new Session { Result = -5M },
                new Session { Result = 10M },
                new Session { Result = -5M }
            };

            var expected = new int[]
            {
                0, 1, 3, 6
            };

            var result = new Reporter.CumulativeSessionResults(sessions).HighPointIndexes;

            for (int i = 0; i < expected.Length; i++)
            {
                Debug.WriteLine("expected: {0}, result: {1}", expected[i], result[i]);
                Debug.WriteLine(expected[i] == result[i]);
            }

            Assert.AreEqual(expected.Length, result.Length);
            CollectionAssert.AreEqual(expected, result);
        }
    }
}
