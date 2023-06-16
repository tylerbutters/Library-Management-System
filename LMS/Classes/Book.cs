using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    public class Book
    {
        public string id { get; set; }
        public string cover { get; set; } = "NO_COVER";
        public string title { get; set; }
        public string authorFirstName { get; set; }
        public string authorLastName { get; set; }
        public string tag { get; set; }
        public string summary { get; set; }
        public string isAvailable { get; set; } = "true";
    }
}
