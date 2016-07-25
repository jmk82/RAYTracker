using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;
using System.Diagnostics;
using System.Linq;

namespace RAYTrackerTests
{
    [TestClass]
    public class DataFetcherTests
    {
        [TestMethod]
        public void GetDataTest()
        {
            DataFetcher fetcher = new DataFetcher();

            Debug.WriteLine(fetcher.GetData());
        }

        [TestMethod]
        public void PrintSomeLines()
        {
            FetchedDataParser fp = new FetchedDataParser();
            fp.GetFetchedDataLines();
        }

        [TestMethod]
        public void ParseRegularRowTest()
        {
            FetchedDataParser fp = new FetchedDataParser();

            string row = "['tablename','(M) TURBO panda, 483256216'],";

            var result = fp.GetDataFromRow(row);

            Debug.WriteLine(result);

            Assert.AreEqual("(M) TURBO panda, 483256216", result);
        }

        [TestMethod]
        public void ParseTableSessionTest()
        {
            FetchedDataParser fp = new FetchedDataParser();

            var rows = "[\'sessioncode\',\'3959627607\'],\r\n[\'awardpoints\',\'0\'],\r\n[\'statuspoints\',\'0\'],\r\n[\'tablename\',\'(M) Bangkok, 483254657\'],\r\n[\'startdate\',\'2016-07-24 23:38:49\'],\r\n[\'duration\',\'01:30\'],\r\n[\'gamecount\',\'140\'],\r\n[\'bets\',\'€31.57\'],\r\n[\'wins\',\'€34.95\'],\r\n[\'chipsin\',\'€10.05\'],\r\n[\'chipsout\',\'€13.43\'],\r\n[\'currencycode\',\'EUR\'],\r\n[\'roomcode\',\'483254657\'],\r\n[\'gametype\',\'Holdem NL €0.05/€0.1\'],\r\n[\'ipoints\',\'0\'],\r\n[\'-\',\'ROWBREAK\'],";
            var rowsAsList = rows.Split('\n').ToList();

            var tableSession = fp.ParseTableSession(rowsAsList);

            Debug.WriteLine("Rows: " + rowsAsList.Count);
            Debug.WriteLine("Rows(3): " + rowsAsList.ElementAt(3));

            Assert.AreEqual("(M) Bangkok, 483254657", tableSession.TableName);
            Assert.AreEqual(3.38m, tableSession.Result);
        }
    }
}
