using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using BookStoreDataAccessLibrary;

namespace WpfBookStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly BookStoreDataAccessLibrary.DataService _dataService = null;

        public MainWindow()
        {
            InitializeComponent();
            _dataService = new DataService();
        }

        /// <summary>
        /// Mtehod describes application's behavior when button "Search" in TableItem Printed Products is pushed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBasePrintedProducts_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _dataService.SetConnectionString(BookStoreDataAccessLibrary.DataProvider.SqlServer);
                _dataService.OpenConnection(BookStoreDataAccessLibrary.DataProvider.SqlServer);

                if (PrintedProductsRadioButtonAuthorsName.IsChecked == true)
                {
                    PrintedDataGridView.DataContext = _dataService.SelectFromTablesManyToMany(
                        TableFromBookStoreDb.Books,
                        BooksTableField.BookId,
                        BooksTextBox.Text,
                        TableFromBookStoreDb.Authors,
                        AuthorsTableField.LastName,
                        TableFromBookStoreDb.BooksAndAuthors,
                        BooksAndAuthorsTableField.AuthorId);
                }
                else if (PrintedProductsRadioButtonBookTitle.IsChecked == true)
                {
                    PrintedDataGridView.DataContext = _dataService.SelectFromSingleTable(
                        TableFromBookStoreDb.Books,
                        BooksTableField.Title,
                        BooksTextBox.Text);
                }
                else if (PrintedProductsRadioButtonBookId.IsChecked == true)
                {
                    PrintedDataGridView.DataContext = _dataService.SelectFromSingleTable(
                        TableFromBookStoreDb.Books,
                        BooksTableField.BookId,
                        BooksTextBox.Text);
                }
                else if (string.IsNullOrEmpty(BooksTextBox.Text) || string.IsNullOrWhiteSpace(BooksTextBox.Text) ||
                         PrintedProductsRadioButtonShowAll.IsChecked == true)
                {
                    PrintedDataGridView.DataContext = _dataService.SelectAllFromTable(TableFromBookStoreDb.Books);
                }

                _dataService.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _dataService.CloseConnection();
            }
        }

        /// <summary>
        /// Mtehod describes application's behavior when button "Search" in TableItem Authors is pushed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBaseAuthors_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _dataService.SetConnectionString(BookStoreDataAccessLibrary.DataProvider.SqlServer);
                _dataService.OpenConnection(BookStoreDataAccessLibrary.DataProvider.SqlServer);

                if (AuthorsRadioButtonAuthorsName.IsChecked == true)
                {
                    AuthorsDataGridView.DataContext = _dataService.SelectFromSingleTable(TableFromBookStoreDb.Authors,
                        AuthorsTableField.LastName, AuthorsTextBox.Text);
                }
                else if (AuthorsRadioButtonAuthorsId.IsChecked == true)
                {
                    AuthorsDataGridView.DataContext = _dataService.SelectFromSingleTable(TableFromBookStoreDb.Authors,
                        AuthorsTableField.AuthorId, AuthorsTextBox.Text);
                }
                else if (string.IsNullOrEmpty(AuthorsTextBox.Text) || string.IsNullOrWhiteSpace(AuthorsTextBox.Text) ||
                         AuthorsRadioButtonShowAll.IsChecked == true)
                {
                    AuthorsDataGridView.DataContext = _dataService.SelectAllFromTable(TableFromBookStoreDb.Authors);
                }
                _dataService.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _dataService.CloseConnection();
            }

        }

        /// <summary>
        /// Mtehod describes application's behavior when button "Search" in TableItem Shops is pushed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBaseBookShops_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _dataService.SetConnectionString(BookStoreDataAccessLibrary.DataProvider.SqlServer);
                _dataService.OpenConnection(BookStoreDataAccessLibrary.DataProvider.SqlServer);

                if (ShopRadioButtonTitle.IsChecked == true)
                {
                    BookShopsDataGridView.DataContext = _dataService.SelectFromSingleTable(TableFromBookStoreDb.Shops,
                        ShopsTableField.ShopTitle, ShopTextBox.Text);
                }
                else if (ShopRadioButtonCustomersLastName.IsChecked == true)
                {
                    BookShopsDataGridView.DataContext = _dataService.SelectFromTablesManyToMany(
                        TableFromBookStoreDb.Shops,
                        ShopsTableField.ShopId,
                        ShopTextBox.Text,
                        TableFromBookStoreDb.Customers,
                        CustomersTableField.Cus_LastName,
                        TableFromBookStoreDb.Sales,
                        SalesTableField.CustomerId);
                }
                else if (string.IsNullOrEmpty(ShopTextBox.Text) || string.IsNullOrWhiteSpace(ShopTextBox.Text) ||
                         AuthorsRadioButtonShowAll.IsChecked == true)
                {
                    BookShopsDataGridView.DataContext = _dataService.SelectAllFromTable(TableFromBookStoreDb.Shops);
                }
                _dataService.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _dataService.CloseConnection();
            }
        }

        /// <summary>
        /// Mtehod describes application's behavior when button "Search" in TableItem Sales is pushed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBaseSales_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _dataService.SetConnectionString(BookStoreDataAccessLibrary.DataProvider.SqlServer);
                _dataService.OpenConnection(BookStoreDataAccessLibrary.DataProvider.SqlServer);

                if (SaleRadioButtonShop.IsChecked == true)
                {
                    SalesDataGridView.DataContext = _dataService.JoinFromTables(
                        TableFromBookStoreDb.Sales,
                        CustomersTableField.CustomerId,
                        SalesTableField.ShopId,
                        SalesTableField.DateOfOperation,
                        TableFromBookStoreDb.Customers,
                        CustomersTableField.Cus_LastName,
                        TableFromBookStoreDb.Shops,
                        ShopsTableField.ShopTitle,
                        SalesTextBox.Text
                        );
                }
                else if (string.IsNullOrEmpty(SalesTextBox.Text) || string.IsNullOrWhiteSpace(SalesTextBox.Text) ||
                         SaleRadioButtonShowAll.IsChecked == true)
                {
                    SalesDataGridView.DataContext = _dataService.SelectAllFromTable(TableFromBookStoreDb.Sales);
                }
                _dataService.CloseConnection();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _dataService.CloseConnection();
            }
        }

        /// <summary>
        /// Mtehod describes application's behavior when button "Search" in TableItem Customers is pushed 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBaseCustomers_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _dataService.SetConnectionString(BookStoreDataAccessLibrary.DataProvider.SqlServer);
                _dataService.OpenConnection(BookStoreDataAccessLibrary.DataProvider.SqlServer);

                CustomersDataGridView.DataContext = _dataService.SelectAllFromTable(TableFromBookStoreDb.Customers);
                _dataService.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _dataService.CloseConnection();
            }
        }
    }
}
