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
    /// Contains methods for the Loan, Reserve and member information displayed within the MemberMainPage.
    /// </summary>
    public partial class MemberHomePage : Page
    {
        public event EventHandler<Reserve> CancelReserve;
        public event EventHandler<Loan> RenewLoan;
        private Member member { get; set; }
        public MemberHomePage(Member loggedInMember)
        {
            InitializeComponent();

            if (loggedInMember is null)
            {
                throw new NullReferenceException();
            }

            member = loggedInMember;

            ID.Text = member.id;
            FirstName.Text = member.firstName;
            LastName.Text = member.lastName;
            Email.Text = member.email;

            ReservesArea.ItemsSource = member.reservedBooks;
            LoansArea.ItemsSource = member.loanedBooks;
        }

        //If the cancel button is clicked, CancelReserve event is raised by invoking the event. Also updates the UI using ItemsSource.
        //Passes the 'selectedReserve.book' as an argument so the 'CancelReserve' method knows which book was selected.
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Reserve selectedReserve = (sender as Button).DataContext as Reserve;
                if (selectedReserve is null)
                {
                    throw new NullReferenceException();
                }
                CancelReserve?.Invoke(sender, selectedReserve);

                ReservesArea.ItemsSource = null;
                ReservesArea.ItemsSource = member.reservedBooks;
            }
        }

        private void RenewButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to renew?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Loan selectedLoan = (sender as Button).DataContext as Loan;
                if (selectedLoan is null)
                {
                    throw new NullReferenceException();
                }
                RenewLoan?.Invoke(sender, selectedLoan);

                LoansArea.ItemsSource = null;
                LoansArea.ItemsSource = member.loanedBooks;
            }
        }
    }
}
