using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            PersonListInformationHandler.cs
 */

namespace AddressCollectorApplication.Web
{
    /// <summary>
    /// Class for handeling the extraction of the persons id number.
    /// </summary>
    class PersonListInformationHandler
    {
        private const string URL_TEMPLATE = "http://www.torget.se/personer/{0}/q_-/{1}/";
        private WebClientHandler webClientHandler = null;

        /// <summary>
        /// Property for the postal number.
        /// </summary>
        public int PostalNumber { get; set; }
        
        /// <summary>
        /// Property for the number of the site page.
        /// </summary>
        public int PageNumber { get; set; }
        
        /// <summary>
        /// Property for the url of the site.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Method for returning the id of the persons as a list of strings.
        /// </summary>
        /// <param name="regex">The regular expression for extracting the id´s.</param>
        /// <returns>A list of id´s.</returns>
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

        /// <summary>
        /// Checks if the site is valid by checking a key value.
        /// </summary>
        /// <returns>If the loading of the site has been successful.</returns>
        private bool IsSuccessful()
        {
            return webClientHandler.ContainsString(String.Format(URL_TEMPLATE, PostalNumber, PageNumber));
        }
    }
}
