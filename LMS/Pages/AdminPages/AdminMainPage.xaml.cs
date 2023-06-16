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
        public bool isOnMemberPage = true;

        public MemberTable memberTable = new MemberTable();
        public AddMemberPage addMemberPage = new AddMemberPage();
        public ViewMemberPage viewMemberPage;

        public BookTable bookTable = new BookTable();
        public AddBookPage addBookPage = new AddBookPage();
        public ViewBookPage viewBookPage;

        public DataGrid bookDataGrid;
        public DataGrid memberDataGrid;
        public AdminMainPage()
        {
            InitializeComponent();
            SearchBar.KeyDown += SearchbarKeyDown;
            memberDataGrid = memberTable.memberDataGridInfo;
            bookDataGrid = bookTable.bookDataGridInfo;

            PageContent.Content = memberTable;

            memberTable.navigateToViewMemberPage += NavigateToViewMemberPage;
            bookTable.navigateToViewBookPage += NavigateToViewBookPage;
        }

        public delegate void NavigateToLogin(object send, RoutedEventArgs e);
        public event NavigateToLogin navigateToLoginPage;

        private void SearchMembers(object sender, RoutedEventArgs e)
        {
            PageContent.Content = memberTable;
            string searchInput = SearchBar.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Member> searchResults = FileManagement.LoadMembers().Where(member =>
                    member.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.firstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.lastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    member.email.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                memberDataGrid.ItemsSource = searchResults;
            }
            else
            {
                memberDataGrid.ItemsSource = FileManagement.LoadMembers();
            }
        }
        private void SearchBooks(object sender, RoutedEventArgs e)
        {
            PageContent.Content = bookTable;
            string searchInput = SearchBar.Text.Trim();

            if (!string.IsNullOrEmpty(searchInput))
            {
                List<Book> searchResults = FileManagement.LoadBooks().Where(book =>
                    book.id.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorFirstName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.authorLastName.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.type.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.genre.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    book.isAvailable.IndexOf(searchInput, StringComparison.OrdinalIgnoreCase) >= 0
                ).ToList();

                bookDataGrid.ItemsSource = searchResults;
            }
            else
            {
                bookDataGrid.ItemsSource = FileManagement.LoadMembers();
            }
        }
        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToLoginPage(sender, e);
        }
        public void NavigateToViewMemberPage(object send, RoutedEventArgs e)
        {
            viewMemberPage = new ViewMemberPage(memberTable.selectedMember);
            PageContent.Content = viewMemberPage;
        }
        public void NavigateToViewBookPage(object send, RoutedEventArgs e)
        {
            viewBookPage = new ViewBookPage(bookTable.selectedBook);
            PageContent.Content = viewBookPage;
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            if (isOnMemberPage)
            {
                PageContent.Content = addMemberPage;
            }
            else
            {
                PageContent.Content = addBookPage;
            }
        }
        private void MemberPageButtonClick(object sender, RoutedEventArgs e)
        {
            isOnMemberPage = true;
            BookPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#6FAB4A");
            BookPageButton.Foreground = new SolidColorBrush(Colors.White);
            MemberPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#FDFEEE");
            MemberPageButton.Foreground = new SolidColorBrush(Colors.Black);
            PageContent.Content = memberTable;
        }
        private void BookPageButtonClick(object sender, RoutedEventArgs e)
        {
            isOnMemberPage = false;
            BookPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#FDFEEE");
            BookPageButton.Foreground = new SolidColorBrush(Colors.Black);
            MemberPageButton.Background = (Brush)new BrushConverter().ConvertFrom("#6FAB4A");
            MemberPageButton.Foreground = new SolidColorBrush(Colors.White);
            PageContent.Content = bookTable;
        }
        private void SearchbarKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (isOnMemberPage)
                {
                    SearchMembers(sender, e);
                }
                else
                {
                    SearchBooks(sender, e);
                }
            }
        }
        private void SearchIconButtonClick(object sender, RoutedEventArgs e)
        {
            if (isOnMemberPage)
            {
                SearchMembers(sender, e);
            }
            else
            {
                SearchBooks(sender, e);
            }
        }
    }
}
