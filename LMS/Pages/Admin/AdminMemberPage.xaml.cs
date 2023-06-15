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

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for Admin_Homepage.xaml
    /// </summary>
    public partial class AdminMemberPage : Page
    {
        public delegate void NavigateToLogin(object send, RoutedEventArgs e);
        public event NavigateToLogin navigateToLoginPage;
        public AdminMemberPage()
        {
            InitializeComponent();
            MemberGrid.ItemsSource = FileManagement.LoadMembers();

            SearchBox.KeyDown += SearchbarKeyDown;
        }
        private void SearchbarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchMembers();
            }
        }

        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    PerformSearch();
        //}

        private void SearchMembers()
        {
            //Retrieves search term from the 'searchbox', 'trim() removes any trailing whitespace.
            string searchInput = SearchBox.Text.Trim();

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

                MemberGrid.ItemsSource = searchResults;
            }
            else
            {
                MemberGrid.ItemsSource = FileManagement.LoadMembers();
            }
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToLoginPage(sender, e);
        }

        private void AddMemberButtonClick(object sender, RoutedEventArgs e)
        {
            //add member
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
