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

namespace LMS.Pages.MemberPages
{
    /// <summary>
    /// Interaction logic for MemberHomePage.xaml
    /// </summary>
    public partial class MemberHomePage : Page
    {
        public Member memberInfo { get; set; }
        public MemberHomePage(Member loggedInMember)
        {
            InitializeComponent();
            memberInfo = loggedInMember;
            ID.Text = memberInfo.id;
            FirstName.Text = memberInfo.firstName;
            LastName.Text = memberInfo.lastName;
            Email.Text = memberInfo.email;

            ReservesArea.ItemsSource = memberInfo.reservedBooks;

        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
