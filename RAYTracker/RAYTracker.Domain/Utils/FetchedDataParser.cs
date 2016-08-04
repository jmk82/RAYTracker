using System;
using System.Collections.Generic;
using RAYTracker.Domain.Model;

namespace RAYTracker.Domain.Utils
{
    public class FetchedDataParser
    {
        private readonly Reader _reader;

        public FetchedDataParser()
        {
        }

        public FetchedDataParser(DataFetcher fetcher)
        {
            _reader = new Reader(fetcher.GetFetchedStreamReader());
        }

        public FetchedDataParser(DataFetcher fetcher, bool tournamentParser)
        {
            _reader = new Reader(fetcher.GetFetchedTournamentStreamReader());
        }

        public IList<string> GetFetchedDataLines()
        {
            var lines = _reader.GetAllLinesAsStrings();

            return lines;
        }

        public IList<Session> ParseTableSessions(IList<string> rows)
        {
            // Poistetaan kaksi ensimmäistä riviä, joiden jälkeen alkaa istuntodata 16 riviä per istunto
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            // Serveriltä tuleva virheilmoitus on alunperin kolmen rivin mittainen
            if (rows.Count == 1)
            {
                throw new UnauthorizedAccessException("Ongelma haettaessa tietoja palvelimelta. Palvelimen viesti:\n\"" + rows[0].Split('\'')[3] +"\"");
            }

            IList<Session> tableSessions = new List<Session>();
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

        public Session ParseTableSession(IList<string> rows)
        {
            var tableSession = new Session();
            string[] rowDatas = new string[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                rowDatas[i] = GetDataFromRow(rows[i]);
            }

            tableSession.TableName = rowDatas[3];
            tableSession.StartTime = Convert.ToDateTime(rowDatas[4]);
            tableSession.SessionDuration = DataConverter.ParseDuration(rowDatas[5]);
            tableSession.EndTime = tableSession.StartTime + tableSession.SessionDuration;
            tableSession.HandsPlayed = int.Parse(rowDatas[6]);
            tableSession.GameType = DataConverter.AssignGameType(rowDatas[13], rowDatas[3]);
            tableSession.TotalBetsMade = DataConverter.ParseCurrency(rowDatas[7]);
            tableSession.TotalWonAmount = DataConverter.ParseCurrency(rowDatas[8]);
            tableSession.ChipsBought = DataConverter.ParseCurrency(rowDatas[9]);
            tableSession.ChipsCashedOut = DataConverter.ParseCurrency(rowDatas[10]);
            tableSession.Result = tableSession.ChipsCashedOut - tableSession.ChipsBought;

            return tableSession;
        }

        public string GetDataFromRow(string row)
        {
            var tokens = row.Split('\'');

            return tokens[3].Trim();
        }

        public IList<Tournament> ParseTournaments(IList<string> rows)
        {
            // Poistetaan kaksi ensimmäistä riviä, joiden jälkeen alkaa turnausdata 13 riviä per turnaus
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            // Serveriltä tuleva virheilmoitus on alunperin kolmen rivin mittainen
            if (rows.Count == 1)
            {
                throw new UnauthorizedAccessException("Ongelma haettaessa tietoja palvelimelta. Palvelimen viesti:\n\"" + rows[0].Split('\'')[3] + "\"");
            }

            IList<Tournament> tournaments = new List<Tournament>();

            var tournamentRows = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                if (i > 0 && i % 13 == 0)
                {
                    tournaments.Add(ParseTournament(tournamentRows));
                    tournamentRows.Clear();
                }

                tournamentRows.Add(rows[i]);
            }

            tournaments.Add(ParseTournament(tournamentRows));

            return tournaments;
        }

        public Tournament ParseTournament(IList<string> rows)
        {
            var tournament = new Tournament();
            string[] rowDatas = new string[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                rowDatas[i] = GetDataFromRow(rows[i]);
            }

            tournament.Name = rowDatas[1];
            tournament.FinalPosition = int.Parse(rowDatas[2]);
            tournament.Type = rowDatas[3];
            tournament.StartTime = Convert.ToDateTime(rowDatas[4]);
            tournament.TotalBuyIn = DataConverter.ParseCurrency(rowDatas[7]);
            tournament.BuyIn = rowDatas[8];
            tournament.Win = DataConverter.ParseCurrency(rowDatas[7]);

            return tournament;
        }
    }
}
