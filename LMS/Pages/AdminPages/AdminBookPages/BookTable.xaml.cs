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
    /// <summary>
    /// Interaction logic for MemberTable.xaml
    /// </summary>
    public partial class BookTable : Page
    {
        public event EventHandler<Book> NavigateToViewBookPage;
        public BookTable(List<Book> searchResults)
        {
            InitializeComponent();
            if (searchResults.Count == 0)
            {
                NoResultsText.Visibility = Visibility.Visible;
            }
            BookGrid.ItemsSource = searchResults;
        }
        public void BookClicked(object sender, SelectionChangedEventArgs e)
        {
            Book selectedBook = BookGrid.SelectedItem as Book;
            NavigateToViewBookPage?.Invoke(sender, selectedBook);
        }
    }
}
