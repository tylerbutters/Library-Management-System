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
        public const string memberFile = @".\Database\UserPasswords.csv";
        public const string adminFile = @".\Database\AdminPasswords.csv";

        public delegate void NavigateTo_MemberHomepage(object sender, RoutedEventArgs e);
        public event NavigateTo_MemberHomepage navigateTo_MemberHomepage;
        public delegate void NavigateTo_AdminHomepage(object sender, RoutedEventArgs e);
        public event NavigateTo_AdminHomepage navigateTo_AdminHomepage;
        
        public LoginPage()
        {
            InitializeComponent();
        }
        //Login Check checks the entered email and password against the given list and returns true/false.
        public bool CheckLoginDetails(string file)
        {
            //use if/else loop to check admin CSV first, else member CSV
            string idInput = IDInput.Text;
            string pinInput = PinInput.Text;

            string[] rows = File.ReadAllLines(file);
            List<string> idData = rows.Skip(1).Select(row => row.Split(',')[0]).ToList();
            List<string> pinData = rows.Skip(1).Select(row => row.Split(',')[1]).ToList();

            if (idInput == "" || pinInput == "")
            {
                MessageBox.Show("Please enter both username and password");
            }

            //returns conditional statement
            return idData.Contains(idInput) && pinData.Contains(pinInput);
        }
        //Login button runs "Login_Check()" against user list first, then admin list.
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLoginDetails(memberFile))
            {
                navigateTo_MemberHomepage(sender, e);
            }
            else if (CheckLoginDetails(adminFile))
            {
                //MessageBox.Show("Logging in as Admin");
                navigateTo_AdminHomepage(sender, e);
            }
            else
            {
                MessageBox.Show("Username/Password incorrect");
            }
        }
    }
}
