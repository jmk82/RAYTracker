using System;
using System.IO;
using System.Net;

namespace RAYTracker
{
    public class DataFetcher
    {
        //private string baseUrl = "https://cashier.pt.ray.fi/payments/CashierProxy.php?language=fi&clienttype=poker&casino=ray&servicename=hcash_ray&failaction=1&webcashier=1&skin=ray-ray&realmode=1&preview=0&currency=eur&UseHTMLLayout=1&htmlstyle=2&localtime=20160725041924&script=%2Fpoker_sessions.php";
        private string baseUrl = "https://cashier.pt.ray.fi/payments/CashierProxy.php?language=fi&clienttype=poker&casino=ray&servicename=hcash_ray&failaction=1&webcashier=1&skin=ray-ray&realmode=1&preview=0&currency=eur&UseHTMLLayout=1&htmlstyle=2&localtime=20160725041924&script=%2Fpoker_sessions.php";
        public string Wcusersessionid { get; set; }
        private string startdate = "2010-01-01";

        public DataFetcher(string sessionId)
        {
            Wcusersessionid = sessionId;
        }

        public string GenerateUrl()
        {
            return baseUrl + "&wcusersessionid=" + Wcusersessionid + "&startdate=" + startdate;
        }

        public string GetData()
        {
            var url = GenerateUrl();
            Console.WriteLine(url);

            WebRequest request = WebRequest.Create(url);

            WebResponse response = request.GetResponse();
            
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            
            // Read the content.
            //string responseFromServer = reader.ReadToEnd();
            
            // Display the content.
            //Console.WriteLine(responseFromServer);
            
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return ((HttpWebResponse)response).StatusDescription;
        }

        public StreamReader GetFetchedStreamReader()
        {
            WebRequest request = WebRequest.Create(GenerateUrl());
            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            return new StreamReader(dataStream);
        }
    }
}
