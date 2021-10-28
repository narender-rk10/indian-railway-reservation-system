using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C1ASPRMS.Models
{
    public class ViewUsers
    {
        public int srno { get; set; }
        public string username { get; set; }
        public string fn { get; set; }
        public string mn { get; set; }
        public string ln { get; set; }
        public string gender { get; set; }
        public string dob { get; set; }
        public Int64 mob { get; set; }
        public string email { get; set; }
        public string nationality { get; set; }
        public string country { get; set; }
        public string address { get; set; }
        public string state { get; set; }
        public string pin { get; set; }
    }
}