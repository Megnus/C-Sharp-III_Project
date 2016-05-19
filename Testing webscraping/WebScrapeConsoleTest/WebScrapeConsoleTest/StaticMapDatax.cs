using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapeConsoleTest
{
    public class StaticMapDatax
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