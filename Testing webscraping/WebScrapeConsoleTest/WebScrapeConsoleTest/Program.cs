using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace WebScrapeConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            const string strUrl = "http://www.timeapi.org/utc/now";
            byte[] reqHTML;
            while (true)
            {
                webClient.Headers.Add("User-Agent: Other");
                reqHTML = webClient.DownloadData(strUrl);
                UTF8Encoding objUTF8 = new UTF8Encoding();
                Console.WriteLine(objUTF8.GetString(reqHTML));
                Thread.Sleep(1000);
            }
        }
    }
}