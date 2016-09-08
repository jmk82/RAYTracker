using System.Net.Http;
using System.Threading.Tasks;

namespace RAYTracker.Domain.Utils
{
    public class DataFetcher
    {
        private const string BaseCashGameUrl = "https://cashier.pt.ray.fi/payments/CashierProxy.php?clienttype=poker&casino=ray&realmode=1&UseHTMLLayout=1&htmlstyle=2&script=%2Fpoker_sessions.php";
        private const string BaseTournamentUrl = "https://cashier.pt.ray.fi/payments/CashierProxy.php?clienttype=poker&casino=ray&realmode=1&UseHTMLLayout=1&htmlstyle=2&script=%2Fpoker_tournaments.php";

        public string Wcusersessionid { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public DataFetcher() {}

        public DataFetcher(string sessionId)
        {
            Wcusersessionid = sessionId;
        }

        private string GenerateCashGameUrl()
        {
            var url = BaseCashGameUrl + "&wcusersessionid=" + Wcusersessionid;
            url = AddDatesToUrl(url);
            return url;
        }

        private string GenerateTournamentUrl()
        {
            var url = BaseTournamentUrl + "&wcusersessionid=" + Wcusersessionid;
            url = AddDatesToUrl(url);
            return url;
        }

        private string AddDatesToUrl(string url)
        {
            if (StartDate != null)
            {
                url += "&startdate=" + StartDate;
            }

            if (EndDate != null)
            {
                url += "&enddate=" + EndDate;
            }
            return url;
        }

        public async Task<string> GetCashSessionsAsync(string url = null)
        {
            var client = new HttpClient();
            url = string.IsNullOrEmpty(url) ? GenerateCashGameUrl() : url;
            var data = await client.GetStringAsync(url);
            return data;
        }

        public async Task<string> GetTournamentsAsync(string url = null)
        {
            var client = new HttpClient();
            url = string.IsNullOrEmpty(url) ? GenerateTournamentUrl() : url;
            var data = await client.GetStringAsync(url);
            return data;
        }
    }
}
