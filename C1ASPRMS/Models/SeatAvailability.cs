using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C1ASPRMS.Models
{
    public class SeatAvailability
    {
        public Int32 avail { get; set; }
        public String tno { get; set; }
        public String dt { get; set; }
        public String source { get; set; }
        public String destination { get; set; }
        public String classes { get; set; }
    }
}