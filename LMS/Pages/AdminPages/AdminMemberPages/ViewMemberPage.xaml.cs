using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for ViewMemberPage.xaml
    /// </summary>
    public partial class ViewMemberPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToMemberPage;
        public event EventHandler<Reserve> PlaceLoan;
        public event EventHandler<Reserve> CancelReserve;
        public event EventHandler<Loan> ReturnLoan;
        public event EventHandler<Loan> RenewLoan;
        private bool isEditing { get; set; }
        private bool isConfirmed { get; set; }
        private Member member { get; set; }

        //Member info from login and member class's are passed through parameters and displayed in each text example.
        public ViewMemberPage(Member _member)
        {
            InitializeComponent();

            isEditing = false;
            isConfirmed = false;
            member = _member;

            ID.Text = member.id;
            PIN.Text = member.pin;
            FirstName.Text = member.firstName;
            LastName.Text = member.lastName;
            Email.Text = member.email;
            ReservesArea.ItemsSource = member.reservedBooks;
            LoansArea.ItemsSource = member.loanedBooks;
        }

        private void EditCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                isEditing = true;
                isConfirmed = false;
                SaveButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                EditCancelButton.Content = "Cancel";

                PIN.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                PIN.Padding = new Thickness(20, 0, 0, 0);
                PIN.IsReadOnly = false;
                FirstName.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                FirstName.Padding = new Thickness(20, 0, 0, 0);
                FirstName.IsReadOnly = false;
                LastName.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                LastName.Padding = new Thickness(20, 0, 0, 0);
                LastName.IsReadOnly = false;
                Email.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                Email.Padding = new Thickness(20, 0, 0, 0);
                Email.IsReadOnly = false;
            }
            else
            {
                isEditing = false;
                SaveButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
                EditCancelButton.Content = "Edit";

                PIN.Background = Brushes.Transparent;
                PIN.Padding = new Thickness(0);
                PIN.IsReadOnly = true;
                FirstName.Background = Brushes.Transparent;
                FirstName.Padding = new Thickness(0);
                FirstName.IsReadOnly = true;
                LastName.Background = Brushes.Transparent;
                LastName.Padding = new Thickness(0);
                LastName.IsReadOnly = true;
                Email.Background = Brushes.Transparent;
                Email.Padding = new Thickness(0);
                Email.IsReadOnly = true;

                ID.Text = member.id;
                PIN.Text = member.pin;
                FirstName.Text = member.firstName;
                LastName.Text = member.lastName;
                Email.Text = member.email;
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Member changedInfo = new Member { id = ID.Text, pin = PIN.Text, firstName = FirstName.Text, lastName = LastName.Text, email = Email.Text };
            FileManagement.EditMember(member, changedInfo);

            EditCancelButtonClick(sender, e);
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isConfirmed)
            {
                isConfirmed = true;
                DeleteButton.Content = "Confirm?";
            }
            else
            {
                FileManagement.DeleteMember(member);
                isConfirmed = false;
                NavigateToMemberPage?.Invoke(sender, e);
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Reserve selectedReserve = (sender as Button).DataContext as Reserve;
                CancelReserve?.Invoke(sender, selectedReserve);
                ReservesArea.ItemsSource = null;
                ReservesArea.ItemsSource = member.reservedBooks;
            }
        }

        private void LoanButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to loan?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Reserve selectedReserve = (sender as Button).DataContext as Reserve;
                PlaceLoan?.Invoke(sender, selectedReserve);

                LoansArea.ItemsSource = null;
                LoansArea.ItemsSource = member.loanedBooks;
                ReservesArea.ItemsSource = null;
                ReservesArea.ItemsSource = member.reservedBooks;
            }
        }

        private void ReturnButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to return?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Loan selectedLoan = (sender as Button).DataContext as Loan;
                ReturnLoan?.Invoke(sender, selectedLoan);

                LoansArea.ItemsSource = null;
                LoansArea.ItemsSource = member.loanedBooks;
            }
        }

        private void RenewButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to renew?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                Loan selectedLoan = (sender as Button).DataContext as Loan;
                RenewLoan?.Invoke(sender, selectedLoan);

                LoansArea.ItemsSource = null;
                LoansArea.ItemsSource = member.loanedBooks;
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToMemberPage?.Invoke(sender, e);
        }
    }
}

