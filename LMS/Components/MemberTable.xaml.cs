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

namespace LMS.Components
{
    /// <summary>
    /// Interaction logic for MemberTable.xaml
    /// </summary>
    public partial class MemberTable : UserControl
    {
        public MemberTable()
        {
            InitializeComponent();
            MemberGrid.ItemsSource = FileManagement.LoadMembers();
        }
    }
}
