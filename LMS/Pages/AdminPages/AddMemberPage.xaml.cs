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
        private void AddUserButtonClick(object sender, RoutedEventArgs e)
        {
            string firstNameInput = this.firstNameInput.Text;
            string lastNameInput = this.lastNameInput.Text;
            string emailInput = this.emailInput.Text;
            //create and initialize ID and PIN
            string userID = GenerateRandomID().ToString();
            string userPIN = GenerateRandomPIN().ToString();

            string[] lines = File.ReadAllLines(@"Database\memberInformation.csv");
            var existingEmail = lines.Skip(1).Select(row => row.Split(',')[4]).ToList();
            var existingID = lines.Skip(1).Select(row => row.Split(',')[0]).ToList();
            var existingPIN = lines.Skip(1).Select(row => row.Split(',')[1]).ToList();

            //Check to see if PIN or ID already exist and generate new ones if they do
            while (existingID.Contains(userID)||existingPIN.Contains(userPIN))
            {
                userID=GenerateRandomID().ToString();
                userPIN = GenerateRandomPIN().ToString();
            }
            if (firstNameInput==""||lastNameInput==""||emailInput=="")
            {
                MessageBox.Show("Please Enter all feilds");
                return;
            }
            else if (existingEmail.Contains(emailInput))
            {
                MessageBox.Show("This Email is already registered.");
                return;
            }
            else
            {
                var addDataToCSV = $"{userID},{userPIN},{firstNameInput},{lastNameInput},{emailInput}";
                var newLines = lines.Append(addDataToCSV).ToArray();

                File.WriteAllLines(@"Database\memberInformation.csv", newLines);
                MessageBox.Show("User Added Successfully!\nID: " + userID + "\nPIN: "+ userPIN);
                this.firstNameInput.Text = "";
                this.lastNameInput.Text = "";
                this.emailInput.Text = "";
            }
        }
    }
}
