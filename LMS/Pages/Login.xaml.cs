using System;
using System.Collections.Generic;
using System.IO;
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

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public delegate void NavigateTo_Member_Homepage(object sender, RoutedEventArgs e);
        public event NavigateTo_Member_Homepage Navigateto_member_homepage;
        public delegate void NavigateTo_Admin_Homepage(object sender, RoutedEventArgs e);
        public event NavigateTo_Admin_Homepage Navigateto_admin_homepage;
        public Login()
        {
            InitializeComponent();
        }

        
        //Login Button checks database and then runs NavigateTo_Member_Homepage if login successful
        private void LogIn_Button_Click(object sender, RoutedEventArgs e)
        {
            //use if/else loop to check admin CSV first, else member CSV

            string inputEmail = LoginEmail.Text;
            string inputPassword = LoginPassword.Text;
            string[] lines = File.ReadAllLines(@".\Database\UserPasswords.csv");
            var UserNameData = lines.Skip(1).Select(row => row.Split(',')[0]).ToList();
            var PasswordData = lines.Skip(1).Select(row => row.Split(',')[1]).ToList();

            if (inputEmail == "" || inputPassword == "")
            {
                MessageBox.Show("Please enter both username and password");
                return;
            }
            else if (UserNameData.Contains(inputEmail) && PasswordData.Contains(inputPassword))
            {
                
                Navigateto_member_homepage(sender, e);
                return;
            }
            //else
            //{
                
            //    string[] adminlines = File.ReadAllLines(@".\Database\AdminPasswords.csv");
            //    var adminUserNameData = adminlines.Skip(1).Select(row => row.Split(',')[0]).ToList();
            //    var adminPasswordData = adminlines.Skip(1).Select(row => row.Split(',')[1]).ToList();

            //    if (adminUserNameData.Contains(inputEmail) && PasswordData.Contains(inputPassword))
            //    {
            //        MessageBox.Show("Logging in as Admin");
            //        Navigateto_admin_homepage(sender, e);
            //        return;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Please enter a valid username and password");
            //    }
            //}

            

        }
    }
}
