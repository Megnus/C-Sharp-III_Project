using AddressCollectorApplication.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            StaticMapDataHandler.cs
 */

namespace AddressCollectorApplication.Web
{
    /// <summary>
    /// Class for handeling the extraction of the address information.
    /// </summary>
    /// <seealso cref="https://nuwber.se/search?q=10405&page=1"/>
    class StaticMapDataHandler
    {
        private const string URL_TEMPLATE = "http://www.torget.se/personer/-/TA_{0}/";
        private const string TRIGGER_STRING = "staticMapData";
        private WebClientHandler webClientHandler;
        private int dataId = 0;

        /// <summary>
        /// The constructor of the class. 
        /// </summary>
        public StaticMapDataHandler()
        {
            LoadWebClientHandler();
        }

        /// <summary>
        /// Method for loading the url.
        /// </summary>
        /// <returns>If the loading of site has been successful.</returns>
        private bool LoadWebClientHandler()
        {
            Url = String.Format(URL_TEMPLATE, this.dataId);
            webClientHandler = new WebClientHandler(Url);
            return IsSuccessful();
        }

        /// <summary>
        /// Property for holding the url.
        /// </summary>
        public string Url { get; set; }
      
        /// <summary>
        /// Method for parsing data to a tatisticalmap object.
        /// </summary>
        /// <param name="regex">The regeular expression to be used.</param>
        /// <returns>A statisticalmap object to hold the parsed data.</returns>
        public StaticMapData GetSerializedData(string regex)
        {
            return JsonConvert.DeserializeObject<StaticMapData>(GetStringData(regex));
        }

        /// <summary>
        /// Method for returning a string of the parsed data.
        /// </summary>
        /// <param name="regex">The regular expression to use.</param>
        /// <returns>A string of the parsed data.</returns>
        public string GetStringData(string regex)
        {
            return webClientHandler.GetSiteContent(regex).DefaultIfEmpty(String.Empty).FirstOrDefault();
        }

        /// <summary>
        /// Sets the id of the data.
        /// </summary>
        /// <param name="dataId">The id of the data.</param>
        /// <returns>If the dataid has been successful.</returns>
        public bool SetDataId(int dataId)
        {
            this.dataId = dataId;
            return LoadWebClientHandler();
        }

        /// <summary>
        /// Gets the next id to be handled.
        /// </summary>
        /// <returns>If the loading of the url has been successful.</returns>
        public bool GetNextIndex()
        {
            this.dataId++;
            return LoadWebClientHandler();
        }

        /// <summary>
        /// Method for checking if the site is valid be checking a key string.
        /// </summary>
        /// <returns>Returns true if the site is valid.</returns>
        public bool IsSuccessful()
        {
            return webClientHandler.ContainsString(TRIGGER_STRING);
        }

    }
}
