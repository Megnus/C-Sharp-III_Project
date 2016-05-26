using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapdrawingTest.Data
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Postal> Postals { get; set; }

        //public DbSet<DataIdentity> DataIdentities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

          //  modelBuilder.Entity<Address>().HasRequired(x => x.Persons).WithOptional().Map(m => m.MapKey("AddressId"));

            //modelBuilder.Entity<Address>().HasRequired(p => p.Persons).WithOptional().Map(x => x.MapKey("PersonId"));
            //modelBuilder.Entity<Address>().HasRequired(p => p.Persons).WithOptional().Map(x => x.MapKey("PersonId"));
            //modelBuilder.Entity<Address>().HasRequired(p => p.Persons).WithMany().Map(x => x.MapKey("UrlId"));
            /*modelBuilder.Entity<Postal>().HasRequired(p => p.).WithOptional().Map(x => x.MapKey("PostalCode"));
            modelBuilder.Entity<Address>().HasRequired(p => p.Postal).WithOptional().Map(x => x.MapKey("PostalCode"));

            modelBuilder.Entity<Address>()
             * 
                .HasOptional(pt => pt.Postal)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);*/

            //modelBuilder.Entity<Person>().HasRequired(p => p.Address).WithRequiredDependent().Map(x => x.MapKey("PostalCode"));
            // modelBuilder.Entity<Person>().HasRequired(p => p.address).WithRequiredDependent().Map(x => x.ToTable("Postal"));
            //modelBuilder.Entity<Person>().Map(x => x.ToTable("Postal"));
            //configure model with fluent API 
            //   modelBuilder.Entity<Postal>().Property(p => p.PostalCode).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //    modelBuilder.Entity<Person>()
            //        .HasMany(p => p.Address)
            //        .Map(mc =>
            //        {
            //            mc.ToTable("PostJoinTag");
            //            mc.MapLeftKey("PostId");
            //            mc.MapRightKey("TagId");
            //        });
            //modelBuilder.Entity<Person>(). HasRequired(p => p.Address)
            //    //.WithMany(b => b.Posts)
            //    ,(p => p.);
        }

    }
}
