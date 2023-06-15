using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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

namespace LMS.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminHomepage.xaml
    /// </summary>
    public partial class AdminMainPage : Page
    {
        public MemberNavbar memberNavbar;
        public MemberTable memberTable = new MemberTable();
        public AddMemberPage addMemberPage = new AddMemberPage();
        
        public AdminMainPage()
        {
            InitializeComponent();
            memberNavbar = new MemberNavbar(memberTable.memberDataGrid);
            Navbar.Content = memberNavbar;
            PageContent.Content = memberTable;

            memberNavbar.navigateToAddMemberPage += NavigateToAddMemberPage;
        }

        public void NavigateToAddMemberPage(object send, RoutedEventArgs e)
        {
            PageContent.Content = addMemberPage;
        }
    }
}
