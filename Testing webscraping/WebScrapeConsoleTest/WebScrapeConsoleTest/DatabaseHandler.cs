using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapeConsoleTest
{
    class DatabaseHandler
    {
        public DatabaseHandler()
        {
            //using (var db = new PersonContextx())
            {
                //db.Persons.Where(x => x.Id > 99500).ToList().ForEach(x => Console.WriteLine(x.Name));
            }

            if (true) return;

            for (int i = 0; i < 100000; i++)
            {
                //using (var db = new PersonContextx())
                {
                    var persons = new Personx()
                    {
                        Name = i + ": Magnus Sundström",
                        BirthDate = (DateTime.Today).ToString(),
                        Phone = "070-3945876",
                        //Link = "/bla/bla/..",
                        Address = new Addressx()
                        {
                            Street = "Metargatan",
                            XCoord = 1.001,
                            YCoord = 1.002,
                            sPostal = new Postalx()
                            {
                                City = "Stockholm",
                                PostalCode = "10405"
                            }
                        }
                    };
                    //db.Persons.Add(persons);
                    //Console.WriteLine(persons.Name);
                    //db.SaveChanges();
                }
            }

        }
    }
}
