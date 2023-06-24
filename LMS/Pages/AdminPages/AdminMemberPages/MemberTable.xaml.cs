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
        public event EventHandler<RoutedEventArgs> NavigateToViewMemberPage;
        public Member selectedMember { get; set; }
        public MemberTable(List<Member> searchResults)
        {
            InitializeComponent();
            if (searchResults.Count == 0)
            {
                NoResultsText.Visibility = Visibility.Visible;
            }
            MemberGrid.ItemsSource = searchResults;
            MemberGrid.SelectionChanged += MemberDataGridSelectionChanged;
        }
        public void MemberDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMember = (Member)MemberGrid.SelectedItem;

            NavigateToViewMemberPage?.Invoke(sender, e);
        }
    }
}
