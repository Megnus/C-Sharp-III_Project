using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            Address.cs
 */

namespace AddressCollectorApplication.Data
{
    /// <summary>
    /// Class for holding the information of the address.
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Property representing the id of the address.
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// Property representing the name of the street.
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Property representing the x-coordinate of the location of the address.
        /// </summary>
        public double XCoord { get; set; }

        /// <summary>
        /// Property representing the y-coordinate of the location of the address.
        /// </summary>
        public double YCoord { get; set; }

        /// <summary>
        /// Property representing the postalcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Property representing the a list of persons objects.
        /// </summary>
        public virtual List<Person> Persons { get; set; }

        /// <summary>
        /// Property representing the postal object.
        /// </summary>
        public virtual Postal Postal { get; set; }
    }
}
