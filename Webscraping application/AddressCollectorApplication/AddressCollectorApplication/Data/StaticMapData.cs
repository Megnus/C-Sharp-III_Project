using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AddressCollectorApplication.Data
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

        public StaticMapData()
        {

        }

        public StaticMapData(Person person)
        {
            try
            {
                Id = person.DataId.ToString();
                UrlIndex = person.UrlIndex;
                PageNumber = person.PageNumber;
                Name = person.Name;
                Phone = person.Phone;
                Birthday = person.BirthDate;    
                CoordX = person.Address.XCoord.ToString();
                CoordY = person.Address.YCoord.ToString();
                Addr1 = person.Address.Street;
                PostalCode = person.Address.Postal.PostalCode;
                City = person.Address.Postal.City;
            } catch {

            }
        }

        public Postal GetPostal()
        {
            Postal postal = new Postal()
            {
                City = this.City,
                PostalCode = this.PostalCode,
                Addresses = new List<Address>()
            };

            postal.Addresses.Add(GetAddress(postal));

            return postal;
        }

        public Address GetAddress(Postal postal)
        {
            Address address = new Address()
            {
                Street = this.Addr1,
                XCoord = ConvertToDouble(this.CoordX),
                YCoord = ConvertToDouble(this.CoordY),
                PostalCode = this.PostalCode,
                Persons = new List<Person>(),
                Postal = postal
            };

            address.Persons.Add(GetPerson(address));

            return address;

        }

        public Person GetPerson(Address address)
        {
            return new Person()
            {
                DataId = ConvertToInt(this.Id),
                UrlIndex = this.UrlIndex,
                PageNumber = this.PageNumber,
                Name = this.Name,
                Phone = Regex.Replace(this.Phone, @"(-|\s)+", ""),
                BirthDate = this.Birthday,
                Address = address
            };
        }
    }
}
