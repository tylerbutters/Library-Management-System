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
        private static string dateString { get; set; } = "2023/05/13";
        public static DateTime currentDate { get; set; } = DateTime.Parse(dateString);
        private LoginPage loginPage { get; set; }
        private MemberMainPage memberMainPage { get; set; }
        private AdminMainPage adminMainPage { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            loginPage = new LoginPage();
            MainWindowFrame.Content = loginPage;

            loginPage.NavigateToMemberMainPage += NavigateToMemberMainPage;
            loginPage.NavigateToAdminMainPage += NavigateToAdminMainPage;
        }
        public void NavigateToMemberMainPage(object sender, RoutedEventArgs e)
        {
            memberMainPage = new MemberMainPage(loginPage.loggedInMember);
            memberMainPage.NavigateToLoginPage += NavigateToLoginPage;
            MainWindowFrame.Content = memberMainPage;
        }
        public void NavigateToAdminMainPage(object sender, RoutedEventArgs e)
        {
            adminMainPage = new AdminMainPage();
            adminMainPage.NavigateToLoginPage += NavigateToLoginPage;
            MainWindowFrame.Content = adminMainPage;
        }
        public void NavigateToLoginPage(object sender, RoutedEventArgs e)
        {
            loginPage = new LoginPage();
            loginPage.NavigateToMemberMainPage += NavigateToMemberMainPage;
            loginPage.NavigateToAdminMainPage += NavigateToAdminMainPage;
            MainWindowFrame.Content = loginPage;
        }
    }
}
