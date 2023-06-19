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
        public delegate void NavigateToViewBookPage(object send, RoutedEventArgs e);
        public event NavigateToViewBookPage navigateToViewBookPage;
        public DataGrid bookDataGridInfo;
        public Book selectedBook;
        public BookTable()
        {
            InitializeComponent();
            bookDataGridInfo = BookGrid;
            bookDataGridInfo.SelectionChanged += BookDataGridSelectionChanged;
        }
        private void BookDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedBook = (Book)bookDataGridInfo.SelectedItem;

            navigateToViewBookPage(sender, e);
        }
    }
}
