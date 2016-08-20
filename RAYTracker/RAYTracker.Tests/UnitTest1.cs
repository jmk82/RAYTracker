using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker.Domain.Utils;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RAYTracker.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var dataFetcher = new DataFetcher("MtRnmHbmZYri17FMLFlAcCCgEKCAgDBI");

            var data = await dataFetcher.GetCashSessionsAsync();

            Debug.WriteLine(data);
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var dataFetcher = new DataFetcher("MtRnmHbmZYri17FMLFlAcCCgEKCAgDBI");

            var data = await dataFetcher.GetTournamentsAsync();
            var rows = data.Split('\n');

            Debug.WriteLine(rows.Length);
        }
    }
}
