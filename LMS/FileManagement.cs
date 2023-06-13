using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS
{
    class FileManagement
    {
        private static string MemberFile = @".\Database\memberInformation.csv";
        private static string AdminFile = @".\Database\adminLogins.csv";
        private static string BookFile = @".\Database\bookInformation.csv";
        public static string memberFile 
        { 
            get { return MemberFile; } 
            set { MemberFile = value; } 
        }
        public static string adminFile
        {
            get { return AdminFile; }
            set { AdminFile = value; }
        }
        public static string bookFile
        {
            get { return BookFile; }
            set { BookFile = value; }
        }
    }
}
