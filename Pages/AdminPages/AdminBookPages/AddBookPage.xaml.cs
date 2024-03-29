﻿using System;
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
        private string imageAddress { get; set; }
        private bool isCoverChanged { get; set; }
        public AddBookPage()
        {
            InitializeComponent();

            KeyDown += AddBookPageKeyDown;
        }

        private void AddBookPageKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!ValidateInputs())
                {
                    return;
                }
                SaveNewBook();
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }
            SaveNewBook();
        }

        //Select Image File button handler
        private void SelectImageButtonClick(object sender, RoutedEventArgs e)
        {
            imageAddress = SelectImageDialog();
            CoverImage.Source = new BitmapImage(new Uri(imageAddress));
        }

        //brings up file explorer to choose image
        private string SelectImageDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files(*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Select a cover image"
            };

            bool? result = openFileDialog.ShowDialog(); //if user chose an image

            if (result is true)
            {
                string selectedFile = openFileDialog.FileName;
                selectedImageAddress.Text = selectedFile;
                isCoverChanged = true;
                return selectedFile;
            }
            else
            {
                MessageBox.Show("No Image Selected");
                isCoverChanged = false;
                return null;
            }
        }

        private void SaveNewBook()
        {
            if (titleInput.Text.Contains(",") || authorFirstNameInput.Text.Contains(",") || authorLastNameInput.Text.Contains(",") || subjectInput.Text.Contains(",") || summaryInput.Text.Contains(","))
            {
                // Handle the case where one or more text fields contain a comma
                MessageBox.Show("Cannot contain commas");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Are you sure you want to save?", "Confirmation", MessageBoxButton.YesNo);

            if (result is MessageBoxResult.Yes)
            {
                string readImagePath;
                List<Book> currentBooks = FileManagement.LoadBooks();
                if (!isCoverChanged)
                {
                    readImagePath = "NO_COVER";
                }
                else
                {
                    string clippedTitle = titleInput.Text.Substring(0, Math.Min(titleInput.Text.Length, 40)); //cuts of image name from 40 characters
                    string cleanedTitle = string.Join("_", System.IO.Path.GetInvalidFileNameChars().Aggregate(clippedTitle, (current, c) => current.Replace(c.ToString(), ""))).Replace(" ", "_").ToLower();
                    string writeImagePath = System.IO.Path.Combine(@".\CoverImages\", $"{cleanedTitle}.png");
                    File.Copy(imageAddress, writeImagePath);
                    readImagePath = System.IO.Path.Combine(@"/CoverImages/", $"{cleanedTitle}.png");
                }

                Book newBook = new Book
                {
                    id = GenerateBookID(),
                    cover = readImagePath,
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
        }

        private string GenerateBookID()
        {
            int min = 10000;
            int max = 99999;
            return "B" + new Random().Next(min, max);
        }

        //checks inputs in box
        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(titleInput.Text) ||
                string.IsNullOrEmpty(authorFirstNameInput.Text) ||
                string.IsNullOrEmpty(authorLastNameInput.Text) ||
                string.IsNullOrEmpty(subjectInput.Text) ||
                string.IsNullOrEmpty(summaryInput.Text))
            {
                MessageBox.Show("Please enter all fields.");
                return false;
            }

            return true;
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToBookPage?.Invoke(sender, e);
        }
    }
}
