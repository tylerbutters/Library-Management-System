using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Win32;
namespace LMS.Pages.AdminPages
{
    /// <summary>
    /// Interaction logic for ViewMemberPage.xaml
    /// </summary>
    /// 
    public partial class ViewBookPage : Page
    {
        public event EventHandler<RoutedEventArgs> NavigateToBookPage;
        private bool isEditing { get; set; } = false;
        private bool isConfirmed { get; set; } = false;
        public Book book { get; set; }
        private string imageAddress { get; set; }
        public ViewBookPage(Book _book)
        {
            InitializeComponent();

            book = _book;
            ID.Text = book.id;
            CoverImage.Source = new BitmapImage(new Uri(book.cover, UriKind.Relative));
            Title.Text = book.title;
            AuthorFirstName.Text = book.authorFirstName;
            AuthorLastName.Text = book.authorLastName;
            Subject.Text = book.subject;
            Summary.Text = book.summary;
            Cover.Text = book.cover;
            imageAddress = book.cover;
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigateToBookPage?.Invoke(sender, e);
        }

        private void EditCancelButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                isEditing = true;
                isConfirmed = false;
                SaveButton.Visibility = Visibility.Visible;
                DeleteButton.Visibility = Visibility.Visible;
                SelectImageButton.Visibility = Visibility.Visible;
                EditCancelButton.Content = "Cancel";

                Title.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                Title.Padding = new Thickness(20, 0, 0, 0);
                Title.IsReadOnly = false;
                AuthorFirstName.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                AuthorFirstName.Padding = new Thickness(20, 0, 0, 0);
                AuthorFirstName.IsReadOnly = false;
                AuthorLastName.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                AuthorLastName.Padding = new Thickness(20, 0, 0, 0);
                AuthorLastName.IsReadOnly = false;
                Subject.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                Subject.Padding = new Thickness(20, 0, 0, 0);
                Subject.IsReadOnly = false;
                Summary.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                Summary.Padding = new Thickness(20, 20, 0, 0);
                Summary.IsReadOnly = false;
                Cover.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                Cover.Padding = new Thickness(20, 0, 0, 0);
                Cover.IsReadOnly = true;

            }
            else
            {
                isEditing = false;
                SaveButton.Visibility = Visibility.Hidden;
                DeleteButton.Visibility = Visibility.Hidden;
                EditCancelButton.Content = "Edit";
                SelectImageButton.Visibility = Visibility.Hidden;

                Title.Background = Brushes.Transparent;
                Title.Padding = new Thickness(0);
                Title.IsReadOnly = true;
                AuthorFirstName.Background = Brushes.Transparent;
                AuthorFirstName.Padding = new Thickness(0);
                AuthorFirstName.IsReadOnly = true;
                AuthorLastName.Background = Brushes.Transparent;
                AuthorLastName.Padding = new Thickness(0);
                AuthorLastName.IsReadOnly = true;
                Subject.Background = Brushes.Transparent;
                Subject.Padding = new Thickness(0);
                Subject.IsReadOnly = true;
                Summary.Background = Brushes.Transparent;
                Summary.Padding = new Thickness(0, 20, 0, 0);
                Summary.IsReadOnly = true;
                Cover.Background = Brushes.Transparent;
                Cover.Padding = new Thickness(0);
                Cover.IsReadOnly = true;

                ID.Text = book.id;
                CoverImage.Source = new BitmapImage(new Uri(book.cover, UriKind.Relative));
                Title.Text = book.title;
                AuthorFirstName.Text = book.authorFirstName;
                AuthorLastName.Text = book.authorLastName;
                Subject.Text = book.subject;
                Summary.Text = book.summary;
                Cover.Text = book.cover;
            }
        }
       
        private void SelectImageButtonClick(object sender, RoutedEventArgs e)
        {
            imageAddress = SelectImageDialog();
            CoverImage.Source = new BitmapImage(new Uri(imageAddress, UriKind.Relative));
        }
        private string SelectImageDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = "Select a Cover Image"
            };

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFile = openFileDialog.FileName;
                Cover.Text = selectedFile;
                
                return selectedFile;
            }
            else
            {
                MessageBox.Show("No Image Selected");
                return null;
            }
        }
        private string GenerateNewImageAddress(string title, string existingImageAddressInput)
        {
            string folderPath = @".\CoverImages\";//Folder to contain new Image
            string cleanedExistingAddressInput = string.Join("_", System.IO.Path.GetInvalidPathChars().Aggregate(existingImageAddressInput, (current, c) => current.Replace(c.ToString(), "")));//Removes illegal path characters.
            string fileExtension = System.IO.Path.GetExtension(cleanedExistingAddressInput);//gets file extension of existing image       
            string cleanedTitle = string.Join("_", System.IO.Path.GetInvalidFileNameChars().Aggregate(title, (current, c) => current.Replace(c.ToString(), ""))).Replace(" ", "_");//Removes illegal filename characters from book title.                                                                                                                                                                                        //makes an address and name for the new copy, preserves existing filetype. (does NOT save a copy yet)

            MessageBox.Show($"{folderPath}{cleanedTitle}{fileExtension}");
            return $"{folderPath}{cleanedTitle}{fileExtension}";
        }
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Book changedInfo = new Book { id = ID.Text, cover = book.cover, title = Title.Text, authorFirstName = AuthorFirstName.Text, authorLastName = AuthorLastName.Text, subject = Subject.Text, summary = Summary.Text };
            if (imageAddress != book.cover)
            {
                string newimageAddress = GenerateNewImageAddress(Title.Text, imageAddress);
               
                File.Copy(imageAddress, newimageAddress, true);
            }

            FileManagement.EditBook(book, changedInfo);

            EditCancelButtonClick(sender, e);
        }
        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isConfirmed)
            {
                isConfirmed = true;
                DeleteButton.Content = "Confirm?";
            }
            else
            {
                FileManagement.DeleteBook(book);
                isConfirmed = false;
                if (NavigationService != null && NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }
    }

}
