using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapdrawingTest.Data
{
    public class Postal
    {
        //public int PostalId { get; set; }

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public string PostalCode { get; set; }

        //public string City { get; set; }

        public int PostalId { get; set; }

        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PostalCode { get; set; }

        public string City { get; set; }

        public virtual List<Address> Addresses { get; set; }
    }
}
