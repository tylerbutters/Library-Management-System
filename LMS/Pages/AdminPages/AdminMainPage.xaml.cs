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
        private MemberTable memberTable { get; set; } = new MemberTable();
        private AddMemberPage addMemberPage { get; set; } = new AddMemberPage();
        private ViewMemberPage viewMemberPage { get; set; }
        private BookTable bookTable { get; set; } = new BookTable();
        private AddBookPage addBookPage { get; set; } = new AddBookPage();
        private ViewBookPage viewBookPage { get; set; }
        private DataGrid bookDataGrid { get; set; }
        private DataGrid memberDataGrid { get; set; }
        private Member member { get; set; }
        public AdminMainPage()
        {
            InitializeComponent();
            PageContent.Content = memberTable;

            memberDataGrid = memberTable.memberDataGridInfo;
            bookDataGrid = bookTable.bookDataGridInfo;

            memberTable.NavigateToViewMemberPage += NavigateToViewMemberPage;
            bookTable.NavigateToViewBookPage += NavigateToViewBookPage;
            addMemberPage.NavigateToMemberPage += MemberPageButtonClick;
            addBookPage.NavigateToBookPage += BookPageButtonClick;
            
        }
        private void PlaceLoan(object sender,Reserve reserve)
        {
            Loan newLoan = new Loan(reserve.book,member);
            member.loanedBooks.Add(newLoan);
            member.reservedBooks.Remove(reserve);
            FileManagement.RemoveReserve(reserve);
            FileManagement.SaveNewLoan(newLoan);
        }
        public void NavigateToViewMemberPage(object sender, RoutedEventArgs e)
        {
            member = memberTable.selectedMember;
            viewMemberPage = new ViewMemberPage(member);
            viewMemberPage.PlaceLoan += PlaceLoan;
            PageContent.Content = viewMemberPage;
        }
        public void NavigateToViewBookPage(object sender, RoutedEventArgs e)
        {

            viewBookPage = new ViewBookPage(bookTable.selectedBook);
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
            PageContent.Content = memberTable;
            string searchInput = SearchBar.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Member> searchResults = FileManagement.LoadMembers().Where(member =>
                    member.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.firstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.lastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.email.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                memberDataGrid.ItemsSource = searchResults;
            }
            else
            {
                memberDataGrid.ItemsSource = null;
            }
        }
        private void SearchBooks(object sender, RoutedEventArgs e)
        {
            PageContent.Content = bookTable;
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

                bookDataGrid.ItemsSource = searchResults;
            }
            else
            {
                bookDataGrid.ItemsSource = null;
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
