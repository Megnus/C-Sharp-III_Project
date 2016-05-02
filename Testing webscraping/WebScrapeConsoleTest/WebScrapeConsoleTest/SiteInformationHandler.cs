using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WebScrapeConsoleTest
{
    //WebIndexInformationHandler
    class SiteInformationHandler<T>
    {
        //Match match = Regex.Match(encoded, "(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)", RegexOptions.Singleline)
        //const string strUrl = "http://www.torget.se/personer/Stockholm/TA_79779/";
        private string urlTemplate = "http://www.torget.se/personer/Stockholm/TA_{0}/";
        private string regex;
        private RegexOptions regexOpt;
        private int index = 0;
        private WebClientHandler webClientHandler;
        private string triggerString;
        //Uses the web client handler
        public SiteInformationHandler(string urlTemplate, string triggerString) //string url = String.Format(urlTemplate, personId);
        {
            this.urlTemplate = urlTemplate;
            this.triggerString = triggerString;
            LoadWebClientHandler();
        }

        public T GetSerializedData(string regex, RegexOptions regexOpt)
        {
            return JsonConvert.DeserializeObject<T>(GetStringData(regex, regexOpt));
        }

        public string GetStringData(string regex, RegexOptions regexOpt)
        {
            return webClientHandler.GetSiteContent(regex, regexOpt).First();
        }

        public bool SetIndex(int index)
        {
            this.index = index;
            return LoadWebClientHandler();
        }

        public bool GetNextIndex(string str)
        {
            this.index++;
            return LoadWebClientHandler();
        }

        public bool IsSuccessful()
        {
            return webClientHandler.ContainsString(triggerString);
        }

        private bool LoadWebClientHandler()
        {
            string url = String.Format(urlTemplate, this.index);
            webClientHandler = new WebClientHandler(url);
            return IsSuccessful();
        }
    }
}
