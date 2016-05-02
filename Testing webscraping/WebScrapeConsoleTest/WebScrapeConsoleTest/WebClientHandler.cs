using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebScrapeConsoleTest
{
    class WebClientHandler
    {
        private string encoded;
        const string urlTemplate = "http://www.torget.se/personer/Stockholm/TA_{0}/";

        public WebClientHandler(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers.Add("User-Agent: Other");
                byte[] reqHTML = webClient.DownloadData(url);
                UTF8Encoding objUTF8 = new UTF8Encoding();
                encoded = objUTF8.GetString(reqHTML);
            }
        }

        public bool ContainsString(string str)
        {
            return this.encoded.Contains(str);
        }

        //Match match = Regex.Match(encoded, "(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)", RegexOptions.Singleline);
        public List<string> GetSiteContent(string regex, RegexOptions regexOpt)
        {
            List<string> extractedData = new List<string>();
            Match match = Regex.Match(encoded, regex, regexOpt);

            if (match.Success)
            {
                extractedData.Add(match.Value);
                Console.WriteLine(match.Value);
                match = match.NextMatch();
            }

            return extractedData;
        }
    }
}
/*
        public WebClientHandler()
{
    using (WebClient webClient = new WebClient())
    {
        //WebClient webClient = new WebClient();
        //const string strUrl = "http://www.timeapi.org/utc/now";
        //const string strUrl = "http://www.dn.se";
        //const string strUrl = "http://www.torget.se/personer/11666/-/1/";
        const string strUrl = "http://www.torget.se/personer/Stockholm/TA_79779/";
        byte[] reqHTML;

        webClient.Headers.Add("User-Agent: Other");
        reqHTML = webClient.DownloadData(strUrl);
        UTF8Encoding objUTF8 = new UTF8Encoding();
                
        //var arr = arrayx.Where(x => x.Contains("vader")).ToList<string>();

        List<string> array = objUTF8.GetString(reqHTML).Split().ToList<string>();
        var arr = array.Where(x => x.Contains("itemprop=\"name\"")).ToList<string>();
        //arr.ForEach(x => Console.WriteLine(x));
            //Console.WriteLine(objUTF8.GetString(reqHTML));


            //Console.WriteLine("-----------------------------------");

        //Match match = Regex.Match(objUTF8.GetString(reqHTML), "(?<=href=\").+?(?=\")",
        //RegexOptions.IgnoreCase);

            //Match match = Regex.Match(objUTF8.GetString(reqHTML), "(?<=itemprop=\"name\">).+?(?=</span>)", RegexOptions.Singleline);
        Match match = Regex.Match(objUTF8.GetString(reqHTML), "(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)", RegexOptions.Singleline);
               
        //The new regex
        //(?<=var\sstaticMapData\s=\s\[).+?(?=\];)

        if (match.Success)
        {
            Console.WriteLine(match.Value);
            //match = match.NextMatch();
        }

        //List<string> array = objUTF8.GetString(reqHTML).Split().ToList<string>();
        //var arrayx = array.Where(x => !String.IsNullOrEmpty(x)).ToList();
        //Regex regex = new Regex("src=\".*\"");
        //var regarray = arrayx.Where(x => regex.Match(x).Success).ToList();
        //regarray.ForEach(x => Console.WriteLine(x));

    }
}
 */
