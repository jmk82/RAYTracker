using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAYTracker;
using System.Diagnostics;

namespace RAYTrackerTests
{
    [TestClass]
    public class ParserTests
    {
        private string testRow =
            "(M) Hongkong, 483253799	21-07-2016 18:53	03:08	Holdem NL €0.1/€0.2	€119.36	€126.96	293	€23.14	€30.74";

        [TestMethod]
        public void ParseLineToTrimmedTokensTest()
        {
            FileParser fileParser = new FileParser();
            var tokens = fileParser.ParseLine(testRow);

            Assert.AreEqual("(M) Hongkong, 483253799", tokens[0]);
            Assert.AreEqual("21-07-2016 18:53", tokens[1]);
            Assert.AreEqual("03:08", tokens[2]);
            Assert.AreEqual("Holdem NL €0.1/€0.2", tokens[3]);
            Assert.AreEqual("€119.36", tokens[4]);
            Assert.AreEqual("€126.96", tokens[5]);
            Assert.AreEqual("293", tokens[6]);
            Assert.AreEqual("€23.14", tokens[7]);
            Assert.AreEqual("€30.74", tokens[8]);
        }

        [TestMethod]
        public void CreateSessionFromTokensTest()
        {
            FileParser fileParser = new FileParser();

            var tokens = fileParser.ParseLine(testRow);

            for (int i = 0; i < tokens.Length; i++)
            {
                Debug.WriteLine("token[" + i + "]: [" + tokens[i] + "]");
            }

            var session = fileParser.CreateTableSession(tokens);

            Assert.AreEqual("(M) Hongkong, 483253799", session.TableName);
            Assert.AreEqual(new DateTime(2016, 7, 21, 18, 53, 0), session.StartTime);
            Assert.AreEqual(new TimeSpan(0, 188, 0), session.SessionDuration);
            Assert.AreEqual("Holdem NL €0.1/€0.2", session.GameType);
            Assert.AreEqual(119.36m, session.TotalBetsMade);
            Assert.AreEqual(126.96m, session.TotalWonAmount);
            Assert.AreEqual(293, session.HandsPlayed);
            Assert.AreEqual(23.14m, session.ChipsBought);
            Assert.AreEqual(30.74m, session.ChipsCashedOut);
        }
    }
}
