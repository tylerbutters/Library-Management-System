using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LMS
{
    internal class FileManagement
    {
        public static string MemberFile { get; set; } = @".\Database\memberInformation.csv";
        public static string AdminFile { get; set; } = @".\Database\adminLogins.csv";
        public static string BookFile { get; set; } = @".\Database\bookInformation.csv";
        public static List<Member> LoadMembers()
        {
            string[] rows = File.ReadAllLines(MemberFile);
            IEnumerable<Member> members = from l in rows.Skip(1)
                          let split = l.Split(',')
                          select new Member()
                          {
                              Id = split[0],
                              Pin = split[1],
                              FirstName = split[2],
                              LastName = split[3],
                              Email = split[4],
                          };

            return members.ToList();
        }
    }
}
