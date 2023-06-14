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
        public AdminMemberPage adminMemberPage = new AdminMemberPage();
        public MainWindow()
        {
            InitializeComponent();
            //Display Login in frame at startup
            MainWindowFrame.Content = adminMemberPage;
            NavigateToMemberPage();

            loginPage.NavigateToMemberHomepage += NavigateTo_MemberHomepage;
            loginPage.NaigateToAdminHomepage += NavigateTo_AdminHomepage;
           
        }
        public void NavigateTo_MemberHomepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = memberHomepage; 
        }
        public void NavigateTo_AdminHomepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = adminMemberPage;
        }

        private void NavigateToMemberPage()
        {
            // Create an instance of the MemberPage
            AdminMemberPage adminMemberPage = new AdminMemberPage();

            // Set the Content property of the main window to the MemberPage instance
            this.Content = adminMemberPage;
        }
    }
}
