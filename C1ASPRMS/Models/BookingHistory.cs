using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C1ASPRMS.Models
{
    public class BookingHistory
    {
        public Int32 pnr { get; set; }
        public String bookingdate { get; set; }
        public String trainno { get; set; }
        public String doj { get; set; }
        public String source { get; set; }
        public String destination { get; set; }
        public String classes { get; set; }
        public String bs { get; set; }
        public String p1 { get; set; }
        public String p2 { get; set; }
        public String p3 { get; set; }
        public String p4 { get; set; }
        public String p5 { get; set; }
        public String p6 { get; set; }
        public String q1 { get; set; }
        public String q2 { get; set; }
        public String q3 { get; set; }
        public String q4 { get; set; }
        public String q5 { get; set; }
        public String q6 { get; set; }
        public String s1 { get; set; }
        public String s2 { get; set; }
        public String s3 { get; set; }
        public String s4 { get; set; }
        public String s5 { get; set; }
        public String s6 { get; set; }
        public String pay { get; set; }
        public Int32 tfare { get; set; }
    }
}