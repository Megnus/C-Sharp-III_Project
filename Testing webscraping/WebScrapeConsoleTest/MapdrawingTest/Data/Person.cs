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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DataId { get; set; }

        public int UrlIndex { get; set; }

        public int PageNumber { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string BirthDate { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}
