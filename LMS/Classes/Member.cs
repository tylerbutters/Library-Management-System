using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    internal class Member : Account
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int reserveCounter { get; set; } = 0;
        public int loanCounter { get; set; } = 0;
    }
}
