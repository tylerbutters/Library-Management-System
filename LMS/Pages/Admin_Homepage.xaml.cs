using System;
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

namespace LMS.Pages
{
    /// <summary>
    /// Interaction logic for Admin_Homepage.xaml
    /// </summary>
    public partial class Admin_Homepage : Page
    {
        public List<Member> LoadMembers()
        {

            var rows = File.ReadAllLines(@"./Database/memberInformation.csv");
            var members = from l in rows.Skip(1) //skips first row
                             let split = l.Split(',') //splits cell at comma
                             select new Member()
                             {
                                 id = int.Parse(split[0]),
                                 pin = int.Parse(split[1]),
                                 firstName = split[2],
                                 lastName = split[3],
                                 email = split[4],
                             };
           
            return members.ToList(); ;
        }
        public Admin_Homepage()
        {
            InitializeComponent();
            MemberGrid.ItemsSource = LoadMembers();
        }
    }
}
