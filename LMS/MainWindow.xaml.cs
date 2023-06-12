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
        public Login Login;
        public Member_Homepage Member_Homepage;
        public Admin_Homepage Admin_Homepage;
        
        public MainWindow()
        {
            //Initialize instance of each page
            Login = new Login();
            Member_Homepage = new Member_Homepage();
            Admin_Homepage = new Admin_Homepage();
            //inital project creation
            InitializeComponent();
            //Display Login in frame at startup
            MainWindowFrame.Content = Login;

            Login.Navigateto_member_homepage += NavigateTo_Member_Homepage;
            Login.Navigateto_admin_homepage += NavigateTo_Admin_Homepage;
        }
       public void NavigateTo_Member_Homepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = Member_Homepage; 
        }
        public void NavigateTo_Admin_Homepage(object sender, RoutedEventArgs e)
        {
            MainWindowFrame.Content = Admin_Homepage;
        }

    }
}
