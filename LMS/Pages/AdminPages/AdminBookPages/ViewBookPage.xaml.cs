using System;
using System.Collections.Generic;
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

namespace LMS.Pages.AdminPages
{
    public partial class ViewBookPage : Page
    {
        public Book bookInfo { get; set; }
        public ViewBookPage(Book book)
        {
            bookInfo = book;
            InitializeComponent();

            ID.Text = bookInfo.id;
            Title.Text = bookInfo.title;
            AuthorFirstName.Text = bookInfo.authorFirstName;
            AuthorLastName.Text = bookInfo.authorLastName;
            Tag.Text = bookInfo.tag;
            Summary.Text = bookInfo.summary;
            
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
