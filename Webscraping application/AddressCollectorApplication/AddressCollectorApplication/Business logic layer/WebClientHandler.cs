using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            WebClientHandler.cs
 */

namespace AddressCollectorApplication.Web
{
    /// <summary>
    /// Class for handeling the parsing of the site.
    /// </summary>
    class WebClientHandler
    {
        private string encoded;
        const string urlTemplate = "http://www.torget.se/personer/Stockholm/TA_{0}/";

        /// <summary>
        /// The constructor of the class. The constructor loads the content of the site and keeps on until the
        /// site has been rendered.
        /// </summary>
        /// <param name="url">The url of the site.</param>
        public WebClientHandler(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                while (true)
                {
                    try
                    {
                        webClient.Headers.Add("User-Agent: Other");
                        byte[] reqHTML = webClient.DownloadData(url);
                        UTF8Encoding objUTF8 = new UTF8Encoding();
                        encoded = objUTF8.GetString(reqHTML);
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(10000);
                    }   
                }    
            }
        }

        /// <summary>
        /// Checks if the content of the site contains a key value string.
        /// </summary>
        /// <param name="str">The string to be checked.</param>
        /// <returns>Returns true if the site contains the key value string.</returns>
        public bool ContainsString(string str)
        {
            return this.encoded.Contains(str);
        }

        /// <summary>
        /// Method to parse the content of the site and returning a list of strings as a result.
        /// </summary>
        /// <param name="regex">The regular expression to be used.</param>
        /// <returns>Returns a list of string representing the parsed result.</returns>
        public List<string> GetSiteContent(string regex)
        {
            List<string> extractedData = new List<string>();
            Match match = Regex.Match(encoded, regex, RegexOptions.Singleline);

            while (match.Success)
            {
                extractedData.Add(match.Value);
                match = match.NextMatch();
            }

            return extractedData;
        }
    }
}
