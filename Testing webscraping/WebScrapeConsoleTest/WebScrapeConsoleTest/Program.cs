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
           // WriteableBitmapDemo.doit();
            Thread thread = new Thread(WriteableBitmapDemo.doit);
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();


            if (true) return;

            //new WebClientHandler(79779);
            SiteInformationHandler<StaticMapData> siteInformationHandler =
                new SiteInformationHandler<StaticMapData>("http://www.torget.se/personer/Stockholm/TA_{0}/", "staticMapData");

            siteInformationHandler.SetIndex(79779);
            int x = 0;
            while (x++ < 1000)
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
        public static int Converterta(string input)
        {
            int output = 0;
            return int.TryParse(input, out output) ? output : 0;
        }
    }

    public class StaticMapData
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CoordY { get; set; }

        public string CoordX { get; set; }

        public string Addr1 { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }

        public string Link { get; set; }

        public string Birthday { get; set; }

        public static int Converterta(string input)
        {
            int output = 0;
            return int.TryParse(input, out output) ? output : 0;
        }

        public static double Convertertd(string input)
        {
            double output = 0;
            return double.TryParse(input, out output) ? output : 0;
        }

        public Person GetPerson()
        {
            return new Person()
            {
                PersonId = Converterta(this.Id),
                Name = this.Name,
                Phone = this.Phone,
                //Link = this.Link,
                BirthDate = this.Birthday,
                Address = new Address()
                {
                    Street = this.Addr1,
                    XCoord = Convertertd(this.CoordX),
                    YCoord = Convertertd(this.CoordY),
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