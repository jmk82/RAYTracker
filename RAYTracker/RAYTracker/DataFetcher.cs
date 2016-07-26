using System.IO;
using System.Net;

namespace RAYTracker
{
    public class DataFetcher
    {
        private const string BaseUrl = "https://cashier.pt.ray.fi/payments/CashierProxy.php?language=fi&clienttype=poker&casino=ray&servicename=hcash_ray&failaction=1&webcashier=1&skin=ray-ray&realmode=1&preview=0&currency=eur&UseHTMLLayout=1&htmlstyle=2&localtime=20160725041924&script=%2Fpoker_sessions.php";
        public string Wcusersessionid { get; set; }
        public string StartDate { get; set; } = "2016-07-24";
        public string EndDate { get; set; }

        public DataFetcher() {}

        public DataFetcher(string sessionId)
        {
            Wcusersessionid = sessionId;
        }

        public string GenerateUrl()
        {
            var url = BaseUrl + "&wcusersessionid=" + Wcusersessionid + "&startdate=" + StartDate;

            if (EndDate != null)
            {
                url += "&enddate=" + EndDate;
            }

            return url;
        }

        public StreamReader GetFetchedStreamReader()
        {
            var request = WebRequest.Create(GenerateUrl());
            var response = request.GetResponse();

            var dataStream = response.GetResponseStream();
            return new StreamReader(dataStream);
        }
    }
}
