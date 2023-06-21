using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Member : Account
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public int reserveCounter { get; set; }
        public int loanCounter { get; set; }
        public List<Reserve> reservedBooks { get; set; }
        public List<Loan> loanedBooks { get; set; }
        public Member()
        {
            reservedBooks = new List<Reserve>();
            loanedBooks = new List<Loan>();
            reserveCounter = reservedBooks.Count;
            loanCounter = loanedBooks.Count;
        }
    }
}
