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
    /// Interaction logic for ViewMemberPage.xaml
    /// </summary>
    public partial class ViewMemberPage : Page
    {
        private bool isEditing { get; set; } = false;
        private bool isConfirmed { get; set; } = false;
        private Member memberInfo { get; set; }

        //Member info from login and member class's are passed through parameters and displayed in each text example.
        public ViewMemberPage(Member member)
        {
            memberInfo = member;
            InitializeComponent();

            ID.Text = memberInfo.id;
            PIN.Text = memberInfo.pin;
            FirstName.Text = memberInfo.firstName;
            LastName.Text = memberInfo.lastName;
            Email.Text = memberInfo.email;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void EditCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                isEditing = true;
                isConfirmed = false;
                SaveButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                EditCancelButton.Content = "Cancel";
                ID.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                ID.Padding = new Thickness(20, 0, 0, 0);

                PIN.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                PIN.Padding = new Thickness(20, 0, 0, 0);
                PIN.IsReadOnly = false;
                FirstName.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                FirstName.Padding = new Thickness(20, 0, 0, 0);
                FirstName.IsReadOnly = false;
                LastName.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                LastName.Padding = new Thickness(20, 0, 0, 0);
                LastName.IsReadOnly = false;
                Email.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                Email.Padding = new Thickness(20, 0, 0, 0);
                Email.IsReadOnly = false;
            }
            else
            {
                isEditing = false;
                SaveButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
                EditCancelButton.Content = "Edit";

                ID.Background = Brushes.Transparent;
                ID.Padding = new Thickness(0);
                ID.IsReadOnly = true;
                PIN.Background = Brushes.Transparent;
                PIN.Padding = new Thickness(0);
                PIN.IsReadOnly = true;
                FirstName.Background = Brushes.Transparent;
                FirstName.Padding = new Thickness(0);
                FirstName.IsReadOnly = true;
                LastName.Background = Brushes.Transparent;
                LastName.Padding = new Thickness(0);
                LastName.IsReadOnly = true;
                Email.Background = Brushes.Transparent;
                Email.Padding = new Thickness(0);
                Email.IsReadOnly = true;
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {

        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isConfirmed)
            {
                isConfirmed = true;
                DeleteButton.Content = "Confirm?";
            }
            else
            {
                //delete user
                isConfirmed = false;
            }
        }
    }
}
