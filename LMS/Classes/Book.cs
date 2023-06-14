using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Book
    {
        public int Id { get; set; }
        public string Cover { get; set; }
        public string Title { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string Type { get; set; }
        public string Genre { get; set; }
        public string Summary { get; set; }
        public bool IsAvailable { get; set; }
    }
}
