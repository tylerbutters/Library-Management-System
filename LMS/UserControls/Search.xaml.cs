using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LMS.UserControls
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();

        }

        //executes search operation when the search button is clicked
        private void SearchButton_Click(object sender, RoutedEventArgs e)
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
