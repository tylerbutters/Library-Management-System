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
    /// Interaction logic for AddBookPage.xaml
    /// </summary>
    public partial class AddBookPage : Page
    {
        public delegate void NavigateToBookPage(object sender, RoutedEventArgs e);
        public event NavigateToBookPage navigateToBookPage;
        public AddBookPage()
        {
            InitializeComponent();
        }
        public int GenerateRandomID()
        {
            int min = 10000; 
            int max = 99999;
            return new Random().Next(min, max);
        }
        private void SaveNewBookButtonClick(object sender, RoutedEventArgs e)
        {
            //only filters in accounts of type Book
            List<Book> currentBooks = FileManagement.LoadAccounts().OfType<Book>().ToList();

            if (titleInput.Text == "" || authorFirstNameInput.Text == "" || authorLastNameInput.Text == "" || tagInput.Text == "")
            {
                MessageBox.Show("Please Enter all feilds");
                return;
            }

            Book newBook = new Book
            {
                id = GenerateRandomID().ToString(),
                title = titleInput.Text,
                authorFirstName = authorFirstNameInput.Text,
                authorLastName = authorLastNameInput.Text,
                tag = tagInput.Text
            };

            //Check to see if title already exist and generate new ones if they do
            foreach (Book currentBook in currentBooks)
            {
                if (currentBook.title == newBook.title)
                {
                    MessageBox.Show("This Email is already registered.");
                    return;
                }
                else if (currentBook.id == newBook.id)
                {
                    newBook.id = GenerateRandomID().ToString();
                }
            }

            string newBookInfo = $"{newBook.id},{newBook.cover},{newBook.title},{newBook.authorFirstName},{newBook.authorLastName},{newBook.tag},{newBook.summary},{newBook.isAvailable}";
            string[] currentRows = File.ReadAllLines(FileManagement.BookFile);
            string[] newRows = currentRows.Append(newBookInfo).ToArray();
            File.WriteAllLines(FileManagement.BookFile, newRows);
            MessageBox.Show("User Added Successfully!\n");

            titleInput.Text = "";
            authorFirstNameInput.Text = "";
            authorLastNameInput.Text = "";
            tagInput.Text = "";
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            navigateToBookPage(sender, e);
        }
    }
}
