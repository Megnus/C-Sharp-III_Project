using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapdrawingTest.Data
{
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

        public int ConvertToInt(string input)
        {
            int output = 0;
            return int.TryParse(input, out output) ? output : 0;
        }

        public double ConvertToDouble(string input)
        {
            double output = 0;
            return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out output) ? output : 0;
        }

        public Postal GetPostal()
        {
            return new Postal()
            {
                City = this.City,
                PostalCode = this.PostalCode,
                Addresses = new List<Address>() { GetAddress() }
            };
        }

        public Address GetAddress()
        {
            return new Address()
            {
                Street = this.Addr1,
                XCoord = ConvertToDouble(this.CoordX),
                YCoord = ConvertToDouble(this.CoordY),
                Persons = new List<Person>() { GetPerson() }
            };
        }

        public Person GetPerson()
        {
            return new Person()
            {
                UrlId = ConvertToInt(this.Id),
                Name = this.Name,
                Phone = this.Phone,
                BirthDate = this.Birthday,
            };
        }
    }
}
