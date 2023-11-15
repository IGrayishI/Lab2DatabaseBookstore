using Lab2Bookstore.Data;
using Lab2Bookstore.Models;
using System;
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
using System.Windows.Shapes;

namespace Lab2Bookstore
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        Book currentBook = new();
        Store currentStore = new();
        Inventory currentInventory = new();
        List<Book> allBooks;
        List<Author> authors;
        List<Store> allStores;
        List<Inventory> allInventory;

        LiberaDbContext dbContext = new LiberaDbContext();
        public EditWindow()
        {
            allInventory = dbContext.Inventories.ToList();
            authors = dbContext.Authors.ToList();
            allBooks = dbContext.Books.ToList();
            allStores = dbContext.Stores.ToList();
            InitializeComponent();
            ListBoxOfBooks.ItemsSource = allBooks;
            ComboBoxSelectStore.ItemsSource = allStores;
        }

        private void ListBoxOfBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentBook = ListBoxOfBooks.SelectedItem as Book;
            TextBoxTitle.Text = currentBook.Title;
            TextBoxAuthor.Text = currentBook.Author.FirstName + " " + currentBook.Author.LastName;
            TextBoxISBN.Text = currentBook.Isbn13;
            TextBoxCategory.Text = currentBook.Catagory;
            TextBoxLanguage.Text = currentBook.Language;
            TextBoxPrice.Text = currentBook.Price.Value.ToString();
            if (currentStore != null)
            {
                currentInventory = ComboBoxSelectStore.SelectedItem as Inventory;
                TextBoxAvailbleCopies.Text = currentInventory.Amount.ToString();

            }

        }

        private void ComboBoxSelectStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentStore = ComboBoxSelectStore.SelectedItem as Store;
        }
    }
}
