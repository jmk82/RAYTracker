using RAYTracker.Model;
using System;
using System.Collections.Generic;

namespace RAYTracker
{
    public class FetchedDataParser
    {
        private readonly Reader _reader;
        private readonly DataConverter _dataConverter;

        public FetchedDataParser()
        {
            _dataConverter = new DataConverter();
        }

        public FetchedDataParser(DataFetcher fetcher)
        {
            _dataConverter = new DataConverter();
            _reader = new Reader(fetcher.GetFetchedStreamReader());
        }

        public IList<string> GetFetchedDataLines()
        {
            var lines = _reader.GetAllLinesAsStrings();

            return lines;
        }

        public IList<TableSession> ParseTableSessions(IList<string> rows)
        {
            // Poistetaan kaksi ensimmäistä riviä, joiden jälkeen alkaa istuntodata 16 riviä per istunto
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            // Serveriltä tuleva virheilmoitus on alunperin kolmen rivin mittainen
            if (rows.Count == 1)
            {
                throw new UnauthorizedAccessException("Ongelma haettaessa tietoja palvelimelta. Palvelimen viesti:\n\"" + rows[0].Split('\'')[3] +"\"");
            }

            IList<TableSession> tableSessions = new List<TableSession>();
            var tableSessionRows = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                if (i > 0 && i%16 == 0)
                {
                    tableSessions.Add(ParseTableSession(tableSessionRows));
                    tableSessionRows.Clear();
                }

                tableSessionRows.Add(rows[i]);
            }

            tableSessions.Add(ParseTableSession(tableSessionRows));

            return tableSessions;
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
            tableSession.SessionDuration = _dataConverter.ParseDuration(rowDatas[5]);
            tableSession.EndTime = tableSession.StartTime + tableSession.SessionDuration;
            tableSession.HandsPlayed = int.Parse(rowDatas[6]);
            tableSession.GameType = _dataConverter.AssignGameType(rowDatas[13]);
            tableSession.TotalBetsMade = _dataConverter.ParseCurrency(rowDatas[7]);
            tableSession.TotalWonAmount = _dataConverter.ParseCurrency(rowDatas[8]);
            tableSession.ChipsBought = _dataConverter.ParseCurrency(rowDatas[9]);
            tableSession.ChipsCashedOut = _dataConverter.ParseCurrency(rowDatas[10]);
            tableSession.Result = tableSession.ChipsCashedOut - tableSession.ChipsBought;

            return tableSession;
        }

        public string GetDataFromRow(string row)
        {
            var tokens = row.Split('\'');

            return tokens[3].Trim();
        }
    }
}
