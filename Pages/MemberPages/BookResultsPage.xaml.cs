﻿using System;
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
using System.Collections.ObjectModel;
using System.Globalization;

namespace LMS.Pages.MemberPages
{
    /// <summary>
    /// Displays search results
    /// </summary>
    public partial class BookResultsPage : Page
    {
        //Events enable 'MemberHomePage' methods to subscribe to it when 'ReserveButtonClick' is clicked.
        public event EventHandler<Book> PlaceReserve;
        public event EventHandler<Reserve> CancelReserve;
        public event EventHandler<Book> NavigateToBookInfoPage;

        //holds the list of search results. It is used to store the books that match the search criteria provided to the page.
        private List<Book> results { get; set; }
        private Member member { get; set; }

        //'searchResults' represents the list of books that match the search criteria, and 'searchInput' is the text used for the search.
        //Inside the constructor, 'searchresults' are assigned to 'results', and the UI elements (ResultsContainer and ResultText) are populated with the search results and search input information     
        public BookResultsPage(List<Book> searchResults, string searchInput, Member _member)
        {
            InitializeComponent();

            if (searchResults is null || _member is null)
            {
                throw new NullReferenceException();
            }

            results = searchResults;
            member = _member;

            foreach (Book book in results)
            {
                foreach (Loan loan in member.loanedBooks)
                {
                    if (book.id == loan.bookId)
                    {
                        book.isLoanedByUser = true;
                    }
                }
                foreach (Reserve reserve in member.reservedBooks)
                {
                    if (book.id == reserve.bookId)
                    {
                        book.isReservedByUser = true;
                    }
                }

            }

            ResultsContainer.ItemsSource = results;
            ResultText.Text = searchInput;
        }

        //Extracts 'selectedBook' from 'DataContext' and invokes 'PlaceReserve' or 'CancelReserve' depending on the books reserve status.S
        //Updates UI using 'ResultsContainer.ItemsSource'.
        private void ReserveCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Book selectedBook = (sender as Button).DataContext as Book;
            if (selectedBook is null)
            {
                throw new NullReferenceException();
            }

            if (!selectedBook.isReservedByUser) //button says "reserve"
            {
                PlaceReserve?.Invoke(sender, selectedBook);
                selectedBook.isReservedByUser = true;
            }
            else //button says "cancel"
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel?", "Confirmation", MessageBoxButton.YesNo);

                if (result is MessageBoxResult.Yes)
                {
                    Reserve reservedBook = member.reservedBooks.FirstOrDefault(reserve => reserve.bookId == selectedBook.id);
                    CancelReserve?.Invoke(sender, reservedBook);
                    selectedBook.isReservedByUser = false;
                    selectedBook.isReserved = false;
                }
            }

            ResultsContainer.ItemsSource = null;
            ResultsContainer.ItemsSource = results;
        }
        private void ItemClick(object sender, MouseButtonEventArgs e)
        {
            Book clickedItem = (sender as FrameworkElement).DataContext as Book;
            if (clickedItem is null)
            {
                throw new NullReferenceException();
            }
            NavigateToBookInfoPage?.Invoke(sender, clickedItem);
        }

    }
}
