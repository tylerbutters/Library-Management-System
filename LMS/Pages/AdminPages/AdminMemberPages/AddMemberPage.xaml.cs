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
        public delegate void NavigateToMemberPage(object sender, RoutedEventArgs e);
        public event NavigateToMemberPage navigateToMemberPage;
        public AddMemberPage()
        {
            InitializeComponent();
        }
        public int GenerateRandomID()
        {
            int min = 10000; 
            int max = 99999;
            return new Random().Next(min, max);
        }
        public int GenerateRandomPIN()
        {
            int min = 1000;
            int max = 9999;
            return new Random(Guid.NewGuid().GetHashCode()).Next(min, max);
        }
        private void SaveNewMemberButtonClick(object sender, RoutedEventArgs e)
        {
            Member newMember = new Member
            {
                id = GenerateRandomID().ToString(),
                pin = GenerateRandomPIN().ToString(),
                firstName = firstNameInput.Text,
                lastName = lastNameInput.Text,
                email = emailInput.Text
            };

            List<Member> currentMembers = FileManagement.LoadMembers();

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
                    newMember.id = GenerateRandomID().ToString();
                }
            }

            if (firstNameInput.Text == "" || lastNameInput.Text == "" || emailInput.Text == "")
            {
                MessageBox.Show("Please Enter all feilds");
                return;
            }

            FileManagement.SaveMembers(newMember);

            firstNameInput.Text = "";
            lastNameInput.Text = "";
            emailInput.Text = "";
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToMemberPage(sender, e);
        }
    }
}
