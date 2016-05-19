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

        public static int ConvertToInt(string input)
        {
            int output = 0;
            return int.TryParse(input, out output) ? output : 0;
        }

        public static double ConvertToDouble(string input)
        {
            double output = 0;
            return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out output) ? output : 0;
        }

        public Postal GetPostal()
        {
            return new Postal()
            {
                City = this.City,
                PostalCode = this.PostalCode
            };
        }

        public Address GetAddress()
        {
            Debug.WriteLine(this.CoordX + "  " + this.CoordY);
            return new Address()
            {
                Street = this.Addr1,
                //XCoord = this.ConvertToDouble(this.CoordX),
                //YCoord = this.ConvertToDouble(this.CoordY),
                XCoord = ConvertToDouble(this.CoordX),
                YCoord = ConvertToDouble(this.CoordY),
                Postal = GetPostal()
            };
        }

        public Person GetPerson()
        {
            return new Person()
            {
                PersonId = ConvertToInt(this.Id),
                Name = this.Name,
                Phone = this.Phone,
                BirthDate = this.Birthday,
                Address = this.GetAddress()
            };
        }
    }
}
