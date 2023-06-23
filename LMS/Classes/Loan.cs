using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Loan
    {
        public string bookId { get; set; }
        public string memberId { get; set; }
        public string dateDue { get; set; }
        public bool isDue { get;set; }
        public Book book { get; set; }
        public Loan(Book _book, Member member)
        {
            book = _book;
            bookId = book.id;
            memberId = member.id;
            dateDue = DateTime.Parse(MainWindow.datestr).AddDays(14).ToString("yyyy-MM-dd");
        }
    }
}
