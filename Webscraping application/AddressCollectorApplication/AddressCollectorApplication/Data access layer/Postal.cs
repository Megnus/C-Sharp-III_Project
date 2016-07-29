using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            Postal.cs
 */

namespace AddressCollectorApplication.Data
{
    /// <summary>
    /// Class for holding the information of the postal number and city name.
    /// </summary>
    public class Postal
    {
        /// <summary>
        /// Property representing the id of the postal code.
        /// </summary>
        public int PostalId { get; set; }

        /// <summary>
        /// Property representing the postal code.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Property representing the name of the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Property representing the a list of address objects.
        /// </summary>
        public virtual List<Address> Addresses { get; set; }
    }
}
