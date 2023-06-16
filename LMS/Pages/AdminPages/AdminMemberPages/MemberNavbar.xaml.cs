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
    public partial class MemberNavbar : Page
    {
        public delegate void NavigateToLogin(object send, RoutedEventArgs e);
        public event NavigateToLogin navigateToLoginPage;
        public delegate void NavigateToAdminBookPage(object send, RoutedEventArgs e);
        public event NavigateToAdminBookPage navigateToAdminBookPage;
        public delegate void NavigateToAddMemberPage(object send, RoutedEventArgs e);
        public event NavigateToAddMemberPage navigateToAddMemberPage;
        public delegate void NavigateToMemberTablePage(object send, RoutedEventArgs e);
        public event NavigateToMemberTablePage navigateToMemberTablePage;

        public DataGrid memberTable;
        public MemberNavbar(DataGrid memberDataGrid)
        {
            InitializeComponent();
            SearchBar.KeyDown += SearchbarKeyDown;
            memberTable = memberDataGrid;
        }

        private void SearchMembers(object sender, RoutedEventArgs e)
        {
            navigateToMemberTablePage(sender, e);
            //Retrieves search term from the 'searchbox', 'trim() removes any trailing whitespace.
            string searchInput = SearchBar.Text.Trim();

            //checks if 'searchterm' is not empty. uses 'where' method to search for a match for each member property.
            //Filtered results are converted to a list and assigned to the 'MemberGrid.ItemsSource'
            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Member> searchResults = FileManagement.LoadMembers().Where(member =>
                    member.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.firstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.lastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.email.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                memberTable.ItemsSource = searchResults;
            }
            else
            {
                memberTable.ItemsSource = FileManagement.LoadMembers();
            }
        }
        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToLoginPage(sender, e);
        }

        private void AddMemberButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToAddMemberPage(sender, e);
        }

        private void BookPageButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToAdminBookPage(sender, e);
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
