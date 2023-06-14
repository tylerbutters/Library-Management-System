using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Member
    {
        public int Id { get; set; }
        public int Pin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ReserveCounter { get; set; } = 0;
        public int LoanCounter { get; set; } = 0;
    }
}
