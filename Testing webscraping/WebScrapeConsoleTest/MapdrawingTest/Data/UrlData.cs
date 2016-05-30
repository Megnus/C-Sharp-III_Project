using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapdrawingTest.Data
{
    public class UrlData
    {
        public int UrlDataId { get; set; }

        public int UrlIndex { get; set; }

        public int PageNumber { get; set; }

        public virtual List<Person> Persons { get; set; }
    }
}
