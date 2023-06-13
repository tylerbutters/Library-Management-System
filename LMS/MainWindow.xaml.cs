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
        public LoginPage loginPage;
        public MemberHomepage memberHomepage;
        public AdminHomepage adminHomepage;
        
        public MainWindow()
        {
            //Initialize instance of each page
            loginPage = new LoginPage();
            memberHomepage = new MemberHomepage();
            adminHomepage = new AdminHomepage();
            //inital project creation
            InitializeComponent();
            //Display Login in frame at startup
            MainWindowFrame.Content = loginPage;

            loginPage.navigateTo_MemberHomepage += NavigateTo_MemberHomepage;
            loginPage.navigateTo_AdminHomepage += NavigateTo_AdminHomepage;
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
