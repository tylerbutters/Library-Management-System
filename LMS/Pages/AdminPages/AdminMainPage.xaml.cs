using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LMS.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminHomepage.xaml
    /// </summary>
    public partial class AdminMainPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToLoginPage;
        private bool isOnMemberPage { get; set; }
        private MemberTable memberTable { get; set; }
        private AddMemberPage addMemberPage { get; set; } = new AddMemberPage();
        private ViewMemberPage viewMemberPage { get; set; }
        private BookTable bookTable { get; set; }
        private AddBookPage addBookPage { get; set; } = new AddBookPage();
        private ViewBookPage viewBookPage { get; set; }
        private Member member { get; set; }
        private Book book { get; set; }

        public AdminMainPage()
        {
            InitializeComponent();
            isOnMemberPage = true;
            MemberPageButton.IsEnabled = false;
            BookPageButton.IsEnabled = true;
            memberTable = new MemberTable(new List<Member>());
            PageContent.Content = memberTable;

            addMemberPage.NavigateToMemberPage += SearchMembers;
            addBookPage.NavigateToBookPage += SearchBooks;
        }

        //removes loan from file and updates book's status
        private void ReturnLoan(object sender, Loan loan)
        {
            if (loan is null)
            {
                throw new NullReferenceException();
            }

            member.loanedBooks.Remove(loan);

            List<Book> books = FileManagement.LoadBooks();
            Book returnedBook = books.FirstOrDefault(book => book.id == loan.bookId);

            if (returnedBook is null)
            {
                throw new NullReferenceException();
            }

                returnedBook.isLoaned = false;

            FileManagement.RemoveLoan(loan);
            FileManagement.WriteAllBooks(books);
        }

        private void RenewLoan(object sender, Loan loan)
        {
            if (loan is null)
            {
                throw new NullReferenceException();
            }

            DateTime dueDate = DateTime.Parse($"{MainWindow.currentDate.Year}/" + loan.dueDate);
            TimeSpan difference = dueDate.Subtract(MainWindow.currentDate);

            if (difference.Days < 2)
            {
                member.loanedBooks.Remove(loan);
                FileManagement.RemoveLoan(loan);
                loan.dueDate = dueDate.AddDays(7).ToString("yyyy/MM/dd"); //adds 7 days to loan
                FileManagement.SaveNewLoan(loan);
                loan.dueDate = DateTime.Parse(loan.dueDate).ToString("MM/dd"); //immediately changes format for viewing in program.
                member.loanedBooks.Add(loan);
                MessageBox.Show("Book renewed for 7 days");
            }
            else
            {
                MessageBox.Show("Please wait " + difference.Days + " more days until renewing");
            }
        }

        private void CancelReserve(object sender, Reserve reserve)
        {
            if (reserve is null)
            {
                throw new NullReferenceException();
            }

            List<Book> books = FileManagement.LoadBooks();
            Book reservedBook = books.FirstOrDefault(book => book.id == reserve.bookId);

            if (reservedBook is null)
            {
                throw new NullReferenceException();
            }

            reservedBook.isReserved = false;
            FileManagement.WriteAllBooks(books);
            member.reservedBooks.Remove(reserve);
            FileManagement.RemoveReserve(reserve);
        }

        //creates new loan object, changes its book statuses accordingly, then writes to file.
        private void PlaceLoan(object sender, Reserve reserve)
        {
            if (reserve is null)
            {
                throw new NullReferenceException();
            }

            Loan newLoan = new Loan(reserve.book, member)
            {
                dueDate = MainWindow.currentDate.AddDays(14).ToString("yyyy/MM/dd") // Initially sets the date to proper format
            };
            FileManagement.SaveNewLoan(newLoan);

            newLoan.dueDate = DateTime.Parse(newLoan.dueDate).ToString("MM/dd"); // Immediately changes format for viewing in the program
            member.loanedBooks.Add(newLoan);

            List<Book> books = FileManagement.LoadBooks();
            Book loanedBook = books.FirstOrDefault(book => book.id == newLoan.bookId);

            if (loanedBook is null)
            {
                throw new NullReferenceException();
            }

            loanedBook.isLoaned = true;
            loanedBook.isReserved = false;
            member.reservedBooks.Remove(reserve);
            FileManagement.RemoveReserve(reserve);
            FileManagement.WriteAllBooks(books);
        }

        public void NavigateToViewMemberPage(object sender, Member selectedMember)
        {
            if (selectedMember is null)
            {
                throw new NullReferenceException();
            }

            member = selectedMember;
            viewMemberPage = new ViewMemberPage(selectedMember);
            viewMemberPage.NavigateToMemberPage += SearchMembers;
            viewMemberPage.PlaceLoan += PlaceLoan;
            viewMemberPage.ReturnLoan += ReturnLoan;
            viewMemberPage.RenewLoan += RenewLoan;
            viewMemberPage.CancelReserve += CancelReserve;
            PageContent.Content = viewMemberPage;
        }

        public void NavigateToViewBookPage(object sender, Book selectedBook)
        {
            if (selectedBook is null)
            {
                throw new NullReferenceException();
            }

            book = selectedBook;
            viewBookPage = new ViewBookPage(book);
            viewBookPage.NavigateToBookPage += SearchBooks;
            PageContent.Content = viewBookPage;
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToLoginPage?.Invoke(sender, e);
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (isOnMemberPage)
            {
                PageContent.Content = addMemberPage;
            }
            else
            {
                PageContent.Content = addBookPage;
            }
        }

        private void MemberPageButtonClick(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
            SearchBarLostFocus(sender, e);
            isOnMemberPage = true;
            MemberPageButton.IsEnabled = false;
            BookPageButton.IsEnabled = true;
            BookPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#6FAB4A");
            BookPageButton.Foreground = Brushes.White;
            MemberPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#FDFEEE");
            MemberPageButton.Foreground = Brushes.Black;
            memberTable = new MemberTable(new List<Member>());
            PageContent.Content = memberTable;
        }

        private void BookPageButtonClick(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
            SearchBarLostFocus(sender, e);
            isOnMemberPage = false;
            MemberPageButton.IsEnabled = true;
            BookPageButton.IsEnabled = false;
            BookPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#FDFEEE");
            BookPageButton.Foreground = Brushes.Black;
            MemberPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#6FAB4A");
            MemberPageButton.Foreground = Brushes.White;
            bookTable = new BookTable(new List<Book>());
            PageContent.Content = bookTable;
        }

        private void SearchbarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (isOnMemberPage)
                {
                    SearchMembers(sender, e);
                }
                else
                {
                    SearchBooks(sender, e);
                }
            }
        }

        private void SearchIconButtonClick(object sender, RoutedEventArgs e)
        {
            if (isOnMemberPage)
            {
                SearchMembers(sender, e);
            }
            else
            {
                SearchBooks(sender, e);
            }
        }

        private void SearchMembers(object sender, RoutedEventArgs e)
        {
            string searchInput = SearchBar.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Member> searchResults = FileManagement.LoadMembers().Where(member =>
                    member.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.firstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.lastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.email.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                memberTable = new MemberTable(searchResults);
                memberTable.MemberGrid.SelectionChanged += memberTable.MemberClicked;
                memberTable.NavigateToViewMemberPage += NavigateToViewMemberPage;
                PageContent.Content = memberTable;
            }
        }

        private void SearchBooks(object sender, RoutedEventArgs e)
        {
            string searchInput = SearchBar.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Book> searchResults = FileManagement.LoadBooks().Where(book =>
                    book.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.title.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorFirstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorLastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.subject.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 || 
                    book.summary.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                bookTable = new BookTable(searchResults);
                bookTable.BookGrid.SelectionChanged += bookTable.BookClicked;
                bookTable.NavigateToViewBookPage += NavigateToViewBookPage;
                PageContent.Content = bookTable;
            }
        }

        private void SearchBarGotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBar.Text == "Search...")
            {
                SearchBar.Text = string.Empty;
                SearchBar.FontWeight = FontWeights.Normal;
                SearchBar.Foreground = Brushes.Black;
            }
        }

        private void SearchBarLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBar.Text))
            {
                SearchBar.Text = "Search...";
                SearchBar.Foreground = Brushes.Gray;
            }
        }
    }
}
