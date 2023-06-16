using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LMS
{
    public class FileManagement
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
                                              id = split[0],
                                              pin = split[1],
                                              firstName = split[2],
                                              lastName = split[3],
                                              email = split[4],
                                          };

            return members.ToList();
        }
        public static List<Book> LoadBooks()
        {
            string[] rows = File.ReadAllLines(BookFile);
            IEnumerable<Book> books = from l in rows.Skip(1)
                                        let split = l.Split(',')
                                        select new Book()
                                        {
                                            id = split[0],
                                            cover = split[1],
                                            title = split[2],
                                            authorFirstName = split[3],
                                            authorLastName = split[4],
                                            type = split[5],
                                            genre = split[6],
                                            summary = split[7],
                                            isAvailable = split[8],
                                        };

            return books.ToList();
        }

    }
}
