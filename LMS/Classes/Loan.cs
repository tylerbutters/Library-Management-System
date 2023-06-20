using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Loan
    {
        public string dateLoaned { get; set; }
        public string dateDue { get; set; }
        public Book book { get; set; }
    }
}
