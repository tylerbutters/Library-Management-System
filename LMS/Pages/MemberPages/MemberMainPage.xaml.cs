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
    /// Interaction logic for MemberMainPage.xaml
    /// </summary>
    public partial class MemberMainPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToLoginPage;
        private MemberHomePage memberHomePage { get; set; }
        private BookResultsPage bookResultsPage { get; set; }
        public Member member { get; set; }

        public MemberMainPage(Member loggedInMember)
        {
            InitializeComponent();
            member = loggedInMember;
            memberHomePage = new MemberHomePage(member);
            PageContent.Content = memberHomePage;
            memberHomePage.CancelReserve += CancelReserve;
        }
        private void CancelReserve(object sender, Book book)
        {
            Reserve reserve = member.reservedBooks.FirstOrDefault(r => r.bookId == book.id);
            List<Book> books = FileManagement.LoadBooks();
            foreach (Book _book in books)
            {
                if (_book.id == reserve.bookId)
                {
                    _book.isReserved = false;
                }
            }
            FileManagement.WriteAllBooks(books);
            member.reservedBooks.Remove(reserve);
            FileManagement.RemoveReserve(reserve);

        }
        private void PlaceReserve(object sender, Book _book)
        {
            Reserve newReserve = new Reserve(_book, member);
            List<Book> books = FileManagement.LoadBooks();
            foreach (Book book in books)
            {
                if (book.id == newReserve.bookId)
                {
                    book.isReserved = true;
                    if (book.isLoaned)
                    {
                        List<Loan> loans = FileManagement.LoadLoans(new Member());
                        foreach (Loan loan in loans)
                        {
                            if (book.id == loan.bookId)
                            {
                                newReserve.dateDueBack = loan.dateDue;
                            }
                        }
                    }
                }
            }
            member.reservedBooks.Add(newReserve);
            FileManagement.SaveNewReserve(newReserve);
            FileManagement.WriteAllBooks(books);
        }
        private void SearchbarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
                bookResultsPage = new BookResultsPage(searchResults, searchInput);
                bookResultsPage.PlaceReserve += PlaceReserve;
                bookResultsPage.CancelReserve += CancelReserve;
                PageContent.Content = bookResultsPage;
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
        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToLoginPage?.Invoke(sender, e);
        }

        private void HomeButtonClick(object send, RoutedEventArgs e)
        {
            memberHomePage = new MemberHomePage(member);
            memberHomePage.CancelReserve += CancelReserve;
            PageContent.Content = memberHomePage;
        }
    }
}
