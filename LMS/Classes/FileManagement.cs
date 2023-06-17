using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

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

        public static List<Member> LoadMembers()
        {
            return LoadAccounts().OfType<Member>().ToList();
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
                                          tag = split[5],
                                          summary = split[6],
                                          isAvailable = split[7],
                                      };

            return books.ToList();
        }
        public static void SaveBooks(Book newBook)
        {
            string newBookInfo = $"{newBook.id},{newBook.cover},{newBook.title},{newBook.authorFirstName},{newBook.authorLastName},{newBook.tag},{newBook.summary},{newBook.isAvailable}";
            string[] currentRows = File.ReadAllLines(BookFile);
            string[] newRows = currentRows.Append(newBookInfo).ToArray();
            File.WriteAllLines(BookFile, newRows);
            MessageBox.Show("Book Added Successfully!\n");
        }

        public static void SaveMembers(Member newMember)
        {
            string newMemberInfo = $"{newMember.id},{newMember.pin},{newMember.firstName},{newMember.lastName},{newMember.email}";
            string[] currentRows = File.ReadAllLines(AccountFile);
            string[] newRows = currentRows.Append(newMemberInfo).ToArray();
            File.WriteAllLines(AccountFile, newRows);
            MessageBox.Show("Member Added Successfully!\nID: " + newMember.id + "\nPIN: " + newMember.pin);
        }
    }
}