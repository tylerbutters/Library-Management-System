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
        public static string AccountFile { get; set; } = @".\Database\accountInformation.csv";
        public static string BookFile { get; set; } = @".\Database\bookInformation.csv";
        public static List<Account> LoadAccounts()
        {
            string[] rows = File.ReadAllLines(AccountFile);

            List<Account> accounts = rows.Skip(1).Select(l =>
               {
                   string[] split = l.Split(',');

                   Account account;
                   bool isAdmin = split[0] == "true";

                   if (isAdmin)
                   {
                       account = new Admin();
                   }
                   else
                   {
                       account = new Member()
                       {
                           firstName = split[3],
                           lastName = split[4],
                           email = split[5],
                       };
                   }

                   account.id = split[1];
                   account.pin = split[2];

                   return account;
               }).ToList();

            return accounts;
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
