using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Globalization;


namespace LMS
{
    public class FileManagement
    {
        private const string AccountFile = @".\Databases\accountInformation.csv";
        private const string BookFile = @".\Databases\bookInformation.csv";
        private const string ReserveFile = @".\Databases\reserveInformation.csv";
        private const string LoanFile = @".\Databases\loanInformation.csv";
        public static List<Account> LoadAccounts()
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string[] rows = File.ReadAllLines(AccountFile);

            List<Account> accounts = rows.Skip(1).Select(l =>
               {
                   string[] split = l.Split(',');

                   Account account;
                   //bool isAdmin = split[0] == "true";

                   if (split[0].Contains("M"))
                   {
                       Member member = new Member
                       {
                           id = split[0],
                           pin = split[1],
                           firstName = textInfo.ToTitleCase(split[2]),
                           lastName = textInfo.ToTitleCase(split[3]),
                           email = split[4],
                       };

                       account = member;
                   }
                   else
                   {
                       account = new Admin();
                   }

                   account.id = split[0];
                   account.pin = split[1];

                   return account;
               }).ToList();

            foreach (Member member in accounts.OfType<Member>())
            {
                member.reservedBooks = LoadMembersReserves(member);
            }


            return accounts;
        }

        public static List<Member> LoadMembers()
        {
            return LoadAccounts().OfType<Member>().ToList();
        }
        public static List<Book> LoadBooks()
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            string[] rows = File.ReadAllLines(BookFile);

            IEnumerable<Book> books = from l in rows.Skip(1)
                                      let split = l.Split(',')
                                      select new Book()
                                      {
                                          id = split[0],
                                          cover = split[1],
                                          title = textInfo.ToTitleCase(split[2]),
                                          authorFirstName = textInfo.ToTitleCase(split[3]),
                                          authorLastName = textInfo.ToTitleCase(split[4]),
                                          tag = textInfo.ToTitleCase(split[5]),
                                          summary = split[6],
                                          isAvailable = split[7],
                                      };
            return books.ToList();
        }
        public static List<Reserve> LoadReserves(Member member)
        {
            string[] rows = File.ReadAllLines(ReserveFile);

            IEnumerable<Reserve> reserves = from l in rows.Skip(1)
                                            let split = l.Split(',')
                                            let bookId = split[0]
                                            let book = LoadBookById(bookId)
                                            select new Reserve(book, member)
                                            {
                                                bookId = bookId,
                                                memberId = split[1],
                                                dateReserved = split[2],
                                                dateAvailable = split[3],
                                                book = book,
                                            };


            return reserves.ToList();
        }
        public static List<Reserve> LoadMembersReserves(Member member)
        {
            List<Reserve> reserves = LoadReserves(member);
            List<Reserve> membersReserves = new List<Reserve>();
            foreach (Reserve reserve in reserves)
            {
                if (reserve.memberId == member.id)
                {
                    membersReserves.Add(reserve);
                }
            }
            return membersReserves;
        }
        private static Book LoadBookById(string bookId)
        {
            List<Book> books = LoadBooks();
            foreach (Book book in books)
            {
                if (book.id == bookId)
                {
                    return book;
                }
            }
            return null;
        }
        public static void SaveNewBook(Book newBook)
        {
            string newBookInfo = $"{newBook.id},{newBook.cover},{newBook.title.ToLower()},{newBook.authorFirstName.ToLower()},{newBook.authorLastName.ToLower()},{newBook.tag.ToLower()},{newBook.summary.ToLower()},{newBook.isAvailable}";
            string[] currentRows = File.ReadAllLines(BookFile);
            string[] newRows = currentRows.Append(newBookInfo).ToArray();
            File.WriteAllLines(BookFile, newRows);
            MessageBox.Show("Book Added Successfully!\n");
        }
        public static void SaveNewMember(Member newMember)
        {
            string newmember = $"{newMember.isAdmin},{newMember.id},{newMember.pin},{newMember.firstName.ToLower()},{newMember.lastName.ToLower()},{newMember.email}";
            string[] currentRows = File.ReadAllLines(AccountFile);
            string[] newRows = currentRows.Append(newmember).ToArray();
            File.WriteAllLines(AccountFile, newRows);
            MessageBox.Show("Member Added Successfully!\nID: " + newMember.id + "\nPIN: " + newMember.pin);
        }
        public static void SaveNewReserve(Reserve reserve)
        {
            string reserveString = $"{reserve.bookId},{reserve.memberId},{reserve.dateReserved},{reserve.dateAvailable}";

            string[] currentRows = File.ReadAllLines(ReserveFile);
            string[] newRows = currentRows.Append(reserveString).ToArray();
            File.WriteAllLines(ReserveFile, newRows);
        }
        public static void RemoveReserve(Reserve reserve)
        {
            string reserveString = $"{reserve.bookId},{reserve.memberId},{reserve.dateReserved},{reserve.dateAvailable}";

            string[] currentRows = File.ReadAllLines(ReserveFile);
            string[] newRows = currentRows.Where(row => row != reserveString).ToArray();
            File.WriteAllLines(ReserveFile, newRows);
        }
    }
}