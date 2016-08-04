using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAYTracker.Domain;

namespace RAYTrackerTests
{
    [TestClass()]
    public class ProgramTests
    {
        private string _filename;

        [TestInitialize]
        public void SetUp()
        {
            var sessionData =
                "(M) Johannesburg, 483254687\t22-07-2016 23:18\t03:14\tHoldem NL €0.1/€0.2\t€125.53\t€\n112.63\t301\t€54.47\t€41.57\r\n(M) Jakutsk, 483254114\t23-07-2016 00:01\t02:31\tHoldem NL €0.1/€0.2\t€248.60\t€299.33\t246\t€21.60\t€72.33\r\nANTE - Haapamäki, 482085661\t22-07-2016 21:54\t04:37\tHoldem NL €0.1/€0.2\t€290.21\t€374.12\t366\t€21.96\t€105.87\r\n(M) TURBO jänis, 483253872\t23-07-2016 00:46\t01:28\tHoldem NL €0.05/€0.1\t€54.50\t€56.06\t138\t€14.62\t€16.18\r\n(M) TURBO orava, 483255571\t23-07-2016 01:41\t00:29\tHoldem NL €0.05/€0.1\t€21.03\t€3.99\t37\t€27.33\t€10.29";
            var currentDirectory = Directory.GetCurrentDirectory();
            _filename = currentDirectory + "\\testdata.txt";
            File.WriteAllText(_filename, sessionData, Encoding.UTF8);
        }

        [TestMethod()]
        public void ImportFromFileTest()
        {
            var program = new Program();
            program.Filename = _filename;
            program.ImportFromFile();

            Debug.WriteLine("Imported " + program.Sessions.Count + " sessions");

            //Assert.AreEqual(new DateTime(2016, 7, 22, 23, 18, 0), program.PlayingSessions[0].StartTime);
            Assert.AreEqual(72.33m, program.Sessions[0].ChipsCashedOut);
        }
    }
}