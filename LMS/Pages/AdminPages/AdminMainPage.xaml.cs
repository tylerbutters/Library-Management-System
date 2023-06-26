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
        private bool isOnMemberPage { get; set; } = true;
        private MemberTable memberTable { get; set; }
        private AddMemberPage addMemberPage { get; set; } = new AddMemberPage();
        private ViewMemberPage viewMemberPage { get; set; }
        private BookTable bookTable { get; set; }
        private AddBookPage addBookPage { get; set; } = new AddBookPage();
        private ViewBookPage viewBookPage { get; set; }
        private Member member { get; set; }

        public AdminMainPage()
        {
            InitializeComponent();
            memberTable = new MemberTable(new List<Member>());
            PageContent.Content = memberTable;

            addMemberPage.NavigateToMemberPage += SearchMembers;
            addBookPage.NavigateToBookPage += SearchBooks;
        }

        //removes loan from file and updates book's status
        private void ReturnLoan(object sender, Loan loan)
        {
            member.loanedBooks.Remove(loan);

            List<Book> books = FileManagement.LoadBooks();
            foreach (Book book in books)
            {
                if (book.id == loan.bookId)
                {
                    book.isLoaned = false;
                }
            }
            FileManagement.RemoveLoan(loan);
            FileManagement.WriteAllBooks(books);
        }

        //creates new loan object, changes its book statuses accordingly, then writes to file.
        private void PlaceLoan(object sender, Reserve reserve)
        {
            Loan newLoan = new Loan(reserve.book, member)
            {
                dateDue = MainWindow.currentDate.AddDays(14).ToString("yyyy/MM/dd") //initally sets the date to proper format
            };
            FileManagement.SaveNewLoan(newLoan);

            newLoan.dateDue = DateTime.Parse(newLoan.dateDue).ToString("MM/dd"); //immediately changes format for viewing in program.
            member.loanedBooks.Add(newLoan);
            List<Book> books = FileManagement.LoadBooks();
            foreach (Book book in books)
            {
                if (book.id == newLoan.bookId)
                {
                    book.isLoaned = true;
                    book.isReserved = false;
                }
            }
            member.reservedBooks.Remove(reserve);
            FileManagement.RemoveReserve(reserve);
            FileManagement.WriteAllBooks(books);
        }

        public void NavigateToViewMemberPage(object sender, RoutedEventArgs e)
        {
            member = memberTable.selectedMember;
            viewMemberPage = new ViewMemberPage(memberTable.selectedMember);
            viewMemberPage.NavigateToMemberPage += SearchMembers;
            viewMemberPage.PlaceLoan += PlaceLoan;
            viewMemberPage.ReturnLoan += ReturnLoan;
            PageContent.Content = viewMemberPage;
        }

        public void NavigateToViewBookPage(object sender, RoutedEventArgs e)
        {
            viewBookPage = new ViewBookPage(bookTable.selectedBook);
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
                memberTable.MemberGrid.SelectionChanged += memberTable.MemberDataGridSelectionChanged;
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
                    book.subject.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                bookTable = new BookTable(searchResults);
                bookTable.BookGrid.SelectionChanged += bookTable.BookDataGridSelectionChanged;
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
