using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace RAYTracker.Domain.Model
{
    public class Session : IEquatable<Session>
    {
        public string TableName { get; set; }
        public DateTime StartTime { get; set; }

        // T‰llainen ratkaisu koska TimeSpanin serialisointi ei XmlSerialiserilla onnistu
        [XmlIgnore]
        public TimeSpan Duration
        {
            get { return XmlConvert.ToTimeSpan(DurationString); }
            set { DurationString = XmlConvert.ToString(value); }
        }
        [Browsable(false)]
        [XmlElement("Duration")]
        public string DurationString { get; set; }

        public DateTime EndTime { get; set; }
        public GameType GameType { get; set; }
        public decimal TotalBetsMade { get; set; }
        public decimal TotalWonAmount { get; set; }
        public int HandsPlayed { get; set; }
        public decimal ChipsBought { get; set; }
        public decimal ChipsCashedOut { get; set; }
        public decimal Result { get; set; }
        public double HandsPerHour => HandsPlayed / Duration.TotalHours;

        public static IList<Session> OrderSessions(ICollection<Session> sessions)
        {
            return sessions.OrderBy(t => t.StartTime).ThenBy(t => t.EndTime).ToList();
        }

        public bool Equals(Session other)
        {
            if (other == null) return false;

            var oneHour = new TimeSpan(1, 0, 0);

            return (this.TableName == other.TableName &&
                   this.StartTime == other.StartTime &&
                   this.EndTime == other.EndTime) ||

                   /* T‰st‰ seuraavat tarkistukset lis‰tty, koska RAY:n serveri palauttaa kaikki ajat siin‰ ajassa (kes‰/talvi),
                   jossa ollaan teht‰ess‰ kysely. Aika siis voi heitt‰‰ tasan tunnin suuntaan tai toiseen riippuen siit‰ koska tiedot haetaan
                   */
                   (this.TableName == other.TableName &&
                   this.StartTime + oneHour == other.StartTime &&
                   this.EndTime + oneHour == other.EndTime &&
                   this.HandsPlayed == other.HandsPlayed &&
                   this.Result == other.Result) ||

                   (this.TableName == other.TableName &&
                   this.StartTime - oneHour == other.StartTime &&
                   this.EndTime - oneHour == other.EndTime &&
                   this.HandsPlayed == other.HandsPlayed &&
                   this.Result == other.Result);
        }
    }
}