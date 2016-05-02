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
        /// https://msdn.microsoft.com/en-us/library/dd997305(v=vs.110).aspx
        /// https://msdn.microsoft.com/en-us/library/dd267265(v=vs.110).aspx
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //new WebClientHandler(79779);
            SiteInformationHandler<StaticMapData> siteInformationHandler =
                new SiteInformationHandler<StaticMapData>("http://www.torget.se/personer/Stockholm/TA_{0}/");

            siteInformationHandler.SetIndex(79779);

            while (true)
            {
                if (!siteInformationHandler.GetNextIndex())
                {
                    continue;
                }

                StaticMapData staticMapData =
                    siteInformationHandler.GetSerializedData("(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)", RegexOptions.Singleline);

                staticMapData.Birthday =
                    siteInformationHandler.GetStringData("(?<=födelsedag\\n<\\/h2>\n<p>).+?(?=<br\\/>)", RegexOptions.Singleline);

                Person person = staticMapData.GetPerson();

                using (var db = new PersonContext())
                {
                    db.Persons.Add(person);
                    db.SaveChanges();
                }

            }

            Console.ReadKey();
        }

       /* static void secondMain(string[] args)
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
            Person person = MigrateData(staticMapData);
            Console.WriteLine(person.Name);
        }

        private static Person MigrateData(StaticMapData staticMapData)
        {
            return new Person()
            {
                Id = staticMapData.Id,
                Name = staticMapData.Name,
                Phone = staticMapData.Phone,
                Link = staticMapData.Link,
                BirthDate = staticMapData.Birthday,
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
        }*/
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

        public string Birthday { get; set; }

        public Person GetPerson()
        {
            return new Person()
            {
                Id = this.Id,
                Name = this.Name,
                Phone = this.Phone,
                Link = this.Link,
                BirthDate = this.Birthday,
                Address = new Address()
                {
                    Street = this.Addr1,
                    XCoord = this.CoordX,
                    YCoord = this.CoordY,
                    Postal = new Postal()
                    {
                        City = this.City,
                        PostalCode = this.PostalCode
                    }
                }
            };
        }
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