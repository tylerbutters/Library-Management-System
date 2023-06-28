using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace LMS.Pages.MemberPages
{
    /// <summary>
    /// Contains methods for interacting with search bar, logout button and home button.
    /// </summary>
    public partial class MemberMainPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToLoginPage;
        private MemberHomePage memberHomePage { get; set; }
        private BookResultsPage bookResultsPage { get; set; }
        private BookInfoPage bookInfoPage { get; set; }
        private Member member { get; set; }

        public MemberMainPage(Member loggedInMember)
        {
            InitializeComponent();
            member = loggedInMember;
            memberHomePage = new MemberHomePage(member);
            PageContent.Content = memberHomePage;
            memberHomePage.RenewLoan += RenewLoan;
            memberHomePage.CancelReserve += CancelReserve;
            memberHomePage.NavigateToBookInfoPage += NavigateToBookInfoPage;
        }

        private void NavigateToBookInfoPage(object sender, Book book)
        {
            bookInfoPage = new BookInfoPage(book, member);
            //bookInfoPage.NavigateBackToResults += SearchBooks;
            bookInfoPage.PlaceReserve += PlaceReserve;
            bookInfoPage.CancelReserve += CancelReserve;
            PageContent.Content = bookInfoPage;
        }
        //initializes a new 'Reserve' object with the clicked 'book' and 'member'
        //loads every book and checks which book matches the i.d of the selected book, updates the selected one with '_book.isReserved = true;' and saves changes using 'FileManagement'.
        private void PlaceReserve(object sender, Book _book)
        {
            if (_book is null)
            {
                throw new NullReferenceException();
            }

            Reserve newReserve = new Reserve(_book, member);
            List<Book> books = FileManagement.LoadBooks();
            Book reservedBook = books.FirstOrDefault(book => book.id == newReserve.bookId);

            if (reservedBook is null)
            {
                throw new NullReferenceException();
            }

            reservedBook.isReserved = true;

            if (reservedBook.isLoaned)
            {
                Loan loan = FileManagement.LoadLoans().FirstOrDefault(l => l.bookId == reservedBook.id);

                if (loan is null)
                {
                    throw new NullReferenceException();
                }

                newReserve.estAvailDate = loan.dueDate;
            }
            else
            {
                newReserve.estAvailDate = "Now";
            }

            member.reservedBooks.Add(newReserve);
            FileManagement.SaveNewReserve(newReserve);
            FileManagement.WriteAllBooks(books);
        }


        //Searches for the book associated with the passed 'book' object
        //updates the 'isReserved' status in 'bookInformation.csv' and Removes book from 'member.reservedBooks'
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
                reservedBook.isReserved = false;
            }

            reservedBook.isReserved = false;
            FileManagement.WriteAllBooks(books);
            member.reservedBooks.Remove(reserve);
            FileManagement.RemoveReserve(reserve);
        }

        private void RenewLoan(object sender, Loan loan)
        {
            if (loan is null)
            {
                throw new NullReferenceException();
            }

            DateTime dueDate = DateTime.Parse($"{MainWindow.currentDate.Year}/" + loan.dueDate);
            TimeSpan difference = dueDate.Subtract(MainWindow.currentDate);

            if (difference.Days >= 2)
            {
                MessageBox.Show("Please wait " + difference.Days + " more days until renewing");
            }
            else
            {
                member.loanedBooks.Remove(loan);
                FileManagement.RemoveLoan(loan);
                loan.dueDate = dueDate.AddDays(7).ToString("yyyy/MM/dd"); //adds 7 days to loan
                FileManagement.SaveNewLoan(loan);
                loan.dueDate = DateTime.Parse(loan.dueDate).ToString("MM/dd"); //immediately changes format for viewing in program.
                member.loanedBooks.Add(loan);
                MessageBox.Show("Book renewed for 7 days");
            }
        }

        private void SearchbarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                SearchBooks(sender, e);
            }
        }

        private void SearchIconButtonClick(object sender, RoutedEventArgs e)
        {
            SearchBooks(sender, e);
        }

        private void SearchBooks(object sender, RoutedEventArgs e)
        {
            string searchInput = SearchBar.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Book> searchResults = FileManagement.LoadBooks().Where(book =>
                    book.title.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorFirstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorLastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.subject.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                bookResultsPage = new BookResultsPage(searchResults, searchInput, member);
                bookResultsPage.PlaceReserve += PlaceReserve;
                bookResultsPage.CancelReserve += CancelReserve;
                bookResultsPage.NavigateToBookInfoPage += NavigateToBookInfoPage;
                PageContent.Content = bookResultsPage;
            }
        }

        private void SearchBarGotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBar.Text is "Search...")
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
        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToLoginPage?.Invoke(sender, e);
        }

        private void HomeButtonClick(object send, RoutedEventArgs e)
        {
            memberHomePage = new MemberHomePage(member);
            memberHomePage.CancelReserve += CancelReserve;
            memberHomePage.RenewLoan += RenewLoan;
            memberHomePage.NavigateToBookInfoPage += NavigateToBookInfoPage;
            PageContent.Content = memberHomePage;
        }
    }
}
