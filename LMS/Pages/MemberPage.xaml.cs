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
    public partial class MemberPage : Page
    {
        public MemberPage()
        {
            InitializeComponent();
            MemberGrid.ItemsSource = FileManagement.LoadMembers();

            SearchBox.KeyDown += SearchBox_KeyDown;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
        }

        //private void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    PerformSearch();
        //}

        private void PerformSearch()
        {
            //Retrieves search term from the 'searchbox', 'trim() removes any trailing whitespace.
            string searchTerm = SearchBox.Text.Trim();

            //checks if 'searchterm' is not empty. uses 'where' method to search for a match for each member property.
            //Filtered results are converted to a list and assigned to the 'MemberGrid.ItemsSource'
            if (!string.IsNullOrEmpty(searchTerm))
            {
                List<Member> searchResults = FileManagement.LoadMembers().Where(member =>
                    member.Id.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.FirstName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.LastName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.Email.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                MemberGrid.ItemsSource = searchResults;
            }
            else
            {
                MemberGrid.ItemsSource = FileManagement.LoadMembers();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
