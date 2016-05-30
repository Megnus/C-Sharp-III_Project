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

        public int UrlIndex { get; set; }

        public int PageNumber { get; set; }

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
                PostalCode = this.PostalCode,
                Persons = new List<Person>() { GetPerson() }
            };
        }

        public Person GetPerson()
        {
            return new Person()
            {
                DataId = ConvertToInt(this.Id),
                UrlIndex = this.UrlIndex,
                PageNumber = this.PageNumber,
                //UrlData = new List<UrlData>() { GetUrlData() },
                Name = this.Name,
                Phone = this.Phone,
                BirthDate = this.Birthday,
            };
        }

     /*   private UrlData urlData;

        public void SetUrlData(UrlData urlData)
        {
            if (urlData != null)
            {
                this.urlData = urlData;
            }
            else
            {
                this.urlData = new UrlData()
                    {
                        UrlIndex = this.UrlIndex,
                        PageNumber = this.PageNumber,
                        Persons = new List<Person>() { GetPerson() }
                    };
            }
        }

        public UrlData GetUrlData()
        {
            return new UrlData()
            {
                UrlIndex = this.UrlIndex,
                PageNumber = this.PageNumber,
                Persons = new List<Person>() { GetPerson() }
            };

        }*/
    }
}
