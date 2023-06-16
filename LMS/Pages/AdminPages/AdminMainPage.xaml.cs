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
        public ViewMemberPage viewMemberPage;

        public BookNavbar bookNavbar;
        public BookTable bookTable = new BookTable();
        public AddBookPage addBookPage = new AddBookPage();
        public ViewBookPage viewBookPage;

        public AdminMainPage()
        {
            InitializeComponent();
            memberNavbar = new MemberNavbar(memberTable.memberDataGrid);
            Navbar.Content = memberNavbar;
            PageContent.Content = memberTable;

            memberNavbar.navigateToAdminBookPage += NavigateToAdminBookPage;
            memberNavbar.navigateToAddMemberPage += NavigateToAddMemberPage;
            memberNavbar.navigateToMemberTablePage += NavigateToMemberTablePage;
            memberTable.navigateToViewMemberPage += NavigateToViewMemberPage;

            bookNavbar = new BookNavbar(bookTable.bookDataGrid);
            bookNavbar.navigateToAdminMemberPage += NavigateToAdminMemberPage;
            bookNavbar.navigateToAddBookPage += NavigateToAddBookPage;
            bookNavbar.navigateToBookTablePage += NavigateToBookTablePage;
            bookTable.navigateToViewBookPage += NavigateToViewBookPage;
        }
        public void NavigateToAdminMemberPage(object send, RoutedEventArgs e)
        {
            //memberNavbar = new MemberNavbar(memberTable.memberDataGrid);
            Navbar.Content = memberNavbar;
            PageContent.Content = memberTable;
        }
        public void NavigateToMemberTablePage(object send, RoutedEventArgs e)
        {
            PageContent.Content = memberTable;
        }
        public void NavigateToAddMemberPage(object send, RoutedEventArgs e)
        {
            PageContent.Content = addMemberPage;
        }
        public void NavigateToViewMemberPage(object send, RoutedEventArgs e)
        {
            viewMemberPage = new ViewMemberPage(memberTable.selectedMember);
            PageContent.Content = viewMemberPage;
        }


        public void NavigateToAdminBookPage(object send, RoutedEventArgs e)
        {
            //bookNavbar = new BookNavbar(bookTable.bookDataGrid);
            Navbar.Content = bookNavbar;
            PageContent.Content = bookTable;
        }
        public void NavigateToBookTablePage(object send, RoutedEventArgs e)
        {
            PageContent.Content = bookTable;
        }
        public void NavigateToAddBookPage(object send, RoutedEventArgs e)
        {
            PageContent.Content = addBookPage;
        }

        public void NavigateToViewBookPage(object send, RoutedEventArgs e)
        {
            viewBookPage = new ViewBookPage(bookTable.selectedBook);
            PageContent.Content = viewBookPage;
        }
    }
}
