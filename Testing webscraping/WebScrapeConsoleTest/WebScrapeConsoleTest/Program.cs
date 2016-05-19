using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;
//using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

    class CQ_EnqueueDequeuePeek
    {
        // Demonstrates:
        // ConcurrentQueue<T>.Enqueue()
        // ConcurrentQueue<T>.TryPeek()
        // ConcurrentQueue<T>.TryDequeue()
        public static void dothis()
        {
            // Construct a ConcurrentQueue.
            ConcurrentQueue<int> cq = new ConcurrentQueue<int>();


            new Thread(() =>
            {
                for (int i = 0; true; i++)
                {
                    Thread.Sleep(90);
                    cq.Enqueue(i++);
                }
            }).Start();

            new Thread(() =>
            {
                int j;
                  while (true)
                  {
                      Thread.Sleep(100);
                      cq.TryDequeue(out j);
                      var s = cq.GetEnumerator();
                      while (s.MoveNext())
                      {
                          Console.Write(s.Current);
                      }
                      Console.WriteLine();
                      //Console.WriteLine(j + "\t" + cq.Count);
                  }
            }).Start();


            //// Populate the queue.
            //for (int i = 0; i < 10000; i++) cq.Enqueue(i);

            //int j;
            //for (int i = 0; i < 10000; i++)
            //{
            //    //cq.TryPeek(out j);
            //    cq.TryDequeue(out j);
            //    Console.WriteLine(j);
            //}


            /*
            // Peek at the first element.
            int result;
            if (!cq.TryPeek(out result))
            {
                Console.WriteLine("CQ: TryPeek failed when it should have succeeded");
            }
            else if (result != 0)
            {
                Console.WriteLine("CQ: Expected TryPeek result of 0, got {0}", result);
            }

            int outerSum = 0;
            // An action to consume the ConcurrentQueue.
            Action action = () =>
            {
                int localSum = 0;
                int localValue;
                while (cq.TryDequeue(out localValue)) localSum += localValue;
                Interlocked.Add(ref outerSum, localSum);
            };

            // Start 4 concurrent consuming actions.
            Parallel.Invoke(action, action, action, action);

            Console.WriteLine("outerSum = {0}, should be 49995000", outerSum);*/
        }
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

            CQ_EnqueueDequeuePeek.dothis();

            if (true) return;



            // WriteableBitmapDemo.doit();
            Thread thread = new Thread(WriteableBitmapDemo.doit);
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();


            if (true) return;

            //new WebClientHandler(79779);
            SiteInformationHandler<StaticMapDatax> siteInformationHandler =
                new SiteInformationHandler<StaticMapDatax>("http://www.torget.se/personer/Stockholm/TA_{0}/", "staticMapData");

            siteInformationHandler.SetIndex(79779);
            int x = 0;
            while (x++ < 1000)
            {
                if (!siteInformationHandler.GetNextIndex())
                {
                    continue;
                }

                //StaticMapDatax staticMapData =
                //    siteInformationHandler.GetSerializedData("(?<=var\\sstaticMapData\\s=\\s\\[).+?(?=\\];)", RegexOptions.Singleline);

                //staticMapData.Birthday =
                //    siteInformationHandler.GetStringData("(?<=födelsedag\\n<\\/h2>\n<p>).+?(?=<br\\/>)", RegexOptions.Singleline);

                //Personx person = staticMapData.GetPerson();

                //using (var db = new PersonContextx())
                //{
                //    db.Persons.Add(person);
                //    db.SaveChanges();
                //}

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

    public class StaticMapDataX
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

        public Personx GetPerson()
        {
            return new Personx()
            {
                PersonId = Converterta(this.Id),
                Name = this.Name,
                Phone = this.Phone,
                //Link = this.Link,
                BirthDate = this.Birthday,
                Address = new Addressx()
                {
                    Street = this.Addr1,
                    XCoord = Convertertd(this.CoordX),
                    YCoord = Convertertd(this.CoordY),
                    sPostal = new Postalx()
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