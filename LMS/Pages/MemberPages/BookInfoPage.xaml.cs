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
    /// Interaction logic for BookInfoPage.xaml
    /// </summary>
    public partial class BookInfoPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateBackToResults;
        public event EventHandler<Book> PlaceReserve;
        private Book book { get; set; }
        public BookInfoPage(Book _book)
        {
            InitializeComponent();
            book = _book;

            DataContext = book;
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            Book selectedBook = (sender as Button).DataContext as Book;

            PlaceReserve?.Invoke(sender, selectedBook);
            book.isReserved = true;

            DataContext = null;
            DataContext = book;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateBackToResults?.Invoke(sender, e);
        }
    }
}
