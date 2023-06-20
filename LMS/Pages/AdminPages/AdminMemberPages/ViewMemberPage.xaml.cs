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
    public partial class ViewMemberPage : Page
    {
  
        public Member memberInfo { get; set; }

        //Member info from login and member class's are passed through parameters and displayed in each text example.
        public ViewMemberPage(Member member)
        {
            memberInfo = member;
            InitializeComponent();

            ID.Text = memberInfo.id;
            FirstName.Text = memberInfo.firstName;
            LastName.Text = memberInfo.lastName;
            Email.Text = memberInfo.email;
            Password.Text = memberInfo.pin;
            
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
