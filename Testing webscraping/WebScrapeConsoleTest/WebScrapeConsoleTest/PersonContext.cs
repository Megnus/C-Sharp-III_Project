using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapeConsoleTest
{
    public class PersonContextx : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DbSet<Addressx> Addresses { get; set; }

        public DbSet<Postalx> Postals { get; set; }
    }
}
