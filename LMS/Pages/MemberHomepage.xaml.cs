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

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for Member_Homepage.xaml
    /// </summary>
    public partial class MemberHomepage : Page

    {
        public delegate void NavigateTo_addUser(object sender, RoutedEventArgs e);
        public event NavigateTo_addUser NavigateToaddUser;
        public MemberHomepage()
        {
            InitializeComponent();
        }

        private void User_button_click(object sender, RoutedEventArgs e)
        {
            NavigateToaddUser(sender, e);
        }
    }
}
