using Lab2Bookstore.Data;
using Lab2Bookstore.Models;
using Microsoft.EntityFrameworkCore;
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
        Book? currentBook = new();
        
        List<Book> allBooks;
        List<Store> allStores;
        List<Author> allAuthors;

        LiberaDbContext dbContext;
        public EditWindow()
        {
            
            InitializeComponent();
            dbContext = new();

            LoadContent();
            
        }

        /// <summary>
        /// Resets and repopulates the ListBoxOfBooks, SelectedStores, Authors. To show the updates.
        /// </summary>
        private async Task LoadContent()
        {
            allStores = await dbContext.Stores.ToListAsync();
            allAuthors = await dbContext.Authors.ToListAsync();
            allBooks = await dbContext.Books.ToListAsync();
            ListBoxOfBooks.ItemsSource = allBooks;
            ComboBoxAuthor.ItemsSource = allAuthors;
        }

        private void ListBoxOfBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ResetTextBoxes();

        }

        /// <summary>
        /// Sets the text fields in the window with the information of a selected book.
        /// If no book is selected, it sets the values to empty or null.
        /// </summary>
        private void ResetTextBoxes()
        {

            currentBook = ListBoxOfBooks.SelectedItem as Book;
            if (currentBook == null)
            {
                TBTitle.Text = string.Empty;
                ComboBoxAuthor.SelectedItem = null;
                TBISBN.Text = string.Empty;
                TBCategory.Text = string.Empty;
                TBLanguage.Text = string.Empty;
                TBPrice.Text = string.Empty;
                TBReleaseDate.Text = string.Empty;
            } else
            {
                TBTitle.Text = currentBook.Title;
                ComboBoxAuthor.SelectedItem = allAuthors.FirstOrDefault(author => author.AuthorId == currentBook.Author.AuthorId);
                TBISBN.Text = currentBook.Isbn13;
                TBCategory.Text = currentBook.Catagory;
                TBLanguage.Text = currentBook.Language;
                TBPrice.Text = currentBook.Price.Value.ToString();
                TBReleaseDate.Text = currentBook.ReleaseDate.ToString();
               
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            
            if (currentBook != null)
            {
                foreach (Store store in allStores)
                {
                    Inventory? inventoryToRemove = dbContext.Inventories.SingleOrDefault(inv => inv.Isbn13 == currentBook.Isbn13 && inv.StoreId == store.StoreId);
                    
                    if (inventoryToRemove != null)
                    {
                        dbContext.Inventories.Remove(inventoryToRemove);
                    }
                }
                dbContext.Books.Remove(currentBook);
                dbContext.SaveChanges();
                _ = LoadContent();

            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxAuthor.SelectedItem != null && TBTitle.Text != null && TBCategory.Text != null && TBISBN.Text != null && TBLanguage.Text != null && TBPrice.Text != null && ComboBoxAuthor.SelectedItem != null && TBReleaseDate.Text != null)
            {
                Book newBook = new()
                {
                    Author = ComboBoxAuthor.SelectedItem as Author,
                    Title = TBTitle.Text,
                    Catagory = TBCategory.Text,
                    Isbn13 = TBISBN.Text,
                    Language = TBLanguage.Text,
                    Price = decimal.Parse(TBPrice.Text),
                    ReleaseDate = DateTime.Parse(TBReleaseDate.Text)
                };


                if(!allBooks.Any(b => b.Isbn13 == newBook.Isbn13 ))
                {
                    foreach (Store store in allStores)
                    {
                        Inventory newInventory = new Inventory() { Isbn13 = newBook.Isbn13, StoreId = store.StoreId, Amount = 0 };
                        dbContext.Inventories.Add(newInventory);
                    }
                    newBook.Author.Books.Add(newBook);
                    dbContext.Books.Add(newBook);
                    dbContext.SaveChanges();
                    _ = LoadContent();
                }

            } else
            {
                MessageBox.Show("Please fill all fields.");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxAuthor.SelectedItem != null && TBTitle.Text != null && TBCategory.Text != null && TBISBN.Text != null && TBLanguage.Text != null && TBPrice.Text != null && ComboBoxAuthor.SelectedItem != null && TBReleaseDate.Text != null)
            {
                if (currentBook != null)
                {
                    if (currentBook.Isbn13 != TBISBN.Text)
                    {
                        foreach (Store store in allStores)
                        {
                            string targetISBN = currentBook.Isbn13;
                            var currentInventory = store.Inventories.FirstOrDefault(inv => inv.Isbn13 == targetISBN);
                            if (currentInventory != null)
                            currentInventory.Isbn13 = TBISBN.Text;
                        }
                    }
                }
                currentBook.Author = ComboBoxAuthor.SelectedItem as Author;
                currentBook.Title = TBTitle.Text;
                currentBook.Catagory = TBCategory.Text;
                currentBook.Isbn13 = TBISBN.Text;
                currentBook.Language = TBLanguage.Text;
                currentBook.Price = decimal.Parse(TBPrice.Text);
                currentBook.ReleaseDate = DateTime.Parse(TBReleaseDate.Text);

                dbContext.SaveChanges();
                _ = LoadContent();

            } else
            {
                MessageBox.Show("Please fill all fields.");
            }
        }

        private void BtnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            AddAuthor addAuthorWindow = new();
            addAuthorWindow.Show();
            this.Close();
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new();
            this.Close();
            main.Show();
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TBTitle.Text = string.Empty;
            ComboBoxAuthor.SelectedItem = null;
            TBISBN.Text = string.Empty;
            TBCategory.Text = string.Empty;
            TBLanguage.Text = string.Empty;
            TBPrice.Text = string.Empty;
            TBReleaseDate.Text = string.Empty;
        }
    }
}
