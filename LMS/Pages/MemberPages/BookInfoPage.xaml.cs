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
        
        public event EventHandler<Book> PlaceReserve;
        public event EventHandler<Reserve> CancelReserve;
        private Book book { get; set; }
        private Member member { get; set; }
        public BookInfoPage(Book _book, Member _member)
        {
            InitializeComponent();

            if (_book is null || _member is null)
            {
                throw new NullReferenceException();
            }

            book = _book;
            member = _member;

            DataContext = book;
        }

        private void ReserveCancelButtonClick(object sender, RoutedEventArgs e)
        {
            
            if (!book.isReservedByUser) //button says "reserve"
            {
                PlaceReserve?.Invoke(sender, book);
                book.isReservedByUser = true;
            }
            else //button says "cancel"
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButton.YesNo);

                if (result is MessageBoxResult.Yes)
                {
                    Reserve reservedBook = member.reservedBooks.FirstOrDefault(reserve => reserve.bookId == book.id);
                    if (reservedBook is null)
                    {
                        throw new NullReferenceException();
                    }
                    CancelReserve?.Invoke(sender, reservedBook);
                    book.isReservedByUser = false;
                }
            }

            DataContext = null;
            DataContext = book;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
