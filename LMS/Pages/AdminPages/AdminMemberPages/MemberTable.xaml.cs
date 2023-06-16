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
    public partial class MemberTable : Page
    {
        public delegate void NavigateToViewMemberPage (object send, RoutedEventArgs e);
        public event NavigateToViewMemberPage navigateToViewMemberPage;
        public DataGrid memberDataGrid { get; internal set;}
        internal Member selectedMember { get; set; }
        public MemberTable()
        {
            InitializeComponent();
            memberDataGrid = MemberGrid;
            memberDataGrid.SelectionChanged += MemberDataGridSelectionChanged;
        }
        private void MemberDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMember = (Member)memberDataGrid.SelectedItem;

            navigateToViewMemberPage(sender, e);
        }
    }
}
