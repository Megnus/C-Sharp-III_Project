using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MapdrawingTest.Web
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
