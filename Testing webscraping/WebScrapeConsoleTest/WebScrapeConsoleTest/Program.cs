using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WebScrapeConsoleTest
{
    public class Test
    {
        public string PostalId { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }

    public class Tester
    {
        public string PostalId { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }
    }

    class Program
    {
        /// http://www.dotnetperls.com/webclient
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //new DatabaseHandler();
            Test test = new Test();
            test.City = "New York!";
            //ObjectMapper.GetProperites(test);
            Tester tester = ObjectMapper<Tester>.Map(test);
            
            Console.WriteLine(tester.City);


            if (true) return;

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

                /*
                var arr = arrayx.Where(x => x.Contains("vader")).ToList<string>();*/

                List<string> array = objUTF8.GetString(reqHTML).Split().ToList<string>();
                var arr = array.Where(x => x.Contains("itemprop=\"name\"")).ToList<string>();
                //arr.ForEach(x => Console.WriteLine(x));
                Console.WriteLine(objUTF8.GetString(reqHTML));


                Console.WriteLine("-----------------------------------");

                //Match match = Regex.Match(objUTF8.GetString(reqHTML), "(?<=href=\").+?(?=\")",
                //RegexOptions.IgnoreCase);

                Match match = Regex.Match(objUTF8.GetString(reqHTML), "(?<=itemprop=\"name\">).+?(?=</span>)", RegexOptions.Singleline);
                
                //The new regex
                //(?<=var\sstaticMapData\s=\s\[).+?(?=\];)

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

        static void secondMain(string[] args)
        {
            string jsonMapData = @"
                {
                    id: ""79779""
                    ,name : ""Ivar Stenlund""
                    , coordy : ""57.7349392786725""
                    ,coordx : ""12.9991228305308""
                    ,addr1 : ""Rådjursgatan 16""
                    ,postalcode : ""50732""
                    ,city : ""Brämhult""
                    ,phone : ""F##24 84 38""
                    ,link : ""/foretag/Br%C3%A4mhult/79779/-/""
                }";

            string birthDate = "januari 04";

            StaticMapData staticMapData = JsonConvert.DeserializeObject<StaticMapData>(jsonMapData);
            Person person = MigrateData(staticMapData, birthDate);
            Console.WriteLine(person.Name);
        }

        private static Person MigrateData(StaticMapData staticMapData, string birthDate)
        {
            return new Person()
            {
                Id = staticMapData.Id,
                Name = staticMapData.Name,
                Phone = staticMapData.Phone,
                Link = staticMapData.Link,
                BirthDate = birthDate,
                Address = new Address()
                {
                    Street = staticMapData.Addr1,
                    XCoord = staticMapData.CoordX,
                    YCoord = staticMapData.CoordY,
                    Postal = new Postal()
                    {
                        City = staticMapData.City,
                        PostalCode = staticMapData.PostalCode
                    }
                }
            };
        }
    }

    public class StaticMapData
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public double CoordY { get; set; }
        
        public double CoordX { get; set; }
        
        public string Addr1 { get; set; }
        
        public int PostalCode { get; set; }
        
        public string City { get; set; }
        
        public string Phone { get; set; }
        
        public string Link { get; set; }
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