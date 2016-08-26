using RAYTracker.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RAYTracker.Domain.Utils
{
    public class DataParser
    {
        private string _data;

        public DataParser(string data)
        {
            _data = data;
        }

        public IList<Session> ParseSessions()
        {
            IList<string> rows = _data.Split('\n').ToList();

            // Poistetaan mahdolliset tyhjät rivit
            rows = RemoveEmptyRows(rows);

            // Serveriltä tuleva virheilmoitus on alunperin kolmen rivin mittainen
            if (rows.Count == 2 || rows.Count == 3)
            {
                throw new UnauthorizedAccessException("Ongelma haettaessa tietoja palvelimelta. Palvelimen viesti:\n\"" + _data);
            }//rows[0].Split('\'')[3] + "\""

            // Poistetaan kaksi ensimmäistä riviä, joiden jälkeen alkaa istuntodata 16 riviä per istunto
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            IList<Session> sessions = new List<Session>();
            var sessionRows = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                if (i > 0 && i%16 == 0)
                {
                    sessions.Add(ParseSession(sessionRows));
                    sessionRows.Clear();
                }

                sessionRows.Add(rows[i]);
            }

            sessions.Add(ParseSession(sessionRows));

            return sessions;
        }

        private static IList<string> RemoveEmptyRows(IList<string> rows)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i] == "")
                {
                    rows.RemoveAt(i);
                    i--;
                }
            }

            return rows;
        }

        public Session ParseSession(IList<string> rows)
        {
            var session = new Session();
            string[] rowDatas = new string[rows.Count];

            for (int i = 0; i < rows.Count; i++)
            {
                rowDatas[i] = GetDataFromRow(rows[i]);
            }

            session.TableName = rowDatas[3];
            session.StartTime = Convert.ToDateTime(rowDatas[4]);
            session.Duration = DataConverter.ParseDuration(rowDatas[5]);
            session.EndTime = session.StartTime + session.Duration;
            session.HandsPlayed = int.Parse(rowDatas[6]);
            session.GameType = DataConverter.AssignGameType(rowDatas[13], rowDatas[3]);
            session.TotalBetsMade = DataConverter.ParseCurrency(rowDatas[7]);
            session.TotalWonAmount = DataConverter.ParseCurrency(rowDatas[8]);
            session.ChipsBought = DataConverter.ParseCurrency(rowDatas[9]);
            session.ChipsCashedOut = DataConverter.ParseCurrency(rowDatas[10]);
            session.Result = session.ChipsCashedOut - session.ChipsBought;

            return session;
        }

        public string GetDataFromRow(string row)
        {
            var tokens = row.Split('\'');

            return tokens[3].Trim();
        }

        public IList<Tournament> ParseTournaments()
        {
            IList<string> rows = _data.Split('\n').ToList();

            // Poistetaan mahdolliset tyhjät rivit
            rows = RemoveEmptyRows(rows);

            // Poistetaan kaksi ensimmäistä riviä, joiden jälkeen alkaa turnausdata 13 riviä per turnaus
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            // Serveriltä tuleva virheilmoitus on alunperin kolmen rivin mittainen
            if (rows.Count == 1 || rows.Count == 0)
            {
                throw new UnauthorizedAccessException("Ongelma haettaessa tietoja palvelimelta. Palvelimen viesti:\n\"" + rows[0].Split('\'')[3] + "\"");
            }

            IList<Tournament> tournaments = new List<Tournament>();

            var tournamentRows = new List<string>();

            for (int i = 0; i < rows.Count; i++)
            {
                if (i > 0 && i % 14 == 0)
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

            for (int i = 0; i < rowDatas.Length; i++)
            {
                Debug.WriteLine("[" + i + "]: " + rowDatas[i]);
            }

            tournament.Id = rowDatas[0];
            tournament.Name = rowDatas[1];
            tournament.FinalPosition = int.Parse(rowDatas[2]);
            tournament.Type = rowDatas[3];
            tournament.StartTime = Convert.ToDateTime(rowDatas[4]);
            tournament.TotalBuyIn = DataConverter.ParseCurrency(rowDatas[8]);
            tournament.BuyIn = rowDatas[9];
            tournament.Win = DataConverter.ParseCurrency(rowDatas[11]);

            return tournament;
        }
    }
}
