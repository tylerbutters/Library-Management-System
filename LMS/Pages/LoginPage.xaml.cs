using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        //Event handlers for navigating to Member, or Admin homepages.
        public delegate void NavigateToMemberHomepage(object sender, RoutedEventArgs e);
        public event NavigateToMemberHomepage navigateToMemberHomepage;
        public delegate void NavigateToAdminMainPage(object sender, RoutedEventArgs e);
        public event NavigateToAdminMainPage navigateToAdminMainPage;
        public LoginPage()
        {
            InitializeComponent();

            IDInput.KeyDown += LoginFeildsKeyDown;
            PINInput.KeyDown += LoginFeildsKeyDown;
        }
        public Account AuthenticateLoginDetails()
        {
            List<Account> accounts = FileManagement.LoadAccounts();
            string idInput = IDInput.Text;
            string pinInput = PINInput.Text;

            foreach (Account account in accounts)
            {
                if (account.id == idInput && account.pin == pinInput)
                {
                    return account;
                }
            }

            return null;
        }
        //checks the account type and navigates them to their respective page
        private void HandleAuthentication(object sender, RoutedEventArgs e)
        {
            Account authenticatedAccount = AuthenticateLoginDetails();

            if (authenticatedAccount != null)
            {
                if (authenticatedAccount is Member)
                {
                    navigateToMemberHomepage(sender, e);
                }
                else if (authenticatedAccount is Admin)
                {
                    navigateToAdminMainPage(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Cannot find account with those details");
            }
        }

        private void LoginFeildsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleAuthentication(sender, e);
            }
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            HandleAuthentication(sender, e);
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
