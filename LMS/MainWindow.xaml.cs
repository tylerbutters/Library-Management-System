using LMS.Pages;
using LMS.Pages.AdminPages;
using LMS.Pages.MemberPages;
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

namespace LMS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LoginPage loginPage;
        public MemberMainPage memberMainPage;
        public AdminMainPage adminMainPage;

        public MainWindow()
        {
            InitializeComponent();
            loginPage = new LoginPage();
            MainWindowFrame.Content = loginPage;

            loginPage.navigateToMemberMainPage += NavigateToMemberMainPage;
            loginPage.navigateToAdminMainPage += NavigateToAdminMainPage;
        }


        public void NavigateToMemberMainPage(object sender, RoutedEventArgs e)
        {
            memberMainPage = new MemberMainPage(loginPage.loggedInMember);
            memberMainPage.navigateToLoginPage += NavigateToLoginPage;
            MainWindowFrame.Content = memberMainPage;
        }


        public void NavigateToAdminMainPage(object sender, RoutedEventArgs e)
        {
            adminMainPage = new AdminMainPage();
            adminMainPage.navigateToLoginPage += NavigateToLoginPage;
            MainWindowFrame.Content = adminMainPage;
        }

        public void NavigateToLoginPage(object sender, RoutedEventArgs e)
        {
            loginPage = new LoginPage();
            loginPage.navigateToMemberMainPage += NavigateToMemberMainPage;
            loginPage.navigateToAdminMainPage += NavigateToAdminMainPage;
            MainWindowFrame.Content = loginPage;
            
        }
    }


}
