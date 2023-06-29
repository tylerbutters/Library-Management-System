using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// Interaction logic for AddMemberPage.xaml
    /// </summary>
    public partial class AddMemberPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToMemberPage;
        public AddMemberPage()
        {
            InitializeComponent();

            KeyDown += AddMemberPageKeyDown;
        }

        private void AddMemberPageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!ValidateInputs())
                {
                    return;
                }
                SaveNewMember();
            }
        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }
            SaveNewMember();
        }

        private void SaveNewMember()
        {
            if (firstNameInput.Text.Contains(",") || lastNameInput.Text.Contains(",") || emailInput.Text.Contains(","))
            {
                // Handle the case where one or more text fields contain a comma
                MessageBox.Show("Cannot contain commas");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to save?", "Confirmation", MessageBoxButton.YesNo);

            if (result is MessageBoxResult.Yes)
            {
                List<Member> currentMembers = FileManagement.LoadMembers();

                Member newMember = new Member
                {
                    id = GenerateMemberID(),
                    pin = GenerateMemberPIN(),
                    firstName = firstNameInput.Text,
                    lastName = lastNameInput.Text,
                    email = emailInput.Text
                };

                //Check to see if PIN or email already exist and generate new ones if they do
                foreach (Member currentMember in currentMembers)
                {
                    if (currentMember.email == newMember.email)
                    {
                        MessageBox.Show("This Email is already registered.");
                        return;
                    }
                    else if (currentMember.id == newMember.id)
                    {
                        newMember.id = GenerateMemberID();
                    }
                }

                FileManagement.SaveNewMember(newMember);

                firstNameInput.Text = "";
                lastNameInput.Text = "";
                emailInput.Text = "";
            }
        }

        public string GenerateMemberID()
        {
            int min = 10000;
            int max = 99999;
            return "M" + new Random().Next(min, max);
        }

        public string GenerateMemberPIN()
        {
            int min = 1000;
            int max = 9999;
            return new Random().Next(min, max).ToString();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(firstNameInput.Text) ||
                string.IsNullOrEmpty(lastNameInput.Text) ||
                string.IsNullOrEmpty(emailInput.Text))
            {
                MessageBox.Show("Please enter all fields.");
                return false;
            }

            return true;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToMemberPage?.Invoke(sender, e);
        }
    }
}
