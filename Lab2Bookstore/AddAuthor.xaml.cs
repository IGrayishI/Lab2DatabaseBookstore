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
    /// Interaction logic for AddAuthor.xaml
    /// </summary>
    public partial class AddAuthor : Window
    {
        LiberaDbContext dbContext;
        Author currentAuthor;
        public AddAuthor()
        {
            InitializeComponent();
            dbContext = new();
            LoadContent();
        }

        private void LoadContent()
        {
            var allAuthors = dbContext.Authors.ToList();
            LBOfAuthors.ItemsSource = allAuthors;
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if(TBFirstName.Text != null && TBLastName.Text != null && TBBirthDate.Text != null)
            {
                Author newA = new Author()
                {
                    FirstName = TBFirstName.Text,
                    LastName = TBLastName.Text,
                    Birthdate = DateTime.Parse(TBBirthDate.Text)
                };
                dbContext.Authors.Add(newA);
                dbContext.SaveChanges();
                LoadContent();
            } else
            {
                MessageBox.Show("Please enter all fields");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (currentAuthor != null)
            {
                currentAuthor.FirstName = TBFirstName.Text;
                currentAuthor.LastName = TBLastName.Text;
                currentAuthor.Birthdate = DateTime.Parse(TBBirthDate.Text);

                dbContext.SaveChanges();
                LoadContent();
            }
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentAuthor != null)
            {
                var hasBooks = await dbContext.Books.FirstOrDefaultAsync(b => b.Author == currentAuthor);
                if (hasBooks == null)
                {
                    dbContext.Authors.Remove(currentAuthor);
                    dbContext.SaveChanges();
                    LoadContent();

                } else
                {
                    MessageBox.Show($"{currentAuthor.FirstName} {currentAuthor.LastName} still has books in the system and can't be removed.");
                }
            }
        }

        private void LBOfAuthors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentAuthor = LBOfAuthors.SelectedItem as Author;

            if (currentAuthor != null)
            {
                TBFirstName.Text = currentAuthor.FirstName;
                TBLastName.Text = currentAuthor.LastName;
                TBBirthDate.Text = currentAuthor.Birthdate.ToString();
            } else
            {
                Clear();
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            EditWindow editWindow = new EditWindow();
            this.Close();
            editWindow.Show();

        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            TBFirstName.Text = string.Empty;
            TBLastName.Text = string.Empty;
            TBBirthDate.Text = string.Empty;
        }
    }
}
