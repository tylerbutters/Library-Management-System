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
        public LoginPage loginPage = new LoginPage();
        public MemberMainPage memberMainPage = new MemberMainPage(new Member());
        public AdminMainPage adminMainPage = new AdminMainPage();

        public MainWindow()
        {
            InitializeComponent();

            MainWindowFrame.Content = loginPage;
            
            loginPage.navigateToMemberMainPage += NavigateToMemberMainPage;
            loginPage.navigateToAdminMainPage += NavigateToAdminMainPage;
            adminMainPage.navigateToLoginPage += NavigateToLoginPage;
            memberMainPage.navigateToLoginPage += NavigateToLoginPage;
        }

        public void NavigateToMemberMainPage(object sender, RoutedEventArgs e)
        {
            memberMainPage = new MemberMainPage(loginPage.loggedInMember);
            MainWindowFrame.Content = memberMainPage;
        }

        public void NavigateToAdminMainPage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = adminMainPage;
        }

        public void NavigateToLoginPage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = loginPage;
        }
    }


}
