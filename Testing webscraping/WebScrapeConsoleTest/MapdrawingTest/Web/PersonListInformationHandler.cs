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

        public int PostalNum { get; set; }

        public int PageNum { get; set; }

        //public void GetNext()
        //{
        //    PageNum++;
            
        //    if (!IsSuccessful())
        //    {
        //        PostalNum++;
        //        PageNum = 1;
        //    }

        //    string url = String.Format(urlTemplate, PostalNum, PageNum);
        //    webClientHandler = new WebClientHandler(url);
        //}

        ////string url = String.Format(urlTemplate, personId);

        public void SetPostalNumber(int postalNumber)
        {
            this.PostalNum = postalNumber;
        }

        public List<string> GetNextList(string regex)
        {
             List<String> list = new List<String>();
             do
             {
                 string url = String.Format(URL_TEMPLATE, PostalNum, PageNum);
                 WebClientHandler webClientHandler = new WebClientHandler(url);
                 list = webClientHandler.GetSiteContent(regex);
                 PageNum++;

                 if (list.Count <= 0 || !webClientHandler.ContainsString(url))
                 {
                     PageNum = 1;
                     PostalNum++;
                 }
             } while (PageNum == 1);


            return list;
        }

        private bool IsSuccessful()
        {
            return webClientHandler.ContainsString(String.Format(URL_TEMPLATE, PostalNum, PageNum));
        }
    }
}
