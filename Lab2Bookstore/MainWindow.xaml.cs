using Lab2Bookstore.Data;
using Lab2Bookstore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2Bookstore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LiberaDbContext dbContext;
        Book? currentBook;
        Inventory toCurrentInventory;
        Inventory fromCurrentInventory;
        Store? ToStore;
        Store? FromStore;
        List<Inventory> allInventory;
        List<Author> authors;
        List<Book> allBooks;
        List<Store> allStores;
        public MainWindow()
        {
            InitializeComponent();
            dbContext = new LiberaDbContext();
            CBSelectStoreTo.Visibility = Visibility.Hidden;
            LStoreTo.Visibility = Visibility.Hidden;
            BtnTransferBook.Visibility = Visibility.Hidden;
            _ = LoadContent();

        }

        private async Task LoadContent()
        {
            authors = await dbContext.Authors.ToListAsync();
            allStores = await dbContext.Stores.ToListAsync();
            allBooks = await dbContext.Books.ToListAsync();
            allInventory = await dbContext.Inventories.ToListAsync();
            ListBoxOfBooks.ItemsSource = allBooks;
            CBSelectStoreFrom.ItemsSource = allStores;
            CBSelectStoreTo.ItemsSource = allStores;
        }

        private void BtnGoToEditWindow_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow();
            this.Close();
            editWindow.Show();
        }

        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TransferBooks()
        {
            if (CBSelectStoreFrom.SelectedItem != null && CBSelectStoreTo.SelectedItem != null)
            {
                Store fromCurrentStore = CBSelectStoreFrom.SelectedItem as Store;
                string fromTargetISBN = currentBook.Isbn13;
                fromCurrentInventory = fromCurrentStore.Inventories.FirstOrDefault(inv => inv.Isbn13 == fromTargetISBN);

                Store toCurrentStore = CBSelectStoreTo.SelectedItem as Store;
                string toTargetISBN = currentBook.Isbn13;
                toCurrentInventory = toCurrentStore.Inventories.FirstOrDefault(inv => inv.Isbn13 == toTargetISBN);

                fromCurrentInventory.Amount--;
                toCurrentInventory.Amount++;

            }
        }

        private void ListBoxOfBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReloadContent();
        }

        private void ReloadContent()
        {
            if (ListBoxOfBooks.SelectedItem != null)
            {
                currentBook = ListBoxOfBooks.SelectedItem as Book;
                ToStore = CBSelectStoreTo.SelectedItem as Store;
                FromStore = CBSelectStoreFrom.SelectedItem as Store;
                TBAuthor.Text = currentBook.Author.LastName;
                TBCategory.Text = currentBook.Catagory;
                TBISBN.Text = currentBook.Isbn13;
                TBPrice.Text = currentBook.Price.ToString();
                TBTitle.Text = currentBook.Title;
                UpdateCopies();

            } else
            {
                currentBook = null;
                ToStore = null;
                FromStore = null;
                TBAuthor.Text = string.Empty;
                TBCategory.Text = string.Empty;
                TBISBN.Text = string.Empty;
                TBPrice.Text = string.Empty;
                TBTitle.Text = string.Empty;
                TBAvailbleCopiesTo.Text = string.Empty;
            }
        }

        private void BtnTransferBook_Click(object sender, RoutedEventArgs e)
        {
            if (CBSelectStoreTo != null)
            {
                TransferBooks();
                dbContext.SaveChanges();
                _ = LoadContent();
                ReloadContent();

            } else
            {
                MessageBox.Show("Please select a store to send to.");
            }
        }

        private void BtnRemoveBookFromStore_Click(object sender, RoutedEventArgs e)
        {
            if(CBSelectStoreFrom.SelectedItem != null)
            {
                Store fromCurrentStore = CBSelectStoreFrom.SelectedItem as Store;
                string targetISBN = currentBook.Isbn13;
                fromCurrentInventory = fromCurrentStore.Inventories.FirstOrDefault(inv => inv.Isbn13 == targetISBN);

                fromCurrentInventory.Amount--;
                dbContext.SaveChanges();
                _ = LoadContent();
                ReloadContent();
            }
            else
            {
                MessageBox.Show("Please select a store to remove from");
            }
        } 

        private void BtnAddBookToStore_Click(object sender, RoutedEventArgs e)
        {
            if (CBSelectStoreFrom.SelectedItem != null)
            {
                Store fromCurrentStore = CBSelectStoreFrom.SelectedItem as Store;
                string targetISBN = currentBook.Isbn13;
                fromCurrentInventory = fromCurrentStore.Inventories.FirstOrDefault(inv => inv.Isbn13 == targetISBN);

                fromCurrentInventory.Amount++;
                dbContext.SaveChanges();
                _ = LoadContent();
                ReloadContent();

            } else
            {
                MessageBox.Show("Please select a store to add to");
            }
        }

        private void CBSelectStoreFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBSelectStoreTo.Visibility != Visibility.Visible)
            {
                LStoreFrom.Content = "Select Store From: ";
                CBSelectStoreTo.Visibility = Visibility.Visible;
                LStoreTo.Visibility = Visibility.Visible;
                BtnTransferBook.Visibility = Visibility.Visible;
            }
                UpdateCopies();
            
        }

        private void CBSelectStoreTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCopies();
        }

        private void UpdateCopies()
        {
            if (CBSelectStoreTo.SelectedItem != null)
            {
                if (currentBook != null)
                {
                    Store toCurrentStore = CBSelectStoreTo.SelectedItem as Store;
                    string targetISBN = currentBook.Isbn13;
                    toCurrentInventory = toCurrentStore.Inventories.FirstOrDefault(inv => inv.Isbn13 == targetISBN);
                    if (toCurrentInventory == null)
                    {
                        TBAvailbleCopiesTo.Text = "0";
                    }
                    else
                    {
                        TBAvailbleCopiesTo.Text = toCurrentInventory.Amount.ToString();
                    }
                }
            }

            if (CBSelectStoreFrom.SelectedItem != null)
            {
                if (currentBook != null)
                {
                    Store fromCurrentStore = CBSelectStoreFrom.SelectedItem as Store;
                    string targetISBN = currentBook.Isbn13;
                    fromCurrentInventory = fromCurrentStore.Inventories.FirstOrDefault(inv => inv.Isbn13 == targetISBN);
                    if (fromCurrentInventory == null)
                    {
                        TBAvailbleCopiesFrom.Text = "0";
                    }

                    else
                    {
                        TBAvailbleCopiesFrom.Text = fromCurrentInventory.Amount.ToString();
                    }
                }
            }
        }
    }
}
