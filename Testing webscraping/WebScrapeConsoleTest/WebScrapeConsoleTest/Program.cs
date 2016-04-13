using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

namespace WebScrapeConsoleTest
{
    class Program
    {
        /// <summary>
        /// http://www.dotnetperls.com/webclient
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (WebClient webClient = new WebClient())
            {
                //WebClient webClient = new WebClient();
                //const string strUrl = "http://www.timeapi.org/utc/now";
                const string strUrl = "http://www.dn.se";
                byte[] reqHTML;

                webClient.Headers.Add("User-Agent: Other");
                reqHTML = webClient.DownloadData(strUrl);
                UTF8Encoding objUTF8 = new UTF8Encoding();

                /*
                var arr = arrayx.Where(x => x.Contains("vader")).ToList<string>();*/

                Match match = Regex.Match(objUTF8.GetString(reqHTML), "(?<=href=\").+?(?=\")",
                    RegexOptions.IgnoreCase);

                while (match.Success)
                {
                    Console.WriteLine(match.Value);
                    match = match.NextMatch();
                }
              
                //List<string> array = objUTF8.GetString(reqHTML).Split().ToList<string>();
                //var arrayx = array.Where(x => !String.IsNullOrEmpty(x)).ToList();
                //Regex regex = new Regex("src=\".*\"");
                //var regarray = arrayx.Where(x => regex.Match(x).Success).ToList();
                //regarray.ForEach(x => Console.WriteLine(x));

            }
            Console.ReadKey();

        }
    }
}

//using (var client = new NoKeepAlivesWebClient())
//{
//    // Some code
//}

//public class NoKeepAlivesWebClient : WebClient
//{
//    protected override WebRequest GetWebRequest(Uri address)
//    {
//        var request = base.GetWebRequest(address);
//        if (request is HttpWebRequest)
//        {
//            ((HttpWebRequest)request).KeepAlive = false;
//        }

//        return request;
//    }
//}