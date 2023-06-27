using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Reserve
    {
        public string bookId { get; set; }
        public string memberId { get; set; }
        public string estAvailDate { get; set; }
        //public bool isAvailableToLoan { get; set; }
        public Book book { get; set; }
        public Reserve(Book _book, Member _member)
        {
            book = _book;
            bookId = book.id;
            memberId = _member.id;
        }
    }
}
