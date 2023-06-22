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
            List<string> rows = File.ReadAllLines(AccountFile).ToList();

            List<Account> accounts = new List<Account>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                string accountId = split[0];
                string accountPin = split[1];

                if (accountId.StartsWith("M"))
                {
                    Member member = new Member
                    {
                        id = accountId,
                        pin = accountPin,
                        firstName = textInfo.ToTitleCase(split[2]),
                        lastName = textInfo.ToTitleCase(split[3]),
                        email = split[4]
                    };

                    member.reservedBooks = LoadMembersReserves(member);
                    accounts.Add(member);
                }
                else
                {
                    Admin admin = new Admin
                    {
                        id = accountId,
                        pin = accountPin
                    };

                    accounts.Add(admin);
                }
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
            List<string> rows = File.ReadAllLines(BookFile).ToList();

            List<Book> books = new List<Book>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                Book book = new Book()
                {
                    id = split[0],
                    cover = split[1],
                    title = textInfo.ToTitleCase(split[2]),
                    authorFirstName = textInfo.ToTitleCase(split[3]),
                    authorLastName = textInfo.ToTitleCase(split[4]),
                    subject = textInfo.ToTitleCase(split[5]),
                    summary = split[6],
                    isAvailable = bool.Parse(split[7]),
                };

                books.Add(book);
            }

            return books;
        }

        public static List<Reserve> LoadReserves(Member member)
        {
            List<string> rows = File.ReadAllLines(ReserveFile).ToList();

            List<Reserve> reserves = new List<Reserve>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');
                string bookId = split[0];
                Book book = LoadBookById(bookId);

                Reserve reserve = new Reserve(book, member)
                {
                    bookId = bookId,
                    memberId = split[1],
                    dateReserved = split[2],
                    dateAvailable = split[3],
                    book = book
                };

                reserves.Add(reserve);
            }

            return reserves;
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
            string bookString = $"{newBook.id},{newBook.cover},{newBook.title.ToLower()},{newBook.authorFirstName.ToLower()},{newBook.authorLastName.ToLower()},{newBook.subject.ToLower()},{newBook.summary.ToLower()},{newBook.isAvailable}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            rows.Add(bookString);
            File.WriteAllLines(BookFile, rows);
            MessageBox.Show("Book Added Successfully!\n");
        }

        public static void SaveNewMember(Member newMember)
        {
            string memberString = $"{newMember.isAdmin},{newMember.id},{newMember.pin},{newMember.firstName.ToLower()},{newMember.lastName.ToLower()},{newMember.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.Add(memberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Member Added Successfully!\nID: " + newMember.id + "\nPIN: " + newMember.pin);
        }

        public static void DeleteBook(Book book)
        {
            string bookString = $"{book.id},{book.cover},{book.title.ToLower()},{book.authorFirstName.ToLower()},{book.authorLastName.ToLower()},{book.subject.ToLower()},{book.summary.ToLower()},{book.isAvailable}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            rows.Remove(bookString);
            File.WriteAllLines(BookFile, rows);
            MessageBox.Show("Book Deleted Successfully!\n");
        }

        public static void DeleteMember(Member member)
        {
            string memberString = $"{member.isAdmin},{member.id},{member.pin},{member.firstName.ToLower()},{member.lastName.ToLower()},{member.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.Remove(memberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Member Deleted Successfully!\n");
        }

        public static void EditMember(Member currntMemberinfo, Member newMemberInfo)
        {
            string currentMemberString = $"{currntMemberinfo.isAdmin},{currntMemberinfo.id},{currntMemberinfo.pin},{currntMemberinfo.firstName.ToLower()},{currntMemberinfo.lastName.ToLower()},{currntMemberinfo.email}";
            string newMemberString = $"{newMemberInfo.isAdmin},{newMemberInfo.id},{newMemberInfo.pin},{newMemberInfo.firstName.ToLower()},{newMemberInfo.lastName.ToLower()},{newMemberInfo.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.Remove(currentMemberString);
            rows.Add(newMemberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Details Saved Successfully!\n");
        }

        public static void SaveNewReserve(Reserve reserve)
        {
            string reserveString = $"{reserve.bookId},{reserve.memberId},{reserve.dateReserved},{reserve.dateAvailable}";

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();
            rows.Add(reserveString);
            File.WriteAllLines(ReserveFile, rows);
        }

        public static void RemoveReserve(Reserve reserve)
        {
            string reserveString = $"{reserve.bookId},{reserve.memberId},{reserve.dateReserved},{reserve.dateAvailable}";

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();
            rows.Remove(reserveString);
            File.WriteAllLines(ReserveFile, rows);
        }

        public static void RemoveLoan(Loan loan)
        {
            string loanString = $"{loan.bookId},{loan.memberId},{loan.dateLoaned},{loan.dateDue}";

            List<string> rows = File.ReadAllLines(LoanFile).ToList();
            rows.Remove(loanString);
            File.WriteAllLines(LoanFile, rows);
        }

    }
}