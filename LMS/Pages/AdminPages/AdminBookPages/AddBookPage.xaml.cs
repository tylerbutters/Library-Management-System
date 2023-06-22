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
using System.ComponentModel;
using System.Reflection;
using Microsoft.Win32;
using System.Runtime.CompilerServices;

namespace LMS.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for AddBookPage.xaml
    /// </summary>
    public partial class AddBookPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToBookPage;
        public string imageAddress { get; set; }
        public AddBookPage()
        {
            InitializeComponent();
        }
        private string GenerateBookID()
        {
            int min = 10000;
            int max = 99999;
            return "B" + new Random().Next(min, max);
        }
        private string GenerateNewImageAddress(string title, string existingImageAddressInput)
        {
            string folderPath = @".\CoverImages\";//Folder to contain new Image
            string cleanedExistingAddressInput = string.Join("_", System.IO.Path.GetInvalidPathChars().Aggregate(existingImageAddressInput, (current, c) => current.Replace(c.ToString(), "")));//Removes illegal path characters.
            string fileExtension = System.IO.Path.GetExtension(cleanedExistingAddressInput);//gets file extension of existing image       
            string cleanedTitle = string.Join("_", System.IO.Path.GetInvalidFileNameChars().Aggregate(title, (current, c) => current.Replace(c.ToString(), ""))).Replace(" ", "_");//Removes illegal filename characters from book title.                                                                                                                                                                                        //makes an address and name for the new copy, preserves existing filetype. (does NOT save a copy yet)
           
            return $"{folderPath}{cleanedTitle}{fileExtension}";
        }

        //Select Image File button handler
        private void SelectImageButtonClick(object sender, RoutedEventArgs e)
        {
            imageAddress = SelectImageDialog();
        }
        private string SelectImageDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Select a cover image"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFile = openFileDialog.FileName;
                selectedImageAddress.Text = selectedFile;
                return selectedFile;
            }
            else
            {
                MessageBox.Show("No Image Selected");
                return null;
            }
        }
        private void SaveNewBookButtonClick(object sender, RoutedEventArgs e)
        {
            //only filters in accounts of type Book
            List<Book> currentBooks = FileManagement.LoadBooks();

            if (titleInput.Text == "" || authorFirstNameInput.Text == "" || authorLastNameInput.Text == "" || subjectInput.Text == "" || summaryInput.Text == "")
            {
                MessageBox.Show("Please Enter all feilds");
                return;
            }

            string newImageAddress = GenerateNewImageAddress(titleInput.Text, imageAddress); //creates new image address and filename
            File.Copy(imageAddress, newImageAddress);

            Book newBook = new Book
            {
                id = GenerateBookID(),
                cover = newImageAddress,
                title = titleInput.Text,
                authorFirstName = authorFirstNameInput.Text,
                authorLastName = authorLastNameInput.Text,
                subject = subjectInput.Text,
                summary = summaryInput.Text,
            };

            //Check to see if title already exist and generate new ones if they do
            foreach (Book currentBook in currentBooks)
            {
                if (currentBook.title == newBook.title && currentBook.authorFirstName == newBook.authorFirstName && currentBook.authorLastName == newBook.authorLastName)
                {
                    MessageBox.Show("This book is already registered.");
                    return;
                }
                else if (currentBook.id == newBook.id)
                {
                    newBook.id = GenerateBookID().ToString();
                }
            }

            FileManagement.SaveNewBook(newBook);

            titleInput.Text = "";
            authorFirstNameInput.Text = "";
            authorLastNameInput.Text = "";
            subjectInput.Text = "";
            summaryInput.Text = "";
            selectedImageAddress.Text = "";
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToBookPage?.Invoke(sender, e);
        }
    }
}
