using MapdrawingTest.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MapdrawingTest.Web
{
    class StaticMapDataHandler
    {
        //https://nuwber.se/search?q=10405&page=1
        private const string URL_TEMPLATE = "http://www.torget.se/personer/-/TA_{0}/";
        private const string TRIGGER_STRING = "staticMapData";
        private int dataId = 0;
        private WebClientHandler webClientHandler;
      
        public StaticMapDataHandler()
        {
            LoadWebClientHandler();
        }

        public StaticMapData GetSerializedData(string regex)
        {
            return JsonConvert.DeserializeObject<StaticMapData>(GetStringData(regex));
        }

        public string GetStringData(string regex)
        {
            return webClientHandler.GetSiteContent(regex).DefaultIfEmpty(String.Empty).FirstOrDefault();
        }

        public bool SetDataId(int dataId)
        {
            this.dataId = dataId;
            return LoadWebClientHandler();
        }

        public bool GetNextIndex()
        {
            this.dataId++;
            return LoadWebClientHandler();
        }

        public bool IsSuccessful()
        {
            return webClientHandler.ContainsString(TRIGGER_STRING);
        }

        private bool LoadWebClientHandler()
        {
            string url = String.Format(URL_TEMPLATE, this.dataId);
            webClientHandler = new WebClientHandler(url);
            return IsSuccessful();
        }
    }
}
