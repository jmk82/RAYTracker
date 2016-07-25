using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace RAYTracker
{
    public class FetchedDataParser
    {
        private Reader _reader;
        private Parser _parser;

        public FetchedDataParser()
        {
            DataFetcher fetcher = new DataFetcher();
            _parser = new Parser();
            _reader = new Reader(fetcher.GetFetchedStreamReader());
        }

        public IList<string> GetFetchedDataLines()
        {
            var lines = _reader.GetAllLinesAsStrings();

            lines.RemoveAt(0);
            lines.RemoveAt(0);

            return lines;
        }

        public string[] ParseTableSessions()
        {
            var lines = GetFetchedDataLines();
            IList<TableSession> tableSessions = new List<TableSession>();
            var tableSessionRows = new List<string>();

            for (int i = 0; i < 16; i++)
            {
                tableSessionRows.Add(lines[i]);
            }

            tableSessions.Add(ParseTableSession(tableSessionRows));

            return new string[1];
        }

        public TableSession ParseTableSession(IList<string> rows)
        {
            var tableSession = new TableSession();
            string[] rowDatas = new string[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                rowDatas[i] = GetDataFromRow(rows[i]);
            }

            tableSession.TableName = rowDatas[3];
            tableSession.StartTime = Convert.ToDateTime(rowDatas[4]);
            tableSession.SessionDuration = _parser.ParseDuration(rowDatas[5]);
            tableSession.EndTime = tableSession.StartTime + tableSession.SessionDuration;
            tableSession.GameType = rowDatas[13];
            tableSession.TotalBetsMade = _parser.ParseCurrency(rowDatas[7]);
            tableSession.TotalWonAmount = _parser.ParseCurrency(rowDatas[8]);
            tableSession.ChipsBought = _parser.ParseCurrency(rowDatas[9]);
            tableSession.ChipsCashedOut = _parser.ParseCurrency(rowDatas[10]);
            tableSession.Result = tableSession.ChipsCashedOut - tableSession.ChipsBought;

            return tableSession;
        }

        public string GetDataFromRow(string row)
        {
            var tokens = row.Split('\'');

            return tokens[3].Trim();
        }

        //public TableSession CreateTableSession(string[] tokens)
        //{
        //    TableSession session = new TableSession();

        //    session.TableName = tokens[0];
        //    session.StartTime = Convert.ToDateTime(tokens[1]);
        //    session.SessionDuration = ParseDuration(tokens[2]);
        //    session.EndTime = session.StartTime + session.SessionDuration;
        //    session.GameType = tokens[3];
        //    session.TotalBetsMade = ParseCurrency(tokens[4]);
        //    session.TotalWonAmount = ParseCurrency(tokens[5]);
        //    session.HandsPlayed = Int32.Parse(tokens[6]);
        //    session.ChipsBought = ParseCurrency(tokens[7]);
        //    session.ChipsCashedOut = ParseCurrency(tokens[8]);
        //    session.Result = session.ChipsCashedOut - session.ChipsBought;

        //    return session;
        //}
    }
}
