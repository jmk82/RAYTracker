using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;

namespace RAYTrackerTests
{
    [TestClass]
    public class ReporterTests
    {
        [TestMethod]
        public void DailyReportTest()
        {
            var fileName = CreateAndGetTestDataFile();
            var program = new Program();
            program.Filename = fileName;
            program.ImportFromFile();

            var tableSessions = program.TableSessions;

            var report = tableSessions.GroupBy(t => t.StartTime.Date);

            foreach (var day in report)
            {
                Debug.WriteLine(day.Key.Date + ":  " + day.Sum(s => s.Result) + " €");
            }
        }

        [TestMethod]
        public void GameTypeReportTest()
        {
            var fileName = CreateAndGetTestDataFile();
            var program = new Program();
            program.Filename = fileName;
            program.ImportFromFile();

            var tableSessions = program.TableSessions;

            var report = tableSessions.GroupBy(t => t.GameType);

            foreach (var day in report)
            {
                Debug.WriteLine(day.Key + ":  " + day.Sum(s => s.Result) + " €");

                //foreach (var sess in day)
                //{
                //    Debug.WriteLine(" " + sess.Result);
                //}
            }
        }

        private string CreateAndGetTestDataFile()
        {
            var sessionData =
                "(M) Johannesburg, 483254687\t22-07-2016 23:18\t03:14\tHoldem NL €0.1/€0.2\t€125.53\t€112.63\t301\t€54.47\t€41.57\r\n(M) Jakutsk, 483254114\t23-07-2016 00:01\t02:31\tHoldem NL €0.1/€0.2\t€248.60\t€299.33\t246\t€21.60\t€72.33\r\nANTE - Haapamäki, 482085661\t22-07-2016 21:54\t04:37\tHoldem NL €0.1/€0.2\t€290.21\t€374.12\t366\t€21.96\t€105.87\r\n(M) TURBO jänis, 483253872\t23-07-2016 00:46\t01:28\tHoldem NL €0.05/€0.1\t€54.50\t€56.06\t138\t€14.62\t€16.18\r\n(M) TURBO orava, 483255571\t23-07-2016 01:41\t00:29\tHoldem NL €0.05/€0.1\t€21.03\t€3.99\t37\t€27.33\t€10.29";
            var currentDirectory = Directory.GetCurrentDirectory();
            var fileName = currentDirectory + "\\testdata.txt";
            File.WriteAllText(fileName, sessionData, Encoding.UTF8);

            return fileName;
        }
    }
}
