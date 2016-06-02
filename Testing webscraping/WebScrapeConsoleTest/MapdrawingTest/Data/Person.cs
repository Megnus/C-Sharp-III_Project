using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapdrawingTest.Data
{
    public class Person
    {
        //public int PersonId { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DataId { get; set; }

        public int UrlIndex { get; set; }

        public int PageNumber { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string BirthDate { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address_ { get; set; }

        //public virtual List<UrlData> UrlData { get; set; }

        //[ForeignKey("AddressId")]
        //public int AddressId { get; set; }

        //public Address Address { get; set; }
        
        //public string Link { get; set; }

      //  public string PostalCode { get; set; }

        //public Address Address { get; set; }
    }
}
