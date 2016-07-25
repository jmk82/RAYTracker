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
            FetchedDataParser fp = new FetchedDataParser(new DataFetcher("FIDdyY0gA7OCt7FM3GmgcPAAIJDgsNB4"));

            Debug.WriteLine(fp.GetFetchedDataLines().Count);
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

        [TestMethod]
        public void ParseTableSessionsTest()
        {
            FetchedDataParser fp = new FetchedDataParser();

            var rows =
                "[\r\n[\'-\',\'Ok\'],\r\n[\'sessioncode\',\'3959627519\'],\r\n[\'awardpoints\',\'0\'],\r\n[\'statuspoints\',\'0\'],\r\n[\'tablename\',\'(M) TURBO panda, 483256216\'],\r\n[\'startdate\',\'2016-07-24 23:36:21\'],\r\n[\'duration\',\'01:35\'],\r\n[\'gamecount\',\'105\'],\r\n[\'bets\',\'€27.20\'],\r\n[\'wins\',\'€10.54\'],\r\n[\'chipsin\',\'€26.66\'],\r\n[\'chipsout\',\'€10.00\'],\r\n[\'currencycode\',\'EUR\'],\r\n[\'roomcode\',\'483256216\'],\r\n[\'gametype\',\'Holdem NL €0.05/€0.1\'],\r\n[\'ipoints\',\'0\'],\r\n[\'-\',\'ROWBREAK\'],\r\n[\'sessioncode\',\'3959627607\'],\r\n[\'awardpoints\',\'0\'],\r\n[\'statuspoints\',\'0\'],\r\n[\'tablename\',\'(M) Bangkok, 483254657\'],\r\n[\'startdate\',\'2016-07-24 23:38:49\'],\r\n[\'duration\',\'01:30\'],\r\n[\'gamecount\',\'140\'],\r\n[\'bets\',\'€31.57\'],\r\n[\'wins\',\'€34.95\'],\r\n[\'chipsin\',\'€10.05\'],\r\n[\'chipsout\',\'€13.43\'],\r\n[\'currencycode\',\'EUR\'],\r\n[\'roomcode\',\'483254657\'],\r\n[\'gametype\',\'Holdem NL €0.05/€0.1\'],\r\n[\'ipoints\',\'0\'],\r\n[\'-\',\'ROWBREAK\'],\r\n[\'sessioncode\',\'3959627797\'],\r\n[\'awardpoints\',\'0\'],\r\n[\'statuspoints\',\'0\'],\r\n[\'tablename\',\'(M) TURBO kärppä, 483254289\'],\r\n[\'startdate\',\'2016-07-25 00:03:12\'],\r\n[\'duration\',\'01:05\'],\r\n[\'gamecount\',\'96\'],\r\n[\'bets\',\'€51.49\'],\r\n[\'wins\',\'€60.17\'],\r\n[\'chipsin\',\'€11.02\'],\r\n[\'chipsout\',\'€19.70\'],\r\n[\'currencycode\',\'EUR\'],\r\n[\'roomcode\',\'483254289\'],\r\n[\'gametype\',\'Holdem NL €0.05/€0.1\'],\r\n[\'ipoints\',\'0\'],\r\n[\'-\',\'ROWBREAK\'],\r\n[\'sessioncode\',\'3959627793\'],\r\n[\'awardpoints\',\'0\'],\r\n[\'statuspoints\',\'0\'],\r\n[\'tablename\',\'ANTE - Hakala, 482086191\'],\r\n[\'startdate\',\'2016-07-25 00:06:40\'],\r\n[\'duration\',\'01:01\'],\r\n[\'gamecount\',\'80\'],\r\n[\'bets\',\'€24.43\'],\r\n[\'wins\',\'€32.49\'],\r\n[\'chipsin\',\'€16.36\'],\r\n[\'chipsout\',\'€24.42\'],\r\n[\'currencycode\',\'EUR\'],\r\n[\'roomcode\',\'482086191\'],\r\n[\'gametype\',\'Holdem NL €0.05/€0.1\'],\r\n[\'ipoints\',\'0\'],\r\n[\'-\',\'ROWBREAK\'],";
            var rowsAsList = rows.Split('\n').ToList();

            var tableSessions = fp.ParseTableSessions(rowsAsList);

            Debug.WriteLine("Rows: " + rowsAsList.Count);
            Debug.WriteLine("Table sessions: " + tableSessions.Count);

            foreach (var session in tableSessions)
            {
                Debug.WriteLine(session.TableName);
            }
        }

        [TestMethod]
        public void ParseFetchedDataToTableSessionsTest()
        {
            FetchedDataParser fp = new FetchedDataParser(new DataFetcher("FIDdyY0gA7OCt7FM3GmgcPAAIJDgsNB4"));

            var rows = fp.GetFetchedDataLines();

            var tableSessions = fp.ParseTableSessions(rows);

            Debug.WriteLine("Rows: " + rows.Count);
            Debug.WriteLine("Table sessions: " + tableSessions.Count);

            foreach (var session in tableSessions)
            {
                Debug.WriteLine(session.TableName);
            }
        }
    }
}
