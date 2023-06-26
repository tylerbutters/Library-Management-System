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

        private const string ReturnLog = @".\Logs\returnLogs.csv";
        private const string NearDueLog = @".\Logs\nearDueLogs.csv";
        private const string OverdueLog = @".\Logs\overdueLogs.csv";

        //private static List<Book> allBooks = LoadBooks();
        //private static List<Loan> allLoans = LoadLoans();
        //private static List<Reserve> allReserves = LoadReserves();


        //READ METHODS

        //loads all accounts, splits the members and admins. the members info is read from file then populates their loan and reserve lists with functions.
        public static List<Account> LoadAccounts()
        {
            if (!File.Exists(AccountFile))
            {
                // Handle the scenario where the account file is missing
                throw new FileNotFoundException("file not found.");
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            List<string> rows = File.ReadAllLines(AccountFile).ToList();

            List<Account> accounts = new List<Account>();
            if (rows.Count >= 2) //incase file is empty
            {
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
            }

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
            List<Reserve> reserves = LoadReserves();
            foreach (Member member in members) //checks what reserves match the members id
            {
                member.reservedBooks = reserves.Where(reserve => reserve.memberId == member.id).ToList();
            }
        }

        //Loads all loans and returns all 'member's loans' that match the logged-in member's i.d
        public static void LoadMembersLoans(List<Member> members)
        {
            List<Loan> loans = LoadLoans();
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
                throw new FileNotFoundException("file not found.");
            }

            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            List<string> rows = File.ReadAllLines(BookFile).ToList();
            List<Book> books = new List<Book>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');

                if (split.Length >= 9) //checks if row has out of range index
                {
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
                else
                {
                    Console.WriteLine($"Invalid row: {row}");
                }
            }

            return books;
        }

        //reads all reserve info from file and adds to a list
        public static List<Reserve> LoadReserves()
        {
            if (!File.Exists(ReserveFile))
            {
                // Handle the scenario where the account file is missing
                throw new FileNotFoundException("file not found.");
            }

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();

            List<Reserve> reserves = new List<Reserve>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');
                string bookId = split[0];
                Book book = LoadBooks().FirstOrDefault(b => b.id == bookId); //links reserve to book

                if (book == null)
                {
                    Console.WriteLine($"Warning: Book with ID {bookId} not found.");
                    continue;
                }
                Reserve reserve = new Reserve(book, new Member())
                {
                    bookId = bookId,
                    memberId = split[1],
                    dateDueBack = split[2],
                    book = book
                };

                //reserve.isAvailableToLoan = !reserve.book.isLoaned; //checks if reserve is available to loan by checking if it's book is loaned or not

                if (reserve.dateDueBack != "Now")
                {
                    reserve.dateDueBack = DateTime.Parse(reserve.dateDueBack).ToString("MM/dd"); //formats the date from yyyy/MM/dd to just MM/dd, unless the date is "now"
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
                throw new FileNotFoundException("file not found.");
            }

            List<string> rows = File.ReadAllLines(LoanFile).ToList();

            List<Loan> loans = new List<Loan>();

            foreach (string row in rows.Skip(1))
            {
                string[] split = row.Split(',');
                string bookId = split[0];
                Book book = LoadBooks().FirstOrDefault(b => b.id == bookId); //links loan to book

                Loan loan = new Loan(book, new Member())
                {
                    memberId = split[1],
                    dateDue = split[2],
                };

                loans.Add(loan);
            }

            CheckLoanDates(loans);

            return loans;
        }

        //checks the due date for the loans and writes logs to files accordingly
        private static void CheckLoanDates(List<Loan> loans)
        {
            DateTime dueDate;
            TimeSpan timeDifference;

            foreach (Loan loan in loans)
            {
                dueDate = DateTime.Parse(loan.dateDue);
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

                loan.dateDue = DateTime.Parse(loan.dateDue).ToString("MM/dd"); //formats the date from yyyy/MM/dd to just MM/dd, unless the date is "now"
            }
        }

        //WRITE METHODS

        public static void WriteAllBooks(List<Book> books)
        {
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
            string bookString = $"{newBook.id},{newBook.cover},{newBook.title.ToLower()},{newBook.authorFirstName.ToLower()},{newBook.authorLastName.ToLower()},{newBook.subject.ToLower()},{newBook.summary.ToLower()},{newBook.isLoaned},{newBook.isReserved}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            rows.Add(bookString);
            File.WriteAllLines(BookFile, rows);
            MessageBox.Show("Book Added Successfully!\n");
        }

        public static void SaveNewMember(Member newMember)
        {
            string memberString = $"{newMember.id},{newMember.pin},{newMember.firstName.ToLower()},{newMember.lastName.ToLower()},{newMember.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.Add(memberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Member Added Successfully!\nID: " + newMember.id + "\nPIN: " + newMember.pin);
        }

        public static void DeleteBook(Book book)
        {
            string bookString = $"{book.id},{book.cover},{book.title.ToLower()},{book.authorFirstName.ToLower()},{book.authorLastName.ToLower()},{book.subject.ToLower()},{book.summary.ToLower()},{book.isLoaned},{book.isReserved}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            rows.RemoveAll(row => row == bookString);
            File.WriteAllLines(BookFile, rows);
            MessageBox.Show("Book Deleted Successfully!\n");
        }

        public static void DeleteMember(Member member)
        {
            string memberString = $"{member.id},{member.pin},{member.firstName.ToLower()},{member.lastName.ToLower()},{member.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            rows.RemoveAll(row => row == memberString);
            File.WriteAllLines(AccountFile, rows);
            MessageBox.Show("Member Deleted Successfully!\n");
        }

        public static void EditMember(Member currentInfo, Member changedInfo)
        {
            string currentInfoString = $"{currentInfo.id},{currentInfo.pin},{currentInfo.firstName.ToLower()},{currentInfo.lastName.ToLower()},{currentInfo.email}";
            string changedInfoString = $"{changedInfo.id},{changedInfo.pin},{changedInfo.firstName.ToLower()},{changedInfo.lastName.ToLower()},{changedInfo.email}";
            List<string> rows = File.ReadAllLines(AccountFile).ToList();
            List<string> newRows = rows.Select(row => row == currentInfoString ? changedInfoString : row).ToList();
            File.WriteAllLines(AccountFile, newRows);
            MessageBox.Show("Details Saved Successfully!\n");
        }

        public static void EditBook(Book currentInfo, Book newInfo)
        {
            string currentInfoString = $"{currentInfo.id},{currentInfo.cover},{currentInfo.title.ToLower()},{currentInfo.authorFirstName.ToLower()},{currentInfo.authorLastName.ToLower()},{currentInfo.subject.ToLower()},{currentInfo.summary.ToLower()},{currentInfo.isLoaned},{currentInfo.isReserved}";
            string changedInfoString = $"{newInfo.id},{newInfo.cover},{newInfo.title.ToLower()},{newInfo.authorFirstName.ToLower()},{newInfo.authorLastName.ToLower()},{newInfo.subject.ToLower()},{newInfo.summary.ToLower()},{newInfo.isLoaned},{newInfo.isReserved}";
            List<string> rows = File.ReadAllLines(BookFile).ToList();
            List<string> newRows = rows.Select(row => row == currentInfoString ? changedInfoString : row).ToList();
            File.WriteAllLines(BookFile, newRows);
            MessageBox.Show("Book Edited Successfully!\n");
        }

        public static void SaveNewReserve(Reserve reserve)
        {
            //converts back to yyyy/MM/yy format unless date is "now"
            string reserveString = $"{reserve.bookId},{reserve.memberId},";
            if (reserve.dateDueBack == "Now")
            {
                reserveString += $"{reserve.dateDueBack}";
            }
            else
            {
                reserveString += $"{MainWindow.currentDate.Year}/{reserve.dateDueBack}";
            }

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();
            rows.Add(reserveString);
            File.WriteAllLines(ReserveFile, rows);
        }

        public static void SaveNewLoan(Loan loan)
        {
            string loanString = $"{loan.bookId},{loan.memberId},{loan.dateDue}";

            List<string> rows = File.ReadAllLines(LoanFile).ToList();
            rows.Add(loanString);
            File.WriteAllLines(LoanFile, rows);
        }

        public static void RemoveReserve(Reserve reserve)
        {
            //converts back to yyyy/MM/yy format unless date is "now"
            string reserveString = $"{reserve.bookId},{reserve.memberId},";
            if (reserve.dateDueBack == "Now")
            {
                reserveString += $"{reserve.dateDueBack}";
            }
            else
            {
                reserveString += $"{MainWindow.currentDate.Year}/{reserve.dateDueBack}";
            }

            List<string> rows = File.ReadAllLines(ReserveFile).ToList();
            rows.Remove(reserveString);
            File.WriteAllLines(ReserveFile, rows);
        }

        public static void RemoveLoan(Loan loan)
        {
            string loanString = $"{loan.bookId},{loan.memberId},{MainWindow.currentDate.Year}/{loan.dateDue}";

            List<string> rows = File.ReadAllLines(LoanFile).ToList();
            rows.Remove(loanString);

            File.AppendAllText(ReturnLog, "Book: " + loan.bookId + ", Loaned by: " + loan.memberId + ", Returned at: " + MainWindow.currentDate);
            File.WriteAllLines(LoanFile, rows);
        }
    }
}