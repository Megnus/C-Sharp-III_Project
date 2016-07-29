using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Author:          Magnus Sundström
 * Creation Date:   2016-07-22
 * File:            InformationContext.cs
 */

namespace AddressCollectorApplication.Data
{
    /// <summary>
    /// This class inherits the dbcontext and is used to map the objects to tables in
    /// the database.
    /// </summary>
    public class InformationContext : DbContext
    {
        /// <summary>
        /// DbSet instance for access the person entity.
        /// </summary>
        public DbSet<Person> Persons { get; set; }

        /// <summary>
        /// DbSet instance for access the address entity.
        /// </summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>
        /// DbSet instance for access the postal entity.
        /// </summary>
        public DbSet<Postal> Postals { get; set; }

        /// <summary>
        /// Overriding method for enabeling lazy loading.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    }
}
