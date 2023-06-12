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
    public partial class Login : Page
    {
        const string userLink = @".\Database\UserPasswords.csv";
        const string adminLink = @".\Database\AdminPasswords.csv";
        public delegate void NavigateTo_Member_Homepage(object sender, RoutedEventArgs e);
        public event NavigateTo_Member_Homepage Navigateto_member_homepage;
        public delegate void NavigateTo_Admin_Homepage(object sender, RoutedEventArgs e);
        public event NavigateTo_Admin_Homepage Navigateto_admin_homepage;
        //Login Check checks the entered email and password against the given list and returns true/false.
        public Login()
        {
            InitializeComponent();
        }
        public bool Login_Check(string databaseLink)
        {
            string inputEmail = LoginEmail.Text;
            string inputPassword = LoginPassword.Text;
            string[] lines = File.ReadAllLines(databaseLink);
            var UserNameData = lines.Skip(1).Select(row => row.Split(',')[0]).ToList();
            var PasswordData = lines.Skip(1).Select(row => row.Split(',')[1]).ToList();
            if (inputEmail == "" || inputPassword == "")
            {
                MessageBox.Show("Please enter both username and password");
                return false;
            }
            else if (UserNameData.Contains(inputEmail) && PasswordData.Contains(inputPassword))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //Login button runs "Login_Check()" against user list first, then admin list.
        private void LogIn_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Login_Check(userLink))
            {
                Navigateto_member_homepage(sender, e);
            }
            else if (Login_Check(adminLink))
            {
                MessageBox.Show("Logging in as Admin");
                Navigateto_admin_homepage(sender, e);
            }
            else
            {
                MessageBox.Show("Username/Password incorrect");
                return;
            }
        }
    }
}
