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
        }
        //Login Check checks the entered email and password against the given list and returns true/false.
        public bool CheckLoginDetails(string file)
        {
            string idInput = IDInput.Text;
            string pinInput = PinInput.Text;
            string[] rows = File.ReadAllLines(file);
            IEnumerable<LoginPage> logins = (from l in rows.Skip(1)
                                 let split = l.Split(',')
                                 select new LoginPage()
                                 {
                                     Id = split[0],
                                     Pin = split[1],
                                 }).ToList();
           
            foreach (LoginPage login in logins){
                if (login.Id == idInput && login.Pin == pinInput)
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
