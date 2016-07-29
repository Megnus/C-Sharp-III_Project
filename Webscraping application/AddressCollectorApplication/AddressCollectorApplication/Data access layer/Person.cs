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
 * File:            Person.cs
 */

namespace AddressCollectorApplication.Data
{
    /// <summary>
    /// Class for holding the information of the person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Property representing the id of the person.
        /// This is set in the code and not auto-generated.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DataId { get; set; }

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
        /// Property representing the phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Property representing the day of birth of the person.
        /// </summary>
        /// <remarks>The information does not hold the year of birth.</remarks>
        public string BirthDate { get; set; }

        /// <summary>
        /// Property representing the id of the address.
        /// </summary>
        /// <remarks>Obsolete?</remarks>
        public int AddressId { get; set; }

        /// <summary>
        /// Property representing a list of address objects.
        /// </summary>
        public virtual Address Address { get; set; }
    }
}
