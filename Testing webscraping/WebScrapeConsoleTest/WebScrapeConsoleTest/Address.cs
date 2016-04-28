using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapeConsoleTest
{
    public class Address
    {
        public int AddressId { get; set; }

        public string Street { get; set; }

        public Postal Postal { get; set; }

        public double XCoord { get; set; }

        public double YCoord { get; set; }
    }
}
