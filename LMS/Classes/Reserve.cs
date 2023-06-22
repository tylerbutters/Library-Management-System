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
        public string dateDueBack { get; set; }
        public bool isAvailableToLoan { get; set; } = false;
        public Book book { get; set; }
        public Reserve(Book _book, Member member)
        {
            book = _book;
            bookId = book.id;
            memberId = member.id;
            if(book.isLoaned == false)
            {
                isAvailableToLoan = true;
            }
        }
    }
}
