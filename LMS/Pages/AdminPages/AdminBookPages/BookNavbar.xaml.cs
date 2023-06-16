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

namespace LMS.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for MemberNavbar.xaml
    /// </summary>
    public partial class BookNavbar : Page
    {
        public delegate void NavigateToLogin(object send, RoutedEventArgs e);
        public event NavigateToLogin navigateToLoginPage;
        public delegate void NavigateToAdminMemberPage(object send, RoutedEventArgs e);
        public event NavigateToAdminMemberPage navigateToAdminMemberPage;
        public delegate void NavigateToAddBookPage(object send, RoutedEventArgs e);
        public event NavigateToAddBookPage navigateToAddBookPage;
        public delegate void NavigateToBookTablePage(object send, RoutedEventArgs e);
        public event NavigateToBookTablePage navigateToBookTablePage;

        public DataGrid bookTable;
        public BookNavbar(DataGrid bookDataGrid)
        {
            InitializeComponent();
            SearchBar.KeyDown += SearchbarKeyDown;
            bookTable = bookDataGrid;
        }

        private void SearchMembers(object sender, RoutedEventArgs e)
        {
            navigateToBookTablePage(sender, e);
            //Retrieves search term from the 'searchbox', 'trim() removes any trailing whitespace.
            string searchInput = SearchBar.Text.Trim();

            //checks if 'searchterm' is not empty. uses 'where' method to search for a match for each member property.
            //Filtered results are converted to a list and assigned to the 'MemberGrid.ItemsSource'
            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Book> searchResults = FileManagement.LoadBooks().Where(book =>
                    book.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorFirstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorLastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.type.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.genre.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.isAvailable.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                bookTable.ItemsSource = searchResults;
            }
            else
            {
                bookTable.ItemsSource = FileManagement.LoadMembers();
            }
        }
        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToLoginPage(sender, e);
        }
        private void AddBookButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToAddBookPage(sender, e);
        }
        private void MemberPageButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToAdminMemberPage(sender, e);
        }
        private void SearchbarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchMembers(sender, e);
            }
        }
        private void SearchIconButtonClick(object sender, RoutedEventArgs e)
        {
            SearchMembers(sender, e);
        }
    }
}
