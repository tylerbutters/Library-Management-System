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
    /// Interaction logic for BookResultsPage.xaml
    /// </summary>
    public partial class BookResultsPage : Page
    {
        public event EventHandler<Book> PlaceReserve;
        public event EventHandler<Book> CancelReserve;

        private List<Book> results { get; set; }

        public BookResultsPage(List<Book> searchResults, string searchInput, Member member)
        {
            InitializeComponent();
            results = searchResults;

            foreach (Book book in results)
            {
                Reserve reserve = member.reservedBooks.FirstOrDefault(r => r.bookId == book.id);
                if (reserve != null)
                {
                    book.isReserved = true;
                }
            }

            ResultsContainer.ItemsSource = results;
            ResultText.Text = searchInput;
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            Button reserveButton = (Button)sender;
            Book selectedBook = (Book)reserveButton.DataContext;

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
