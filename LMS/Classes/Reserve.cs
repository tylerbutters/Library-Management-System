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
        public string dateReserved { get; set; }
        public string dateAvailable { get; set; }
        public Book book { get; set; }
        public Reserve(Book _book, Member member)
        {
            book = _book;
            bookId = book.id;
            memberId = member.id;
        }
    }
}
