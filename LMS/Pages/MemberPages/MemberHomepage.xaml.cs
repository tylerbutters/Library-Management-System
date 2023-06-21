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
    /// Interaction logic for MemberHomePage.xaml
    /// </summary>
    public partial class MemberHomePage : Page
    {
        public event EventHandler<Book> CancelReserve;
        public Member member { get; set; }
        public MemberHomePage(Member loggedInMember)
        {
            InitializeComponent();
            member = loggedInMember;
            ID.Text = member.id;
            FirstName.Text = member.firstName;
            LastName.Text = member.lastName;
            Email.Text = member.email;

            ReservesArea.ItemsSource = member.reservedBooks;

        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Reserve selectedReserve = (sender as Button).DataContext as Reserve;
            selectedReserve.book.isReserved = false;
            CancelReserve(this, selectedReserve.book);

            ReservesArea.ItemsSource = null;
            ReservesArea.ItemsSource = member.reservedBooks;
        }
    }
}
