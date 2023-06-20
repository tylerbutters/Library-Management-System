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
using System.Collections.ObjectModel;
using System.Globalization;

namespace LMS.Pages.MemberPages
{
    /// <summary>
    /// Interaction logic for BookResultsPage.xaml
    /// </summary>
    public partial class BookResultsPage : Page
    {
        private List<Book> results { get; set; }
       
        public BookResultsPage(List<Book> searchResults, string searchInput)
        {
            InitializeComponent();
            
            results = searchResults;

            //CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            //TextInfo textInfo = cultureInfo.TextInfo;
            //foreach (Book book in results)
            //{
            //    book.title = textInfo.ToTitleCase(book.title);
            //    book.authorFirstName = textInfo.ToTitleCase(book.authorFirstName);
            //    book.authorLastName = textInfo.ToTitleCase(book.authorLastName);
            //    book.tag = textInfo.ToTitleCase(book.tag);
            //}
            ResultsContainer.ItemsSource = results;
            ResultText.Text = searchInput;
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
