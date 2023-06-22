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
using System.Collections.ObjectModel;
using System.Globalization;

namespace LMS.Pages.MemberPages
{
    /// <summary>
    /// Displays search results
    /// </summary>
    public partial class BookResultsPage : Page
    {
        //Events enable 'MemberHomePage' methods to subscribe to it when 'ReserveButtonClick' is clicked.
        public event EventHandler<Book> PlaceReserve;
        public event EventHandler<Book> CancelReserve;

        //holds the list of search results. It is used to store the books that match the search criteria provided to the page.
        private List<Book> results { get; set; }

        //'searchResults' represents the list of books that match the search criteria, and 'searchInput' is the text used for the search.
        //Inside the constructor, 'searchresults' are assigned to 'results', and the UI elements (ResultsContainer and ResultText) are populated with the search results and search input information     
        public BookResultsPage(List<Book> searchResults, string searchInput)
        {
            InitializeComponent();
            results = searchResults;
            ResultsContainer.ItemsSource = results;
            ResultText.Text = searchInput;
        }

        //Extracts 'selectedBook' from 'DataContext' and invokes 'PlaceReserve' or 'CancelReserve' depending on the books reserve status.S
        //Updates UI using 'ResultsContainer.ItemsSource'.
        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            Book selectedBook = (sender as Button).DataContext as Book;

            if (!selectedBook.isReserved)
            {
                PlaceReserve(this, selectedBook);
                selectedBook.isReserved = true;
            }
            else
            {
                CancelReserve(this, selectedBook);
                selectedBook.isReserved = false;
            }
            ResultsContainer.ItemsSource = null;
            ResultsContainer.ItemsSource = results;
        }
    }
}
