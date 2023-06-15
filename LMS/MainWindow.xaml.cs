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
        public MemberHomepage memberHomepage = new MemberHomepage();
        public AdminMainPage AdminMainPage = new AdminMainPage();
        public MainWindow()
        {
            InitializeComponent();
            //Display Login in frame at startup
            MainWindowFrame.Content = AdminMainPage;
            //MainWindowFrame.Content = loginPage;

            loginPage.navigateToMemberHomepage += NavigateToMemberHomepage;
            loginPage.navigateToAdminMainPage += NavigateToAdminHomepage;
            //AdminMainPage.navigateToLoginPage += NavigateToLoginPage;
        }
        public void NavigateToMemberHomepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = memberHomepage;
        }
        public void NavigateToAdminHomepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = AdminMainPage;
        }

        public void NavigateToLoginPage(object send, RoutedEventArgs e)
        {
            MainWindowFrame.Content = loginPage;
        }
    }
}
