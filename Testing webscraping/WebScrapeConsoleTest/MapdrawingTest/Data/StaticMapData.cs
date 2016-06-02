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
            Postal postal = new Postal()
            {
                City = this.City,
                PostalCode = this.PostalCode,
                Addresses = new List<Address>()
            };

            postal.Addresses.Add(GetAddress(postal.PostalId, postal));

            return postal;
        }

        public Address GetAddress(int postalId, Postal postal)
        {
            Address address = new Address()
            {
                Street = this.Addr1,
                XCoord = ConvertToDouble(this.CoordX),
                YCoord = ConvertToDouble(this.CoordY),
                PostalCode = this.PostalCode,
                Persons = new List<Person>(),
                PostalId = postalId,
                Postal_ = postal
            };

            address.Persons.Add(GetPerson(address.AddressId, address));

            return address;

        }

        public Person GetPerson(int addressId, Address address)
        {
            return new Person()
            {
                DataId = ConvertToInt(this.Id),
                UrlIndex = this.UrlIndex,
                PageNumber = this.PageNumber,
                Name = this.Name,
                Phone = this.Phone,
                BirthDate = this.Birthday,
                AddressId = addressId,
                Address_ = address
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
