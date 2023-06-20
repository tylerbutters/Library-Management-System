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
        public DataGrid memberDataGridInfo { get; internal set;}
        internal Member selectedMember { get; set; }
        public MemberTable()
        {
            InitializeComponent();
            //Assigns Datagrid object with the MemberGrid
            memberDataGridInfo = MemberGrid;

            //Creates event listener for whenever an item in the datagrid is selected, then runs the selectionchanged function.
            memberDataGridInfo.SelectionChanged += MemberDataGridSelectionChanged;
        }
        private void MemberDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMember = (Member)memberDataGridInfo.SelectedItem;

            navigateToViewMemberPage(sender, e);
        }
    }
}
