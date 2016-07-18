using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MapdrawingTest.Web
{
    class PersonListInformationHandler
    {
        private const string URL_TEMPLATE = "http://www.torget.se/personer/{0}/q_-/{1}/";
        private string regex;
        private RegexOptions regexOpt;  
        private int index = 0;
        private WebClientHandler webClientHandler;
        private string triggerString;

        public int PostalNumber { get; set; }
        public int PageNumber { get; set; }
        public string Url { get; set; }

        public List<string> GetNextList(string regex)
        {
             List<String> list = new List<String>();
             do
             {
                 Url = String.Format(URL_TEMPLATE, PostalNumber, PageNumber);
                 WebClientHandler webClientHandler = new WebClientHandler(Url);
                 list = webClientHandler.GetSiteContent(regex);
                 PageNumber++;

                 if (list.Count <= 0 || !webClientHandler.ContainsString(Url))
                 {
                     PageNumber = 1;
                     PostalNumber++;
                 }
             } while (PageNumber == 1);


            return list;
        }

        private bool IsSuccessful()
        {
            return webClientHandler.ContainsString(String.Format(URL_TEMPLATE, PostalNumber, PageNumber));
        }
    }
}
