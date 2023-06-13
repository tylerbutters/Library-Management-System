using LMS.Pages;
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
        public AdminHomepage adminHomepage = new AdminHomepage();
        public MainWindow()
        {
            InitializeComponent();
            
            //MainWindowFrame.Content = loginPage;
            //MainWindowFrame.Content = memberHomepage;
            MainWindowFrame.Content = adminHomepage;

            loginPage.NavigateToMemberHomepage += NavigateTo_MemberHomepage;
            loginPage.NaigateToAdminHomepage += NavigateTo_AdminHomepage;
        }
        public void NavigateTo_MemberHomepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = memberHomepage; 
        }
        public void NavigateTo_AdminHomepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = adminHomepage;
        }
    }
}
