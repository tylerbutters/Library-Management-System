﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    /// Interaction logic for Admin_Homepage.xaml
    /// </summary>
    /// 
    public partial class AdminMainPage : Page
    {
        public MemberTable memberTable = new MemberTable();
        public MemberNavbar memberNavbar  = new MemberNavbar();
        public AdminMainPage()
        {
            InitializeComponent();
            Navbar.Content = memberNavbar;
            PageContent.Content = memberTable;
        }
    }
}
