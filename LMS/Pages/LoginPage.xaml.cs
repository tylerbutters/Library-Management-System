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
        public string Id;
        public string Pin;

        public delegate void NavigateTo_MemberHomepage(object sender, RoutedEventArgs e);
        public event NavigateTo_MemberHomepage NavigateToMemberHomepage;
        public delegate void NavigateTo_AdminHomepage(object sender, RoutedEventArgs e);
        public event NavigateTo_AdminHomepage NaigateToAdminHomepage;
        public LoginPage()
        {
            InitializeComponent();

            IDInput.KeyDown += LoginFeilds_KeyDown;
            PinInput.KeyDown += LoginFeilds_KeyDown;
        }

        private void LoginFeilds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CheckLoginDetails(FileManagement.MemberFile))
                {
                    NavigateToMemberHomepage(sender, e);
                }
                else if (CheckLoginDetails(FileManagement.AdminFile))
                {
                    //MessageBox.Show("Logging in as Admin");
                    NaigateToAdminHomepage(sender, e);
                }
                else
                {
                    MessageBox.Show("Cannot find account with those details");
                }
            }
        }

        //Login Check checks the entered email and password against the given list and returns true/false.
        public bool CheckLoginDetails(string file)
        {
            string idInput = IDInput.Text;
            string pinInput = PinInput.Text;
            string[] rows = File.ReadAllLines(file);
            //member and admin classes inherit from account class. code just puts the id and pin data into the account object.
            //then compares to the inputted data. since admin and member both inherit from account this function can be used for both cases.
            IEnumerable<Account> accounts = (from l in rows.Skip(1)
                                 let split = l.Split(',')
                                 select new Account()
                                 {
                                     id = split[0],
                                     pin = split[1],
                                 }).ToList();
           
            foreach (Account account in accounts){
                if (account.id == idInput && account.pin == pinInput)
                {
                    return true;
                }
            }
            return false;
        }
        //Login button runs "Login_Check()" against user list first, then admin list.
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLoginDetails(FileManagement.MemberFile))
            {
                NavigateToMemberHomepage(sender, e);
            }
            else if (CheckLoginDetails(FileManagement.AdminFile))
            {
                //MessageBox.Show("Logging in as Admin");
                NaigateToAdminHomepage(sender, e);
            }
            else
            {
                MessageBox.Show("Cannot find account with those details");
            }
        }
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
