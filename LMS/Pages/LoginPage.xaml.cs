﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public delegate void NavigateTo_MemberHomepage(object sender, RoutedEventArgs e);
        public event NavigateTo_MemberHomepage NavigateToMemberHomepage;
        public delegate void NavigateTo_AdminHomepage(object sender, RoutedEventArgs e);
        public event NavigateTo_AdminHomepage NaigateToAdminHomepage;

        public LoginPage()
        {
            InitializeComponent();
        }
        //Login Check checks the entered email and password against the given list and returns true/false.
        public bool CheckLoginDetails(string file)
        {
            //use if/else loop to check admin CSV first, else member CSV
            string idInput = IDInput.Text;
            string pinInput = PinInput.Text;

            string[] rows = File.ReadAllLines(file);
            List<string> idData = rows.Skip(1).Select(row => row.Split(',')[0]).ToList();
            List<string> pinData = rows.Skip(1).Select(row => row.Split(',')[1]).ToList();

            //returns conditional statement
            return idData.Contains(idInput) && pinData.Contains(pinInput);
        }
        //Login button runs "Login_Check()" against user list first, then admin list.
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckLoginDetails(FileManagement.MemberFile))
            {
                NavigateToMemberHomepage(sender, e);
            }
            else if (CheckLoginDetails(FileManagement.AdminFile))
            {
                //MessageBox.Show("Logging in as Admin");
                NaigateToAdminHomepage(sender, e);
            }
            else
            {
                MessageBox.Show("Cannot find account with those details");
            }
        }
        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
