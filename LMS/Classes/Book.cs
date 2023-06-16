using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Book
    {
        public int id { get; set; }
        public string cover { get; set; }
        public string title { get; set; }
        public string authorFirstName { get; set; }
        public string authorLastName { get; set; }
        public string type { get; set; }
        public string genre { get; set; }
        public string summary { get; set; }
        public string isAvailable { get; set; }
    }
}
