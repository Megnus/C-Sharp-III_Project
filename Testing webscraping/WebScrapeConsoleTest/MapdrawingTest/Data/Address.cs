using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapdrawingTest.Data
{
    public class Address
    {
        //public int AddressId { get; set; }

        //public string Street { get; set; }

        //public Postal Postal { get; set; }

        //public double XCoord { get; set; }

        //public double YCoord { get; set; }

        public int AddressId { get; set; }

        public string Street { get; set; }

        public double XCoord { get; set; }

        public double YCoord { get; set; }

        public string PostalCode { get; set; }

        public virtual List<Person> Persons { get; set; }

        public int PostalId { get; set; }

        public virtual Postal Postal_ { get; set; }
    }
}
