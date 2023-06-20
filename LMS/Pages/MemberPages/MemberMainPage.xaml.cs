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
        public delegate void NavigateToLoginPage(object sender, RoutedEventArgs e);
        public event NavigateToLoginPage navigateToLoginPage;
        private MemberHomePage memberHomePage { get; set; }
        private BookResultsPage bookResultsPage { get; set; }
        public Member memberInfo { get; set; }

        public MemberMainPage(Member loggedInMember)
        {
            InitializeComponent();
            memberInfo = loggedInMember;
            memberHomePage = new MemberHomePage(memberInfo);
            PageContent.Content = memberHomePage;
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
                    book.tag.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();
                bookResultsPage = new BookResultsPage(searchResults, searchInput);

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
            navigateToLoginPage(sender, e);
        }

        private void HomeButtonClick(object send, RoutedEventArgs e)
        {
            PageContent.Content = memberHomePage;
        }
    }
}
