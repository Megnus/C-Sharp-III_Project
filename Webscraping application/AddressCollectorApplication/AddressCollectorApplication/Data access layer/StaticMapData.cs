using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            StaticMapData.cs
 */

namespace AddressCollectorApplication.Data
{
    /// <summary>
    /// Class for holding the raw information of the person, city and address.
    /// The class does also contain methods for parsing converting the data.
    /// </summary>
    public class StaticMapData
    {
        /// <summary>
        /// Property representing the person id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Property representing the index of the url.
        /// </summary>
        public int UrlIndex { get; set; }

        /// <summary>
        /// Property representing the number of the site page.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Property representing the name of the person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property representing the x-coordinate of the location of the address.
        /// </summary>
        public string CoordY { get; set; }

        /// <summary>
        /// Property representing the y-coordinate of the location of the address.
        /// </summary>
        public string CoordX { get; set; }

        /// <summary>
        /// Property representing the street name and number.
        /// </summary>
        public string Addr1 { get; set; }

        /// <summary>
        /// Property representing the postalcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Property representing the name of the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Property representing the phonenumber.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Property representing the 
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Property representing the day of birth of the person.
        /// </summary>
        /// <remarks>The information does not hold the year of birth.</remarks>
        public string Birthday { get; set; }

        /// <summary>
        /// Method for converting a string of input to a integer.
        /// </summary>
        /// <param name="input">The input as a string.</param>
        /// <returns>A integer of the input.</returns>
        public int ConvertToInt(string input)
        {
            int output = 0;
            return int.TryParse(input, out output) ? output : 0;
        }

        /// <summary>
        /// Method for converting a string of input to a double.
        /// </summary>
        /// <param name="input">The input as a string.</param>
        /// <returns>A double of the input.</returns>
        public double ConvertToDouble(string input)
        {
            double output = 0;
            return double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out output) ? output : 0;
        }

        /// <summary>
        /// Overloading constructor with no input arguments.
        /// </summary>
        public StaticMapData() { }

        /// <summary>
        /// The constructor that takes a person object as a input and converts the data to the properties.
        /// </summary>
        /// <param name="person">A person object.</param>
        public StaticMapData(Person person)
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
        }

        /// <summary>
        /// Method for returning a postal object created from the properites data.
        /// </summary>
        /// <returns>A postal object.</returns>
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

        /// <summary>
        /// Method for returning a address object created from the properites data.
        /// </summary>
        /// <returns>A address object.</returns>
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

        /// <summary>
        /// Method for returning a person object created from the properites data.
        /// </summary>
        /// <returns>A person object.</returns>
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
