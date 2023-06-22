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
        public ViewBookPage(Book _book)
        {
            InitializeComponent();

            book = _book;
            ID.Text = book.id;
            Title.Text = book.title;
            AuthorFirstName.Text = book.authorFirstName;
            AuthorLastName.Text = book.authorLastName;
            Subject.Text = book.subject;
            Summary.Text = book.summary;
            selectedImageAddress.Text = book.cover;

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
                selectedImageAddress.Background = (Brush)new BrushConverter().ConvertFrom("#e7ead4");
                selectedImageAddress.Padding = new Thickness(20, 0, 0, 0);
                selectedImageAddress.IsReadOnly = true;

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
                selectedImageAddress.Background = Brushes.Transparent;
                selectedImageAddress.Padding = new Thickness(0);
                selectedImageAddress.IsReadOnly = true;

            }
        }
        
        private void SelectImageButtonClick(object sender, RoutedEventArgs e)
        {
            string imageAddress = SelectImageDialog();
            File.Copy(imageAddress, book.cover, true);
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
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            Book changedInfo = new Book { id = ID.Text, cover = book.cover, title = Title.Text, authorFirstName = AuthorFirstName.Text, authorLastName = AuthorLastName.Text, subject = Subject.Text, summary = Summary.Text };
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
