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

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for addUser.xaml
    /// </summary>
    public partial class addUser : Page
    {
        public addUser()
        {
            InitializeComponent();
        }
        public int GenerateRandomPIN()
        {
            int _min = 10000;
            int _max = 99999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        private void AddUserScrollVeiwer_Changed(object sender, RoutedEventArgs e)
        { }

        private void Add_User_Button_Click(object sender, RoutedEventArgs e)
        {
            string firstNameInput = first_name_input.Text;
            string lastNameInput = last_name_input.Text;
            string emailInput = email_input.Text;
            //generate random ID
            //generate random PIN
            string userID = "1234";
            string userPIN = "1111";

            string[] lines = File.ReadAllLines(@"Database\memberInformation.csv");
            var existingEmail = lines.Skip(1).Select(row => row.Split(',')[4]).ToList();
            var existingID = lines.Skip(1).Select(row => row.Split(',')[0]).ToList();
            var existingPIN = lines.Skip(1).Select(row => row.Split(',')[1]).ToList();

            //generate random ID; loop check against existingID (existingPIN.Contains(userPIN)) 
            while (existingPIN.Contains(userPIN))
            {
                userPIN=GenerateRandomPIN().ToString();
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
                MessageBox.Show("User Added Successfully");
                MessageBox.Show("your ID Number is: ", userID);
                MessageBox.Show("your PIN number is: ", userPIN);
                first_name_input.Text = "";
                last_name_input.Text = "";
                email_input.Text = "";

            }
        }
    }
}
