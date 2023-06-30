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

        private const string ReturnLog = @".\NotificationLogs\returnLogs.csv";
        private const string NearDueLog = @".\NotificationLogs\nearDueLogs.csv";
        private const string OverdueLog = @".\NotificationLogs\overdueLogs.csv";

        //READ METHODS

        //loads all accounts, splits the members and admins. the members info is read from file then populates their loan and reserve lists with functions.
        public static List<Account> LoadAccounts()
        {
            if (!File.Exists(AccountFile)) // Handle the scenario where the account file is missing
            {
                throw new FileNotFoundException("file not found.", AccountFile);
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            List<string> rows = File.ReadAllLines(AccountFile).ToList();

            List<Account> accounts = new List<Account>();

            if (!(rows.Count >= 2)) //incase file is empty
            {
                throw new FormatException("file empty");
            }

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                if (split[0].StartsWith("M")) //if Id starts with M
                {
                    Member member = new Member
                    {
                        id = split[0],
                        pin = split[1],
                        firstName = textInfo.ToTitleCase(split[2]),
                        lastName = textInfo.ToTitleCase(split[3]),
                        email = split[4]
                    };


                    accounts.Add(member);
                }
                else
                {
                    Admin admin = new Admin
                    {
                        id = split[0],
                        pin = split[1]
                    };

                    accounts.Add(admin);
                }
            }
            LoadMembersReserves(accounts.OfType<Member>().ToList());
            LoadMembersLoans(accounts.OfType<Member>().ToList());

            return accounts;
        }

        //simply a shorthand 
        public static List<Member> LoadMembers()
        {
            return LoadAccounts().OfType<Member>().ToList();
        }

        //Loads all reserves and returns all 'member's Reserves' that match the logged-in member's i.d
        public static void LoadMembersReserves(List<Member> members)
        {
            if (members is null)
            {
                throw new NullReferenceException();
            }

            List<Reserve> reserves = LoadReserves();
            if (reserves is null)
            {
                return;
            }

            foreach (Member member in members) //checks what reserves match the members id
            {
                member.reservedBooks = reserves.Where(reserve => reserve.memberId == member.id).ToList();
            }
        }

        //Loads all loans and returns all 'member's loans' that match the logged-in member's i.d
        public static void LoadMembersLoans(List<Member> members)
        {
            if (members is null)
            {
                throw new NullReferenceException();
            }

            List<Loan> loans = LoadLoans();

            if (loans is null)
            {
                return;
            }

            foreach (Member member in members) //checks what reserves match the members id
            {
                member.loanedBooks = loans.Where(reserve => reserve.memberId == member.id).ToList();
            }
        }

        //reads book data from file, splits the data into individual properties and adds them to a list. The list of Book objects is then returned as the result of the method.
        public static List<Book> LoadBooks()
        {
            if (!File.Exists(BookFile))
            {
                // Handle the scenario where the account file is missing
                throw new FileNotFoundException("file not found.", BookFile);
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            List<string> rows = File.ReadAllLines(BookFile).ToList();
            List<Book> books = new List<Book>();

            if (!(rows.Count >= 2))
            {
                throw new FormatException("file empty");
            }

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                if (split.Length != 9)
                {
                    throw new FormatException("file format is wrong");
                }

                Book book = new Book()
                {
                    id = split[0],
                    cover = split[1],
                    title = textInfo.ToTitleCase(split[2]),
                    authorFirstName = textInfo.ToTitleCase(split[3]),
                    authorLastName = textInfo.ToTitleCase(split[4]),
                    subject = textInfo.ToTitleCase(split[5]),
                    summary = split[6],
                    isLoaned = bool.Parse(split[7]),
                    isReserved = bool.Parse(split[8])
                };

                books.Add(book);
            }

            return books;
        }

        //reads all reserve info from file and adds to a list
        public static List<Reserve> LoadReserves()
        {
            if (!File.Exists(ReserveFile)) // Handle the scenario where the account file is missing
            {
                throw new FileNotFoundException("file not found.", ReserveFile);
            }

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();

            if (!(rows.Count >= 2))
            {
                Console.WriteLine("File empty");
                return null;
            }

            List<Reserve> reserves = new List<Reserve>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                if (split.Length != 3)
                {
                    throw new FormatException("file format is wrong");
                }

                string bookId = split[0];
                Book book = LoadBooks().FirstOrDefault(b => b.id == bookId); //links reserve to book

                if (book is null)
                {
                    throw new NullReferenceException();
                }

                Reserve reserve = new Reserve(book, new Member())
                {
                    memberId = split[1],
                    estAvailDate = split[2],
                };

                if (reserve.estAvailDate != "Now")
                {
                    reserve.estAvailDate = DateTime.Parse(reserve.estAvailDate).ToString("MM/dd"); //formats the date from yyyy/MM/dd to just MM/dd, unless the date is "now"
                }

                reserves.Add(reserve);
            }

            return reserves;
        }

        //reads info from file and adds to list
        public static List<Loan> LoadLoans()
        {
            if (!File.Exists(LoanFile))
            {
                // Handle the scenario where the account file is missing
                throw new FileNotFoundException("file not found.", LoanFile);
            }

            List<string> rows = File.ReadAllLines(LoanFile).ToList();

            if (!(rows.Count >= 2))
            {
                Console.WriteLine("File empty");
                return null;
            }

            List<Loan> loans = new List<Loan>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                if (split.Length != 3)
                {
                    throw new FormatException("file format is wrong");
                }

                string bookId = split[0];
                Book book = LoadBooks().FirstOrDefault(b => b.id == bookId); //links loan to book

                if (book is null)
                {
                    throw new NullReferenceException();
                }

                Loan loan = new Loan(book, new Member())
                {
                    memberId = split[1],
                    dueDate = split[2],
                };

                loans.Add(loan);
            }

            CheckLoanDates(loans);

            return loans;
        }

        //checks the due date for the loans and writes logs to files accordingly
        private static void CheckLoanDates(List<Loan> loans)
        {
            if (loans is null)
            {
                throw new NullReferenceException();
            }
            if (loans.Count is 0)
            {
                return;
            }

            DateTime dueDate;
            TimeSpan timeDifference;

            foreach (Loan loan in loans)
            {
                dueDate = DateTime.Parse(loan.dueDate);
                timeDifference = dueDate.Subtract(MainWindow.currentDate);

                if (dueDate <= MainWindow.currentDate) //checks if duedate is past currentdate/due
                {
                    string overdueMessage = "Book: " + loan.bookId + ", Loaned by: " + loan.memberId + ", Overdue at: " + MainWindow.currentDate;
                    if (!File.ReadAllText(OverdueLog).Contains(overdueMessage)) //checks if the message is already in the log
                    {
                        File.AppendAllText(OverdueLog, overdueMessage + Environment.NewLine);
                    }
                }
                else if (timeDifference.TotalDays <= 2 && timeDifference.TotalDays >= 0) //checks if book due date is within 2 days.
                {
                    string nearDueMessage = "Book: " + loan.bookId + ", Loaned by: " + loan.memberId + ", Due on: " + dueDate.ToString("MM/dd/yyyy");
                    if (!File.ReadAllText(NearDueLog).Contains(nearDueMessage)) //checks if the message is already in the log
                    {
                        File.AppendAllText(NearDueLog, nearDueMessage + Environment.NewLine);
                    }
                }

                loan.dueDate = DateTime.Parse(loan.dueDate).ToString("MM/dd"); //formats the date from yyyy/MM/dd to just MM/dd, unless the date is "now"
            }
        }

        //WRITE METHODS

        public static void WriteAllBooks(List<Book> books)
        {
            if (books is null)
            {
                throw new NullReferenceException();
            }
            
            List<string> bookRows = new List<string>();
            string headerRow = "id,cover,title,authorFirstName,authorLastName,subject,summary,isLoaned,isReserved";
            bookRows.Add(headerRow);

            foreach (Book book in books)
            {
                string bookRow = $"{book.id},{book.cover},{book.title.ToLower()},{book.authorFirstName.ToLower().ToLower()},{book.authorLastName.ToLower()},{book.subject.ToLower()},{book.summary.ToLower()},{book.isLoaned},{book.isReserved}";
                bookRows.Add(bookRow);
            }

            File.WriteAllLines(BookFile, bookRows);
        }

        public static void SaveNewBook(Book newBook)
        {
            if (newBook is null)
            {
                throw new NullReferenceException();
            }

            string bookString = $"{newBook.id},{newBook.cover},{newBook.title.ToLower()},{newBook.authorFirstName.ToLower()},{newBook.authorLastName.ToLower()},{newBook.subject.ToLower()},{newBook.summary.ToLower()},{newBook.isLoaned},{newBook.isReserved}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            rows.Add(bookString);
            File.WriteAllLines(BookFile, rows);
            MessageBox.Show("Book Added Successfully!\n");
        }

        public static void SaveNewMember(Member newMember)
        {
            if (newMember is null)
            {
                throw new NullReferenceException();
            }

            string memberString = $"{newMember.id},{newMember.pin},{newMember.firstName.ToLower()},{newMember.lastName.ToLower()},{newMember.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.Add(memberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Member Added Successfully!\nID: " + newMember.id + "\nPIN: " + newMember.pin);
        }

        public static void DeleteBook(Book book)
        {
            if (book is null)
            {
                throw new NullReferenceException();
            }

            string bookString = $"{book.id},{book.cover},{book.title.ToLower()},{book.authorFirstName.ToLower()},{book.authorLastName.ToLower()},{book.subject.ToLower()},{book.summary.ToLower()},{book.isLoaned},{book.isReserved}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            rows.RemoveAll(row => row == bookString);
            File.WriteAllLines(BookFile, rows);
            MessageBox.Show("Book Deleted Successfully!\n");
        }

        public static void DeleteMember(Member member)
        {
            if (member is null)
            {
                throw new NullReferenceException();
            }

            string memberString = $"{member.id},{member.pin},{member.firstName.ToLower()},{member.lastName.ToLower()},{member.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.RemoveAll(row => row == memberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Member Deleted Successfully!\n");
        }

        public static void EditMember(Member currentInfo, Member changedInfo)
        {
            if (currentInfo is null || changedInfo is null)
            {
                throw new NullReferenceException();
            }

            string currentInfoString = $"{currentInfo.id},{currentInfo.pin},{currentInfo.firstName.ToLower()},{currentInfo.lastName.ToLower()},{currentInfo.email}";
            string changedInfoString = $"{changedInfo.id},{changedInfo.pin},{changedInfo.firstName.ToLower()},{changedInfo.lastName.ToLower()},{changedInfo.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            List<string> newRows = rows.Select(row => row == currentInfoString ? changedInfoString : row).ToList();
            File.WriteAllLines(AccountFile, newRows);
            MessageBox.Show("Details Saved Successfully!\n");
        }

        public static void EditBook(Book currentInfo, Book changedInfo)
        {
            if (currentInfo is null || changedInfo is null)
            {
                throw new NullReferenceException();
            }

            string currentInfoString = $"{currentInfo.id},{currentInfo.cover},{currentInfo.title.ToLower()},{currentInfo.authorFirstName.ToLower()},{currentInfo.authorLastName.ToLower()},{currentInfo.subject.ToLower()},{currentInfo.summary.ToLower()},{currentInfo.isLoaned},{currentInfo.isReserved}";
            string changedInfoString = $"{changedInfo.id},{changedInfo.cover},{changedInfo.title.ToLower()},{changedInfo.authorFirstName.ToLower()},{changedInfo.authorLastName.ToLower()},{changedInfo.subject.ToLower()},{changedInfo.summary.ToLower()},{changedInfo.isLoaned},{changedInfo.isReserved}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            List<string> newRows = rows.Select(row => row == currentInfoString ? changedInfoString : row).ToList();
            File.WriteAllLines(BookFile, newRows);
            MessageBox.Show("Book Edited Successfully!\n");
        }

        public static void SaveNewReserve(Reserve reserve)
        {
            if (reserve is null)
            {
                throw new NullReferenceException();
            }

            //converts back to yyyy/MM/yy format unless date is "now"
            string reserveString = $"{reserve.bookId},{reserve.memberId},";
            if (reserve.estAvailDate == "Now")
            {
                reserveString += $"{reserve.estAvailDate}";
            }
            else
            {
                reserveString += $"{MainWindow.currentDate.Year}/{reserve.estAvailDate}";
            }

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();
            rows.Add(reserveString);
            File.WriteAllLines(ReserveFile, rows);
        }

        public static void SaveNewLoan(Loan loan)
        {
            if (loan is null)
            {
                throw new NullReferenceException();
            }

            string loanString = $"{loan.bookId},{loan.memberId},{loan.dueDate}";

            List<string> rows = File.ReadAllLines(LoanFile).ToList();
            rows.Add(loanString);
            File.WriteAllLines(LoanFile, rows);
        }

        public static void RemoveReserve(Reserve reserve)
        {
            if (reserve is null)
            {
                throw new NullReferenceException();
            }

            //converts back to yyyy/MM/yy format unless date is "now"
            string reserveString = $"{reserve.bookId},{reserve.memberId},";
            if (reserve.estAvailDate == "Now")
            {
                reserveString += $"{reserve.estAvailDate}";
            }
            else
            {
                reserveString += $"{MainWindow.currentDate.Year}/{reserve.estAvailDate}";
            }

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();
            rows.Remove(reserveString);
            File.WriteAllLines(ReserveFile, rows);
        }

        public static void RemoveLoan(Loan loan)
        {
            if (loan is null)
            {
                throw new NullReferenceException();
            }

            string loanString = $"{loan.bookId},{loan.memberId},{MainWindow.currentDate.Year}/{loan.dueDate}";

            List<string> rows = File.ReadAllLines(LoanFile).ToList();
            rows.Remove(loanString);

            File.AppendAllText(ReturnLog, "Book: " + loan.bookId + ", Loaned by: " + loan.memberId + ", Returned at: " + MainWindow.currentDate + Environment.NewLine);
            File.WriteAllLines(LoanFile, rows);
        }
    }
}