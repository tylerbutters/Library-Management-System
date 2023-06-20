using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Account
    {
        public bool isAdmin { get; set; } = false;
        public string id { get; set; }
        public string pin { get; set; }
    }
    public class Admin : Account { }
}
